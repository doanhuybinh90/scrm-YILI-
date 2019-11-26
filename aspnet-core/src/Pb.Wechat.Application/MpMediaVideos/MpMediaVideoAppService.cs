using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Pb.Wechat.Dto;
using Pb.Wechat.MpMediaVideos.Dto;
using Pb.Wechat.MpMediaVideos.Exporting;
using Pb.Wechat.UserMps;
using Pb.Wechat.WxMedias;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.MpMediaVideos
{
    //[AbpAuthorize(AppPermissions.Pages_MpEvents)]
    public class MpMediaVideoAppService : AsyncCrudAppService<MpMediaVideo, MpMediaVideoDto, int, GetMpMediaVideosInput, MpMediaVideoDto, MpMediaVideoDto>, IMpMediaVideoAppService
    {
        private readonly IMpMediaVideoListExcelExporter _rewardListExcelExporter;
        private readonly IUserMpAppService _userMpAppService;
        private readonly IWxMediaAppService _wxMediaAppService;
        public MpMediaVideoAppService(IRepository<MpMediaVideo, int> repository, IMpMediaVideoListExcelExporter rewardListExcelExporter, IUserMpAppService userMpAppService, IAppFolders appFolders, IWxMediaAppService wxMediaAppService) : base(repository)
        {
            _rewardListExcelExporter = rewardListExcelExporter;
            _userMpAppService = userMpAppService;
            _wxMediaAppService = wxMediaAppService;
        }

        protected override IQueryable<MpMediaVideo> CreateFilteredQuery(GetMpMediaVideosInput input)
        {
            return Repository.GetAll()
                 .Where(c => c.MpID == input.MpID)
                .WhereIf(!input.MediaID.IsNullOrWhiteSpace(), c => c.MediaID.Contains(input.MediaID))
                 .WhereIf(!input.Title.IsNullOrWhiteSpace(), c => c.Title.Contains(input.Title))
                  .WhereIf(!input.Description.IsNullOrWhiteSpace(), c => c.Description.Contains(input.Description))
                   .WhereIf(!input.FileID.IsNullOrWhiteSpace(), c => c.FileID.Contains(input.FileID));
        }
        public async Task<FileDto> GetListToExcel(GetMpMediaVideosInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _rewardListExcelExporter.ExportToFile(dtos);
        }

        //public override async Task<MpMediaVideoDto> Create(MpMediaVideoDto input)
        //{
        //    input.MpID = await _userMpAppService.GetDefaultMpId();
        //    if (string.IsNullOrWhiteSpace(input.MediaID))
        //        input.MediaID = await _wxMediaAppService.UploadVideo(input.FileID, input.MediaID, input.Title, input.Description);
        //    input.LastModificationTime = DateTime.Now;
        //    return await base.Create(input);
        //}
        public async Task<MpMediaVideoDto> GetModelByReplyTypeAsync(string mediaId, int mpId)
        {
            CheckGetPermission();
            //有错误
            var entity = await AsyncQueryableExecuter.FirstOrDefaultAsync(Repository.GetAll()
                .Where(m => m.IsDeleted == false)
                .WhereIf(!string.IsNullOrWhiteSpace(mediaId), c => c.MediaID == mediaId)
                .WhereIf(mpId != 0, c => c.MpID == mpId));

            return MapToEntityDto(entity);
        }

        public override Task Delete(EntityDto<int> input)
        {
            var model = Repository.Get(input.Id);
            if (model != null)
            {
                if (!string.IsNullOrWhiteSpace(model.MediaID))
                    _wxMediaAppService.DelFileFromWx(model.MpID, model.MediaID);
                return base.Delete(input);
            }
            else
                throw new UserFriendlyException("对不起，删除素材失败");

        }

    }
}
