using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Pb.Wechat.MpMediaImageTypes
{
    public class MpMediaImageType : Entity<int>, IAudited, ISoftDelete
    {
        [Required(ErrorMessage = "企业号ID不能为空")]
        public int MpID { get; set; }

        public string MediaTypeName { get; set; }
        public long? CreatorUserId { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
