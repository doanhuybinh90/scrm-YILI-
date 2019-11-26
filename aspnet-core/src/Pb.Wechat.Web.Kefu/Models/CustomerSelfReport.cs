using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pb.Wechat.Web.Kefu.Models
{
    public class CustomerSelfReport
    {
        
        public double TotalOnLineTime { get; set; }
        public double TotalReceiveCount { get; set; }
        public double TotalServiceCount { get; set; }
        public double TotalMsgCount { get; set; }
        public double? TotalAvgScore { get; set; }

        public double DailyOnLineTime { get; set; }
        public double DailyReceiveCount { get; set; }
        public double DailyServiceCount { get; set; }
        public double DailyMsgCount { get; set; }
        public double? DailyAvgScore { get; set; }
        public List<CustomerSelfReportDetail> list { get; set; }
    }

    public class CustomerSelfReportDetail
    {
        public string Date { get; set; }
        public decimal OnLineTime { get; set; }
        public int ReceiveCount { get; set; }
        public int ServiceCount { get; set; }
        public int MsgCount { get; set; }
        public decimal AvgScore { get; set; }
    }

    public class CustomerDailyReportDto
    {
        public long Id { get; set; }
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