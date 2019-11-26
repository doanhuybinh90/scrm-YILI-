using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.ComponentModel;

namespace Pb.Wechat.CustomerServiceReports.Dto
{
    [AutoMap(typeof(CustomerServiceReport))]
    public class CustomerServiceReportDto : EntityDto<long>
    {
        public DateTime CreationTime { get; set; }

        public int MpID { get; set; }

        public int CustomerId { get; set; }

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
