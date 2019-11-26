using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Pb.Wechat.Dto;
using Pb.Wechat.CustomerMediaVideos.Dto;
using Pb.Wechat.CustomerMediaVideos.Exporting;
using Pb.Wechat.UserMps;
using Pb.Wechat.WxMedias;
using System.Linq;
using System.Threading.Tasks;
using Pb.Wechat.CustomerServiceResponseTexts;
using System;
using Pb.Wechat.CustomerServiceConversations.Dto;
using Pb.Wechat.CustomerServiceResponseTexts.Dto;

namespace Pb.Wechat.CustomerMediaVideos
{
    //[AbpAuthorize(AppPermissions.Pages_MpEvents)]
    public class CustomerMediaVideoAppService : AsyncCrudAppService<CustomerMediaVideo, CustomerMediaVideoDto, int, GetCustomerMediaVideosInput, CustomerMediaVideoDto, CustomerMediaVideoDto>, ICustomerMediaVideoAppService
    {
        private readonly ICustomerMediaVideoListExcelExporter _rewardListExcelExporter;
        private readonly IUserMpAppService _userMpAppService;
        private readonly IWxMediaAppService _wxMediaAppService;
        private readonly IRepository<CustomerServiceResponseText, int> _cusRepository;
        public CustomerMediaVideoAppService(IRepository<CustomerMediaVideo, int> repository, ICustomerMediaVideoListExcelExporter rewardListExcelExporter, IUserMpAppService userMpAppService, IAppFolders appFolders, IWxMediaAppService wxMediaAppService, IRepository<CustomerServiceResponseText, int> cusRepository) : base(repository)
        {
            _rewardListExcelExporter = rewardListExcelExporter;
            _userMpAppService = userMpAppService;
            _wxMediaAppService = wxMediaAppService;
            _cusRepository = cusRepository;
        }

        protected override IQueryable<CustomerMediaVideo> CreateFilteredQuery(GetCustomerMediaVideosInput input)
        {
            return Repository.GetAll()
                 .Where(c => c.MpID == input.MpID)
                .WhereIf(!input.MediaID.IsNullOrWhiteSpace(), c => c.MediaID.Contains(input.MediaID))
                 .WhereIf(!input.Title.IsNullOrWhiteSpace(), c => c.Title.Contains(input.Title))
                  .WhereIf(!input.Description.IsNullOrWhiteSpace(), c => c.Description.Contains(input.Description));
        }
        public async Task<FileDto> GetListToExcel(GetCustomerMediaVideosInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _rewardListExcelExporter.ExportToFile(dtos);
        }


        public async Task<CustomerMediaVideoDto> GetModelByReplyTypeAsync(string mediaId, int mpId)
        {
            CheckGetPermission();
            //有错误
            var entity = await AsyncQueryableExecuter.FirstOrDefaultAsync(Repository.GetAll()
                .Where(m => m.IsDeleted == false)
                .WhereIf(!string.IsNullOrWhiteSpace(mediaId), c => c.MediaID == mediaId)
                .WhereIf(mpId != 0, c => c.MpID == mpId));

            return MapToEntityDto(entity);
        }

        public override async Task<CustomerMediaVideoDto> Create(CustomerMediaVideoDto input)
        {
            var result = await base.Create(input);
            await _cusRepository.InsertAsync(new CustomerServiceResponseText
            {
                Title = input.Title,
                Description = input.Description,
                CreationTime = DateTime.Now,
                IsDeleted = false,
                LastModificationTime = DateTime.Now,
                MediaId = input.MediaID,
                MpID = input.MpID,
                PreviewImgUrl = input.FilePathOrUrl,
                MartialId = result.Id,
                ResponseType = ResponseType.common.ToString(),
                ReponseContentType = (int)CustomerServiceMsgType.video,
                TypeId = input.TypeId,
                TypeName = input.TypeName,
                ResponseText = input.Title
            });
            return result;
        }
        public override async Task<CustomerMediaVideoDto> Update(CustomerMediaVideoDto input)
        {
            var result = await base.Update(input);
            var updateModel = await _cusRepository.FirstOrDefaultAsync(m => m.MartialId == result.Id);
            updateModel.Title = input.Title;
            updateModel.Description = input.Description;
            updateModel.LastModificationTime = DateTime.Now;
            updateModel.MediaId = input.MediaID;
            updateModel.PreviewImgUrl = input.FilePathOrUrl;
            updateModel.TypeId = input.TypeId;
            updateModel.TypeName = input.TypeName;
            updateModel.ResponseText = input.Title;
            await _cusRepository.UpdateAsync(updateModel);
           
            return result;
        }


        public override Task Delete(EntityDto<int> input)
        {
            var model = Repository.Get(input.Id);
            if (model != null)
            {
                if (!string.IsNullOrWhiteSpace(model.MediaID))
                    _wxMediaAppService.DelFileFromWx(model.MpID, model.MediaID);
                _cusRepository.Delete(m => m.MartialId == model.Id);
                return base.Delete(input);
            }
            else
                throw new UserFriendlyException("对不起，删除素材失败");

        }

    }
}
