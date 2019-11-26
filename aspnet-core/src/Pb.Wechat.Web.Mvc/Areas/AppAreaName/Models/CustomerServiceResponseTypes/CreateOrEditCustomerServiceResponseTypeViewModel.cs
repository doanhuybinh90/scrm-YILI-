using Abp.AutoMapper;
using Pb.Wechat.CustomerServiceResponseTypes.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.CustomerServiceResponseTypes
{
    [AutoMapFrom(typeof(CustomerServiceResponseTypeDto))]
    public class CreateOrEditCustomerServiceResponseTypeViewModel : CustomerServiceResponseTypeDto
    {
        public bool IsEditMode => Id!=0;
        public CreateOrEditCustomerServiceResponseTypeViewModel(CustomerServiceResponseTypeDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditCustomerServiceResponseTypeViewModel() {
        }
    }
}
