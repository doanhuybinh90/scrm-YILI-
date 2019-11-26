using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Pb.Wechat.Auditing.Exporting;
using Pb.Wechat.Dto;
using Pb.Wechat.CustomerServiceWorkTimes.Dto;
using Pb.Wechat.UserMps;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Abp.Runtime.Caching;

namespace Pb.Wechat.CustomerServiceWorkTimes
{
    //[AbpAuthorize(AppPermissions.Pages_CustomerServiceWorkTimes)]
    public class CustomerServiceWorkTimeAppService : AsyncCrudAppService<CustomerServiceWorkTime, CustomerServiceWorkTimeDto, int, GetCustomerServiceWorkTimesInput, CustomerServiceWorkTimeDto, CustomerServiceWorkTimeDto>, ICustomerServiceWorkTimeAppService
    {

        private readonly IUserMpAppService _userMpAppService;
        private readonly ICustomerServiceWorkTimeListExcelExporter _CustomerServiceWorkTimeListExcelExporter;
        private readonly ICacheManager _cacheManager;
        public CustomerServiceWorkTimeAppService(IRepository<CustomerServiceWorkTime, int> repository, ICustomerServiceWorkTimeListExcelExporter CustomerServiceWorkTimeListExcelExporter, IUserMpAppService userMpAppService, ICacheManager cacheManager) : base(repository)
        {
            _CustomerServiceWorkTimeListExcelExporter = CustomerServiceWorkTimeListExcelExporter;
            _userMpAppService = userMpAppService;
            _cacheManager = cacheManager;

        }

        protected override IQueryable<CustomerServiceWorkTime> CreateFilteredQuery(GetCustomerServiceWorkTimesInput input)
        {
            return Repository.GetAll()
                .Where(c => c.MpID == input.MpID)
                .WhereIf(!string.IsNullOrWhiteSpace(input.WeekDay), c => c.WeekDay == input.WeekDay)
               ;
        }
        public async override Task<CustomerServiceWorkTimeDto> Create(CustomerServiceWorkTimeDto input)
        {
            var model =Repository.FirstOrDefault(m => m.MpID == input.MpID && m.WeekDay == input.WeekDay);
            input.LastModificationTime = DateTime.Now;
            CustomerServiceWorkTimeDto result = null;
            if (model==null)
            {
                if (string.IsNullOrWhiteSpace(input.MorningEndHour))
                    input.MorningEndHour = "0";
                if (string.IsNullOrWhiteSpace(input.MorningEndMinute))
                    input.MorningEndMinute = "0";
                if (string.IsNullOrWhiteSpace(input.MorningStartHour))
                    input.MorningStartHour = "0";
                if (string.IsNullOrWhiteSpace(input.MorningStartMinute))
                    input.MorningStartMinute = "0";
                if (string.IsNullOrWhiteSpace(input.AfternoonEndHour))
                    input.AfternoonEndHour = "0";
                if (string.IsNullOrWhiteSpace(input.AfternoonEndMinute))
                    input.AfternoonEndMinute = "0";
                if (string.IsNullOrWhiteSpace(input.AfternoonStartHour))
                    input.AfternoonStartHour = "0";
                if (string.IsNullOrWhiteSpace(input.AfternoonStartMinute))
                    input.AfternoonStartMinute = "0";
                result= await base.Create(input);
            }
            else
            {
                model.MorningEndHour = input.MorningEndHour;
                model.MorningEndMinute = input.MorningEndMinute;
                model.MorningStartHour = input.MorningStartHour;
                model.MorningStartMinute = input.MorningStartMinute;
                model.AfternoonEndHour = input.AfternoonEndHour;
                model.AfternoonEndMinute = input.AfternoonEndMinute;
                model.AfternoonStartHour = input.AfternoonStartHour;
                model.AfternoonStartMinute = input.AfternoonStartMinute;
                model.LastModificationTime = input.LastModificationTime;
                var data = ObjectMapper.Map<CustomerServiceWorkTimeDto>(model);
                result=await  Update(data);
            }

            await _cacheManager.GetCache(AppConsts.Cache_CallBack).RemoveAsync("WorkTime");
            await GetWorkTimeCache();
            return result;
        }
        public async override Task<CustomerServiceWorkTimeDto> Update(CustomerServiceWorkTimeDto input)
        {
            var result= await base.Update(input);
            await _cacheManager.GetCache(AppConsts.Cache_CallBack).RemoveAsync("WorkTime");
            await GetWorkTimeCache();
            return result;
        }
        public async Task<FileDto> GetListToExcel(GetCustomerServiceWorkTimesInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _CustomerServiceWorkTimeListExcelExporter.ExportToFile(dtos);
        }

