using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Pb.Wechat.Dto;
using Pb.Wechat.CustomerMediaImages.Dto;
using Pb.Wechat.CustomerMediaImages.Exporting;
using Pb.Wechat.UserMps;
using Pb.Wechat.WxMedias;
using System.Linq;
using System.Threading.Tasks;
using Pb.Wechat.CustomerServiceResponseTexts;
using System;
using Pb.Wechat.CustomerServiceResponseTexts.Dto;
using Pb.Wechat.CustomerServiceConversations.Dto;

namespace Pb.Wechat.CustomerMediaImages
{
    public class CustomerMediaImageAppService : AsyncCrudAppService<CustomerMediaImage, CustomerMediaImageDto, int, GetCustomerMediaImagesInput, CustomerMediaImageDto, CustomerMediaImageDto>, ICustomerMediaImageAppService
    {
        private readonly ICustomerMediaImageListExcelExporter _CustomerMediaImageListExcelExporter;
        private readonly IUserMpAppService _userMpAppService;
        private readonly IWxMediaAppService _wxMediaAppService;
        private readonly IRepository<CustomerServiceResponseText, int> _cusRepository;
        public CustomerMediaImageAppService(IRepository<CustomerMediaImage, int> repository, ICustomerMediaImageListExcelExporter CustomerMediaImageListExcelExporter, IUserMpAppService userMpAppService, IWxMediaAppService wxMediaAppService, IRepository<CustomerServiceResponseText, int> cusRepository) : base(repository)
        {
            _CustomerMediaImageListExcelExporter = CustomerMediaImageListExcelExporter;
            _userMpAppService = userMpAppService;
            _wxMediaAppService = wxMediaAppService;
            _cusRepository = cusRepository;
        }

        protected override IQueryable<CustomerMediaImage> CreateFilteredQuery(GetCustomerMediaImagesInput input)
        {

            return Repository.GetAll()
                .WhereIf(input.MpID != 0, c => c.MpID == input.MpID)
                .WhereIf(!input.MediaID.IsNullOrWhiteSpace(), c => c.MediaID.Contains(input.MediaID))
                .WhereIf(!input.Title.IsNullOrWhiteSpace(), c => c.Name.Contains(input.Title))
                .WhereIf(input.CreationStartTime != null, c => c.CreationTime >= input.CreationStartTime)
                .WhereIf(input.CreationEndTime != null, c => c.CreationTime <= input.CreationEndTime);
        }

        public async Task<FileDto> GetListToExcel(GetCustomerMediaImagesInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _CustomerMediaImageListExcelExporter.ExportToFile(dtos);
        }

        public async Task<CustomerMediaImageDto> GetByMediaID(string mediaID)
        {

            var data = await AsyncQueryableExecuter.FirstOrDefaultAsync(Repository.GetAll().Where(m => m.MediaID == mediaID && m.IsDeleted == false));
            return MapToEntityDto(data);
        }

        public override async Task<CustomerMediaImageDto> Create(CustomerMediaImageDto input)
        {
            var result = await base.Create(input);
            await _cusRepository.InsertAsync(new CustomerServiceResponseText
            {
                ImageName = input.Name,
                CreationTime = DateTime.Now,
                IsDeleted = false,
                LastModificationTime = DateTime.Now,
                MediaId = input.MediaID,
                MpID = input.MpID,
                PreviewImgUrl = input.FilePathOrUrl,
                MartialId = result.Id,
                ResponseType = ResponseType.common.ToString(),
                ReponseContentType = (int)CustomerServiceMsgType.image,
                TypeId = input.TypeId,
                TypeName = input.TypeName,
                ResponseText = input.Name
            });
            return result;
        }
        public override async Task<CustomerMediaImageDto> Update(CustomerMediaImageDto input)
        {
            var result = await base.Update(input);
            var updateModel = await _cusRepository.FirstOrDefaultAsync(m => m.MartialId == result.Id);
            updateModel.Title = input.Name;
            updateModel.ImageName = input.Name;
            updateModel.LastModificationTime = DateTime.Now;
            updateModel.MediaId = input.MediaID;
            updateModel.PreviewImgUrl = input.FilePathOrUrl;
            updateModel.TypeId = input.TypeId;
            updateModel.TypeName = input.TypeName;
            updateModel.ResponseText = input.Name;
            await _cusRepository.UpdateAsync(updateModel);
            return result;
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
                _cusRepository.Delete(m => m.MartialId == model.Id);
                return base.Delete(input);
            }

            throw new UserFriendlyException("对不起，删除素材失败");
        }

    }
}
