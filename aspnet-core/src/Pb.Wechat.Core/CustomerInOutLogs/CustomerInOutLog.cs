using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace Pb.Wechat.CustomerInOutLogs
{
    public class CustomerInOutLog : Entity<long>
    {
        public int CustomerId { get; set; }
        public int InOutState { get; set; }
        public DateTime CreationTime { get; set; }

    }
}
