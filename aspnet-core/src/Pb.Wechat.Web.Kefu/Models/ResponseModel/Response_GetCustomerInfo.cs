using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Kefu.Model
{
    public class Response_GetCustomerInfo : Response_Base
    {
        public string nickName { get; set; }
        public string headImgUrl { get; set; }
        public bool autoJoin { get; set; }
        public int autoJoinCount { get; set; }
        public string openId { get; set; }
        public bool autoJoinReply { get; set; }
        public string autoJoinReplyText { get; set; }
        public bool autoLeaveReply { get; set; }
        public string autoLeaveReplyText { get; set; }
    }
}
