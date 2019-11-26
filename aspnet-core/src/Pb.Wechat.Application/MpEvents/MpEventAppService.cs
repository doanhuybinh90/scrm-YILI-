using Abp.Application.Services;
using Abp.Linq.Extensions;
using Abp.Domain.Repositories;
using Pb.Wechat.Dto;
using Pb.Wechat.MpEvents.Dto;
using Pb.Wechat.MpEvents.Exporting;
using System.Linq;
using System.Threading.Tasks;
using Pb.Wechat.UserMps;
using Abp.Runtime.Caching;
using Pb.Wechat.MpAccounts;

namespace Pb.Wechat.MpEvents
{
    //[AbpAuthorize(AppPermissions.Pages_MpEvents)]
    public class MpEventAppService : AsyncCrudAppService<MpEvent, MpEventDto, int, GetMpEventsInput, MpEventDto, MpEventDto>, IMpEventAppService
    {
        private readonly IUserMpAppService _userMpAppService;
        private readonly IMpEventListExcelExporter _mpAccountListExcelExporter;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<MpAccount, int> _mpAccountRepository;
        public MpEventAppService(IRepository<MpEvent, int> repository, IMpEventListExcelExporter mpAccountListExcelExporter, IUserMpAppService userMpAppService, ICacheManager cacheManager, IRepository<MpAccount, int> mpAccountRepository) : base(repository)
        {
            _mpAccountListExcelExporter = mpAccountListExcelExporter;
            _userMpAppService = userMpAppService;
            _cacheManager = cacheManager;
            _mpAccountRepository = mpAccountRepository;
        }


        protected override IQueryable<MpEvent> CreateFilteredQuery(GetMpEventsInput input)
        {
          
            var inputtype = input.EventType == null ? "" : input.EventType.ToString();
            var msgtype = input.ReplyType == null ? "" : input.ReplyType.ToString();
            return Repository.GetAll()
                .Where(c => c.MpID == input.MpID)
                .WhereIf(input.MpID != 0, c => c.MpID == input.MpID)
                .WhereIf(input.EventType != null, c => c.EventType == inputtype)
                .WhereIf(input.ReplyType != null, c => c.ReplyType == msgtype);
        }

        public override async Task<MpEventDto> Update(MpEventDto input)
        {
            var account = await _mpAccountRepository.FirstOrDefaultAsync(m => m.Id == input.MpID);

            await _cacheManager.GetCache(AppConsts.Cache_CallBack).ClearAsync();

            return await base.Update(input);
        }
        public override async Task<MpEventDto> Create(MpEventDto input)
        {
            var account= await _mpAccountRepository.FirstOrDefaultAsync(m => m.Id == input.MpID);
            await _cacheManager.GetCache(AppConsts.Cache_CallBack).ClearAsync();

            var result = Repository.GetAll().FirstOrDefault(m => m.IsDeleted == false && m.MpID == input.MpID && m.EventType == input.EventType);
            if (result == null)
                return await base.Create(input);
            else
            {
                input.Id = result.Id;
                return await base.Update(input);
            }
        }
        public async Task<FileDto> GetListToExcel(GetMpEventsInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _mpAccountListExcelExporter.ExportToFile(dtos);
        }

        public async Task<MpEventDto> GetModelByReplyTypeAsync(string replyType, int mpId)
        {
            CheckGetPermission();

            var entity = await AsyncQueryableExecuter.FirstOrDefaultAsync(Repository.GetAll()
                .Where(m => m.IsDeleted == false)
                .WhereIf(!string.IsNullOrWhiteSpace(replyType), c => c.ReplyType == replyType)
                .WhereIf(mpId != 0, c => c.MpID == mpId));

            return MapToEntityDto(entity);
        }
        public async Task<MpEventDto> GetModelByEventTypeAsync(string eventType, int mpId)
        {
            CheckGetPermission();

            var entity = await AsyncQueryableExecuter.FirstOrDefaultAsync(Repository.GetAll()
                .Where(m => m.IsDeleted == false)
                .WhereIf(!string.IsNullOrWhiteSpace(eventType), c => c.EventType == eventType)
                .WhereIf(mpId != 0, c => c.MpID == mpId));

            return MapToEntityDto(entity);
        }

    }
}
