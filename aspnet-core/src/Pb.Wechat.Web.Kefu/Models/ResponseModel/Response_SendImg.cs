using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pb.Wechat.Web.Kefu.Model
{
    public class Response_SendImg: Response_IncreaseCommonReply
    {
        public string src { get; set; }
    }
}