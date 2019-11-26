using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pb.Wechat.Web.Kefu.Models
{
    public class MpAccountDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string AccountType { get; set; }

        public string AppId { get; set; }

        public string AppSecret { get; set; }

        public string Token { get; set; }

        public string EncodingAESKey { get; set; }

        public string MchID { get; set; }

        public string WxPayAppSecret { get; set; }

        public string CertPhysicalPath { get; set; }

        public string CertPassword { get; set; }
        public string Remark { get; set; }
        public string TaskAccessToken { get; set; }
        public string WxAccount { get; set; }
    }
}