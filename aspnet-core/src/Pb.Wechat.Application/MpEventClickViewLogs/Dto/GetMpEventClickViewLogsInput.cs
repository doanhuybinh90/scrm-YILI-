using Abp.Extensions;
using Abp.Runtime.Validation;
using Pb.Wechat.Dto;

namespace Pb.Wechat.MpEventClickViewLogs.Dto
{
    public class GetMpEventClickViewLogsInput : PagedAndSortedInputDto, IShouldNormalize
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
