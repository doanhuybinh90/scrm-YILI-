using System.ComponentModel;

namespace Pb.Wechat.CYProblems.Dto
{
    /// <summary>
    /// 春雨医生问题类型
    /// </summary>
    [Description("春雨医生问题类型")]
    public enum CYProblemState
    {
        /// <summary>
        /// 准备提问
        /// </summary>
        [Description("准备提问")]
        Prepare = 0,
        /// <summary>
        /// 提问中
        /// </summary>
        [Description("提问中")]
        Asking = 1,
        /// <summary>
        /// 结束
        /// </summary>
        [Description("结束")]
        Closed = 2,
    }
    /// <summary>
    /// 春雨医生问题内容类型
    /// </summary>
    [Description("春雨医生问题内容类型")]
    public enum CYProblemContentType
    {
        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本")]
        text = 0,
        /// <summary>
        /// 图片
        /// </summary>
        [Description("图片")]
        image = 1,
        /// <summary>
        /// 语音
        /// </summary>
        [Description("语音")]
        audio = 2,
        /// <summary>
        /// 病人资料
        /// </summary>
        [Description("病人资料")]
        patient_meta = 3,
    }
    /// <summary>
    /// 春雨医生问题内容发送者
    /// </summary>
    [Description("春雨医生问题内容发送者")]
    public enum CYProblemContentSender
    {
        /// <summary>
        /// 用户
        /// </summary>
        [Description("用户")]
        user = 0,
        /// <summary>
        /// 医生
        /// </summary>
        [Description("医生")]
        doctor = 1,
    }
}
