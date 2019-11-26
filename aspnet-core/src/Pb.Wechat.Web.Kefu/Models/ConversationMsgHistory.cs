using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pb.Wechat.Web.Kefu.Models
{
    public class ConversationMsgHistory
    {
        public string senderType { get; set; }
        public long Id { get; set; }
        public string msgType { get; set; }
        public string msgContent { get; set; }
        public string sendTime { get; set; }
        public string customerOpenId { get; set; }
        public string customerNickName { get; set; }
        public string customerHeadImgUrl { get; set; }
    }
}