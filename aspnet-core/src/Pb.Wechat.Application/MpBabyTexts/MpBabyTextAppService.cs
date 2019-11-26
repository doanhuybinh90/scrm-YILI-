using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using Pb.Wechat.Auditing.Exporting;
using Pb.Wechat.Dto;
using Pb.Wechat.MpBabyTexts.Dto;
using Pb.Wechat.UserMps;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.MpBabyTexts
{
    //[AbpAuthorize(AppPermissions.Pages_MpBabyTexts)]
    public class MpBabyTextAppService : AsyncCrudAppService<MpBabyText, MpBabyTextDto, int, GetMpBabyTextsInput, MpBabyTextDto, MpBabyTextDto>, IMpBabyTextAppService
    {

        private readonly IUserMpAppService _userMpAppService;
        private readonly IMpBabyTextListExcelExporter _MpBabyTextListExcelExporter;
        public MpBabyTextAppService(IRepository<MpBabyText, int> repository, IMpBabyTextListExcelExporter MpBabyTextListExcelExporter, IUserMpAppService userMpAppService) : base(repository)
        {
            _MpBabyTextListExcelExporter = MpBabyTextListExcelExporter;
            _userMpAppService = userMpAppService;


        }

        protected override IQueryable<MpBabyText> CreateFilteredQuery(GetMpBabyTextsInput input)
        {

            return Repository.GetAll()
                //.Where(c => c.MpID == input.MpID)
                .WhereIf(input.BabyAge != null, c => c.BabyAge == input.BabyAge)
                .WhereIf(!string.IsNullOrWhiteSpace(input.BabyText), c => c.BabyText.Contains(input.BabyText));

        }
        /// <summary>
        /// 计算起始日与结束日
        /// </summary>
        /// <param name="weekCount"></param>
        /// <param name="beginday"></param>
        /// <param name="endday"></param>
        /// <param name="monthCount"></param>
        /// <param name="unBorned"></param>
        private void GetDayRange(int weekCount, out int? beginday, out int? endday, int monthCount = 0, bool unBorned = false)
        {
            beginday = null;
            endday = null;
            if (!unBorned)//已出生
            {
                if (monthCount <= 12)
                {

                    endday = monthCount * 4 * 7 + weekCount * 7;
                    beginday = endday - 7 + 1;
                    if (monthCount == 12 && weekCount == 4)
                    {
                        endday = 366;
                    }

                }
                else
                {
                    int days = 0;
                    int n = 1;
                    var mod = monthCount % 12;
                    if (mod == 0)
                    {
                        n = monthCount / 12;
                        days = n * 366;
                    }
                    else
                    {
                        n = (monthCount - mod) / 12;
                        days = n * 366;
                    }
                    endday = 30 * (monthCount - 12 * n) + days;
                    beginday = 30 * (monthCount - 12 * n - 1) + 1 + days;

                }
            }
            else//怀孕阶段
            {
                beginday = (40 - weekCount + 1) * -1 * 7;
                endday = (40 - weekCount + 1) * -1 * 7 + 7;
                if (endday > 0)
                    endday = 0;
                else
                {
                    if (weekCount != 1)
                        beginday++;
                }

            }

        }
        public override async Task<MpBabyTextDto> Update(MpBabyTextDto input)
        {
            if (input.BabyAge != -9999 && input.BabyTextType == BabyTextType.UnBorn.ToString())
            {
                //input.BabyAge = -1 * input.UnbornWeek;
                int? beginday = null;
                int? endday = null;
                GetDayRange(input.UnbornWeek, out beginday, out endday, unBorned: true);
                input.BeginDay = beginday;
                input.EndDay = endday;
            }
            if (input.BabyAge != -9999 && input.BabyTextType == BabyTextType.OneYear.ToString())
            {
                //input.BabyAge = input.OneYearMonth * 4 + input.OneYearWeek;
                int? beginday = null;
                int? endday = null;
                GetDayRange(input.OneYearWeek, out beginday, out endday, monthCount: input.OneYearMonth, unBorned: false);
                input.BeginDay = beginday;
                input.EndDay = endday;
            }
            if (input.BabyAge != -9999 && input.BabyTextType == BabyTextType.Over.ToString())
            {
                //input.BabyAge = input.OverYear * 4 * 12 + input.OverMonth * 4;
                int? beginday = null;
                int? endday = null;
                GetDayRange(0, out beginday, out endday, monthCount: input.OverYear * 12 + input.OverMonth, unBorned: false);
                input.BeginDay = beginday;
                input.EndDay = endday;
            }
            var count = await Repository.CountAsync(m => m.IsDeleted == false && m.MpID == input.MpID && m.BeginDay == input.BeginDay && m.EndDay == input.EndDay && m.Id != input.Id);
            if (count > 0)
                throw new UserFriendlyException("对不起，不能添加相同的宝宝周龄信息。");
            return await base.Update(input);
        }
        public override async Task<MpBabyTextDto> Create(MpBabyTextDto input)
        {
            var mpid = await _userMpAppService.GetDefaultMpId();
            input.MpID = mpid;
            input.LastModificationTime = DateTime.Now;
            if (input.BabyAge != -9999 && input.BabyTextType == BabyTextType.UnBorn.ToString())
            {
                //input.BabyAge = -1 * input.UnbornWeek;
                int? beginday = null;
                int? endday = null;
                GetDayRange(input.UnbornWeek, out beginday, out endday, unBorned: true);
                input.BeginDay = beginday;
                input.EndDay = endday;
            }
            if (input.BabyAge != -9999 && input.BabyTextType == BabyTextType.OneYear.ToString())
            {
                //input.BabyAge = input.OneYearMonth * 4 + input.OneYearWeek;
                int? beginday = null;
                int? endday = null;
                GetDayRange(input.OneYearWeek, out beginday, out endday, monthCount: input.OneYearMonth, unBorned: false);
                input.BeginDay = beginday;
                input.EndDay = endday;
            }
            if (input.BabyAge != -9999 && input.BabyTextType == BabyTextType.Over.ToString())
            {
                //input.BabyAge = input.OverYear * 4 * 12 + input.OverMonth * 4;

                int? beginday = null;
                int? endday = null;
                GetDayRange(0, out beginday, out endday, monthCount: input.OverYear * 12 + input.OverMonth, unBorned: false);
                input.BeginDay = beginday;
                input.EndDay = endday;
            }
            var model = Repository.FirstOrDefault(m => m.BeginDay == input.BeginDay && m.EndDay==input.EndDay && m.IsDeleted == false && m.MpID == mpid);
            if (model != null)
            {
                model.BabyText = input.BabyText;
                var data = ObjectMapper.Map<MpBabyTextDto>(model);
                return await base.Update(data);
            }
            else
                return await base.Create(input);
        }
        public async Task<FileDto> GetListToExcel(GetMpBabyTextsInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _MpBabyTextListExcelExporter.ExportToFile(dtos);
        }

        public async Task<MpBabyTextDto> GetFirstOrDefaultByInput(GetMpBabyTextsInput input)
        {
            return MapToEntityDto(await AsyncQueryableExecuter.FirstOrDefaultAsync(CreateFilteredQuery(input)));
        }

        public async Task<MpBabyTextDto> GetMaxWeekText(int week)
        {
            var babyage = Repository.GetAll().Where(m => m.IsDeleted == false && m.BabyAge <= week).Max(m => m.BabyAge);
            if (week >= 0 && babyage > 0 && week - babyage > 4)
                return ObjectMapper.Map<MpBabyTextDto>(await Repository.FirstOrDefaultAsync(m => m.IsDeleted == false && m.BabyAge == -9999));

            return ObjectMapper.Map<MpBabyTextDto>(await Repository.FirstOrDefaultAsync(m => m.IsDeleted == false && m.BabyAge == babyage));
        }

        public async Task<MpBabyTextDto> GetTextByday(int day)
        {
            var model = await Repository.FirstOrDefaultAsync(m => m.IsDeleted == false && m.BeginDay <= day && m.EndDay >= day);
            if (model == null)
                return ObjectMapper.Map<MpBabyTextDto>(await Repository.FirstOrDefaultAsync(m => m.IsDeleted == false && m.BabyAge == -9999));
            else
                return ObjectMapper.Map<MpBabyTextDto>(model);
        }
    }
}
