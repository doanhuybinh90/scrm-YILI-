using Abp.Extensions;
using Abp.Runtime.Validation;
using Pb.Wechat.Dto;
using Pb.Wechat.MpEvents.Dto;

namespace Pb.Wechat.MpMessages.Dto
{
    public class GetMpMessagesInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public int MpID { get; set; }
        public MpMessageType? MessageTypeX { get; set; }
        public string WxMsgID { get; set; }
        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "CreationTime DESC";
            }
        }
    }
}
