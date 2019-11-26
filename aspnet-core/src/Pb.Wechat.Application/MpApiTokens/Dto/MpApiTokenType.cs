using System.ComponentModel;

namespace Pb.Wechat.MpApiTokens.Dto
{
    /// <summary>
    /// 公众号令牌类型
    /// </summary>
    [Description("公众号令牌类型")]
    public enum MpApiTokenType
    {
        [Description("普通")]
        Nomal,
        [Description("红包")]
        Redpackage,
    }
}
