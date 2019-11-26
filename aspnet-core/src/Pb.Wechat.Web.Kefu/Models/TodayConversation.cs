using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pb.Wechat.Web.Kefu.Models
{
    public class TodayConversation
    {
        public int CustomerId { get; set; }
        public string CustomerOpenId { get; set; }
        public int FanId { get; set; }
        public string FanOpenId { get; set; }
        public int State { get; set; }
        public string HeadImgUrl { get; set; }
        public string NickName { get; set; }
    }
}