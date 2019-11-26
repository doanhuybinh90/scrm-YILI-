using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Pb.Wechat.CustomerServiceOnlines.Dto
{
    [AutoMap(typeof(CustomerServiceOnline))]
    public class AutoJoinCustomerDto : EntityDto<int>
    {
        [Description("公众号ID")]
        public int MpID { get; set; }
        [Description("客服账号前缀")]
        public string PreKfAccount { get; set; }
        [Description("公众号账号")]
        public string PublicNumberAccount { get; set; }
        [Description("客服完整账号，格式为：帐号前缀@公众号微信号")]
        public string KfAccount { get; set; }
        [Description("客服昵称")]
        public string KfNick { get; set; }
        [Description("客服本地头像Url")]
        public string LocalHeadingUrl { get; set; }
        [Description("客服本地头像物理地址")]
        public string LocalHeadFilePath { get; set; }
        [Description("客服微信头像Url")]
        public string KfHeadingUrl { get; set; }

        [Description("客服人员微信号")]
        public string KfWx { get; set; }
        [Description("客服账号登录密码")]
        public string KfPassWord { get; set; }
        public DateTime? LastModificationTime { get; set; }
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
        [Description("是否自动接入")]
        public bool AutoJoin { get; set; }
        [Description("自动接入数量")]
        public int AutoJoinCount { get; set; }
        public string KfType { get; set; }
        public int ConversationCount { get; set; }
        public decimal JoinPercent { get; set; }
    }
}
