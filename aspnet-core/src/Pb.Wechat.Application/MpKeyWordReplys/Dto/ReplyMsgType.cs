using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Pb.Wechat.MpKeyWordReplys.Dto
{
    [Description("微信消息类型")]
    public enum ReplyMsgType
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        none,
        /// <summary>
        /// 图文消息
        /// </summary>
        [Description("图文消息")]
        mpnews,
        /// <summary>
        /// 多图文消息
        /// </summary>
        [Description("多图文消息")]
        mpmultinews,
        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本消息")]
        text,
        /// <summary>
        /// 语音
        /// </summary>
        [Description("语音消息")]
        voice,
        /// <summary>
        /// 图片
        /// </summary>
        [Description("图片消息")]
        image,
        /// <summary>
        /// 视频
        /// </summary>
        [Description("视频消息")]
        video,
        [Description("客服在线")]
        kf,
        [Description("春雨医生")]
        doctor
    }
}
