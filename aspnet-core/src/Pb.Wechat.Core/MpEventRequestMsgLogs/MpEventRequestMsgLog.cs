using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace Pb.Wechat.MpEventRequestMsgLogs
{
    public class MpEventRequestMsgLog : Entity<int>, IAudited, ISoftDelete
    {
        public int MpID { get; set; }
        public string OpenID { get; set; }
        public string MsgType { get; set; }
        public string MsgId { get; set; }
        public string MediaId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AUrl { get; set; }
        public string Content { get; set; }

        public long? CreatorUserId { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
