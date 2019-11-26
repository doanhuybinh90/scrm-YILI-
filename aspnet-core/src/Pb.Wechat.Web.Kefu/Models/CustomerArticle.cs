using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pb.Wechat.Web.Kefu.Models
{
    public class CustomerArticle
    {
        public int Id { get; set; }
        public int MpID { get; set; }


        public string Title { get; set; }
     
        public string Description { get; set; }

        public string AUrl { get; set; }
    
        public string PicMediaID { get; set; }
        public string FilePathOrUrl { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }
        public int TypeId { get; set; }
  
        public string TypeName { get; set; }

        public string MediaID { get; set; }
    }
}