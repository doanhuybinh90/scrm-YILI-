using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Pb.Wechat.CustomerServiceWorkTimes
{
    public class CustomerServiceWorkTime : Entity<int>, IAudited, ISoftDelete
    {
        [Required(ErrorMessage = "公众号ID不能为空")]
        public int MpID { get; set; }
        [Description("周几")]
        public string WeekDay { get; set; }
        [Description("上午上班时")]
        public string  MorningStartHour { get; set; }
        [Description("上午上班分")]
        public string MorningStartMinute { get; set; }
        [Description("上午下班时")]
        public string MorningEndHour { get; set; }
        [Description("上午下班分")]
        public string MorningEndMinute { get; set; }
        [Description("下午上班时")]
        public string AfternoonStartHour { get; set; }
        [Description("下午上班分")]
        public string AfternoonStartMinute { get; set; }
        [Description("下午下班时")]
        public string AfternoonEndHour { get; set; }
        [Description("下午下班分")]
        public string AfternoonEndMinute { get; set; }
        public long? CreatorUserId { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
