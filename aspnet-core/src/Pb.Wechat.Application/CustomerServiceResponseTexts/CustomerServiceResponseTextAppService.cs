using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Pb.Wechat.Auditing.Exporting;
using Pb.Wechat.Dto;
using Pb.Wechat.CustomerServiceResponseTexts.Dto;
using Pb.Wechat.UserMps;
using System;
using System.Linq;
using System.Threading.Tasks;
using Pb.Wechat.CustomerServiceWorkTimes;
using Abp.Application.Services.Dto;

namespace Pb.Wechat.CustomerServiceResponseTexts
{
    //[AbpAuthorize(AppPermissions.Pages_CustomerServiceResponseTexts)]
    public class CustomerServiceResponseTextAppService : AsyncCrudAppService<CustomerServiceResponseText, CustomerServiceResponseTextDto, int, GetCustomerServiceResponseTextsInput, CustomerServiceResponseTextDto, CustomerServiceResponseTextDto>, ICustomerServiceResponseTextAppService
    {

        private readonly IUserMpAppService _userMpAppService;
        private readonly ICustomerServiceResponseTextListExcelExporter _CustomerServiceResponseTextListExcelExporter;
        private readonly ICustomerServiceWorkTimeAppService _customerServiceWorkTimeAppService;
        public CustomerServiceResponseTextAppService(IRepository<CustomerServiceResponseText, int> repository, ICustomerServiceResponseTextListExcelExporter CustomerServiceResponseTextListExcelExporter, IUserMpAppService userMpAppService,  ICustomerServiceWorkTimeAppService customerServiceWorkTimeAppService) : base(repository)
        {
            _CustomerServiceResponseTextListExcelExporter = CustomerServiceResponseTextListExcelExporter;
            _userMpAppService = userMpAppService;
            _customerServiceWorkTimeAppService = customerServiceWorkTimeAppService;

        }

        protected override IQueryable<CustomerServiceResponseText> CreateFilteredQuery(GetCustomerServiceResponseTextsInput input)
        {
            var inputtype = input.ResponseType == null ? "" : input.ResponseType.ToString();
            var commonType = ResponseType.common.ToString();
            return Repository.GetAll()
                .WhereIf(input.MpID!=0,c => c.MpID == input.MpID)
                .WhereIf(input.NotCommon!=null,c=>c.ResponseType!= commonType)
                .WhereIf(input.ResponseType != null, c => c.ResponseType == inputtype)
                .WhereIf(!string.IsNullOrWhiteSpace(input.ResponseText), c => c.ResponseText.Contains(input.ResponseText))
                .WhereIf(input.ResponseContentType!=null,c=>c.ReponseContentType==input.ResponseContentType)
                .WhereIf(!string.IsNullOrWhiteSpace(input.TitleOrDescription),c=>c.Title.Contains(input.TitleOrDescription) || c.Description.Contains(input.TitleOrDescription))
                .WhereIf(input.TypeId!=null,c=>c.TypeId==input.TypeId);
        }
        public override async Task<CustomerServiceResponseTextDto> Update(CustomerServiceResponseTextDto input)
        {
            if (string.IsNullOrWhiteSpace(input.MediaId))
                input.MediaId = Guid.NewGuid().ToString();
            if (input.ResponseType != ResponseType.common.ToString())
            {
                var model = Repository.FirstOrDefault(m => m.ResponseType == input.ResponseType && m.IsDeleted == false && m.MpID == input.MpID);
                if (model != null)
                {
                    model.ResponseText = input.ResponseText;
                    model.TypeId = input.TypeId;
                    model.TypeName = input.TypeName;
                    var data = ObjectMapper.Map<CustomerServiceResponseTextDto>(model);
                    return await base.Update(data);
                }
                
            }
            return await base.Update(input);
        }
        
        public override async Task<CustomerServiceResponseTextDto> Create(CustomerServiceResponseTextDto input)
        {
         
       
            input.LastModificationTime = DateTime.Now;
            if (string.IsNullOrWhiteSpace(input.MediaId))
                input.MediaId = Guid.NewGuid().ToString();
            if (input.ResponseType != ResponseType.common.ToString())
            {
                var model = Repository.FirstOrDefault(m => m.ResponseType == input.ResponseType && m.IsDeleted == false && m.MpID == input.MpID);
                if (model != null)
                {
                    model.ResponseText = input.ResponseText;
                    model.TypeId = input.TypeId;
                    model.TypeName = input.TypeName;
                    var data = ObjectMapper.Map<CustomerServiceResponseTextDto>(model);
                    return await base.Update(data);
                }
                else
                    return await base.Create(input);
            }
            else
                return await base.Create(input);
        }
        public async Task<FileDto> GetListToExcel(GetCustomerServiceResponseTextsInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _CustomerServiceResponseTextListExcelExporter.ExportToFile(dtos);
        }

        public async Task<CustomerServiceResponseTextDto> GetFirstOrDefaultByInput(GetCustomerServiceResponseTextsInput input)
        {
            return MapToEntityDto(await AsyncQueryableExecuter.FirstOrDefaultAsync(CreateFilteredQuery(input)));
        }

        public async Task<string> GetCustomerDefaultResponseString(int mpId)
        {
            var isWorkingtime = await CheckIsWorkingTime(mpId);
            var type = ResponseType.unwork.ToString();
            if (isWorkingtime)
            {
                type = ResponseType.working.ToString(); 
            }
            
            return (await Repository.FirstOrDefaultAsync(m => m.IsDeleted == false && m.ResponseType == type)).ResponseText;
        }
        public async Task<string> GetWaitResponseString(int mpId)
        {
            return (await Repository.FirstOrDefaultAsync(m => m.IsDeleted == false && m.ResponseType == ResponseType.wait.ToString())).ResponseText;
        }
        /// <summary>
        /// 判定是否上班时间
        /// </summary>
        /// <param name="mpId"></param>
        /// <returns></returns>
        public async Task<bool> CheckIsWorkingTime(int mpId)
        {
            var workTimeList = await _customerServiceWorkTimeAppService.GetWorkTimeCache();
            var date = DateTime.Now.TimeOfDay;
            var weekDay = (int)DateTime.Now.DayOfWeek;
            if (workTimeList.Any(m => m.MpID == mpId && m.WeekDay == weekDay && date >= m.BeginTime && date <= m.EndTime))
                return true;
            else
                return false;


            //var data = await _cusWorkRepository.FirstOrDefaultAsync(m => m.MpID == mpId && m.WeekDay == weekDay);
            //if (data == null)
            //    return false;
            //else
            //{
            //    var hour = date.Hour;
            //    var min = date.Minute;

            //    var morningstartdate = new DateTime(date.Year, date.Month, date.Day, int.Parse(data.MorningStartHour), int.Parse(data.MorningStartMinute), 0);
            //    var morningenddate = new DateTime(date.Year, date.Month, date.Day, int.Parse(data.MorningEndHour), int.Parse(data.MorningEndMinute), 0);

            //    var afternoonstartdate = new DateTime(date.Year, date.Month, date.Day, int.Parse(data.AfternoonStartHour), int.Parse(data.AfternoonStartMinute), 0);
            //    var afternoonenddate = new DateTime(date.Year, date.Month, date.Day, int.Parse(data.AfternoonEndHour), int.Parse(data.AfternoonEndMinute), 0);
            //    if (date >= morningstartdate && date <= morningenddate)
            //        return true;
            //    else
            //    {
            //        if (date >= afternoonstartdate && date <= afternoonenddate)
            //            return true;
            //        else
            //            return false;
            //    }

            //}
        }
        
       
    }
}
