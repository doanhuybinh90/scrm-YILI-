using Abp.AutoMapper;
using Pb.Wechat.MpMediaVoices.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.MpMediaVoices
{
    [AutoMapFrom(typeof(MpMediaVoiceDto))]
    public class CreateOrEditMpMediaVoiceViewModel : MpMediaVoiceDto
    {
        public bool IsEditMode => Id > 0;
        public CreateOrEditMpMediaVoiceViewModel(MpMediaVoiceDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditMpMediaVoiceViewModel() {
        }
    }
}
