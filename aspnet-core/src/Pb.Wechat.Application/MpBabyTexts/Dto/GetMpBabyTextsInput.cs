using Abp.Extensions;
using Abp.Runtime.Validation;
using Pb.Wechat.Dto;

namespace Pb.Wechat.MpBabyTexts.Dto
{
    public class GetMpBabyTextsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public int MpID { get; set; }
        public int? BabyAge { get; set; }
        public string BabyText { get; set; }
        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "CreationTime DESC";
            }
        }
    }
}
