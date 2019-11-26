using Abp.Application.Services.Dto;
using System;

namespace Pb.Wechat.CustomerServiceWorkTimes.Dto
{
    public class WorkTimeOutput : EntityDto<int>
    {
        public int MpID { get; set; }
        public string WeekDay { get; set; }
        public string MorningWorkTime { get; set; }
        public string AfternoonWorkTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
    }
}
