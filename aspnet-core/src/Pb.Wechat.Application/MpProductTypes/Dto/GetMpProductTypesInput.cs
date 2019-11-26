using Abp.Extensions;
using Abp.Runtime.Validation;
using Pb.Wechat.Dto;

namespace Pb.Wechat.MpProductTypes.Dto
{
    public class GetMpProductTypesInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Keyword { get; set; }
        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "SortIndex ASC";
            }
        }
    }
}
