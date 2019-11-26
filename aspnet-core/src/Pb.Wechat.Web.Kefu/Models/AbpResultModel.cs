using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pb.Wechat.Web.Kefu.Models
{
    public class AbpResultModel
    {
        public ApiTokenResult result { get; set; }
        public bool? success { get; set; }
        public bool? error { get; set; }
    }
}