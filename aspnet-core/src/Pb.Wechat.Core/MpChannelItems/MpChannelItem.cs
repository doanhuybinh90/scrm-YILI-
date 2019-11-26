using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Pb.Wechat.MpChannelItems
{
    public class MpChannelItem : Entity<int>, IHasCreationTime, ISoftDelete
    {
        [StringLength(500)]
        public string Code { get; set; }
        [StringLength(500)]
        public string Name { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
