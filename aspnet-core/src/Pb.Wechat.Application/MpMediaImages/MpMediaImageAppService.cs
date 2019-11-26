using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Pb.Wechat.Dto;
using Pb.Wechat.MpMediaImages.Dto;
using Pb.Wechat.MpMediaImages.Exporting;
using Pb.Wechat.UserMps;
using Pb.Wechat.WxMedias;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.MpMediaImages
{
    public class MpMediaImageAppService : AsyncCrudAppService<MpMediaImage, MpMediaImageDto, int, GetMpMediaImagesInput, MpMediaImageDto, MpMediaImageDto>, IMpMediaImageAppService
    {
        private readonly IMpMediaImageListExcelExporter _MpMediaImageListExcelExporter;
        private readonly IUserMpAppService _userMpAppService;
        private readonly IWxMediaAppService _wxMediaAppService;
        public MpMediaImageAppService(IRepository<MpMediaImage, int> repository, IMpMediaImageListExcelExporter MpMediaImageListExcelExporter, IUserMpAppService userMpAppService, IWxMediaAppService wxMediaAppService) : base(repository)
        {
            _MpMediaImageListExcelExporter = MpMediaImageListExcelExporter;
            _userMpAppService = userMpAppService;
            _wxMediaAppService = wxMediaAppService;
        }

        protected override IQueryable<MpMediaImage> CreateFilteredQuery(GetMpMediaImagesInput input)
        {

            return Repository.GetAll()
                .Where(c => c.MpID == input.MpID)
                .WhereIf(!input.MediaID.IsNullOrWhiteSpace(), c => c.MediaID.Contains(input.MediaID))
                .WhereIf(!input.Title.IsNullOrWhiteSpace(), c => c.Name.Contains(input.Title))
                .WhereIf(!input.FileID.IsNullOrWhiteSpace(), c => c.FileID.Contains(input.FileID))
                .WhereIf(input.CreationStartTime != null, c => c.CreationTime >= input.CreationStartTime)
                .WhereIf(input.CreationEndTime != null, c => c.CreationTime <= input.CreationEndTime)
                .WhereIf(input.MediaImageType!=null && input.MediaImageType!=0,c=>c.MediaImageType==input.MediaImageType)
                .WhereIf(!string.IsNullOrWhiteSpace(input.MediaImageTypeName),c=>c.MediaImageTypeName.Contains(input.MediaImageTypeName));
        }
        
        public override Task Delete(EntityDto<int> input)
        {
            var model = Repository.Get(input.Id);
            if (model != null)
            {
                if (!string.IsNullOrWhiteSpace(model.MediaID))
                {
                    _wxMediaAppService.DelFileFromWx(model.MpID, model.MediaID);

                }
                return base.Delete(input);
            }

            throw new UserFriendlyException("对不起，删除素材失败");
        }

       
        public async Task<FileDto> GetListToExcel(GetMpMediaImagesInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _MpMediaImageListExcelExporter.ExportToFile(dtos);
        }

        public async Task<MpMediaImageDto> GetByFileID(string fileID)
        {
            var data = await AsyncQueryableExecuter.FirstOrDefaultAsync(Repository.GetAll().Where(m => m.FileID == fileID && m.IsDeleted == false));
            return MapToEntityDto(data);
        }

        public async Task<MpMediaImageDto> GetByMediaID(string mediaID)
        {

            var data = await AsyncQueryableExecuter.FirstOrDefaultAsync(Repository.GetAll().Where(m => m.MediaID == mediaID && m.IsDeleted == false));
            return MapToEntityDto(data);
        }


    }
}
