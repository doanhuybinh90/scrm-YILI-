using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pb.Wechat.Web.Kefu.Models
{
    public class CommonReplyOutput
    {
        public string mediaId { get; set; }
        public string type { get; set; }
        public string text { get; set; }
        public string previewImgUrl { get; set; }
        public int typeId { get; set; }
        public string typeName { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string linkUrl { get; set; }
        public List<ArticleGroup> articleGroups { get; set; } 
    }
    public class ArticleGroup
    {
        public string title { get; set; }
        public string description { get; set; }
        public string previewImgUrl { get; set; }
        public string linkUrl { get; set; }
    }

    public class ExtraArticleGroup: CommonReplyOutput
    {
        public int Id { get; set; }
    }
}