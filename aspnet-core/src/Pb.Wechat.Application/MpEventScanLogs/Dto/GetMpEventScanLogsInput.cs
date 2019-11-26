using Abp.Extensions;
using Abp.Runtime.Validation;
using Pb.Wechat.Dto;

namespace Pb.Wechat.MpEventScanLogs.Dto
{
    public class GetMpEventScanLogsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public int MpID { get; set; }
       
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
