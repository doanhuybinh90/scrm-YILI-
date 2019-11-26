using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pb.Wechat.Web.Kefu.Models
{
    public class SearchQuickReply
    {
        public string Type { get; set; }
        public string Text { get; set; }
        public string PreviewImgUrl { get; set; }
        public string MediaId { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public string Title { get; set; }
        public string ReplyType { get; set; }
        public int UseCount { get; set; }
    }
}