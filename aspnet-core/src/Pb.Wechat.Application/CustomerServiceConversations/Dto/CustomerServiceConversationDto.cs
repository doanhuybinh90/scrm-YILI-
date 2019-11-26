using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.ComponentModel;

namespace Pb.Wechat.CustomerServiceConversations.Dto
{
    [AutoMap(typeof(CustomerServiceConversation))]
    public class CustomerServiceConversationDto : EntityDto<long>
    {
        [Description("公众号ID")]
        public int MpID { get; set; }
        [Description("粉丝ID")]
        public int? FanId { get; set; }
        [Description("粉丝openID")]
        public string FanOpenId { get; set; }
        [Description("客服ID")]
        public int? CustomerId { get; set; }
        [Description("客服openID")]
        public string CustomerOpenId { get; set; }
        public DateTime? StartTalkTime { get; set; }
        public DateTime? EndTalkTime { get; set; }
        [Description("公众号ID")]
        public int State { get; set; }
        public long? LastConversationId { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public string NickName { get; set; }
        public string HeadImgUrl { get; set; }
        [Description("会话评分")]
        public int ConversationScore { get; set; }
    }
}
