using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace Pb.Wechat.UserMps
{
    public class UserMp : Entity<int>, IAudited, ISoftDelete
    {
        [Required(ErrorMessage = "用户不能为空")]
        public long? UserId { get; set; }
        // ID (Primary key)
        /// <summary>公众号ID</summary>	
        [Required(ErrorMessage = "当前公众号不能为空")]
        public int CurrentMpID { get; set; } 

        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }
    }

}
