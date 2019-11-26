using Abp.AutoMapper;
using Pb.Wechat.CustomerArticleGroups.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.CustomerArticleGroups
{
    [AutoMapFrom(typeof(CustomerArticleGroupDto))]
    public class CreateOrEditCustomerArticleGroupViewModel : CustomerArticleGroupDto
    {
        public bool IsEditMode => Id > 0;
        public CreateOrEditCustomerArticleGroupViewModel(CustomerArticleGroupDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditCustomerArticleGroupViewModel() {
        }
    }
}
