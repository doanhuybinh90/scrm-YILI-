using Abp.AutoMapper;
using Pb.Wechat.CustomerMediaVoices.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.CustomerMediaVoices
{
    [AutoMapFrom(typeof(CustomerMediaVoiceDto))]
    public class CreateOrEditCustomerMediaVoiceViewModel : CustomerMediaVoiceDto
    {
        public bool IsEditMode => Id > 0;
        public CreateOrEditCustomerMediaVoiceViewModel(CustomerMediaVoiceDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditCustomerMediaVoiceViewModel() {
        }
    }
}
