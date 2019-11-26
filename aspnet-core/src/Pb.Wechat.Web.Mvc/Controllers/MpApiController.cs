using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Controllers;
using Pb.Wechat.MpApiTokens;
using Pb.Wechat.MpApiTokens.Dto;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Pb.Wechat.Url;
using Senparc.Weixin.MP.TenPayLibV3;
using Senparc.Weixin.MP;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using System.Net.Sockets;
using System.Net;
using System.Linq;
using Abp.Auditing;
using Pb.Wechat.MpAccounts;
using Pb.Wechat.MpUserMembers;
using Pb.Wechat.MpAccessTokenClib;
using Abp.Runtime.Session;
using Pb.Wechat.WxMedias;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using Senparc.Weixin.MP.AdvancedAPIs;
using Abp.Runtime.Caching;
using yilibabyUser;
using Pb.Wechat.Web.Resources;
using Pb.Wechat.MpMessages;
using Pb.Wechat.TaskGroupMessages;
using Pb.Wechat.MpMessages.Dto;
using Pb.Wechat.MpFans;
using Pb.Wechat.MpGroups.Dto;
using Pb.Wechat.MpChannels.Dto;
using Abp.Web.Models;
using Pb.Wechat.MpGroups;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Pb.Wechat.CustomerServiceOnlines;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pb.Wechat.Web.Controllers
{
    public class MpApiController : AbpController
    {
        private readonly IMpAccountAppService _mpAccountAppService;
        private readonly IMpApiTokenAppService _mpApiTokenAppService;
        private readonly IWebUrlService _webUrlService;
        private readonly IMpUserMemberAppService _mpUserMemberAppService;
        private readonly IAccessTokenContainer _accessTokenContainer;
        private readonly IJsApiTicketContainer _jsApiTicketContainer;
        private readonly IAbpSession _abpSession;
        private readonly IWxMediaAppService _wxMediaAppService;
        private readonly ICacheManager _cacheManager;
        private readonly IMpMessageAppService _mpMessageAppService;
        private readonly ITaskGroupMessageAppService _taskGroupMessageAppService;
        private readonly IMpFanAppService _mpFanAppService;
        private readonly IYiliBabyClubInterfaceService _yiliBabyClubInterfaceService;
        private static string defaultUserName = "";
        private static string defaultUserPwd = "";
        private static string deviceCode = "";
        public readonly IMpGroupAppService _mpGroupAppService;
        private readonly ICustomerServiceOnlineAppService _customerServiceOnlineAppService;
        public IClientInfoProvider ClientInfoProvider { get; set; }

        public MpApiController(IMpApiTokenAppService mpApiTokenAppService,
            IWebUrlService webUrlService,
            IMpAccountAppService mpAccountAppService,
            IMpUserMemberAppService mpUserMemberAppService,
            IAccessTokenContainer accessTokenContainer,
            IJsApiTicketContainer jsApiTicketContainer,
            IAbpSession abpSession,
            IWxMediaAppService wxMediaAppService,
            ICacheManager cacheManager,
            IMpMessageAppService mpMessageAppService,
            ITaskGroupMessageAppService taskGroupMessageAppService
            , IMpFanAppService mpFanAppService
            , IYiliBabyClubInterfaceService yiliBabyClubInterfaceService
            , IMpGroupAppService mpGroupAppService
            , ICustomerServiceOnlineAppService customerServiceOnlineAppService)
        {
            _mpAccountAppService = mpAccountAppService;
            _mpApiTokenAppService = mpApiTokenAppService;
            _webUrlService = webUrlService;
            _mpUserMemberAppService = mpUserMemberAppService;
            ClientInfoProvider = NullClientInfoProvider.Instance;
            _accessTokenContainer = accessTokenContainer;
            _jsApiTicketContainer = jsApiTicketContainer;
            _abpSession = abpSession;
            _wxMediaAppService = wxMediaAppService;
            _cacheManager = cacheManager;
            _mpMessageAppService = mpMessageAppService;
            _taskGroupMessageAppService = taskGroupMessageAppService;
            _mpFanAppService = mpFanAppService;
            _yiliBabyClubInterfaceService = yiliBabyClubInterfaceService;
            _mpGroupAppService = mpGroupAppService;
            defaultUserName = _yiliBabyClubInterfaceService.ApiUserName;
            defaultUserPwd = _yiliBabyClubInterfaceService.ApiPassword;
            deviceCode = _yiliBabyClubInterfaceService.ApiDeviceCode;
            _customerServiceOnlineAppService = customerServiceOnlineAppService;
        }

        [HttpPost]
        public async Task<IActionResult> GetAccessToken(string token, int getnewtoken = 0)
        {
            var account = await GetAccountToken(token, MpApiTokenType.Nomal.ToString());
            if (account == null)
            {
                Logger.Info($"token为“{token}”的静默授权失败，原因：公众号或令牌不存在，Url：{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}");
                return Content("公众号或令牌不存在");
            }
            var _access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, getnewtoken == 1);

            //var _access_token = await Senparc.Weixin.MP.Containers.AccessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, getnewtoken == 1);
            return Json(new
            {
                access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, getnewtoken == 1)
                //access_token = await Senparc.Weixin.MP.Containers.AccessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, getnewtoken == 1),
            });
        }

        [HttpGet]
        public async Task<IActionResult> OAuth2Base(string token, string reurl, string needRegister, string baseEncryp = "1", string wghhd = "", string ActivityId = "", string Classify = "", string DefaultClient = "", string DefaultClientCode = "", string DefaultClientFullName = "", string OfficialCity = "")
        {

            Logger.Info($"传入token：{token}");

            var account = await GetAccountToken(token, MpApiTokenType.Nomal.ToString());
            if (account == null)
            {
                Logger.Info($"token为“{token}”的静默授权失败，原因：公众号或令牌不存在，Url：{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}");
                return Content("公众号或令牌不存在");
            }
            if (string.IsNullOrEmpty(reurl))
            {
                Logger.Info($"token为“{token}”的静默授权失败，原因：reurl为空，Url：{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}");
                return Content("reurl为空");
            }
            if (account.Domain != "*")
            {
                Logger.Info($"进入域名验证：{account.Domain}");
                Uri ru = null;
                try
                {
                    var _reurl = Base64Helper.DecodeBase64(reurl.Replace(" ", "+"));
                    ru = new Uri(_reurl);
                    Logger.Info($"reurl解析：{_reurl}");
                }
                catch (Exception ex)
                {
                    Logger.Error($"token为“{token}”的静默授权失败，原因：解析reurl“{reurl}”错误，Url：{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}", ex);
                    return Content("reurl不正确");
                }
                var redomain = ru.Authority.ToLower();
                if (account.Domain.ToLower() != redomain.ToLower())
                {
                    Logger.Info($"token为“{token}”的静默授权失败，原因：域名{redomain}不在白名单中,白名单为{account.Domain.ToLower()}，Url：{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}");
                    return Content("您的域名未授权调用该接口");
                }
            }
            string appId = account.AppId;
            var url = Senparc.Weixin.MP.AdvancedAPIs.OAuthApi.GetAuthorizeUrl(appId, $"{Request.Scheme}://{Request.Host}{Request.PathBase}/mpapi/OAuth2BaseCallback?mpid={account.MpId}&token={token}&reurl={reurl}&baseEncryp={baseEncryp}&needRegister={needRegister}&wghhd={wghhd}&ActivityId={ActivityId}&Classify={Classify}&DefaultClient={DefaultClient}&DefaultClientCode={DefaultClientCode}&DefaultClientFullName={DefaultClientFullName}&OfficialCity={OfficialCity}", "JeffreySu", Senparc.Weixin.MP.OAuthScope.snsapi_base);
            Logger.Info($"Oauth跳转地址：{url}");
            return Redirect(url);
        }

        [HttpGet]
        public async Task<IActionResult> OAuth2UserInfo(string token, string reurl, string needRegister, string baseEncryp = "1")
        {
            Logger.Info($"传入token：{token}");
            var account = await GetAccountToken(token, MpApiTokenType.Nomal.ToString());
            if (account == null)
            {
                Logger.Info($"token为“{token}”的认证授权失败，原因：公众号或令牌不存在，Url：{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}");
                return Content("公众号或令牌不存在");
            }
            if (string.IsNullOrEmpty(reurl))
            {
                Logger.Info($"token为“{token}”的认证授权失败，原因：reurl为空，Url：{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}");
                return Content("reurl为空");
            }
            if (account.Domain != "*")
            {
                Uri ru = null;
                try
                {
                    ru = new Uri(Base64Helper.DecodeBase64(reurl.Replace(" ", "+")));
                }
                catch (Exception ex)
                {
                    Logger.Error($"token为“{token}”的认证授权失败，原因：解析reurl“{reurl}”错误，Url：{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}", ex);
                    return Content("reurl不正确");
                }
                var redomain = ru.Authority.ToLower();
                if (account.Domain.ToLower() != redomain.ToLower())
                {
                    Logger.Info($"token为“{token}”的认证授权失败，原因：域名{redomain}不在白名单中，Url：{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}");
                    return Content("您的域名未授权调用该接口");
                }
            }
            string appId = account.AppId;
            var url = Senparc.Weixin.MP.AdvancedAPIs.OAuthApi.GetAuthorizeUrl(appId, $"{Request.Scheme}://{Request.Host}{Request.PathBase}/mpapi/OAuth2UserInfoCallback?mpid={account.MpId}&token={token}&reurl={reurl}&baseEncryp={baseEncryp}&needRegister={needRegister}", "JeffreySu", Senparc.Weixin.MP.OAuthScope.snsapi_userinfo);
            Logger.Info($"第一层跳转地址：{url}");
            return Redirect(url);
        }

        [HttpGet]
        public async Task<IActionResult> OAuth2BaseCallback(string token, string reurl, string needRegister, string code, string state, string baseEncryp = "1", string wghhd = "", string ActivityId = "", string Classify = "", string DefaultClient = "", string DefaultClientCode = "", string DefaultClientFullName = "", string OfficialCity = "")
        {

            Logger.Info($"传入ActivityId：{ActivityId},传入Classify：{Classify}");
            var url = Base64Helper.DecodeBase64(reurl.Replace(" ", "+"));
            Logger.Info($"二段解析reurl：{url}");
            if (string.IsNullOrEmpty(code))
            {
                Logger.Info($"token为“{token}”的静默授权失败，原因：拒绝了授权，Url：{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}");
                return Content("您拒绝了授权！");
            }

            if (state != "JeffreySu" && state != "JeffreySu?10000skip=true")
            {
                //这里的state其实是会暴露给客户端的，验证能力很弱，这里只是演示一下
                //实际上可以存任何想传递的数据，比如用户ID，并且需要结合例如下面的Session["OAuthAccessToken"]进行验证
                Logger.Info($"token为“{token}”的静默授权失败，原因：验证失败，Url：{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}");
                return Content("验证失败！请从正规途径进入！");
            }

            var account = await GetAccountToken(token, MpApiTokenType.Nomal.ToString());
            if (account == null)
            {
                Logger.Info($"token为“{token}”的静默授权失败，原因：公众号或令牌不存在，Url：{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}");
                return Content("公众号不存在");
            }

            //通过，用code换取access_token
            Senparc.Weixin.MP.AdvancedAPIs.OAuth.OAuthAccessTokenResult result = null;
            try
            {

                result = Senparc.Weixin.MP.AdvancedAPIs.OAuthApi.GetAccessToken(account.AppId, account.AppSecret, code);
            }
            catch (Exception ex)
            {
                Logger.Info($"token为“{token}”的静默授权在通过code获取token时异常，原因：{ex.Message}，Url：{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}");
                return Content("错误：" + ex.Message);
            }

            if (result.errcode != Senparc.Weixin.ReturnCode.请求成功)
            {
                Logger.Info($"token为“{token}”的静默授权在通过code获取token时异常，原因：{result.errmsg}，Url：{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}");
                return Content("错误：" + result.errmsg);
            }
            Logger.Info($"传入qd={wghhd}");
            string mgccAuthKey = await _mpUserMemberAppService.GetMgccAuthKey(result.openid);

            int? memberType = null;
            var fan = await _mpFanAppService.GetFirstOrDefaultByOpenID(result.openid);
            if (fan != null)//是粉丝
            {

                var qd = "wx";
                var channel = fan.ChannelType;
                if (channel == ChannelEnum.yygw.ToString())
                {
                    qd = "app";
                }
                else
                {
                    qd = "wx";
                }
                if (!string.IsNullOrEmpty(wghhd))
                {
                    Logger.Info($"传入qd赋值{wghhd}");
                    qd = wghhd;
                }
                //判定是否会员
                if (needRegister == "1")
                {
                    Logger.Info($"进入强制注册判定,openid为：{result.openid}");
                    var member = await _mpUserMemberAppService.GetByOpenID(result.openid);
                    if (member == null)//不是会员
                    {
                        Logger.Info($"判定不是会员");
                        url = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/yilibabyclub/registfromwx?qd={qd}&reurl={reurl}&token={token}";
                    }
                    else
                    {
                        if (member.IsBinding)
                            memberType = member.MemberType;
                        else
                        {
                            memberType = null;
                            url = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/yilibabyclub/registfromwx?qd={qd}&reurl={reurl}&token={token}";
                        }
                    }
                }
                else
                {
                    var member = await _mpUserMemberAppService.GetByOpenID(result.openid);
                    if (member != null)//不是会员
                    {
                        if (member.IsBinding)
                            memberType = member.MemberType;
                        else
                            memberType = null;
                    }

                }
            }
            else
            {
                var qd = "wx";
                //判定是否会员
                if (needRegister == "1")
                {
                    var member = await _mpUserMemberAppService.GetByOpenID(result.openid);
                    if (member == null)//不是会员
                    {

                        url = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/yilibabyclub/registfromwx?qd={qd}&reurl={reurl}&token={token}";
                    }
                    else
                    {
                        if (member != null)//不是会员
                        {
                            if (member.IsBinding)
                                memberType = member.MemberType;
                            else
                            {
                                memberType = null;
                                url = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/yilibabyclub/registfromwx?qd={qd}&reurl={reurl}&token={token}";
                            }
                               
                        }
                    }
                }
                else
                {
                    var member = await _mpUserMemberAppService.GetByOpenID(result.openid);
                    if (member != null)//不是会员
                        memberType = member.MemberType;
                }
            }


            var clientInfo = Base64Helper.EncodeBase64(System.Web.HttpUtility.UrlEncode(JsonConvert.SerializeObject(new
            {
                DefaultClient = DefaultClient,
                DefaultClientCode = DefaultClientCode,
                DefaultClientFullName = DefaultClientFullName,
                OfficialCity = OfficialCity
            }), System.Text.Encoding.UTF8));


            if (baseEncryp == "1")
            {
                var paramJson = Base64Helper.EncodeBase64(JsonConvert.SerializeObject(new
                {
                    result.openid,
                    authkey = mgccAuthKey,
                    isfans = fan != null,
                    membertype = memberType,
                    ActivityId = ActivityId,
                    Classify = Classify
                }));
                url = string.Format("{0}{1}ActivityId={3}&Classify={4}&userInfo={2}&clientInfo={5}&token={6}"
                   , url, url.Contains("?") ? "&" : "?", paramJson, ActivityId, Classify, clientInfo, token);

            }
            else
                url = string.Format("{0}{1}openid={2}&authkey={3}&ActivityId={4}&Classify={5}&clientInfo={6}"
                  , url, url.Contains("?") ? "&" : "?", result.openid, mgccAuthKey, ActivityId, Classify, clientInfo);

            Logger.Info($"返回地址：【{url}】");
            return Redirect(url);
        }

        [HttpGet]
        public async Task<IActionResult> OAuth2UserInfoCallback(string token, string reurl, string code, string state, string needRegister, string baseEncryp = "1")
        {
            Logger.Info("进入OAuth2UserInfoCallback");
            var url = Base64Helper.DecodeBase64(reurl.Replace(" ", "+"));
            Logger.Info($"解码Url：{url}");
            Logger.Info($"code为：{code}");
            if (string.IsNullOrEmpty(code))
            {
                Logger.Info($"token为“{token}”的认证授权失败，原因：拒绝了授权，Url：{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}");
                return Content("您拒绝了授权！");
            }

            if (state != "JeffreySu" && state != "JeffreySu?10000skip=true")
            {
                //这里的state其实是会暴露给客户端的，验证能力很弱，这里只是演示一下
                //实际上可以存任何想传递的数据，比如用户ID，并且需要结合例如下面的Session["OAuthAccessToken"]进行验证
                Logger.Info($"token为“{token}”的认证授权失败，原因：验证失败，Url：{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}");
                return Content("验证失败！请从正规途径进入！");
            }

            //通过，用code换取access_token
            var account = await GetAccountToken(token, MpApiTokenType.Nomal.ToString());
            if (account == null)
            {
                Logger.Info($"token为“{token}”的认证授权失败，原因：公众号或令牌不存在，Url：{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}");
                return Content("公众号不存在");
            }

            Senparc.Weixin.MP.AdvancedAPIs.OAuth.OAuthAccessTokenResult result = null;
            try
            {
                result = Senparc.Weixin.MP.AdvancedAPIs.OAuthApi.GetAccessToken(account.AppId, account.AppSecret, code);
            }
            catch (Exception ex)
            {
                Logger.Info($"token为“{token}”的认证授权在通过code获取token时异常，原因：{ex.Message}，Url：{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}");
                return Content("错误：" + ex.Message);
            }

            if (result.errcode != Senparc.Weixin.ReturnCode.请求成功)
            {
                Logger.Info($"token为“{token}”的认证授权在通过code获取token时失败，原因：{result.errmsg}，Url：{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}");
                return Content("错误：" + result.errmsg);
            }
            Logger.Info($"开始获取mgcckey");
            string mgccAuthKey = await _mpUserMemberAppService.GetMgccAuthKey(result.openid);
            Logger.Info($"mgcckey：{mgccAuthKey}");
            //因为这里还不确定用户是否关注本微信，所以只能试探性地获取一下
            Senparc.Weixin.MP.AdvancedAPIs.OAuth.OAuthUserInfo userInfo = null;
            string baseEncrypString = string.Empty;
            try
            {
                //已关注，可以得到详细信息
                userInfo = Senparc.Weixin.MP.AdvancedAPIs.OAuthApi.GetUserInfo(result.access_token, result.openid);
               
                int? memberType = null;
                var fan = await _mpFanAppService.GetFirstOrDefaultByOpenID(result.openid);
                if (fan != null)//是粉丝
                {
                   
                    var qd = "wx";
                    var channel = fan.ChannelType;
                    if (channel == ChannelEnum.yygw.ToString())
                    {
                        qd = "app";
                    }
                    else
                    {
                        qd = "wx";
                    }
                 
                    //判定是否会员
                    if (needRegister == "1")
                    {
                        var member = await _mpUserMemberAppService.GetByOpenID(result.openid);
                        if (member == null)//不是会员
                        {

                            url = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/yilibabyclub/registfromwx?qd={qd}&reurl={reurl}&token={token}";
                        }
                        else
                        {
                            if (member != null)//不是会员
                            {
                                if (member.IsBinding)
                                    memberType = member.MemberType;
                                else
                                {
                                    memberType = null;
                                    url = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/yilibabyclub/registfromwx?qd={qd}&reurl={reurl}&token={token}";
                                }
                                   
                            }
                        }
                    }
                    else
                    {
                        var member = await _mpUserMemberAppService.GetByOpenID(result.openid);

                        if (member != null)//不是会员
                        {
                            if (member.IsBinding)
                                memberType = member.MemberType;
                            else
                                memberType = null;
                        }
                    }
                }
                else
                {
                    var qd = "wx";
                    //判定是否会员
                    if (needRegister == "1")
                    {
                        var member = await _mpUserMemberAppService.GetByOpenID(result.openid);
                        if (member == null)//不是会员
                        {

                            url = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/yilibabyclub/registfromwx?qd={qd}&reurl={reurl}&token={token}";
                        }
                        else
                        {
                            if (member.IsBinding)
                                memberType = member.MemberType;
                            else
                            {
                                memberType = null;
                                url = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/yilibabyclub/registfromwx?qd={qd}&reurl={reurl}&token={token}";
                            }
                                
                        }
                    }
                    else
                    {
                        var member = await _mpUserMemberAppService.GetByOpenID(result.openid);
                        if (member != null)//不是会员
                        {
                            if (member.IsBinding)
                                memberType = member.MemberType;
                            else
                                memberType = null;
                        }
                    }
                }
                if (baseEncryp == "1")
                {
                    var unBase64 = JsonConvert.SerializeObject(new
                    {
                        result.openid,
                        nickname = userInfo.nickname,
                        headimgurl = userInfo.headimgurl,
                        authkey = mgccAuthKey,
                        isfans = fan != null,
                        membertype = memberType
                    });

                    var paramJson = Base64Helper.EncodeBase64(unBase64);

                    Logger.Info($"OAuth2UserInfoCallback接口---userInfo未加密字符串为{unBase64}");
                    Logger.Info($"OAuth2UserInfoCallback接口---userInfo加密字符串为{paramJson}");
                    url = string.Format("{0}{1}userInfo={2}"
                       , url, url.Contains("?") ? "&" : "?", paramJson);
                }
                else
                    url = string.Format("{0}{1}openid={2}&nickname={3}&headimgurl={4}&authkey={5}&membertype={6}&isfans={7}"
                      , url, url.Contains("?") ? "&" : "?", result.openid, userInfo.nickname, userInfo.headimgurl, mgccAuthKey, memberType, fan != null);
               
                Logger.Info($"第二层回跳地址{url}");

                return Redirect(url);
            }
            catch (Senparc.Weixin.Exceptions.ErrorJsonResultException ex)
            {
                Logger.Info($"token为“{token}”的认证授权失败，原因：{ex.Message}，Url：{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}");
                return Content("错误：" + ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Jssdk(string token, string callurl)
        {

            var account = await GetAccountToken(token, MpApiTokenType.Nomal.ToString());
            if (account == null)
            {
                Logger.Info($"token为“{token}”的Jssdk失败，原因：公众号或令牌不存在，Url：{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}");
                return Content("公众号或令牌不存在");
            }
            var ticket = await _jsApiTicketContainer.TryGetJsApiTicketAsync(account.AppId, account.AppSecret);
            //var ticket = await Senparc.Weixin.MP.Containers.JsApiTicketContainer.TryGetJsApiTicketAsync(account.AppId, account.AppSecret);

            var url = Base64Helper.DecodeBase64(callurl.Replace(" ", "+"));

            string timestamp = Convert.ToString(ConvertDateTimeInt(DateTime.Now));
            string nonceStr = createNonceStr();
            string rawstring = "jsapi_ticket=" + ticket + "&noncestr=" + nonceStr + "&timestamp=" + timestamp + "&url=" + url;

            string signature = SHA1_Hash(rawstring);

            return Json(new
            {
                appId = account.AppId,
                nonceStr = nonceStr,
                timestamp = timestamp,
                url = url,
                signature = signature,
                rawString = rawstring,
            });


        }

        [HttpGet]
        public async Task<IActionResult> JssdkJsonP(string token, string callurl, string callback)
        {

            var account = await GetAccountToken(token, MpApiTokenType.Nomal.ToString());
            if (account == null)
            {
                Logger.Info($"token为“{token}”的JssdkJsonp失败，原因：公众号或令牌不存在，Url：{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}");
                return Content("公众号或令牌不存在");
            }

            var ticket = await _jsApiTicketContainer.TryGetJsApiTicketAsync(account.AppId, account.AppSecret);
            //var ticket = await Senparc.Weixin.MP.Containers.JsApiTicketContainer.TryGetJsApiTicketAsync(account.AppId, account.AppSecret);
            var url = Base64Helper.DecodeBase64(callurl.Replace(" ", "+"));
            string timestamp = Convert.ToString(ConvertDateTimeInt(DateTime.Now));
            string nonceStr = createNonceStr();
            string rawstring = "jsapi_ticket=" + ticket + "&noncestr=" + nonceStr + "&timestamp=" + timestamp + "&url=" + url;
            string signature = SHA1_Hash(rawstring);

            return Content(string.IsNullOrEmpty(callback) ? "" : string.Format("{0}({1})", callback, JsonConvert.SerializeObject(new
            {
                appId = account.AppId,
                nonceStr = nonceStr,
                timestamp = timestamp,
                url = url,
                signature = signature,
                rawString = rawstring,
            })));


        }

        [HttpPost]
        [ResponseCache(Duration = AbpZeroTemplateConsts.PageOutputCacheDuration, VaryByQueryKeys = new string[] { "token", "openid" })]
        public async Task<IActionResult> GetUser(string token, string openid)
        {
            var account = await GetAccountToken(token, MpApiTokenType.Nomal.ToString());
            if (account == null)
            {
                Logger.Info($"token为“{token}”的用户信息获取失败，原因：公众号或令牌不存在，Url：{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}");
                return Content("公众号或令牌不存在");
            }

            #region 获取用户信息
            Senparc.Weixin.MP.AdvancedAPIs.User.UserInfoJson userinfo = null;

            try
            {
                var access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret);
                //var access_token = await Senparc.Weixin.MP.Containers.AccessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret);
                userinfo = await Senparc.Weixin.MP.AdvancedAPIs.UserApi.InfoAsync(access_token, openid);
            }
            catch
            {
                try
                {
                    var access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true);
                    //var access_token = await Senparc.Weixin.MP.Containers.AccessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true);
                    userinfo = await Senparc.Weixin.MP.AdvancedAPIs.UserApi.InfoAsync(access_token, openid);
                }
                catch (Exception ex)
                {
                    return Json(new
                    {
                        errorcode = "500",
                        errormsg = ex.Message,
                    });
                }
            }
            #endregion

            return Json(new
            {
                errorcode = "200",
                errormsg = "",
                result = userinfo,
            });
        }

        [HttpGet]
        [ResponseCache(Duration = AbpZeroTemplateConsts.PageOutputCacheDuration, VaryByQueryKeys = new string[] { "token", "openid" })]
        public async Task<IActionResult> GetUserJsonP(string token, string openid, string callback)
        {
            var account = await GetAccountToken(token, MpApiTokenType.Nomal.ToString());
            if (account == null)
            {
                Logger.Info($"token为“{token}”的jsonp用户信息获取失败，原因：公众号或令牌不存在，Url：{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}");
                return Content("公众号或令牌不存在");
            }

            #region 获取用户信息

            Senparc.Weixin.MP.AdvancedAPIs.User.UserInfoJson userinfo = null;

            try
            {
                var access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret);
                //var access_token = await Senparc.Weixin.MP.Containers.AccessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret);
                userinfo = await Senparc.Weixin.MP.AdvancedAPIs.UserApi.InfoAsync(access_token, openid);
            }
            catch
            {
                try
                {
                    var access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true);
                    //var access_token = await Senparc.Weixin.MP.Containers.AccessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true);
                    userinfo = await Senparc.Weixin.MP.AdvancedAPIs.UserApi.InfoAsync(access_token, openid);
                }
                catch (Exception ex)
                {
                    return Content(string.IsNullOrEmpty(callback) ? "" : string.Format("{0}({1})", callback, JsonConvert.SerializeObject(new
                    {
                        errorcode = "500",
                        errormsg = ex.Message,
                    })));
                }
            }
            #endregion

            return Content(string.IsNullOrEmpty(callback) ? "" : string.Format("{0}({1})", callback, JsonConvert.SerializeObject(new
            {
                errorcode = "200",
                errormsg = "",
                result = userinfo,
            })));
        }

        [HttpPost]
        public async Task<IActionResult> SendRedPacket(string token, string openid, string total, string activityname, string sendername, string sendmsg, string remark)
        {
            #region 校验金额
            int rptotal = 0;
            if (!int.TryParse(total, out rptotal))
            {
                Logger.Info($"token为“{token}”的发送红包接口调用失败，原因：金额不正确");
                return Json(new
                {
                    errorcode = "202",
                    errormsg = "金额不正确",
                });
            }
            #endregion

            #region 校验公众号
            var account = await GetAccountToken(token, MpApiTokenType.Redpackage.ToString());
            if (account == null)
            {
                Logger.Info($"token为“{token}”的发送红包接口调用失败，原因：公众号或令牌不存在，Url：{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}");
                return Content("公众号或令牌不存在");
            }
            #endregion

            #region 发送红包
            string mchbillno = DateTime.Now.ToString("HHmmss") + Senparc.Weixin.MP.TenPayLibV3.TenPayV3Util.BuildRandomStr(28);
            //本地或者服务器的证书位置（证书在微信支付申请成功发来的通知邮件中）
            string cert = account.CertPhysicalPath;
            //私钥（在安装证书时设置）
            string password = account.CertPassword;

            string nonceStr;//随机字符串
            string paySign;//签名
            var redpackageresult = RedPackApi.SendNormalRedPack(
                account.AppId,
                account.MchID,
                account.WxPayAppSecret,
                cert,
                openid,
                sendername,
                _webUrlService.WebSiteRootIPFormat,
                rptotal,
                sendmsg,
                activityname,
                remark,
                out nonceStr,
                out paySign,
                mchbillno);

            return Json(new
            {
                errorcode = redpackageresult.result_code == "1" ? "200" : "204",
                errormsg = redpackageresult.return_msg,
            });
            #endregion
        }

        [HttpPost]
        public async Task<IActionResult> GetWxPayJsApi(string token, string openid, string productname, int total, string orderno)
        {
            try
            {

                #region 校验公众号
                var account = await GetAccountToken(token, MpApiTokenType.Redpackage.ToString());
                if (account == null)
                {
                    Logger.Info($"token为“{token}”的发送红包接口调用失败，原因：公众号或令牌不存在，Url：{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}");
                    return Content("公众号或令牌不存在");
                }
                #endregion

                string timeStamp = "";
                string nonceStr = "";
                string paySign = "";

                //生成订单10位序列号，此处用时间和随机数生成，商户根据自己调整，保证唯一
                orderno = string.IsNullOrEmpty(orderno) ? (DateTime.Now.ToString("yyyyMMddHHmmss") + TenPayV3Util.BuildRandomStr(3)) : orderno;

                //创建支付应答对象
                RequestHandler packageReqHandler = new RequestHandler(null);
                //初始化
                packageReqHandler.Init();

                timeStamp = TenPayV3Util.GetTimestamp();
                nonceStr = TenPayV3Util.GetNoncestr();

                //设置package订单参数
                packageReqHandler.SetParameter("appid", account.AppId);       //公众账号ID
                packageReqHandler.SetParameter("mch_id", account.MchID);          //商户号
                packageReqHandler.SetParameter("nonce_str", nonceStr);                    //随机字符串
                packageReqHandler.SetParameter("body", productname);    //商品信息
                packageReqHandler.SetParameter("out_trade_no", orderno);      //商家订单号
                packageReqHandler.SetParameter("total_fee", total.ToString());                  //商品金额,以分为单位(money * 100).ToString()
                packageReqHandler.SetParameter("spbill_create_ip", ClientInfoProvider.ClientIpAddress);   //用户的公网ip，不是商户服务器IP
                packageReqHandler.SetParameter("notify_url", $"{Request.Scheme}://{Request.Host}/wxapi/PayNotifyUrl");            //接收财付通通知的URL
                packageReqHandler.SetParameter("trade_type", TenPayV3Type.JSAPI.ToString());                        //交易类型
                packageReqHandler.SetParameter("openid", openid);                       //用户的openId
                packageReqHandler.SetParameter("attach", $"{account.MpId},{token}");                       //用户的openId

                string sign = packageReqHandler.CreateMd5Sign("key", account.WxPayAppSecret);
                packageReqHandler.SetParameter("sign", sign);                       //签名

                string data = packageReqHandler.ParseXML();

                var result = TenPayV3.Unifiedorder(data);
                var res = XDocument.Parse(result);
                string prepayId = res.Element("xml").Element("prepay_id").Value;

                //设置支付参数
                RequestHandler paySignReqHandler = new RequestHandler(null);
                paySignReqHandler.SetParameter("appId", account.AppId);
                paySignReqHandler.SetParameter("timeStamp", timeStamp);
                paySignReqHandler.SetParameter("nonceStr", nonceStr);
                paySignReqHandler.SetParameter("package", string.Format("prepay_id={0}", prepayId));
                paySignReqHandler.SetParameter("signType", "MD5");
                paySign = paySignReqHandler.CreateMd5Sign("key", account.WxPayAppSecret);

                ViewData["appId"] = account.AppId;
                ViewData["timeStamp"] = timeStamp;
                ViewData["nonceStr"] = nonceStr;
                ViewData["package"] = string.Format("prepay_id={0}", prepayId);
                ViewData["paySign"] = paySign;
                return Json(new
                {
                    success = true,
                    data = new
                    {
                        appId = account.AppId,
                        timeStamp = timeStamp,
                        nonceStr = nonceStr,
                        package = string.Format("prepay_id={0}", prepayId),
                        paySign = paySign,
                        orderNO = orderno
                    }
                });
            }
            catch (Exception ex)
            {
                Logger.Info($"openid:'{openid}',total:{total},productname:'{productname}',ex:'{ex.Message}'");
                return Json(new
                {
                    success = false,
                    msg = ex.Message,
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> PayNotifyUrl()
        {
            var returntmpl = @"<xml>
   <return_code><![CDATA[{0}]]></return_code>
   <return_msg><![CDATA[{1}]]></return_msg>
</xml>";
            try
            {
                ResponseHandler resHandler = new ResponseHandler(null);
                string return_code = resHandler.GetParameter("return_code");
                string return_msg = resHandler.GetParameter("return_msg");
                if (return_code == "SUCCESS")
                {
                    var attachs = resHandler.GetParameter("attach");
                    var mt = attachs.Split(',');
                    string openid = resHandler.GetParameter("openid");
                    string out_trade_no = resHandler.GetParameter("out_trade_no");
                    string total_fee = resHandler.GetParameter("total_fee");

                    #region 校验公众号
                    //var account = await GetAccountToken(mt[1], MpApiTokenType.Redpackage.ToString());
                    var account = await GetAccountToken(mt[0], MpApiTokenType.Redpackage.ToString());
                    if (account == null)
                    {
                        Logger.Info($"attach为“{attachs}”的微信支付回调验证失败，原因：公众号或令牌不存在，Url：{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}");
                        return Content("公众号或令牌不存在");
                    }
                    #endregion

                    resHandler.SetKey(account.WxPayAppSecret);
                    //验证请求是否从微信发过来（安全）
                    if (resHandler.IsTenpaySign())
                    {
                        return Content(string.Format(returntmpl, return_code, return_msg), "text/xml");
                    }
                }
                return Content(string.Format(returntmpl, "FAIL", "Validate Error"), "text/xml");
            }
            catch (Exception ex)
            {
                Logger.Error("微信支付回调验证失败", ex);
                return Content(string.Format(returntmpl, "FAIL", "System Error"), "text/xml");
            }
        }

        #region jssdk私有函数
        private string createNonceStr()
        {
            int length = 16;
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string str = "";
            Random rad = new Random();
            for (int i = 0; i < length; i++)
            {
                str += chars.Substring(rad.Next(0, chars.Length - 1), 1);
            }
            return str;
        }

        /// <summary> 
        /// 将c# DateTime时间格式转换为Unix时间戳格式 
        /// </summary> 
        /// <param name="time">时间</param> 
        /// <returns>double</returns> 
        public int ConvertDateTimeInt(System.DateTime time)
        {
            int intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            intResult = Convert.ToInt32((time - startTime).TotalSeconds);
            return intResult;
        }

        //SHA1哈希加密算法 
        public string SHA1_Hash(string str_sha1_in)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] bytes_sha1_in = System.Text.UTF8Encoding.Default.GetBytes(str_sha1_in);
            byte[] bytes_sha1_out = sha1.ComputeHash(bytes_sha1_in);
            string str_sha1_out = BitConverter.ToString(bytes_sha1_out);
            str_sha1_out = str_sha1_out.Replace("-", "").ToLower();
            return str_sha1_out;
        }
        #endregion

        private async Task<MpAccountTokenOutput> GetAccountToken(string Token, string ApiType)
        {
            return await _mpApiTokenAppService.GetAccountToken(new MpAccountTokenInput()
            {
                Token = Token,
                ApiType = ApiType
            });
        }

        private static string GetClientIpAddress(HttpContext httpContext)
        {
            var clientIp = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ??
httpContext.Request.Headers["REMOTE_ADDR"].FirstOrDefault();

            foreach (var hostAddress in Dns.GetHostAddresses(clientIp))
            {
                if (hostAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    return hostAddress.ToString();
                }
            }

            foreach (var hostAddress in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (hostAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    return hostAddress.ToString();
                }
            }

            return null;
        }
        /// <summary>
        /// 预览接口
        /// </summary>
        /// <param name="mediaID"></param>
        /// <param name="openid"></param>
        /// <param name="authkey"></param>
        /// <returns></returns>
        public async Task<ActionResult> Preview(string mediaID, string openid, string authkey)
        {
            var mediaArray = mediaID.Split(',');
            var mpId = int.Parse(mediaArray[1]);
            var messageType = mediaArray[2];
            var _mediaId = mediaArray[0];
            Logger.Info("===================================================================================================================================================");
            Logger.Info("OpenID为：" + openid);
            await _wxMediaAppService.PreviewMatial(new WxMedias.Dto.PreviewModel
            {
                MediaID = _mediaId,
                MessageType = messageType,
                MpID = mpId,
                OpenID = openid
            });

            return Content("已发送预览");
        }

        public async Task<ActionResult> BindOpenID(int id, string openid)
        {
            var output = await _customerServiceOnlineAppService.Get(new EntityDto<int>(id));
            output.OpenID = openid;
            try
            {
                await _customerServiceOnlineAppService.Update(output);
                return Content("绑定成功");
            }
            catch (Exception ex)
            {

                return Content(ex.Message);
            }
           
        }

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="token">TOKEN值</param>
        /// <param name="openId">用户OpenID</param>
        /// <param name="templateId">模板id</param>
        /// <param name="data">模板数据</param>
        /// <param name="linkUrl">跳转地址</param>
        /// <param name="getnewtoken">是否需要新的token</param>
        /// <returns></returns>
        [HttpPost]
        [DontWrapResult]
        public async Task<IActionResult> SendTemplateMessage(string token, string openId, string templateId, string templateData, string linkUrl = null)
        {
            var account = await GetAccountToken(token, MpApiTokenType.Nomal.ToString());
            string _access_token = "";
            if (account == null)
            {
                Logger.Info($"token为“{token}”的静默授权失败，原因：公众号或令牌不存在，Url：{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}");
                return Content("公众号或令牌不存在");
            }
            try
            {
                _access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            SendTemplateMessageResult sendResult = null;
            var templateModel = JsonConvert.DeserializeObject<TemplateMessage>(templateData);
            try
            {
                sendResult = TemplateApi.SendTemplateMessage(_access_token, openId, templateId, linkUrl, templateModel);
            }
            catch
            {
                try
                {
                    _access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true);
                    sendResult = TemplateApi.SendTemplateMessage(_access_token, openId, templateId, linkUrl, templateModel);
                }
                catch (Exception e)
                {

                    return Json(e.Message);
                }


            }

            return Json(sendResult);
        }

        [HttpPost]
        [DontWrapResult]
        public async Task<IActionResult> GetQRCodeResult(string token, string eventKey, int isLimit = 0, int expire_seconds = 0)
        {
            var QrType = QrCode_ActionName.QR_LIMIT_SCENE;
            if (isLimit == 1)
            {
                QrType = QrCode_ActionName.QR_LIMIT_STR_SCENE;
            }
            var account = await GetAccountToken(token, MpApiTokenType.Nomal.ToString());
            if (account == null)
            {
                Logger.Info($"token为“{token}”的静默授权失败，原因：公众号或令牌不存在，Url：{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}");
                return Content("公众号或令牌不存在");
            }
            var _access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret);
            try
            {
                return Json(await QrCodeApi.CreateAsync(_access_token, expire_seconds, 0, QrType, eventKey));
            }
            catch
            {

                try
                {
                    _access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true);
                    return Json(await QrCodeApi.CreateAsync(_access_token, expire_seconds, 0, QrType, eventKey));
                }
                catch (Exception e)
                {
                    Logger.Error($"Webservice获取带参二维码错误：{e.Message},堆栈：{e.StackTrace}");
                    return Json(null);
                }

            }
        }

        #region 调用美驰查询群发接口

        #region 登陆以及加密
        /// <summary>
        /// 获取AES回执
        /// </summary>
        /// <returns></returns>
        private async Task<ApplyAESEncryptKeyResponse> GetAesKey(bool getnew = false)
        {
            var result = await _cacheManager.GetCache(AppConsts.Cache_AesKeyModal).GetOrDefaultAsync("AesKeyResult");
            if (result == null || getnew)
            {
                UserLoginSoapClient client = new UserLoginSoapClient(UserLoginSoapClient.EndpointConfiguration.UserLoginSoap);

                RSAParameters publicKey = RsaService.GetRsaPublicKey();

                ApplyAESEncryptKeyResponse AesResult = await client.ApplyAESEncryptKeyAsync(new ApplyAESEncryptKeyRequest { DeviceCode = deviceCode, Modulus = Convert.ToBase64String(publicKey.Modulus), Exponent = Convert.ToBase64String(publicKey.Exponent) });

                await _cacheManager.GetCache(AppConsts.Cache_AesKeyModal).SetAsync("AesKeyResult", AesResult);
                return AesResult;
            }
            else
            {
                ApplyAESEncryptKeyResponse AesResult = (ApplyAESEncryptKeyResponse)result;
                return AesResult;
            }
        }
        /// <summary>
        /// 登陆返回美驰Key
        /// </summary>
        /// <param name="mgccAuthKey"></param>
        /// <returns></returns>
        private async Task<string> DefaultLoginAndGetMCKey()
        {
            var userName = string.Empty;
            var userPwd = string.Empty;
            var AesResult = await GetAesKey();

            return await _cacheManager.GetCache(AppConsts.Cache_MCAuthKey).GetAsync<string, string>("DefaultMCAuthKey", async c =>
            {
                UserLoginSoapClient client = new UserLoginSoapClient(UserLoginSoapClient.EndpointConfiguration.UserLoginSoap);
                var EncryptPassword = "";
                try
                {
                    EncryptPassword = AesCryptoService.Encrypt(defaultUserPwd, AesResult.CryptAESKey, AesResult.CryptAESIV);
                }
                catch
                {
                    AesResult = await GetAesKey(true);
                    EncryptPassword = AesCryptoService.Encrypt(defaultUserPwd, AesResult.CryptAESKey, AesResult.CryptAESIV);
                }
                var result = await client.LoginExAsync(new LoginExRequest { UserName = defaultUserName, EncryptPassword = EncryptPassword, DeviceCode = deviceCode });
                return result.AuthKey;
            });

        }

        private async Task<string> UserLoginAngGetMCKey(MpUserMember member)
        {
            var AesResult = await GetAesKey();

            UserLoginSoapClient client = new UserLoginSoapClient(UserLoginSoapClient.EndpointConfiguration.UserLoginSoap);
            var EncryptPassword = "";
            try
            {
                EncryptPassword = AesCryptoService.Encrypt(member.MemberPassword, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }
            catch
            {
                AesResult = await GetAesKey(true);
                EncryptPassword = AesCryptoService.Encrypt(member.MemberPassword, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }
            var result = await client.LoginExAsync(new LoginExRequest { UserName = member.MemeberUserName, EncryptPassword = EncryptPassword, DeviceCode = deviceCode });
            return result.AuthKey;

        }
        #endregion

        [DontWrapResult]
        public async Task<MessageResponseModel> GetSendMembersFromMC(string token, int mpId, int messageId)
        {
            string taskId = Guid.NewGuid().ToString().Replace("-", "");
            List<string> openIdArray = new List<string>();

            //var account = await GetAccountToken(token, MpApiTokenType.Nomal.ToString());
            #region 获取OpenID集合
            yilibabyMember.MemberServiceSoapClient client = new yilibabyMember.MemberServiceSoapClient(yilibabyMember.MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
            var authKey = await DefaultLoginAndGetMCKey();

            var msgData = await _mpMessageAppService.GetById(messageId);
            var groupIdArray = msgData.GroupIds.Split(",").ToList();

            #region 多分组获取openids
            foreach (var item in groupIdArray)
            {
                var msgResult = await _mpGroupAppService.GetItem(int.Parse(item));
                if (msgResult.MotherType == MotherType.ALL.ToString())
                {
                    msgResult.BeginBabyBirthday = new DateTime(1900, 1, 1);
                    msgResult.EndBabyBirthday = new DateTime(1900, 1, 1);
                }
                else if (msgResult.MotherType == MotherType.UnPregnant.ToString())
                {
                    DateTime date = DateTime.Now.AddDays(280);
                    msgResult.BeginBabyBirthday = date;
                    msgResult.EndBabyBirthday = new DateTime(1900, 1, 1);
                }
                else if (msgResult.MotherType == MotherType.Pregnant.ToString())
                {
                    DateTime date = DateTime.Now.AddDays(280);
                    msgResult.BeginBabyBirthday = DateTime.Now;
                    msgResult.EndBabyBirthday = date;
                }
                else if (msgResult.MotherType == MotherType.One.ToString())
                {
                    DateTime date = DateTime.Now.AddDays(-180);
                    msgResult.BeginBabyBirthday = date;
                    msgResult.EndBabyBirthday = DateTime.Now;
                }
                else if (msgResult.MotherType == MotherType.Two.ToString())
                {
                    DateTime date = DateTime.Now.AddDays(-365);
                    DateTime date2 = DateTime.Now.AddDays(-180);
                    msgResult.BeginBabyBirthday = date;
                    msgResult.EndBabyBirthday = date2;
                }
                else if (msgResult.MotherType == MotherType.Three.ToString())
                {
                    DateTime date = DateTime.Now.AddDays(-730);
                    DateTime date2 = DateTime.Now.AddDays(-365);
                    msgResult.BeginBabyBirthday = date;
                    msgResult.EndBabyBirthday = date2;
                }
                else if (msgResult.MotherType == MotherType.Four.ToString())
                {
                    DateTime date = DateTime.Now.AddDays(-730);
                    msgResult.BeginBabyBirthday = new DateTime(1900, 1, 1);
                    msgResult.EndBabyBirthday = date;
                }

                var obj = new
                {
                    BaySex = msgResult.BaySex,
                    OrganizeCity = !string.IsNullOrWhiteSpace(msgResult.OrganizeCity) ? msgResult.OrganizeCity.Split(',') : null,
                    OfficialCity = !string.IsNullOrWhiteSpace(msgResult.OfficialCity) ? msgResult.OfficialCity.Split(',') : null,
                    LastBuyProduct = !string.IsNullOrWhiteSpace(msgResult.LastBuyProduct) ? msgResult.LastBuyProduct.Split(',') : null,
                    MemberCategory = !string.IsNullOrWhiteSpace(msgResult.MemberCategory) ? msgResult.MemberCategory.Split(',') : null,
                    BeginBabyBirthday = msgResult.BeginBabyBirthday,
                    EndBabyBirthday = msgResult.EndBabyBirthday,
                    BeginPointsBalance = msgResult.BeginPointsBalance,
                    EndPointsBalance = msgResult.EndPointsBalance
                };
                var str = JsonConvert.SerializeObject(obj);

                var openIds = await client.FindWeChatMemberByAdvConditionAsync(authKey, str);
                openIdArray.AddRange(openIds);
            }
            #endregion


            #endregion

            MessageResponseModel model = null;
            if (openIdArray.Count > 0)
            {
                openIdArray = openIdArray.Distinct().ToList();
                model = new MessageResponseModel
                {
                    TaskID = taskId,
                    OpenIDs = openIdArray.ToArray(),
                    Count = openIdArray.Count()
                };

            }

            return model;

        }
        #endregion
    }



    public class TemplateMessage
    {
        public TemplateMessageItem first { get; set; }
        public TemplateMessageItem keyword1 { get; set; }
        public TemplateMessageItem keyword2 { get; set; }
        public TemplateMessageItem keyword3 { get; set; }
        public TemplateMessageItem keyword4 { get; set; }
        public TemplateMessageItem remark { get; set; }
    }
    public class TemplateMessageItem
    {
        public string value { get; set; }
        public string color { get; set; }
    }
}
