using Abp.Extensions;
using Abp.Runtime.Validation;
using Pb.Wechat.Dto;

namespace Pb.Wechat.CustomerServiceResponseTexts.Dto
{
    public class GetCustomerServiceResponseTextsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public int MpID { get; set; }
        public ResponseType? ResponseType { get; set; }
        public string ResponseText { get; set; }
        public int? ResponseContentType { get; set; }
        public string TitleOrDescription { get; set; }
        public int? TypeId { get; set; }
        public int? NotCommon { get; set; }
        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "CreationTime DESC";
            }
        }
    }
}
