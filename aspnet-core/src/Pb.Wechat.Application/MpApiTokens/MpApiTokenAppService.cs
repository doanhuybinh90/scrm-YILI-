using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Runtime.Caching;
using Pb.Wechat.Authorization;
using Pb.Wechat.MpAccounts;
using Pb.Wechat.MpApiTokens.Dto;
using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Pb.Wechat.UserMps;

namespace Pb.Wechat.MpApiTokens
{
    public class MpApiTokenAppService : AsyncCrudAppService<MpApiToken, MpApiTokenDto, int, GetMpApiTokensInput, MpApiTokenDto, MpApiTokenDto>, IMpApiTokenAppService
    {
        private readonly ICacheManager _cacheManager;
        IRepository<MpAccount, int> _mpAccountRepository;
        private readonly IUserMpAppService _userMpAppService;
        public MpApiTokenAppService(IRepository<MpApiToken, int> repository, IRepository<MpAccount, int> mpAccountRepository, ICacheManager cacheManager, IUserMpAppService userMpAppService) : base(repository)
        {
            _cacheManager = cacheManager;
            _mpAccountRepository = mpAccountRepository;
            _userMpAppService = userMpAppService;
        }

        protected override IQueryable<MpApiToken> CreateFilteredQuery(GetMpApiTokensInput input)
        {
            var inputtype = !input.TokenType.HasValue ? "" : input.TokenType.Value.ToString();

            return Repository.GetAll()
                .Where(c => c.ParentId == input.ParentId)
                .WhereIf(!string.IsNullOrWhiteSpace(input.TokenName), c => c.Name.Contains(input.TokenName))
                .WhereIf(input.TokenType.HasValue, c => c.ApiType == inputtype);
                //.WhereIf(input.StartTime.HasValue, c => c.EndTime >= input.StartTime.Value);
                //.WhereIf(input.EndTime.HasValue, c => c.StartTime <= input.EndTime.Value);
        }
        [AbpAuthorize(AppPermissions.Pages_MpManagers_MpApiTokens)]
        public override Task<MpApiTokenDto> Create(MpApiTokenDto input)
        {
            input.StartTime = DateTime.Now;
            input.CreationTime = DateTime.Now;
            
            return base.Create(input);
        }
        [AbpAuthorize(AppPermissions.Pages_MpManagers_MpApiTokens)]
        public override Task Delete(EntityDto<int> input)
        {
            return base.Delete(input);
        }
        [AbpAuthorize(AppPermissions.Pages_MpManagers_MpApiTokens)]
        public override Task<MpApiTokenDto> Get(EntityDto<int> input)
        {
            return base.Get(input);
        }
        [AbpAuthorize(AppPermissions.Pages_MpManagers_MpApiTokens)]
        public override Task<PagedResultDto<MpApiTokenDto>> GetAll(GetMpApiTokensInput input)
        {
            return base.GetAll(input);
        }
        [AbpAuthorize(AppPermissions.Pages_MpManagers_MpApiTokens)]
        public override Task<MpApiTokenDto> Update(MpApiTokenDto input)
        {
            input.CreationTime = DateTime.Now;
            return base.Update(input);
        }
        [RemoteService(isEnabled: false)]
        public async Task<MpAccountTokenOutput> GetAccountToken(MpAccountTokenInput input)
        {
            var result = await _cacheManager.GetCache(AppConsts.Cache_MpAccountToken).GetAsync(input.Token, async key =>
              {
                  var now = DateTime.Now;
                  var token = await Repository.FirstOrDefaultAsync(c => c.Token == input.Token && c.ApiType == input.ApiType);
                //&& c.StartTime <= now && c.EndTime >= now
                if (token == null)
                      return null;

                  var account = await _mpAccountRepository.FirstOrDefaultAsync(c => c.Id == token.ParentId);
                  if (account == null)
                      return null;

                  return new MpAccountTokenOutput()
                  {
                      MpId = account.Id,
                      AppId = account.AppId,
                      AppSecret = account.AppSecret,
                      MchID = account.MchID,
                      WxPayAppSecret = account.WxPayAppSecret,
                      CertPhysicalPath = account.CertPhysicalPath,
                      CertPassword = account.CertPassword,
                      Domain = token.Domain,
                      StartTime = token.StartTime,
                      EndTime = token.EndTime
                  };
              });
            return result == null ? null : result as MpAccountTokenOutput;
        }
    }
}
