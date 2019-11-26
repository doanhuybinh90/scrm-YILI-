using Abp.Extensions;
using Abp.Runtime.Validation;
using Pb.Wechat.Dto;
using Pb.Wechat.MpEvents.Dto;

namespace Pb.Wechat.MpEventRequestMsgLogs.Dto
{
    public class GetMpEventRequestMsgLogsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public int MpID { get; set; }
        public MpMessageType? MsgType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
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
