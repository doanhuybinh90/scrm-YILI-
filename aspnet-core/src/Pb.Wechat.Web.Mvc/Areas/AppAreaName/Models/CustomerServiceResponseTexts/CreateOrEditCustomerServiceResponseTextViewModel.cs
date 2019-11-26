using Abp.AutoMapper;
using Pb.Wechat.CustomerServiceResponseTexts.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.CustomerServiceResponseTexts
{
    [AutoMapFrom(typeof(CustomerServiceResponseTextDto))]
    public class CreateOrEditCustomerServiceResponseTextViewModel : CustomerServiceResponseTextDto
    {
        public bool IsEditMode => Id!=0;
        public CreateOrEditCustomerServiceResponseTextViewModel(CustomerServiceResponseTextDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditCustomerServiceResponseTextViewModel() {
        }
    }
}
