using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Pb.Wechat.Auditing.Exporting;
using Pb.Wechat.Dto;
using Pb.Wechat.MpEventScanLogs.Dto;
using Pb.Wechat.UserMps;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.MpEventScanLogs
{
    //[AbpAuthorize(AppPermissions.Pages_MpEventScanLogs)]
    public class MpEventScanLogAppService : AsyncCrudAppService<MpEventScanLog, MpEventScanLogDto, int, GetMpEventScanLogsInput, MpEventScanLogDto, MpEventScanLogDto>, IMpEventScanLogAppService
    {

        private readonly IUserMpAppService _userMpAppService;
        private readonly IMpEventScanLogListExcelExporter _MpEventScanLogListExcelExporter;
        public MpEventScanLogAppService(IRepository<MpEventScanLog, int> repository, IMpEventScanLogListExcelExporter MpEventScanLogListExcelExporter, IUserMpAppService userMpAppService) : base(repository)
        {
            _MpEventScanLogListExcelExporter = MpEventScanLogListExcelExporter;
            _userMpAppService = userMpAppService;


        }

        protected override IQueryable<MpEventScanLog> CreateFilteredQuery(GetMpEventScanLogsInput input)
        {
         
            return Repository.GetAll()
                .Where(c => c.MpID == input.MpID)
                //.WhereIf(!string.IsNullOrWhiteSpace(input.Name), c => c.Name.Contains(input.Name))
                .WhereIf(!string.IsNullOrWhiteSpace(input.OpenID), c => c.OpenID.Contains(input.OpenID));
        }

     
        public override Task<MpEventScanLogDto> Get(EntityDto<int> input)
        {
            return base.Get(input);
        }

        public override async Task<MpEventScanLogDto> Create(MpEventScanLogDto input)
        {
         
            return await base.Create(input);
        }
        public async Task<FileDto> GetListToExcel(GetMpEventScanLogsInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _MpEventScanLogListExcelExporter.ExportToFile(dtos);
        }

        public async Task<MpEventScanLogDto> GetFirstOrDefault()
        {
            return MapToEntityDto(await Repository.FirstOrDefaultAsync(c => 1 == 1));
        }

       


    }
}
