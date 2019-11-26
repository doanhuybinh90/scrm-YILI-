using Pb.Wechat.Web.Models.ChunYuModel;
using Senparc.Weixin.MP.Entities;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Resources
{
    public interface IChunYuMessageHandler
    {
        /// <summary>
        /// 是否接入春雨医生
        /// </summary>
        /// <param name="mpid"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        Task<bool> IsAsking(int mpid, string openid);
        /// <summary>
        /// 创建春雨医生问题
        /// </summary>
        /// <param name="mpid"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        Task<IResponseMessageBase> CreateProblem(int mpid, IRequestMessageBase request);
        /// <summary>
        /// 用户提问
        /// </summary>
        /// <param name="mpid"></param>
        /// <param name="openid"></param>
        /// <param name="msgtype"></param>
        /// <param name="msgcontent"></param>
        /// <returns></returns>
        Task Ask(int mpid, string openid, string msgtype, string msgcontent);
        /// <summary>
        /// 春雨医生未结束，想接入客服时的提示
        /// </summary>
        /// <param name="mpid"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        Task<IResponseMessageBase> CustomerNotice(int mpid, IRequestMessageBase request);
        /// <summary>
        /// 医生回答
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task Answer(DoctorReplyModel data);
        /// <summary>
        /// 医生关闭问题
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task Close(CloseProblemModel data);
        /// <summary>
        /// 用户关闭或系统定期关闭
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        Task<bool> Close(string openid);
    }
}
