using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace Pb.Wechat.MpMediaImages
{
    public class MpMediaImage : Entity<int>, IAudited, ISoftDelete
    {
        [Required(ErrorMessage = "企业号ID不能为空")]
        public int MpID { get; set; }
        
        [StringLength(200)]
        public string MediaID { get; set; }
        [Required(ErrorMessage = "名称不能为空")]
        [StringLength(500)]
        public string Name { get; set; }
        public string FileID { get; set; }
        [StringLength(500)]
        public string FilePathOrUrl { get; set; }
        public long? CreatorUserId { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }
        public int MediaImageType { get; set; }
        public string MediaImageTypeName { get; set; }
    }
}
