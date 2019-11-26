using Abp.Extensions;
using Abp.Runtime.Validation;
using Pb.Wechat.Dto;

namespace Pb.Wechat.MpFansTagItems.Dto
{
    public class GetMpFansTagItemsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Keyword { get; set; }
        public int? MpId { get; set; }
        public int? FansId { get; set; }

        public long[] TagIds { get; set; }
        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Id ASC";
            }
        }
    }
}
