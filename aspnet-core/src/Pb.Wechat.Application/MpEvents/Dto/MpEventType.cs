using System.ComponentModel;

namespace Pb.Wechat.MpEvents.Dto
{
    [Description("事件类型")]
    public enum MpEventType
    {
        [Description("自动回复")]
        AutoReply,
        [Description("主动订阅")]
        Subscribe,
    }
}
