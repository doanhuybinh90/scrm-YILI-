using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Pb.Wechat.MpProductTypes
{
    public class MpProductType : Entity<int>, IAudited, ISoftDelete
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string FilePathOrUrl { get; set; }
        public int SortIndex { get; set; }
        public long? CreatorUserId { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
