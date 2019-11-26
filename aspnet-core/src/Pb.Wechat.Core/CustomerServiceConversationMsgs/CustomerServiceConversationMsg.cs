using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Pb.Wechat.CustomerServiceConversationMsgs
{
    public class CustomerServiceConversationMsg : Entity<long>, IHasCreationTime
    {
        [Required]
        public DateTime CreationTime { get; set; }
        [Description("公众号ID")]
        public int MpID { get; set; }
        [Description("粉丝ID")]
        public int? FanId { get; set; }
        [Description("客服ID")]
        public int? CustomerId { get; set; }
        [Description("消息发送者，0为客服，1为粉丝")]
        public int Sender { get; set; }
        [Description("消息类型，0为文本，1为图片，2为语音，3为视频，4为图文")]
        public int MsgType { get; set; }
        [Description("消息文本内容，非文本类消息，则为快捷回复素材的名称")]
        public string MsgContent { get; set; }
        [Description("微信MediaId，客服发送非文本的快捷回复，使用的素材ID")]
        public string MediaId { get; set; }
        [Description("素材Url，非文本素材的Url地址")]
        public string MediaUrl { get; set; }
        public long ConversationId { get; set; }
    }
}
