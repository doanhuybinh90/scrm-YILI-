using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pb.Wechat.Web.Kefu.Models
{
    public class CustomerServiceConversationDto
    {
        public long Id { get; set; }
       
        public int MpID { get; set; }
        
        public int? FanId { get; set; }
       
        public string FanOpenId { get; set; }
       
        public int? CustomerId { get; set; }
       
        public string CustomerOpenId { get; set; }
        public DateTime? StartTalkTime { get; set; }
        public DateTime? EndTalkTime { get; set; }
        
        public int State { get; set; }
        public long? LastConversationId { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public string NickName { get; set; }
        public string HeadImgUrl { get; set; }
        public int ConversationScore { get; set; }
    }
}