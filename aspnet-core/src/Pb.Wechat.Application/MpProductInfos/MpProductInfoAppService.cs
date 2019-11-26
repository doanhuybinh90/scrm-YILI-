using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Pb.Wechat.Auditing.Exporting;
using Pb.Wechat.Dto;
using Pb.Wechat.MpProductInfos.Dto;
using Pb.Wechat.MpProductTypes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.MpProductInfos
{
    //[AbpAuthorize(AppPermissions.Pages_MpProductInfos)]
    public class MpProductInfoAppService : AsyncCrudAppService<MpProductInfo, MpProductInfoDto, int, GetMpProductInfosInput, MpProductInfoDto, MpProductInfoDto>, IMpProductInfoAppService
    {
        private readonly IMpProductInfoListExcelExporter _MpProductInfoListExcelExporter;
        private readonly IRepository<MpProductType, int> _mpProductTypeRepository;

        public MpProductInfoAppService(IRepository<MpProductInfo, int> repository, IRepository<MpProductType, int> mpProductTypeRepository, IMpProductInfoListExcelExporter MpProductInfoListExcelExporter) : base(repository)
        {
            _MpProductInfoListExcelExporter = MpProductInfoListExcelExporter;
            _mpProductTypeRepository = mpProductTypeRepository;
        }
        public override Task<PagedResultDto<MpProductInfoDto>> GetAll(GetMpProductInfosInput input)
        {
            return base.GetAll(input);
        }
        protected override IQueryable<MpProductInfo> CreateFilteredQuery(GetMpProductInfosInput input)
        {
            return Repository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Title), c => c.Title.Contains(input.Title))
                 .WhereIf(!string.IsNullOrWhiteSpace(input.ProductIntroduce), c => c.ProductIntroduce.Contains(input.ProductIntroduce))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SubTitle), c => c.SubTitle.Contains(input.SubTitle))
                .WhereIf(input.TypeID.HasValue, c => c.TypeId == input.TypeID)
                .WhereIf(!string.IsNullOrWhiteSpace(input.TypeTitle), c => c.TypeTitle.Contains(input.TypeTitle));
        }

        public async Task<FileDto> GetListToExcel(GetMpProductInfosInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _MpProductInfoListExcelExporter.ExportToFile(dtos);
        }

        public async Task<MpProductInfoDto> GetFirstOrDefaultByInput(GetMpProductInfosInput input)
        {
            return MapToEntityDto(await AsyncQueryableExecuter.FirstOrDefaultAsync(CreateFilteredQuery(input)));
        }

        public async Task<List<MpProductListDto>> GetList()
        {
            var typeList = await AsyncQueryableExecuter.ToListAsync(_mpProductTypeRepository.GetAll().Where(c => c.IsDeleted == false).OrderBy(c => c.SortIndex));
            var productList= await AsyncQueryableExecuter.ToListAsync(Repository.GetAll().Where(c => c.IsDeleted == false).OrderBy(c => c.SortIndex));
            var result = new List<MpProductListDto>();
            foreach (var t in typeList) {
                var tp = new MpProductListDto();
                tp.TypeTitle = t.Title;
                tp.FilePathOrUrl = t.FilePathOrUrl;
                //tp.TypeSubTitle = t.SubTitle;
                tp.ProductList = productList.Where(c => c.TypeId == t.Id).Select(MapToEntityDto).ToList();
                result.Add(tp);
            }
            return result;
        }
    }
}
