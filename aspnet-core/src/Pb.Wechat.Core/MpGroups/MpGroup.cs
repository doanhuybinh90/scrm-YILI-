using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace Pb.Wechat.MpGroups
{
    public class MpGroup : Entity<int>, IAudited, ISoftDelete
    {
        [Required(ErrorMessage = "公众号ID不能为空")]
        public int MpID { get; set; }
        [Required(ErrorMessage = "名称不能为空")]
        public string Name { get; set; }
        public int FansCount { get; set; }
        public int ParentID { get; set; }
        public string ParentName { get; set; }
        public string FullPath { get; set; }
        public int Length { get; set; }
        public int ChildCount { get; set; }
        public int WxGroupID { get; set; }
        public long? CreatorUserId { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
