using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pb.Wechat.Web.Kefu.Model
{
    public class Response_CustomerServiceLog: Response_Base
    {
        public int TodayServiceCount { get; set; }
        public int TodayReplyMsgCount { get; set; }
    }
}