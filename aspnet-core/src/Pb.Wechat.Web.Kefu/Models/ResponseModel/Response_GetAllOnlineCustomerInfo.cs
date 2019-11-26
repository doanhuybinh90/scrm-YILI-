using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Kefu.Model
{
    public class Response_GetAllOnlineCustomerInfo : Response_Base
    {
        public string list { get; set; }
        public int count { get; set; }
    }
}
