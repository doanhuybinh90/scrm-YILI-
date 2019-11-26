using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Pb.Wechat.CustomerServiceResponseTypes
{
    public class CustomerServiceResponseType : Entity<int>, IAudited, ISoftDelete
    {

        [Description("类型描述")]
        [StringLength(50)]
        public string TypeDescription { get; set; }
        
        public long? CreatorUserId { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }
       
    }
}
