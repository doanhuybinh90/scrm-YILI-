using System.Collections.Generic;

namespace Pb.Wechat.Web.Models.ChunYuModel
{

    #region 问题模型
    public class QABaseModel
    {
        public string problem_id { get; set; }
        public string user_id { get; set; }
        public string content { get; set; }
        public string sign { get; set; }
        public long atime { get; set; }
        public int error { get; set; }
        public string error_msg { get; set; }
    }
    #endregion

    #region 回答模型
    public class AnswerModel : QABaseModel
    {
        public DoctorInfo doctor { get; set; }
    }
    #endregion

    public class QAModel
    {
        public List<QABaseModel> QuestionList { get; set; }
        public List<AnswerModel> AnswerList { get; set; }
    }
}
