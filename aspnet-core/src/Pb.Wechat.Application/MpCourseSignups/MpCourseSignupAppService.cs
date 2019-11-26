using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Pb.Wechat.Auditing.Exporting;
using Pb.Wechat.Dto;
using Pb.Wechat.MpCourseSignups.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.MpCourseSignups
{
    //[AbpAuthorize(AppPermissions.Pages_MpCourseSignups)]
    public class MpCourseSignupAppService : AsyncCrudAppService<MpCourseSignup, MpCourseSignupDto, int, GetMpCourseSignupsInput, MpCourseSignupDto, MpCourseSignupDto>, IMpCourseSignupAppService
    {
        private readonly IMpCourseSignupListExcelExporter _MpCourseSignupListExcelExporter;

        public MpCourseSignupAppService(IRepository<MpCourseSignup, int> repository, IMpCourseSignupListExcelExporter MpCourseSignupListExcelExporter) : base(repository)
        {
            _MpCourseSignupListExcelExporter = MpCourseSignupListExcelExporter;

        }
        public override Task<PagedResultDto<MpCourseSignupDto>> GetAll(GetMpCourseSignupsInput input)
        {
            return base.GetAll(input);
        }
        protected override IQueryable<MpCourseSignup> CreateFilteredQuery(GetMpCourseSignupsInput input)
        {
            return Repository.GetAll()
                .Where(m => m.MpID == 1)
                .WhereIf(!string.IsNullOrWhiteSpace(input.OpenID), c => c.OpenID.Contains(input.OpenID))
                .WhereIf(!string.IsNullOrWhiteSpace(input.CourseName), c => c.CourseName.Contains(input.CourseName))
                .WhereIf(input.CourseID != 0, c => c.CourseID == input.CourseID)
                .Where(m => m.IsConfirmed == input.IsConfirmed)
                .WhereIf(input.CRMID!=0,c=>c.CRMID==input.CRMID);
        }

        public async Task<FileDto> GetListToExcel(GetMpCourseSignupsInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _MpCourseSignupListExcelExporter.ExportToFile(dtos);
        }

        public async Task<MpCourseSignupDto> GetFirstOrDefaultByInput(GetMpCourseSignupsInput input)
        {
            return MapToEntityDto(await AsyncQueryableExecuter.FirstOrDefaultAsync(CreateFilteredQuery(input)));
        }

        public async Task<List<MpCourseSignupDto>> GetList()
        {
            return (await AsyncQueryableExecuter.ToListAsync(Repository.GetAll().Where(m => m.IsDeleted == false))).Select(MapToEntityDto).ToList();
        }
    }
}
