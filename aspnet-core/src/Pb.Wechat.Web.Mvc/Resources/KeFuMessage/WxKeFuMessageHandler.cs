using Abp.Dependency;
using Abp.Runtime.Caching;
using Castle.Core.Logging;
using Pb.Wechat.CustomerServiceResponseTexts;
using Pb.Wechat.CYConfigs;
using Pb.Wechat.CYConfigs.Dto;
using Pb.Wechat.MpAccessTokenClib;
using Pb.Wechat.MpAccounts;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.CustomService;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Resources
{
    public class WxKeFuMessageHandler : IKeFuMessageHandler
    {
        private readonly IMpAccountAppService _mpAccountAppService;
        private readonly IAccessTokenContainer _accessTokenContainer;
        private readonly ICYConfigAppService _cYConfigAppService;
        private readonly ICacheManager _cacheManager;
        private readonly ICustomerServiceResponseTextAppService _customerServiceResponseTextAppService;
        public ILogger Logger { get; set; }

        public WxKeFuMessageHandler(ICacheManager cacheManager, ICYConfigAppService cYConfigAppService, IMpAccountAppService mpAccountAppService, IAccessTokenContainer accessTokenContainer, ICustomerServiceResponseTextAppService customerServiceResponseTextAppService) {

            _accessTokenContainer = accessTokenContainer;
            _mpAccountAppService = mpAccountAppService;
            _cacheManager = cacheManager;
            _cYConfigAppService = cYConfigAppService;
            _customerServiceResponseTextAppService = customerServiceResponseTextAppService;
            Logger = NullLogger.Instance;
        }

        public async Task<bool> IsAsking(int mpid, string openid)
        {
            var account =await _mpAccountAppService.GetCache(mpid);
            if (account != null)
            {
                GetSessionStateResultJson result = null;
                try
                {
                    result = await CustomServiceApi.GetSessionStateAsync(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret), openid);
                }
                catch
                {
                    result = await CustomServiceApi.GetSessionStateAsync(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true), openid);
                }
                if (result.errcode == ReturnCode.请求成功)
                {
                    if (!string.IsNullOrEmpty(result.kf_account))
                        return true;

                    GetWaitCaseResultJson waitresult = null;
                    try
                    {
                        waitresult = await CustomServiceApi.GetWaitCaseAsync(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret));
                    }
                    catch
                    {
                        waitresult = await CustomServiceApi.GetWaitCaseAsync(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true));
                    }
                    if (waitresult.errcode == ReturnCode.请求成功)
                    {
                        if (waitresult.waitcaselist.Count(c => c.openid == openid) > 0)
                            return true;
                    }
                }
            }
            return false;
        }

        public async Task<IResponseMessageBase> InCustomer(int mpid, IRequestMessageBase request)
        {
            if (await IsAsking(mpid, request.FromUserName))
            {
                return request.CreateResponseMessage<ResponseMessageNoResponse>();
            }
            var text = await _customerServiceResponseTextAppService.GetCustomerDefaultResponseString(mpid);
            await SendCustomerText(mpid, request.FromUserName, text);
            if (await _customerServiceResponseTextAppService.CheckIsWorkingTime(mpid))
                return request.CreateResponseMessage<ResponseMessageTransfer_Customer_Service>();
            else
                return request.CreateResponseMessage<ResponseMessageNoResponse>();
        }

        public async Task<IResponseMessageBase> ChunYuNotice(int mpid, IRequestMessageBase request)
        {
            var config = await GetCYConfig();
            if (config != null)
            {
                var response= request.CreateResponseMessage<ResponseMessageText>();
                response.Content = config.CustomerTaklingAnswer;
                return response;
            }
            return request.CreateResponseMessage<ResponseMessageNoResponse>();
        }

        public async Task Ask(int mpid, string openid, string msgtype, string msgcontent, string format = null)
        {
            await Task.Delay(0);
        }

        #region 获取春雨配置
        /// <summary>
        /// 获取春雨配置
        /// </summary>
        /// <returns></returns>
        private async Task<CYConfigDto> GetCYConfig()
        {
            var result = await _cacheManager.GetCache(AppConsts.Cache_ChunyuConfig).GetAsync(AppConsts.Cache_ChunyuConfig, async (key) =>
            {
                return await _cYConfigAppService.GetConfig();
            });
            return result == null ? null : result as CYConfigDto;
        }
        #endregion
        
        #region 发送客服文本消息
        private async Task SendCustomerText(int mpid, string openid, string text)
        {
            var account = await _mpAccountAppService.GetCache(mpid);
            if (account != null)
            {
                //根据配置信息，调用客服接口，发送问题创建后的默认回复
                try
                {
                    await CustomApi.SendTextAsync(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret), openid, text);
                }
                catch
                {
                    await CustomApi.SendTextAsync(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true), openid, text);
                }
            }
        }
        #endregion
    }
}
