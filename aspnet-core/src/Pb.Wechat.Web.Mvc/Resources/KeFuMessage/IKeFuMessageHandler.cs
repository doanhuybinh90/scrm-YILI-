using Senparc.Weixin.MP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Resources
{
    public interface IKeFuMessageHandler
    {
        /// <summary>
        /// 是否正在接入客服
        /// </summary>
        /// <param name="mpid"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        Task<bool> IsAsking(int mpid, string openid);
        /// <summary>
        /// 用户提问
        /// </summary>
        /// <param name="mpid"></param>
        /// <param name="openid"></param>
        /// <param name="msgtype"></param>
        /// <param name="msgcontent"></param>
        /// <returns></returns>
        Task Ask(int mpid, string openid, string msgtype, string msgcontent,string format=null);
        /// <summary>
        /// 获取接入客服的返回信息
        /// </summary>
        /// <param name="mpid"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        Task<IResponseMessageBase> InCustomer(int mpid, IRequestMessageBase request);
        /// <summary>
        /// 客服未结束，想接入春雨的提示
        /// </summary>
        /// <param name="mpid"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        Task<IResponseMessageBase> ChunYuNotice(int mpid, IRequestMessageBase request);
    }
}
