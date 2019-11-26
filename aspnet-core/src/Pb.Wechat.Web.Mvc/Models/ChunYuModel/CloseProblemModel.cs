namespace Pb.Wechat.Web.Models.ChunYuModel
{
    public class CloseProblemModel
    {
        public int problem_id { get; set; }
        public int user_id { get; set; }
        public string msg { get; set; }
        public string status { get; set; }
        public int price { get; set; }
        public string sign { get; set; }
        public long atime { get; set; }
    }
}
