using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Pb.Wechat.Dto;
using Pb.Wechat.CustomerMediaVoices.Dto;
using Pb.Wechat.CustomerMediaVoices.Exporting;
using Pb.Wechat.UserMps;
using Pb.Wechat.WxMedias;
using System;
using System.Linq;
using System.Threading.Tasks;
using Pb.Wechat.CustomerServiceResponseTexts;
using Pb.Wechat.CustomerServiceResponseTexts.Dto;
using Pb.Wechat.CustomerServiceConversations.Dto;

namespace Pb.Wechat.CustomerMediaVoices
{
    public class CustomerMediaVoiceAppService : AsyncCrudAppService<CustomerMediaVoice, CustomerMediaVoiceDto, int, GetCustomerMediaVoicesInput, CustomerMediaVoiceDto, CustomerMediaVoiceDto>, ICustomerMediaVoiceAppService
    {
        private readonly ICustomerMediaVoiceListExcelExporter _CustomerMediaVoiceListExcelExporter;
        private readonly IUserMpAppService _userMpAppService;
        private readonly IWxMediaAppService _wxMediaAppService;
        private readonly IRepository<CustomerServiceResponseText, int> _cusRepository;
        public CustomerMediaVoiceAppService(IRepository<CustomerMediaVoice, int> repository, ICustomerMediaVoiceListExcelExporter CustomerMediaVoiceListExcelExporter, IUserMpAppService userMpAppService, IWxMediaAppService wxMediaAppService, IRepository<CustomerServiceResponseText, int> cusRepository) : base(repository)
        {
            _CustomerMediaVoiceListExcelExporter = CustomerMediaVoiceListExcelExporter;
            _userMpAppService = userMpAppService;
            _wxMediaAppService = wxMediaAppService;
            _cusRepository = cusRepository;
        }

        protected override IQueryable<CustomerMediaVoice> CreateFilteredQuery(GetCustomerMediaVoicesInput input)
        {

            return Repository.GetAll()
                .Where(c => c.MpID == input.MpID)
                 .WhereIf(!input.MediaID.IsNullOrWhiteSpace(), c => c.MediaID.Contains(input.MediaID))
                  .WhereIf(!input.Title.IsNullOrWhiteSpace(), c => c.Title.Contains(input.Title))
                   .WhereIf(!input.Description.IsNullOrWhiteSpace(), c => c.Description.Contains(input.Description));
        }



      
        public async Task<FileDto> GetListToExcel(GetCustomerMediaVoicesInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _CustomerMediaVoiceListExcelExporter.ExportToFile(dtos);
        }
       



        public override async Task<CustomerMediaVoiceDto> Create(CustomerMediaVoiceDto input)
        {
            var result = await base.Create(input);
            await _cusRepository.InsertAsync(new CustomerServiceResponseText
            {
                Title = input.Title,
                Description=input.Description,
                CreationTime = DateTime.Now,
                IsDeleted = false,
                LastModificationTime = DateTime.Now,
                MediaId = input.MediaID,
                MpID = input.MpID,
                PreviewImgUrl = input.FilePathOrUrl,
                MartialId = result.Id,
                ResponseType = ResponseType.common.ToString(),
                ReponseContentType = (int)CustomerServiceMsgType.voice,
                TypeId = input.TypeId,
                TypeName = input.TypeName,
                ResponseText = input.Title,
                VoiceName = input.Title
            });
            return result;
        }
        public override async Task<CustomerMediaVoiceDto> Update(CustomerMediaVoiceDto input)
        {
            var result = await base.Update(input);
            var updateModel =await _cusRepository.FirstOrDefaultAsync(m => m.MartialId == result.Id);
            updateModel.Title = input.Title;
            updateModel.VoiceName = input.Title;
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
