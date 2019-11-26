using Abp.Extensions;
using Abp.Runtime.Validation;
using Pb.Wechat.Dto;

namespace Pb.Wechat.MpShoppingMallPics.Dto
{
    public class GetMpShoppingMallPicsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public int MpID { get; set; }
        public string Title { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "CreationTime DESC";
            }
        }
    }
}
