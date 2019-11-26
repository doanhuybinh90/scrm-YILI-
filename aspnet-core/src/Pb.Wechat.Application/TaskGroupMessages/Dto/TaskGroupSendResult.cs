namespace Pb.Wechat.TaskGroupMessages.Dto
{
    public class TaskGroupSendResult
    {
        public string TaskID { get; set; }
        public int MpID { get; set; }
        public int SuccessCount { get; set; }
        public int FailCount { get; set; }
        public int SendCount { get; set; }
    }
}
