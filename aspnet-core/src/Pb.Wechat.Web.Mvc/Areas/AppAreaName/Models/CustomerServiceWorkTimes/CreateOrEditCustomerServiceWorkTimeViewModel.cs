using Abp.AutoMapper;
using Pb.Wechat.CustomerServiceWorkTimes.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.CustomerServiceWorkTimes
{
    [AutoMapFrom(typeof(CustomerServiceWorkTimeDto))]
    public class CreateOrEditCustomerServiceWorkTimeViewModel : CustomerServiceWorkTimeDto
    {
        public bool IsEditMode => Id!=0;
        public CreateOrEditCustomerServiceWorkTimeViewModel(CustomerServiceWorkTimeDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditCustomerServiceWorkTimeViewModel() {
        }
    }
}
