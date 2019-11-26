using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace Pb.Wechat.MpShoppingMallPics
{
    public class MpShoppingMallPic : Entity<int>, IAudited, ISoftDelete
    {
        [Required(ErrorMessage = "公众号ID不能为空")]
        public int MpID { get; set; }
        public string Name { get; set; }
        public string LocalPicUrl { get; set; }
        public string LocalPicPath { get; set; }
        public string LinkUrl { get; set; }
        public long? CreatorUserId { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
