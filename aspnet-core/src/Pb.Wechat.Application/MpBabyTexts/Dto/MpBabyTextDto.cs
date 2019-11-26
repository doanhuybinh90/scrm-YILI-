using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System;

namespace Pb.Wechat.MpBabyTexts.Dto
{
    [AutoMap(typeof(MpBabyText))]
    public class MpBabyTextDto : EntityDto<int>
    {
        public int MpID { get; set; }
        public int? BabyAge { get; set; }
        public string BabyText { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public string BabyTextType { get; set; }
        public int UnbornWeek { get; set; }
        public int OneYearMonth { get; set; }
        public int OneYearWeek { get; set; }
        public int OverYear { get; set; }
        public int OverMonth { get; set; }
        public int? BeginDay { get; set; }
        public int? EndDay { get; set; }
    }
}
