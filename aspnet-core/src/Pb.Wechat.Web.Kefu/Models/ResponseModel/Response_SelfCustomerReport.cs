using Pb.Wechat.Web.Kefu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pb.Wechat.Web.Kefu.Model
{
    public class Response_SelfCustomerReport:Response_Base
    {
        public string NickName { get; set; }
        public CustomerSelfReport reportData { get; set; }
    }
}