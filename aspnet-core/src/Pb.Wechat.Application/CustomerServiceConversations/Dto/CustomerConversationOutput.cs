using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Pb.Wechat.CustomerServiceConversations.Dto
{
    public class CustomerConversationOutput : EntityDto<long>
    {
        public string KfNickName { get; set; }
        public string KfOpenId { get; set; }
        public DateTime? StartTalkTime { get; set; }
        public DateTime? EndTalkTime { get; set; }
        public string FanHeadImgUrl { get; set; }
        public int? FanId { get; set; }
        public string FanOpenId { get; set; }
        public string FanNickName { get; set; }
        public int FanMsgCount { get; set; }
        public int CustomerMsgCount { get; set; }
        public int State { get; set; }
        public int ConversationScore { get; set; }
    }
}
