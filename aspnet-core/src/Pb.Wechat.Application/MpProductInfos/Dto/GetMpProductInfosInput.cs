using Abp.Extensions;
using Abp.Runtime.Validation;
using Pb.Wechat.Dto;

namespace Pb.Wechat.MpProductInfos.Dto
{
    public class GetMpProductInfosInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string ProductIntroduce { get; set; }
        public string TypeTitle { get; set; }
        public int? TypeID { get; set; }
        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "SortIndex Asc";
            }
        }
    }
}
