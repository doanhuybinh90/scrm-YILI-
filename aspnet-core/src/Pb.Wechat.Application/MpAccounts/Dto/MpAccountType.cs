using System.ComponentModel;

namespace Pb.Wechat.MpAccounts.Dto
{
    /// <summary>
    /// 公众号类型
    /// </summary>
    [Description("公众号类型")]
    public enum MpAccountType
    {
        [Description("未认证订阅号")]
        UnauthedRead,
        [Description("认证订阅号")]
        AuthedRead,
        [Description("未认证服务号")]
        UnauthedServer,
        [Description("认证服务号")]
        AuthedServer,
    }
}
