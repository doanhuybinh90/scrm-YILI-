using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pb.Wechat.Web.Kefu.Models
{
    public class CustomerServicePrivateResponseText
    {
        public int Id { get; set; }
        public int MpID { get; set; }

        public string ResponseType { get; set; }
        public string ResponseText { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }
        public int UseCount { get; set; }
        public string ReponseContentType { get; set; }
        public string PreviewImgUrl { get; set; }
        public string MediaId { get; set; }
        public string OpenID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}