using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Pb.Wechat.Web.Kefu.Models
{
    [Description("连接状态")]
    public enum ConnectState
    {
        [Description("在线")]
        Connect =1,
        [Description("掉线")]
        UnConnect=2
    }
    [Description("在线状态")]
    public enum OnlineState
    {
        [Description("在线")]
        OnLine = 0,
        [Description("离开")]
        Leave = 1,
        [Description("退出登录")]
        Quit =2
    }
    [Description("消息发送者")]
    public enum Sender
    {
        [Description("客服")]
        customer =1,
        [Description("粉丝")]
        user =0
    }
    [Description("素材类型")]
    public enum MsgType
    {
        [Description("文本")]
        text =0,
        [Description("图片")]
        image =1,
        [Description("语音")]
        voice =2,
        [Description("视频")]
        video =3,
        [Description("图文")]
        mpnews = 4,
        [Description("多图文")]
        mpmultinews = 5
    }

    [Description("客服类型")]
    public enum KFType
    {
        [Description("微信客服")]
        WX = 0,
        [Description("伊利客服")]
        YL = 1
    }

    /// <summary>
    /// 客服会话状态
    /// </summary>
    [Description("客服会话状态")]
    public enum CustomerServiceConversationState
    {
        /// <summary>
        /// 等待中
        /// </summary>
        [Description("等待中")]
        Wait = 0,
        /// <summary>
        /// 提问中
        /// </summary>
        [Description("提问中")]
        Asking = 1,
        /// <summary>
        /// 结束
        /// </summary>
        [Description("结束")]
        Closed = 2,
    }
    /// <summary>
    /// 聊天回复类型
    /// </summary>
    [Description("聊天回复类型")]
    public enum CustomerServiceReplyType
    {
        /// <summary>
        /// 自动接入
        /// </summary>
        [Description("自动接入")]
        AutoIn = 0,
        /// <summary>
        /// 转接
        /// </summary>
        [Description("转接")]
        Transfer = 1,
        /// <summary>
        /// 普通
        /// </summary>
        [Description("普通")]
        Common = 2,
    }
    /// <summary>
    /// 聊天回复类型
    /// </summary>
    [Description("聊天消息类型")]
    public enum CustomerServiceMsgType
    {
        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本")]
        text = 0,
        /// <summary>
        /// 图片
        /// </summary>
        [Description("图片")]
        image = 1,
        /// <summary>
        /// 声音
        /// </summary>
        [Description("声音")]
        voice = 2,
        /// <summary>
        /// 视频
        /// </summary>
        [Description("视频")]
        video = 3,
        /// <summary>
        /// 图文
        /// </summary>
        [Description("图文")]
        mpnews = 4,
        /// <summary>
        /// 多图文
        /// </summary>
        [Description("多图文")]
        mpmultinews = 5,
    }
    /// <summary>
    /// 聊天消息发送者
    /// </summary>
    [Description("聊天消息发送者")]
    public enum CustomerServiceMsgSender
    {
        /// <summary>
        /// 粉丝
        /// </summary>
        [Description("粉丝")]
        user = 0,
        /// <summary>
        /// 客服
        /// </summary>
        [Description("客服")]
        customer = 1,
    }
    [Description("登录状态")]
    public enum InOutState
    {
        [Description("登入")]
        Login =1,
        [Description("登出")]
        Logout =2,
        [Description("断线重连")]
        ReLogin =3,
        online=4,
        leave=5,
        quit=6
    }
}