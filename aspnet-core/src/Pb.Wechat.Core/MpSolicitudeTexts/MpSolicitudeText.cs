using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Pb.Wechat.MpSolicitudeTexts
{
    public class MpSolicitudeText : Entity<int>, IAudited, ISoftDelete
    {
        [Required(ErrorMessage = "公众号ID不能为空")]
        public int MpID { get; set; }

        public int? BabyAge { get; set; }
        public string SolicitudeText { get; set; }
        public long? CreatorUserId { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }
        [StringLength(50)]
        public string SolicitudeTextType { get; set; }
        public int UnbornWeek { get; set; }
        public int OneYearMonth { get; set; }
        public int OneYearWeek { get; set; }
        public int OverYear { get; set; }
        public int OverMonth { get; set; }

        public int? BeginDay { get; set; }
        public int? EndDay { get; set; }
    }
}
