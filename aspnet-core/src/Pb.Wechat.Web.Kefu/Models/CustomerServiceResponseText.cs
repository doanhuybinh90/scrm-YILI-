using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Kefu.Model
{
    public class CustomerServiceResponseText
    {
        public int Id { get; set; }
        public int MpID { get; set; }
        public string ResponseType { get; set; }
        public string ResponseText { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public int UseCount { get; set; }
        public int? ReponseContentType { get; set; }
        public string PreviewImgUrl { get; set; }
        public string MediaId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public string ImageName { get; set; }
        public string VoiceName { get; set; }
        public int MartialId { get; set; }
    }
}
