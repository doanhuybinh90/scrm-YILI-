using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pb.Wechat.Web.Kefu.Models
{
    public class LogInout
    {
        public long Id { get; set; }
        public int CustomerId { get; set; }
        public int InOutState { get; set; }
        public DateTime CreationTime { get; set; }
    }
}