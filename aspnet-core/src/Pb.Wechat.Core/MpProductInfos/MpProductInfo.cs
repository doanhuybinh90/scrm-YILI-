using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace Pb.Wechat.MpProductInfos
{
    public class MpProductInfo : Entity<int>, IAudited, ISoftDelete
    {
      
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string FilePathOrUrl { get; set; }
        public string ProductIntroduce { get; set; }
        public string ProductFormulations { get; set; }
        public int SortIndex { get; set; }
        public int TypeId { get; set; }
        public string TypeTitle { get; set; }
        public long? CreatorUserId { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
