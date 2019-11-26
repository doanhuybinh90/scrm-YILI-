using Abp.Extensions;
using Abp.Runtime.Validation;
using Pb.Wechat.Dto;

namespace Pb.Wechat.CustomerArticleGroupItems.Dto
{
    public class GetCustomerArticleGroupItemsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public int MpID { get; set; }
        public int GroupID { get; set; }
        public int ArticleID { get; set; }
        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Id DESC";
            }
        }
    }
}
