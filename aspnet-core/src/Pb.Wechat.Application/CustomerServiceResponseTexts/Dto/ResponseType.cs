using System.ComponentModel;

namespace Pb.Wechat.CustomerServiceResponseTexts.Dto
{
    [Description("自动回复类型")]
    public enum ResponseType
    {
        [Description("在线回复")]
        working,
        [Description("下班回复")]
        unwork,
        [Description("通用回复")]
        common,
        [Description("待接入回复")]
        wait
    }

    [Description("自定义自动回复")]
    public enum ResponseTypeEx
    {
        [Description("在线回复")]
        working,
        [Description("下班回复")]
        unwork,
        [Description("待接入回复")]
        wait
    }
}
