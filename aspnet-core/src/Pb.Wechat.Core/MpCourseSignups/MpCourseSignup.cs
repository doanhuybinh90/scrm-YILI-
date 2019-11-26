using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace Pb.Wechat.MpCourseSignups
{
    public class MpCourseSignup : Entity<int>, IAudited, ISoftDelete
    {
        public int MpID { get; set; }
        public string OpenID { get; set; }
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Address { get; set; }
        public string Reamrk { get; set; }
        public bool IsConfirmed { get; set; }
        public bool SendMessageState { get; set; }
        public DateTime? SendTime { get; set; }
        public string SendResult { get; set; }
        public int CRMID { get; set; }

        public long? CreatorUserId { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
