using System.ComponentModel;

namespace Pb.Wechat.MpChannels.Dto
{
    public enum ChannelType
    {
        /// <summary>
        /// 临时
        /// </summary>
       [Description("临时")]
        QR_STR_SCENE,
        /// <summary>
        /// 永久
        /// </summary>
        [Description("永久")]
        QR_LIMIT_STR_SCENE

    }
}
