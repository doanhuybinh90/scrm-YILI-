using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace Pb.Wechat.CustomerServiceResponseTexts
{
    public class CustomerServiceResponseText : Entity<int>, IAudited, ISoftDelete
    {
        [Required(ErrorMessage = "公众号ID不能为空")]
        public int MpID { get; set; }

        public string ResponseType { get; set; }
        public string ResponseText { get; set; }
        public long? CreatorUserId { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }
        public int UseCount { get; set; }
        public int? ReponseContentType { get; set; }
        public string PreviewImgUrl { get; set; }
        public string MediaId { get; set; }
        [StringLength(100)]
        public string Title { get; set; }
        public string Description { get; set; }
        [StringLength(50)]
        public string TypeName { get; set; }
        public int TypeId { get; set; }
        [StringLength(100)]
        public string ImageName { get; set; }
        [StringLength(100)]
        public string VoiceName { get; set; }
        public string ArticleTitle { get; set; }
        public string ArticleDescription { get; set; }
        public string ArticleUrl { get; set; }

        public int MartialId { get; set; }

    }
}
