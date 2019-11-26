using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace Pb.Wechat.CustomerMediaImages
{
    public class CustomerMediaImage : Entity<int>, IAudited, ISoftDelete
    {
        [Required(ErrorMessage = "企业号ID不能为空")]
        public int MpID { get; set; }

        [StringLength(200)]
        public string MediaID { get; set; }
        [Required(ErrorMessage = "名称不能为空")]
        [StringLength(500)]
        public string Name { get; set; }
        [StringLength(500)]
        public string FilePathOrUrl { get; set; }
        public long? CreatorUserId { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }
        public int TypeId { get; set; }
        [StringLength(100)]
        public string TypeName { get; set; }
    }
}