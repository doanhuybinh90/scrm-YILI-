using System.ComponentModel;

namespace Pb.Wechat.MpMenus.Dto
{
    [Description("菜单类型")]
    public enum MpMenuType
    {
        [Description("发送信息")]
        click,
        [Description("跳转到网页")]
        view,
        [Description("转接到客服")]
        service,
        [Description("转接到春雨医生")]
        doctorservice
    }
}
