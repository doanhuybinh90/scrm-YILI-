using Abp.AutoMapper;
using Pb.Wechat.CustomerArticles.Dto;

namespace Pb.Wechat.Web.Areas.AppAreaName.Models.CustomerArticles
{
    [AutoMapFrom(typeof(CustomerArticleDto))]
    public class CreateOrEditCustomerArticleViewModel : CustomerArticleDto
    {
        public bool IsEditMode => Id > 0;
        public CreateOrEditCustomerArticleViewModel(CustomerArticleDto output)
        {
            output.MapTo(this);
        }
        public CreateOrEditCustomerArticleViewModel() {
        }
    }
}
