using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Runtime.Caching;
using Pb.Wechat.Auditing.Exporting;
using Pb.Wechat.Dto;
using Pb.Wechat.MpAccounts.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.MpAccounts
{
    //[AbpAuthorize(AppPermissions.Pages_MpAccounts)]
    public class MpAccountAppService : AsyncCrudAppService<MpAccount, MpAccountDto, int, GetMpAccountsInput, MpAccountDto, MpAccountDto>, IMpAccountAppService
    {
        private readonly ICacheManager _cacheManager;
        private readonly IMpAccountListExcelExporter _mpAccountListExcelExporter;
        public MpAccountAppService(IRepository<MpAccount, int> repository, IMpAccountListExcelExporter mpAccountListExcelExporter, ICacheManager cacheManager) : base(repository)
        {
            _mpAccountListExcelExporter = mpAccountListExcelExporter;
            _cacheManager = cacheManager;
        }

        protected override IQueryable<MpAccount> CreateFilteredQuery(GetMpAccountsInput input)
        {
            var inputtype = input.AccountType == null ? "" : input.AccountType.ToString();
            return Repository.GetAll().WhereIf(!string.IsNullOrWhiteSpace(input.AccountName), c => c.Name.Contains(input.AccountName))
                .WhereIf(input.AccountType != null, c => c.AccountType == inputtype)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Remark), c => c.Remark.Contains(input.Remark))
                .WhereIf(!string.IsNullOrWhiteSpace(input.AppId), c => c.AppId.Contains(input.AppId));
        }

        //[AbpAllowAnonymous]
        public override Task<MpAccountDto> Get(EntityDto<int> input)
        {
            return base.Get(input);
        }


        public Task<MpAccountDto> GetCache(int id)
        {
            return _cacheManager.GetCache(AppConsts.Cache_MpAccount).GetAsync(id.ToString(), t => {
                var result = this.Get(new EntityDto<int>() { Id = id });
                return result;
            });
        }

        public override Task<MpAccountDto> Create(MpAccountDto input)
        {
            return base.Create(input);
        }
        public async Task<FileDto> GetListToExcel(GetMpAccountsInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _mpAccountListExcelExporter.ExportToFile(dtos);
        }

        public async Task<MpAccountDto> GetFirstOrDefault()
        {
            return MapToEntityDto(await Repository.FirstOrDefaultAsync(c => 1 == 1));
        }

        public async Task<List<MpAccountDto>> GetList()
        {
            return (await AsyncQueryableExecuter.ToListAsync(Repository.GetAll().Where(m => m.IsDeleted == false)))
                .Select(MapToEntityDto).ToList();
        }
    }
}
