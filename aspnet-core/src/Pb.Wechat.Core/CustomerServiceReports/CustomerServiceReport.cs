using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Pb.Wechat.CustomerServiceReports
{
    public class CustomerServiceReport : Entity<long>, IHasCreationTime
    {
        [Required]
        public DateTime CreationTime { get; set; }
        [Description("公众号ID")]
        public int MpID { get; set; }
        [Description("客服ID")]
        public int CustomerId { get; set; }
        [Description("客服昵称")]
        [StringLength(100)]
        public string NickName { get; set; }
        public DateTime ReportDate { get; set; }
        public decimal OnlineTime { get; set; }
        public int ReceiveCount { get; set; }
        public int ServiceCount { get; set; }
        public int ServiceMsgCount { get; set; }
        public decimal AvgScore { get; set; }
        public int ScoreCount { get; set; }
        public int TotalScore { get; set; }
    }
}
