using Abp.AutoMapper;
using Pb.Wechat.CustomerServiceOnlines.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.CustomerServiceOnlines
{
    [AutoMapFrom(typeof(CustomerServiceOnlineDto))]
    public class CreateOrEditCustomerServiceOnlineViewModel : CustomerServiceOnlineDto
    {
        public bool IsEditMode => Id!=0;
        public CreateOrEditCustomerServiceOnlineViewModel(CustomerServiceOnlineDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditCustomerServiceOnlineViewModel() {
        }
    }
}
