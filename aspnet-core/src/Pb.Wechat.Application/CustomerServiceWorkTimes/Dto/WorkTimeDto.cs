using System;
using System.Collections.Generic;
using System.Text;

namespace Pb.Wechat.CustomerServiceWorkTimes.Dto
{
    public class WorkTimeDto
    {
        public int MpID { get; set; }
        public int WeekDay { get; set; }
        public TimeSpan BeginTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
