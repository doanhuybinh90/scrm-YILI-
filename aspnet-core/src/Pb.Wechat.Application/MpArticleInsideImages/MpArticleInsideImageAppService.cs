using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Pb.Wechat.Auditing.Exporting;
using Pb.Wechat.Dto;
using Pb.Wechat.MpArticleInsideImages.Dto;
using Pb.Wechat.UserMps;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.MpArticleInsideImages
{
    //[AbpAuthorize(AppPermissions.Pages_MpArticleInsideImages)]
    public class MpArticleInsideImageAppService : AsyncCrudAppService<MpArticleInsideImage, MpArticleInsideImageDto, int, GetMpArticleInsideImagesInput, MpArticleInsideImageDto, MpArticleInsideImageDto>, IMpArticleInsideImageAppService
    {

        private readonly IUserMpAppService _userMpAppService;
        private readonly IMpArticleInsideImageListExcelExporter _MpArticleInsideImageListExcelExporter;
        public MpArticleInsideImageAppService(IRepository<MpArticleInsideImage, int> repository, IMpArticleInsideImageListExcelExporter MpArticleInsideImageListExcelExporter, IUserMpAppService userMpAppService) : base(repository)
        {
            _MpArticleInsideImageListExcelExporter = MpArticleInsideImageListExcelExporter;
            _userMpAppService = userMpAppService;


        }

        protected override IQueryable<MpArticleInsideImage> CreateFilteredQuery(GetMpArticleInsideImagesInput input)
        {
           
            return Repository.GetAll()
                .Where(c => c.MpID == input.MpID)
                .WhereIf(!string.IsNullOrWhiteSpace(input.LocalImageUrl), c => c.LocalImageUrl.Contains(input.LocalImageUrl))
                .WhereIf(!string.IsNullOrWhiteSpace(input.WxImageUrl), c => c.WxImageUrl.Contains(input.WxImageUrl));
        }

     
        public override Task<MpArticleInsideImageDto> Get(EntityDto<int> input)
        {
            return base.Get(input);
        }

 
        public async Task<FileDto> GetListToExcel(GetMpArticleInsideImagesInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _MpArticleInsideImageListExcelExporter.ExportToFile(dtos);
        }

        public async Task<MpArticleInsideImageDto> GetFirstOrDefault(GetMpArticleInsideImagesInput input)
        {
            var data = await AsyncQueryableExecuter.FirstOrDefaultAsync(CreateFilteredQuery(input));
            return MapToEntityDto(data);
        }
        public async Task<List<MpArticleInsideImageDto>> GetList(GetMpArticleInsideImagesInput input)
        {
            return (await AsyncQueryableExecuter.ToListAsync(CreateFilteredQuery(input))).Select(MapToEntityDto).ToList();
        }
       


    }
}
