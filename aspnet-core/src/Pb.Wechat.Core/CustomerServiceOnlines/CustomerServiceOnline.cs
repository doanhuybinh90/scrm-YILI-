using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Pb.Wechat.CustomerServiceOnlines
{
    public class CustomerServiceOnline : Entity<int>, IAudited, ISoftDelete
    {
        [Description("公众号ID")]
        public int MpID { get; set; }
        [Description("客服账号前缀")]
        public string PreKfAccount { get; set; }
        [Description("公众号账号")]
        public string PublicNumberAccount { get; set; }
        [Description("客服完整账号，格式为：帐号前缀@公众号微信号")]
        public string KfAccount { get; set; }
        [Description("客服账号登录密码")]
        public string KfPassWord { get; set; }
        [Description("客服昵称")]
        public string KfNick { get; set; }
        
        [Description("客服编号")]
        public string KfId { get; set; }
        [Description("客服本地头像URL")]
        public string LocalHeadingUrl { get; set; }
        [Description("客服本地头像物理地址")]
        public string LocalHeadFilePath { get; set; }
        [Description("客服微信端头像")]
        public string KfHeadingUrl { get; set; }
        [Description("客服人员微信号")]
        public string KfWx { get; set; }
        [Description("如果客服帐号尚未绑定微信号，但是已经发起了一个绑定邀请， 则此处显示绑定邀请的微信号")]
        public string InviteWx { get; set; }
        [Description("如果客服帐号尚未绑定微信号，但是已经发起过一个绑定邀请， 邀请的过期时间，为unix 时间戳")]
        public string InviteExpireTime { get; set; }
        [Description("邀请的状态，有等待确认“waiting”，被拒绝“rejected”， 过期“expired”")]
        public string InviteStatus { get; set; }
        public long? CreatorUserId { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }
        /// <summary>
        /// 在线状态，1为在线，2为离开，3为退出登录
        /// </summary>
        [Description("在线状态，1为在线，2为离开，3为退出登录")]
        public int OnlineState { get; set; }
        /// <summary>
        /// Signalr连接状态，1为在线，2为掉线
        /// </summary>
        [Description("Signalr连接状态，1为在线，2为掉线")]
        public int ConnectState { get; set; }
        public string ConnectId { get; set; }
        public string MessageToken { get; set; }
        public string OpenID { get; set; }
        public bool AutoJoin { get; set; }
        public int AutoJoinCount { get; set; }
        [Description("客服类型，‘WX’为微信客服，‘YL’为伊利客服")]
        public string KfType { get; set; }
        [Description("自动接入，是否启用自动回复语")]
        public bool AutoJoinReply { get; set; }
        [Description("自动接入时，自动回复内容")]
        [StringLength(200)]
        public string AutoJoinReplyText { get; set; }
        [Description("离开时，是否启动自动回复")]
        public bool AutoLeaveReply { get; set; }
        [Description("离开时，自动回复内容")]
        [StringLength(200)]
        public string AutoLeaveReplyText { get; set; }
        [Description("是否客服组长")]
        public bool CustomerServiceManager { get; set; }
    }
}
