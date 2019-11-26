using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pb.Wechat.Web.Kefu.Models
{
    /// <summary>
    /// 用户类
    /// </summary>
    public class UserInfo
    {
        public string ContextId { get; set; }
        public string Name { get; set; }
    }
}