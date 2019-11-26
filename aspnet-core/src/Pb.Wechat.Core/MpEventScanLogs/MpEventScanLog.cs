using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace Pb.Wechat.MpEventScanLogs
{
    public class MpEventScanLog : Entity<int>, IAudited, ISoftDelete
    {
        public int MpID { get; set; }
        public string OpenID { get; set; }
        public string EventContent { get; set; }
        public string EventType { get; set; }
        public string MsgID { get; set; }

        public long? CreatorUserId { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
