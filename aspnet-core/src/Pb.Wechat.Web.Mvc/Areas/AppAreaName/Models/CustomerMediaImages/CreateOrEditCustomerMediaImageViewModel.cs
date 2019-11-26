using Abp.AutoMapper;
using Pb.Wechat.CustomerMediaImages.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.CustomerMediaImages
{
    [AutoMapFrom(typeof(CustomerMediaImageDto))]
    public class CreateOrEditCustomerMediaImageViewModel : CustomerMediaImageDto
    {
        public bool IsEditMode => Id > 0;
        public CreateOrEditCustomerMediaImageViewModel(CustomerMediaImageDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditCustomerMediaImageViewModel() {
        }
    }
}
