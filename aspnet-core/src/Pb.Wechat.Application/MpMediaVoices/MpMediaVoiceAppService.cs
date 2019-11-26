using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Pb.Wechat.Dto;
using Pb.Wechat.MpMediaVoices.Dto;
using Pb.Wechat.MpMediaVoices.Exporting;
using Pb.Wechat.UserMps;
using Pb.Wechat.WxMedias;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.MpMediaVoices
{
    public class MpMediaVoiceAppService : AsyncCrudAppService<MpMediaVoice, MpMediaVoiceDto, int, GetMpMediaVoicesInput, MpMediaVoiceDto, MpMediaVoiceDto>, IMpMediaVoiceAppService
    {
        private readonly IMpMediaVoiceListExcelExporter _MpMediaVoiceListExcelExporter;
        private readonly IUserMpAppService _userMpAppService;
        private readonly IWxMediaAppService _wxMediaAppService;
        public MpMediaVoiceAppService(IRepository<MpMediaVoice, int> repository, IMpMediaVoiceListExcelExporter MpMediaVoiceListExcelExporter, IUserMpAppService userMpAppService, IWxMediaAppService wxMediaAppService) : base(repository)
        {
            _MpMediaVoiceListExcelExporter = MpMediaVoiceListExcelExporter;
            _userMpAppService = userMpAppService;
            _wxMediaAppService = wxMediaAppService;
        }

        protected override IQueryable<MpMediaVoice> CreateFilteredQuery(GetMpMediaVoicesInput input)
        {

            return Repository.GetAll()
                .Where(c => c.MpID == input.MpID)
                 .WhereIf(!input.MediaID.IsNullOrWhiteSpace(), c => c.MediaID.Contains(input.MediaID))
                  .WhereIf(!input.Title.IsNullOrWhiteSpace(), c => c.Title.Contains(input.Title))
                   .WhereIf(!input.FileID.IsNullOrWhiteSpace(), c => c.FileID.Contains(input.FileID))
                   .WhereIf(!input.Description.IsNullOrWhiteSpace(), c => c.Description.Contains(input.Description));
        }



        public override async Task<MpMediaVoiceDto> Create(MpMediaVoiceDto input)
        {
            input.MpID = await _userMpAppService.GetDefaultMpId();
            if (string.IsNullOrWhiteSpace(input.MediaID))
                input.MediaID = await _wxMediaAppService.UploadMedia(input.FileID, input.MediaID);
            input.LastModificationTime = DateTime.Now;
            return await base.Create(input);
        }
        public async Task<FileDto> GetListToExcel(GetMpMediaVoicesInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);
            query = ApplySorting(query, input);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToEntityDto).ToList();

            return _MpMediaVoiceListExcelExporter.ExportToFile(dtos);
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
