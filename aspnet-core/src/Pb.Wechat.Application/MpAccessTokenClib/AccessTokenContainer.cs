using Abp.Application.Services;
using Abp.Runtime.Caching;
using Castle.Core.Logging;
using Senparc.Weixin.Containers;
using Senparc.Weixin.Entities;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.MP.Entities;
using System;
using System.Threading.Tasks;

namespace Pb.Wechat.MpAccessTokenClib
{
    [RemoteService(IsEnabled = false)]
    public class AccessTokenContainer : BaseContainer<AccessTokenBag>, IAccessTokenContainer
    {
        private readonly IJsApiTicketContainer _jsApiTicketContainer;
        private readonly ICacheManager _cacheManager;
        private readonly ILogger _logger;
        private Func<Task<AccessTokenBag>> RegisterFuncAsync;
        public static object Lock = new object();
        public AccessTokenContainer(ICacheManager cacheManager, IJsApiTicketContainer jsApiTicketContainer, ILogger logger)
        {
            _cacheManager = cacheManager;
            _jsApiTicketContainer = jsApiTicketContainer;
            _logger = logger;
        }
        const string LockResourceName = "MP.AccessTokenContainer";

        /// <summary>
        /// 注册应用凭证信息，此操作只是注册，不会马上获取Token，并将清空之前的Token
        /// </summary>
        /// <param name="appId">微信公众号后台的【开发】>【基本配置】中的“AppID(应用ID)”</param>
        /// <param name="appSecret">微信公众号后台的【开发】>【基本配置】中的“AppSecret(应用密钥)”</param>
        /// <param name="name">标记AccessToken名称（如微信公众号名称），帮助管理员识别</param>
        public async Task RegisterAsync(string appId, string appSecret, string name = null)
        {

            //记录注册信息，RegisterFunc委托内的过程会在缓存丢失之后自动重试
            RegisterFuncAsync = async () =>
            {
                var bag = new AccessTokenBag()
                {
                    //Key = appId,
                    Name = name,
                    AppId = appId,
                    AppSecret = appSecret,
                    AccessTokenExpireTime = DateTime.MinValue,
                    AccessTokenResult = new AccessTokenResult()
                };
                await _cacheManager.GetCache(AppConsts.Cache_AccessToken).SetAsync(appId, bag);

                return bag;

            };
            await RegisterFuncAsync();

            //为JsApiTicketContainer进行自动注册

            await _jsApiTicketContainer.RegisterAsync(appId, appSecret, name);

        }


        #region AccessToken

        /// <summary>
        /// 【异步方法】使用完整的应用凭证获取Token，如果不存在将自动注册
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="getNewToken"></param>
        /// <returns></returns>
        public async Task<string> TryGetAccessTokenAsync(string appId, string appSecret, bool getNewToken = false)
        {


            if (!await CheckRegistered(appId) || getNewToken)
            {
                await RegisterAsync(appId, appSecret);
            }
            return await GetAccessTokenAsync(appId, getNewToken);
        }
        public async new Task<bool> CheckRegistered(string appId)
        {
            var result = await _cacheManager.GetCache(AppConsts.Cache_AccessToken).GetOrDefaultAsync(appId);
            if (result == null)
                return false;
            return true;
        }
        /// <summary>
        /// 【异步方法】获取可用Token
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="getNewToken">是否强制重新获取新的Token</param>
        /// <returns></returns>
        public async Task<string> GetAccessTokenAsync(string appId, bool getNewToken = false)
        {

            var result = await GetAccessTokenResultAsync(appId, getNewToken);
            return result.access_token;
        }

        /// <summary>
        /// 获取可用AccessTokenResult对象
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="getNewToken">是否强制重新获取新的Token</param>
        /// <returns></returns>
        public async Task<IAccessTokenResult> GetAccessTokenResultAsync(string appId, bool getNewToken = false)
        {
            if (!await CheckRegistered(appId))
            {
                throw new UnRegisterAppIdException(appId, string.Format("此appId（{0}）尚未注册，请先使用AccessTokenContainer.Register完成注册（全局执行一次即可）！", appId));
            }

            AccessTokenBag accessTokenBag = (AccessTokenBag)_cacheManager.GetCache(AppConsts.Cache_AccessToken).GetOrDefault(appId);

            lock (Lock)
            {

                if (getNewToken || accessTokenBag.AccessTokenExpireTime <= DateTime.Now)
                {
                    //已过期，重新获取
                    accessTokenBag.AccessTokenResult = CommonApi.GetToken(accessTokenBag.AppId, accessTokenBag.AppSecret);
                    accessTokenBag.AccessTokenExpireTime = DateTime.Now.AddSeconds(accessTokenBag.AccessTokenResult.expires_in);
                    _cacheManager.GetCache(AppConsts.Cache_AccessToken).Set(appId, accessTokenBag);
                }
            }

            return accessTokenBag.AccessTokenResult;
        }


        #endregion
    }
}
