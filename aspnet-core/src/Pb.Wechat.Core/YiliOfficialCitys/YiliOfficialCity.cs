using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace Pb.Wechat.YiliOfficialCitys
{
    public class YiliOfficialCity : Entity<int>, IAudited, ISoftDelete
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public int ParentCode { get; set; }
        public long? CreatorUserId { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
