using Abp.Extensions;
using Abp.Runtime.Validation;
using Pb.Wechat.Dto;

namespace Pb.Wechat.CustomerServiceResponseTypes.Dto
{
    public class GetCustomerServiceResponseTypesInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public int? Id { get; set; }
        public string TypeDescription { get; set; }
        
        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "CreationTime DESC";
            }
        }
    }
}
