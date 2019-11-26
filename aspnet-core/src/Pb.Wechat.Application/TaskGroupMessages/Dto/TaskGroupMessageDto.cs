using Abp.AutoMapper;
using Abp.Application.Services.Dto;

namespace Pb.Wechat.TaskGroupMessages.Dto
{
    [AutoMap(typeof(TaskGroupMessage))]
    public class TaskGroupMessageDto : EntityDto<long>
    {
        public int TaskState { get; set; }
        public string TaskID { get; set; }
        public int GroupID { get; set; }
        public int MessageID { get; set; }
        public int MpID { get; set; }
        public string OpenIDs { get; set; }
        public int SendCount { get; set; }
        public int SuccessCount { get; set; }
        public int FailCount { get; set; }
        public string WxMsgID { get; set; }
        public string ErrCode { get; set; }
        public string ErrMsg { get; set; }
        public string MsgDataID { get; set; }
        public string ClearedOpenIDs { get; set; }
    }
}
