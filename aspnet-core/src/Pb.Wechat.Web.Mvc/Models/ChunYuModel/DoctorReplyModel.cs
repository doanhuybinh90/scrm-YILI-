namespace Pb.Wechat.Web.Models.ChunYuModel
{
    public class DoctorReplyModel
    {
        public int problem_id { get; set; }
        public int user_id { get; set; }
        public string content { get; set; }
        public string sign { get; set; }
        public long atime { get; set; }
        public DoctorInfo doctor { get; set; }
    }
}
