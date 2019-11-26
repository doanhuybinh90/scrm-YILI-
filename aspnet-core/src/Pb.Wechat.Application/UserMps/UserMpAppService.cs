using Abp.Application.Services;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Pb.Wechat.Dto;
using Pb.Wechat.UserMps.Dto;
using Pb.Wechat.UserMps.Exporting;
using System.Linq;
using System.Threading.Tasks;
using Abp.Runtime.Session;
using Pb.Wechat.MpAccounts;
using Abp.Runtime.Caching;

namespace Pb.Wechat.UserMps
{
    public class UserMpAppService : AsyncCrudAppService<UserMp, UserMpDto, int, GetUserMpsInput, UserMpDto, UserMpDto>, IUserMpAppService
    {
        private readonly IUserMpListExcelExporter _userMpListExcelExporter;
        private readonly IAbpSession _abpSession;
        private readonly IMpAccountAppService _mpAccountAppService;
        private readonly ICacheManager _cacheManager;
        public UserMpAppService(IRepository<UserMp, int> repository, IUserMpListExcelExporter userMpListExcelExporter, IAbpSession abpSession, IMpAccountAppService mpAccountAppService, ICacheManager cacheManager) : base(repository)
        {
            _userMpListExcelExporter = userMpListExcelExporter;
            _abpSession = abpSession;
            _mpAccountAppService = mpAccountAppService;
            _cacheManager = cacheManager;
        }

        protected override IQueryable<UserMp> CreateFilteredQuery(GetUserMpsInput input)
        {
            return Repository.GetAll()
                .WhereIf(input.UserId != 0, c => c.UserId == input.UserId)
                 .WhereIf(input.CurrentMpID != 0, c => c.CurrentMpID == input.CurrentMpID);
        }
        public async Task<FileDto> GetListToExcel(GetUserMpsInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _userMpListExcelExporter.ExportToFile(dtos);
        }

        public Task<int> GetDefaultMpId()
        {
            var currUserId = _abpSession.UserId;
            if (currUserId != null)
            {

                return _cacheManager.GetCache(AppConsts.Cache_CurrentUserMp).GetAsync(currUserId, async () =>
            {
                int i = 0;

                var model = await Repository.FirstOrDefaultAsync(c => c.UserId == currUserId);
                if (model == null)
                {
                    var account = _mpAccountAppService.GetFirstOrDefault();
                    var resultModel = new UserMpDto { UserId = currUserId, CurrentMpID = account.Id };
                    var result = await base.Create(resultModel);
                    i = result.CurrentMpID;
                }
                else
                    i = model.CurrentMpID;


                return i;
            });
            }
            else
                return null;
        }


    }

}
