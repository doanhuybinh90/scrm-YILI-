using System.ComponentModel;

namespace Pb.Wechat.MpChannels.Dto
{
    public enum ChannelEnum
    {
        [Description("微信")]
        wx,
        [Description("网格化活动")]
        wghAc,
        [Description("营养顾问")]
        yygw,
        [Description("网站关注")]
        web
    }
}
