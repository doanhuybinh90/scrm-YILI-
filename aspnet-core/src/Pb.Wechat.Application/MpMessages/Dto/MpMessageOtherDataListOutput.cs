using Abp.Application.Services.Dto;
using System;

namespace Pb.Wechat.MpMessages.Dto
{
    public class MpMessageOtherDataListOutput : EntityDto<int>
    {
        public string NameOrContent { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public string MessageType { get; set; }
        public string State { get; set; }
        public DateTime? ExecTaskTime { get; set; }
        public int IsTask { get; set; }
        public long? SendCount { get; set; }
        public DateTime? FinishDate { get; set; }
        public long? FailCount { get; set; }
        public long? SuccessCount { get; set; }
        public int SendState { get; set; }
    }
}