        public async Task<CustomerServiceWorkTimeDto> GetFirstOrDefaultByInput(GetCustomerServiceWorkTimesInput input)
        {
            return MapToEntityDto(await AsyncQueryableExecuter.FirstOrDefaultAsync(CreateFilteredQuery(input)));
        }
        public async Task<PagedResultDto<WorkTimeOutput>> GetWorkTimeList(GetCustomerServiceWorkTimesInput input)
        {

            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<WorkTimeOutput>(
                totalCount,
                entities.Select(m => new WorkTimeOutput
                {
                    Id=m.Id,
                    MpID = m.MpID,
                    WeekDay = m.WeekDay,
                    MorningWorkTime = $"{m.MorningStartHour}:{m.MorningStartMinute.PadLeft(2, '0')}至{m.MorningEndHour}:{m.MorningEndMinute.PadLeft(2, '0')}",
                    AfternoonWorkTime = $"{m.AfternoonStartHour}:{m.AfternoonStartMinute.PadLeft(2, '0')}至{m.AfternoonEndHour}:{m.AfternoonEndMinute.PadLeft(2, '0')}",
                    LastModificationTime = m.LastModificationTime
                }).ToList()
            );



        }

        /// <summary>
        /// 判定是否上班时间
        /// </summary>
        /// <param name="mpId"></param>
        /// <returns></returns>
        public async Task<bool> CheckIsWorkingTime(int mpId)
        {
            var date = DateTime.Now;
            var weekDay = ((int)date.DayOfWeek).ToString();
            var data = await Repository.FirstOrDefaultAsync(m => m.MpID == mpId && m.WeekDay == weekDay);
            if (data == null)
                return false;
            else
            {
                var hour = date.Hour;
                var min = date.Minute;

                var morningstartdate = new DateTime(date.Year, date.Month, date.Day, int.Parse(data.MorningStartHour), int.Parse(data.MorningStartMinute), 0);
                var morningenddate = new DateTime(date.Year, date.Month, date.Day, int.Parse(data.MorningEndHour), int.Parse(data.MorningEndMinute), 0);

                var afternoonstartdate = new DateTime(date.Year, date.Month, date.Day, int.Parse(data.AfternoonStartHour), int.Parse(data.AfternoonStartMinute), 0);
                var afternoonenddate = new DateTime(date.Year, date.Month, date.Day, int.Parse(data.AfternoonEndHour), int.Parse(data.AfternoonEndMinute), 0);
                if (date >= morningstartdate && date <= morningenddate)
                    return true;
                else
                {
                    if (date >= afternoonstartdate && date <= afternoonenddate)
                        return true;
                    else
                        return false;
                }

            }
        }
        public override Task Delete(EntityDto<int> input)
        {
            var result=base.Delete(input);
            _cacheManager.GetCache(AppConsts.Cache_CallBack).RemoveAsync("WorkTime");
            GetWorkTimeCache();
            return result;
        }

        /// <summary>
        /// 获取上下班时间列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<WorkTimeDto>> GetWorkTimeCache()
        {
            var result= await _cacheManager.GetCache(AppConsts.Cache_CallBack).GetAsync("WorkTime",async () =>
            {
                var resultList = new List<WorkTimeDto>();
                var dataList = Repository.GetAll().Where(t => t.IsDeleted == false).ToList();

                foreach(var data in dataList)
                {
                    resultList.Add(new WorkTimeDto {
                        MpID = data.MpID,
                        WeekDay=Convert.ToInt32(data.WeekDay),
                        BeginTime=new TimeSpan(!string.IsNullOrWhiteSpace(data.MorningStartHour)?Convert.ToInt32(data.MorningStartHour):0, !string.IsNullOrWhiteSpace(data.MorningStartMinute) ? Convert.ToInt32(data.MorningStartMinute) : 0, 0),
                        EndTime= new TimeSpan(!string.IsNullOrWhiteSpace(data.MorningEndHour) ? Convert.ToInt32(data.MorningEndHour) : 0, !string.IsNullOrWhiteSpace(data.MorningEndMinute) ? Convert.ToInt32(data.MorningEndMinute) : 0, 0)
                    });
                    resultList.Add(new WorkTimeDto
                    {
                        MpID = data.MpID,
                        WeekDay = Convert.ToInt32(data.WeekDay),
                        BeginTime = new TimeSpan(!string.IsNullOrWhiteSpace(data.AfternoonStartHour) ? Convert.ToInt32(data.AfternoonStartHour) : 0, !string.IsNullOrWhiteSpace(data.AfternoonStartMinute) ? Convert.ToInt32(data.AfternoonStartMinute) : 0, 0),
                        EndTime = new TimeSpan(!string.IsNullOrWhiteSpace(data.AfternoonEndHour) ? Convert.ToInt32(data.AfternoonEndHour) : 0, !string.IsNullOrWhiteSpace(data.AfternoonEndMinute) ? Convert.ToInt32(data.AfternoonEndMinute) : 0, 0)
                    });
                }

                return resultList;
            });
            return result == null ? null : result as List<WorkTimeDto>;
        }
    }
}
