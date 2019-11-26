using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Pb.Wechat.MpSolicitudeTexts.Dto
{
    [AutoMap(typeof(MpSolicitudeText))]
    public class MpSolicitudeTextDto : EntityDto
    {
        public int MpID { get; set; }

        public int? BabyAge { get; set; }
        public string SolicitudeText { get; set; }
        public long? CreatorUserId { get; set; }
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
