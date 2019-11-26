using Abp.Domain.Entities;

namespace Pb.Wechat.CYConfigs
{
    public class CYConfig : Entity<int>
    {
        /// <summary>
        /// 引导用户提问的推送文案
        /// </summary>
        public string CreateProblemPreText { get; set; }
        /// <summary>
        /// 用户提问最小长度
        /// </summary>
        public int CreateProblemPreTextLength { get; set; }
        /// <summary>
        /// 创建问题成功后的推送文案
        /// </summary>
        public string CreateProblemText { get; set; }
        /// <summary>
        /// 问题关闭前的推送文案
        /// </summary>
        public string CloseProblemPreText { get; set; }
        /// <summary>
        /// 问题关闭后的推送文案
        /// </summary>
        public string CloseProblemText { get; set; }
        /// <summary>
        /// 长时间不操作的时间阈值
        /// </summary>
        public int NoOperationMinute { get; set; }
        /// <summary>
        /// 长时间不操作的提示文字
        /// </summary>
        public string NoOperationText { get; set; }
        /// <summary>
        /// 长时间不操作判定时长（分钟）
        /// </summary>
        public int NoOperationCloseMinute { get; set; }
        /// <summary>
        /// 长时间不操作关闭的文案
        /// </summary>
        public string NoOperationCloseText { get; set; }
        /// <summary>
        /// 医生文字回答模板
        /// </summary>
        public string AnswerTemplete { get; set; }
        /// <summary>
        /// 问题未关闭时，接入客服系统的推送文案
        /// </summary>
        public string CustomerAnswer { get; set; }
        /// <summary>
        /// 医生首次接入的推送文案
        /// </summary>
        public string DoctorInTemplete { get; set; }
        /// <summary>
        /// 客服正在接入的推送文案
        /// </summary>
        public string CustomerTaklingAnswer { get; set; }
    }
}
