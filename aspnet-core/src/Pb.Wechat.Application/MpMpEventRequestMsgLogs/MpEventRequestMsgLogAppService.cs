using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Pb.Wechat.Auditing.Exporting;
using Pb.Wechat.Dto;
using Pb.Wechat.MpEventRequestMsgLogs.Dto;
using Pb.Wechat.UserMps;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.MpEventRequestMsgLogs
{
    //[AbpAuthorize(AppPermissions.Pages_MpEventRequestMsgLogs)]
    public class MpEventRequestMsgLogAppService : AsyncCrudAppService<MpEventRequestMsgLog, MpEventRequestMsgLogDto, int, GetMpEventRequestMsgLogsInput, MpEventRequestMsgLogDto, MpEventRequestMsgLogDto>, IMpEventRequestMsgLogAppService
    {

        private readonly IUserMpAppService _userMpAppService;
        private readonly IMpEventRequestMsgLogListExcelExporter _mpEventRequestMsgLogListExcelExporter;
        public MpEventRequestMsgLogAppService(IRepository<MpEventRequestMsgLog, int> repository, IMpEventRequestMsgLogListExcelExporter mpEventRequestMsgLogListExcelExporter, IUserMpAppService userMpAppService) : base(repository)
        {
            _mpEventRequestMsgLogListExcelExporter = mpEventRequestMsgLogListExcelExporter;
            _userMpAppService = userMpAppService;


        }

        protected override IQueryable<MpEventRequestMsgLog> CreateFilteredQuery(GetMpEventRequestMsgLogsInput input)
        {
          
            return Repository.GetAll()
                .Where(c => c.MpID == input.MpID);
                //.WhereIf(!string.IsNullOrWhiteSpace(input.Name), c => c.Name.Contains(input.Name))
                //.WhereIf(!string.IsNullOrWhiteSpace(input.PushActivityName), c => c.PushActivityName.Contains(input.PushActivityName));
        }

     
        public override Task<MpEventRequestMsgLogDto> Get(EntityDto<int> input)
        {
            return base.Get(input);
        }

        public override async Task<MpEventRequestMsgLogDto> Create(MpEventRequestMsgLogDto input)
        {
           
            return await base.Create(input);
        }
        public async Task<FileDto> GetListToExcel(GetMpEventRequestMsgLogsInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _mpEventRequestMsgLogListExcelExporter.ExportToFile(dtos);
        }

        public async Task<MpEventRequestMsgLogDto> GetFirstOrDefault()
        {
            return MapToEntityDto(await Repository.FirstOrDefaultAsync(c => 1 == 1));
        }

       


    }
}
