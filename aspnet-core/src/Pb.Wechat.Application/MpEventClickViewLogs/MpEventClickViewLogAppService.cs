using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Pb.Wechat.Auditing.Exporting;
using Pb.Wechat.Dto;
using Pb.Wechat.MpEventClickViewLogs.Dto;
using Pb.Wechat.UserMps;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.MpEventClickViewLogs
{
    //[AbpAuthorize(AppPermissions.Pages_MpEventClickViewLogs)]
    public class MpEventClickViewLogAppService : AsyncCrudAppService<MpEventClickViewLog, MpEventClickViewLogDto, int, GetMpEventClickViewLogsInput, MpEventClickViewLogDto, MpEventClickViewLogDto>, IMpEventClickViewLogAppService
    {

        private readonly IUserMpAppService _userMpAppService;
        private readonly IMpEventClickViewLogListExcelExporter _MpEventClickViewLogListExcelExporter;
        public MpEventClickViewLogAppService(IRepository<MpEventClickViewLog, int> repository, IMpEventClickViewLogListExcelExporter MpEventClickViewLogListExcelExporter, IUserMpAppService userMpAppService) : base(repository)
        {
            _MpEventClickViewLogListExcelExporter = MpEventClickViewLogListExcelExporter;
            _userMpAppService = userMpAppService;


        }

        protected override IQueryable<MpEventClickViewLog> CreateFilteredQuery(GetMpEventClickViewLogsInput input)
        {
 
            return Repository.GetAll()
                .Where(c => c.MpID == input.MpID)
                //.WhereIf(!string.IsNullOrWhiteSpace(input.Name), c => c.Name.Contains(input.Name))
                .WhereIf(!string.IsNullOrWhiteSpace(input.OpenID), c => c.OpenID.Contains(input.OpenID));
        }

     
        public override Task<MpEventClickViewLogDto> Get(EntityDto<int> input)
        {
            return base.Get(input);
        }

        public override async Task<MpEventClickViewLogDto> Create(MpEventClickViewLogDto input)
        {
          
            return await base.Create(input);
        }
        public async Task<FileDto> GetListToExcel(GetMpEventClickViewLogsInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _MpEventClickViewLogListExcelExporter.ExportToFile(dtos);
        }

        public async Task<MpEventClickViewLogDto> GetFirstOrDefault()
        {
            return MapToEntityDto(await Repository.FirstOrDefaultAsync(c=>1==1));
        }

       


    }
}
