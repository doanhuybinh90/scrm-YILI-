using Abp.AutoMapper;
using Pb.Wechat.CustomerMediaVideos.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.CustomerMediaVideos
{
    [AutoMapFrom(typeof(CustomerMediaVideoDto))]
    public class CreateOrEditCustomerMediaVideoViewModel : CustomerMediaVideoDto
    {
        public bool IsEditMode => Id > 0;
        public CreateOrEditCustomerMediaVideoViewModel(CustomerMediaVideoDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditCustomerMediaVideoViewModel() {
        }
    }
}
