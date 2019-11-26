using Abp.Extensions;
using Abp.Runtime.Validation;
using Pb.Wechat.Dto;

namespace Pb.Wechat.CustomerServiceOnlines.Dto
{
    public class GetCustomerServiceOnlinesInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public int MpID { get; set; }
        public string KfNick { get; set; }
        public string KfId { get; set; }
        public string PublicNumberAccount { get; set; }
        public string KfWx { get; set; }
        public int? OnlineState { get; set; }
        public int? ConnectState { get; set; }
        public string OpenID { get; set; }
        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "CreationTime DESC";
            }
        }
    }
}
