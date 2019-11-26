using System.ComponentModel;

namespace Pb.Wechat.Web.Models.ChunYuModel
{
    public class DoctorInfo
    {
        [Description("医生ID")]
        public string id { get; set; }
        [Description("医生姓名")]
        public string name { get; set; }
        [Description("医生头像")]
        public string image { get; set; }
        [Description("医生职称")]
        public string title { get; set; }
        [Description("带医院级别的医生职称")]
        public string level_title { get; set; }
        [Description("科室名称")]
        public string clinic { get; set; }
        [Description("科室号")]
        public string clinic_no { get; set; }
        [Description("医院名字")]
        public string hospital { get; set; }
        [Description("医院级别")]
        public string hospital_grade { get; set; }
        [Description("擅长领域")]
        public string good_at { get; set; }
    }
}
