using Pb.Wechat.Attrbutes;
using System.ComponentModel;

namespace Pb.Wechat.MpMessages.Dto
{
    public enum MpMessageTaskState
    {
        /// <summary>
        /// 待执行
        /// </summary>
        [IntDescription("待执行", 0)]
        Wait = 0,
        /// <summary>
        /// 正在执行
        /// </summary>
        [IntDescription("正在执行", 1)]
        Doing = 1,
        /// <summary>
        /// 执行成功
        /// </summary>
        [IntDescription("执行成功", 2)]
        Success = 2,
        /// <summary>
        /// 执行失败
        /// </summary>
        [IntDescription("执行失败", 99)]
        Fail = 99,
    }
}
