using Abp.AutoMapper;
using Abp.Application.Services.Dto;
using System;

namespace Pb.Wechat.CustomerServiceWorkTimes.Dto
{
    [AutoMap(typeof(CustomerServiceWorkTime))]
    public class CustomerServiceWorkTimeDto : EntityDto<int>
    {
        public int MpID { get; set; }
        public string WeekDay { get; set; }
        public string MorningStartHour { get; set; }
        public string MorningStartMinute { get; set; }
        public string MorningEndHour { get; set; }
        public string MorningEndMinute { get; set; }
        public string AfternoonStartHour { get; set; }
        public string AfternoonStartMinute { get; set; }
        public string AfternoonEndHour { get; set; }
        public string AfternoonEndMinute { get; set; }

        public DateTime? LastModificationTime { get; set; }
    }
}
