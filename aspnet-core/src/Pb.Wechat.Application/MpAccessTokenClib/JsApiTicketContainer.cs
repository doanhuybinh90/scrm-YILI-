using Abp.Application.Services;
using Abp.Runtime.Caching;
using Senparc.Weixin.Containers;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.MP.Entities;
using System;
using System.Threading.Tasks;

namespace Pb.Wechat.MpAccessTokenClib
{
    [RemoteService(IsEnabled = false)]
    public class JsApiTicketContainer : BaseContainer<JsApiTicketBag>, IJsApiTicketContainer
    {
        private readonly ICacheManager _cacheManager;
        public static object Lock = new object();
        private Func<Task<JsApiTicketBag>> RegisterFuncAsync;
        public JsApiTicketContainer(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }
        /*此接口不提供异步方法*/

/* 项目“Pb.Wechat.Application(net461)”的未合并的更改
在此之前:
        public void Register(string appId, string appSecret, string name = null)
在此之后:
        public void RegisterAsync(string appId, string appSecret, string name = null)
*/
        public async Task RegisterAsync(string appId, string appSecret, string name = null)
        {
            //记录注册信息，RegisterFunc委托内的过程会在缓存丢失之后自动重试
            RegisterFuncAsync = async () =>
            {
                var bag = new JsApiTicketBag()
                {
                    Name = name,
                    AppId = appId,
                    AppSecret = appSecret,
                    JsApiTicketExpireTime = DateTime.MinValue,
                    JsApiTicketResult = new JsApiTicketResult()
                };
                await _cacheManager.GetCache(AppConsts.Cache_JsApiTicket).SetAsync(appId, bag);

                return bag;
                
            };
            await RegisterFuncAsync();

        }
        public async Task<bool> CheckRegisteredAsync(string appId)
        {
            var result =await _cacheManager.GetCache(AppConsts.Cache_JsApiTicket).GetOrDefaultAsync(appId);
            if (result == null)
                return false;
            return true;
        }

        /// <summary>
        /// 【异步方法】使用完整的应用凭证获取Ticket，如果不存在将自动注册
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="getNewTicket"></param>
        /// <returns></returns>
        public async Task<string> TryGetJsApiTicketAsync(string appId, string appSecret, bool getNewTicket = false)
        {
            if (!await CheckRegisteredAsync(appId) || getNewTicket)
            {
                await RegisterAsync(appId, appSecret);
            }
            return await GetJsApiTicketAsync(appId, getNewTicket);
        }

        /// <summary>
        ///【异步方法】 获取可用Ticket
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="getNewTicket">是否强制重新获取新的Ticket</param>
        /// <returns></returns>
        public async Task<string> GetJsApiTicketAsync(string appId, bool getNewTicket = false)
        {
            var result = await GetJsApiTicketResultAsync(appId, getNewTicket);
            return result.ticket;
        }

        /// <summary>
        /// 【异步方法】获取可用Ticket
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="getNewTicket">是否强制重新获取新的Ticket</param>
        /// <returns></returns>
        public async Task<JsApiTicketResult> GetJsApiTicketResultAsync(string appId, bool getNewTicket = false)
        {
            if (!await CheckRegisteredAsync(appId))
            {
                throw new UnRegisterAppIdException(null, "此appId尚未注册，请先使用JsApiTicketContainer.Register完成注册（全局执行一次即可）！");
            }

            JsApiTicketBag jsApiTicketBag = (JsApiTicketBag)_cacheManager.GetCache(AppConsts.Cache_JsApiTicket).GetOrDefault(appId);

            lock (Lock)
            {
                if (getNewTicket || jsApiTicketBag.JsApiTicketExpireTime <= DateTime.Now)
                {
                    //已过期，重新获取
                    jsApiTicketBag.JsApiTicketResult = CommonApi.GetTicket(jsApiTicketBag.AppId, jsApiTicketBag.AppSecret);

                    jsApiTicketBag.JsApiTicketExpireTime = DateTime.Now.AddSeconds(jsApiTicketBag.JsApiTicketResult.expires_in);
                    _cacheManager.GetCache(AppConsts.Cache_JsApiTicket).Set(appId, jsApiTicketBag, TimeSpan.FromSeconds(jsApiTicketBag.JsApiTicketResult.expires_in), TimeSpan.FromSeconds(jsApiTicketBag.JsApiTicketResult.expires_in));
                }
            }

            
            return jsApiTicketBag.JsApiTicketResult;
        }

    }
}
