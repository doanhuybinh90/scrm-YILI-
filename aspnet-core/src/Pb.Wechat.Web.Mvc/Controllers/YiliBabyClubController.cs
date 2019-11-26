using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Controllers;
using System.Security.Cryptography;
using Newtonsoft.Json;
using yilibabyUser;
using yilibabyMember;
using yilibabyOffice;
using yilibabyVCIF;
using Abp.Runtime.Caching;
using Pb.Wechat.Web.Resources;
using Pb.Wechat.MpUserMembers;
using Pb.Wechat.MpBabyTexts;
using System.Collections.Generic;
using Pb.Wechat.MpShoppingMallPics;
using Pb.Wechat.MpFans;
using Pb.Wechat.MpAccessTokenClib;
using Senparc.Weixin.MP.AdvancedAPIs;
using Pb.Wechat.MpApiTokens.Dto;
using Pb.Wechat.MpApiTokens;
using Pb.Wechat.MpBabyTexts.Dto;
using Pb.Wechat.MpShoppingMallPics.Dto;
using Pb.Wechat.Url;
using Pb.Wechat.MpProductInfos;
using Pb.Wechat.MpProductInfos.Dto;
using Pb.Wechat.MpCourseSignups;
using Abp.Domain.Uow;
using Pb.Wechat.CustomerServiceOnlines;
using Pb.Wechat.MpCourseSignups.Dto;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pb.Wechat.Web.Controllers
{
    [IgnoreAntiforgeryToken]
    public class YiliBabyClubController : AbpController
    {
        private static string defaultUserName = "";
        private static string defaultUserPwd = "";
        private static string deviceCode = "";
        private static string yiliwechat = "";
        private string MamaBanToYun2OneMediaID = "";
        private string JNHToLongLineMediaID = "";
        //private static string deviceCode = "358352061664911";
        private ICacheManager _cacheManager;
        private readonly IMpUserMemberAppService _mpUserMemberAppService;
        private readonly IMpBabyTextAppService _mpBabyTextAppService;
        private readonly IMpShoppingMallPicAppService _mpShoppingMallPicAppService;
        private readonly IMpFanAppService _mpFanAppService;
        private readonly IAccessTokenContainer _accessTokenContainer;
        private readonly IMpApiTokenAppService _mpApiTokenAppService;
        private readonly IMatialFileService _matialFileService;
        private readonly IYiliBabyClubInterfaceService _yiliBabyClubInterfaceService;
        private readonly IMpProductInfoAppService _mpProductInfoAppService;
        private readonly IMpCourseSignupAppService _mpCourseSignupAppService;
        private readonly ICustomerServiceOnlineAppService _customerServiceOnlineAppService;
        private readonly IWebUrlService _webUrlService;
        public YiliBabyClubController(ICacheManager cacheManager, IMpUserMemberAppService mpUserMemberAppService, IMpBabyTextAppService mpBabyTextAppService, IMpShoppingMallPicAppService mpShoppingMallPicAppService, IMpFanAppService mpFanAppService, IAccessTokenContainer accessTokenContainer, IMpApiTokenAppService mpApiTokenAppService, IMatialFileService matialFileService, IYiliBabyClubInterfaceService yiliBabyClubInterfaceService, IMpProductInfoAppService mpProductInfoAppService, IMpCourseSignupAppService mpCourseSignupAppService, ICustomerServiceOnlineAppService customerServiceOnlineAppService, IWebUrlService webUrlService)
        {
            _cacheManager = cacheManager;
            _mpUserMemberAppService = mpUserMemberAppService;
            _mpBabyTextAppService = mpBabyTextAppService;
            _mpShoppingMallPicAppService = mpShoppingMallPicAppService;
            _mpFanAppService = mpFanAppService;
            _accessTokenContainer = accessTokenContainer;
            _mpApiTokenAppService = mpApiTokenAppService;
            _matialFileService = matialFileService;
            _yiliBabyClubInterfaceService = yiliBabyClubInterfaceService;
            _mpProductInfoAppService = mpProductInfoAppService;
            _mpCourseSignupAppService = mpCourseSignupAppService;
            _customerServiceOnlineAppService = customerServiceOnlineAppService;

            MamaBanToYun2OneMediaID = _matialFileService.MaMaBanToYun2One;
            JNHToLongLineMediaID = _matialFileService.JNHToLongLine;
            defaultUserName = _yiliBabyClubInterfaceService.ApiUserName;
            defaultUserPwd = _yiliBabyClubInterfaceService.ApiPassword;
            deviceCode = _yiliBabyClubInterfaceService.ApiDeviceCode;
            yiliwechat = _yiliBabyClubInterfaceService.Yiliwechat;
            _webUrlService = webUrlService;
        }
        #region 注册
        public IActionResult RegistFromWX()
        {
            ViewBag.RootPath = _webUrlService.GetSiteRootAddress();
            ViewBag.MallApiToken = _yiliBabyClubInterfaceService.MallApiToken;
            ViewBag.OAuthPath = _yiliBabyClubInterfaceService.OAuthPath;
            return View();
        }
        public IActionResult RegistFromApp()
        {
            ViewBag.RootPath = _webUrlService.GetSiteRootAddress();
            ViewBag.MallApiToken = _yiliBabyClubInterfaceService.MallApiToken;
            ViewBag.OAuthPath = _yiliBabyClubInterfaceService.OAuthPath;
            return View();
        }

        public IActionResult Mall()
        {
            ViewBag.RootPath = _webUrlService.GetSiteRootAddress();
            ViewBag.MallApiToken = _yiliBabyClubInterfaceService.MallApiToken;
            ViewBag.OAuthPath = _yiliBabyClubInterfaceService.OAuthPath;
            return View();
        }

        public IActionResult Mother()
        {
            ViewBag.RootPath = _webUrlService.GetSiteRootAddress();
            return View();
        }
        public IActionResult Error()
        {
            return View();
        }
        #endregion

        #region 登陆以及加密
        #region 获取AES回执
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
        #endregion

        #region 用户登录并获取美驰authkey
        /// <summary>
        /// 用户登录并获取美驰authkey
        /// </summary>
        /// <param name="mgccAuthKey"></param>
        /// <param name="newKey"></param>
        /// <returns></returns>
        private async Task<string> UserLoginAndGetMCKey(string mgccAuthKey, bool newKey = false)
        {
            var userName = string.Empty;
            var userPwd = string.Empty;
            var openid = await _cacheManager.GetCache(AppConsts.Cache_AuthKey2OpenID).GetOrDefaultAsync(mgccAuthKey);
            Logger.Info($"获取Cache_AuthKey2OpenID的OpenID{openid}");
            if (openid != null)//MgccKey没过期
            {
                Logger.Info($"OpenID缓存存在，进入会员登录");
                var _memberInfo = await _mpUserMemberAppService.GetByOpenID(openid.ToString());
                if (_memberInfo != null)//会员->用会员帐户名登陆
                {
                    if (newKey == false)
                    {
                        var mckey = await _cacheManager.GetCache(AppConsts.Cache_MCAuthKey).GetOrDefaultAsync(mgccAuthKey);

                        if (mckey != null)
                            return (string)mckey;
                        else
                            return await MemberUserLoginAndGetMCKey(_memberInfo, mgccAuthKey);

                    }
                    else
                        return await MemberUserLoginAndGetMCKey(_memberInfo, mgccAuthKey);
                }
                else//用默认帐户名登陆
                {
                    Logger.Info("不是会员登录:" + mgccAuthKey);
                    return await DefaultUserLoginAndGetMCKey(newKey);
                }
            }
            else
            {
                Logger.Info($"OpenID缓存不存在");
                return "";
            }
        }


        #region 会员个人账号登录并获取美驰authkey
        /// <summary>
        /// 会员个人账号登录并获取美驰authkey
        /// </summary>
        /// <returns></returns>
        private async Task<string> MemberUserLoginAndGetMCKey(MpUserMembers.Dto.MpUserMemberDto _memberInfo, string mgccAuthKey)
        {
            var AesResult = await GetAesKey();
            UserLoginSoapClient client = new UserLoginSoapClient(UserLoginSoapClient.EndpointConfiguration.UserLoginSoap);
            var EncryptPassword = "";
            try
            {
                EncryptPassword = AesCryptoService.Encrypt(_memberInfo.MemberPassword, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }
            catch
            {
                AesResult = await GetAesKey(true);
                EncryptPassword = AesCryptoService.Encrypt(_memberInfo.MemberPassword, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }
            //获取最新的AuthKey
            var result = await client.LoginExAsync(new LoginExRequest
            {
                UserName = _memberInfo.MemeberUserName,
                EncryptPassword = EncryptPassword,
                DeviceCode = deviceCode
            });
            if (result.LoginExResult == 0 && !string.IsNullOrWhiteSpace(result.AuthKey))
            {
                await _cacheManager.GetCache(AppConsts.Cache_MCAuthKey).SetAsync(mgccAuthKey, result.AuthKey);
                return result.AuthKey;
            }
            return string.Empty;
        }
        /// <summary>
        /// 根据会员用户名密码获取美驰key
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private async Task<string> MemberUserLoginAndGetMCKey(string userName, string password)
        {
            var AesResult = await GetAesKey();
            UserLoginSoapClient client = new UserLoginSoapClient(UserLoginSoapClient.EndpointConfiguration.UserLoginSoap);
            var EncryptPassword = "";
            try
            {
                EncryptPassword = AesCryptoService.Encrypt(password, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }
            catch
            {
                AesResult = await GetAesKey(true);
                EncryptPassword = AesCryptoService.Encrypt(password, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }
            //获取最新的AuthKey
            var result = await client.LoginExAsync(new LoginExRequest
            {
                UserName = userName,
                EncryptPassword = EncryptPassword,
                DeviceCode = deviceCode
            });
            if (result.LoginExResult == 0 && !string.IsNullOrWhiteSpace(result.AuthKey))
            {
                return result.AuthKey;
            }
            return string.Empty;
        }
        #endregion

        #region 美驰默认账号登录并获取美驰authkey
        /// <summary>
        /// 美驰默认账号登录并获取美驰authkey
        /// </summary>
        /// <returns></returns>
        private async Task<string> DefaultUserLoginAndGetMCKey(bool getnew = true)
        {
            if (!getnew)
            {
                var mckey = await _cacheManager.GetCache(AppConsts.Cache_MCAuthKey).GetOrDefaultAsync("DefaultMCAuthKey");
                if (mckey != null)
                {
                    return (string)mckey;
                }
            }

            UserLoginSoapClient client = new UserLoginSoapClient(UserLoginSoapClient.EndpointConfiguration.UserLoginSoap);
            var EncryptPassword = "";
            var AesResult = await GetAesKey();
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
            if (result.LoginExResult == 0 && !string.IsNullOrWhiteSpace(result.AuthKey))
            {
                await _cacheManager.GetCache(AppConsts.Cache_MCAuthKey).SetAsync("DefaultMCAuthKey", result.AuthKey);
                return result.AuthKey;
            }
            else
                return "";
        }
        #endregion
        #endregion

        [HttpPost]
        public async Task<int> UserLogin()
        {
            string mgccAuthKey = Request.Form["AuthKey"].ToString();
            string userName = Request.Form["UserName"].ToString();
            string password = Request.Form["Pwd"].ToString();

            Logger.Info($"用户登陆-登陆信息：用户名：{userName},密码：{password}");
            var AesResult = await GetAesKey();
            var openid = (string)await _cacheManager.GetCache(AppConsts.Cache_AuthKey2OpenID).GetOrDefaultAsync(mgccAuthKey);
            if (string.IsNullOrEmpty(openid))
            {
                Logger.Info($"用户登陆-登陆失败：找不到openid");
                return -101;
            }

            UserLoginSoapClient client = new UserLoginSoapClient(UserLoginSoapClient.EndpointConfiguration.UserLoginSoap);
            var EncryptPassword = "";
            try
            {
                EncryptPassword = AesCryptoService.Encrypt(password, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }
            catch
            {
                AesResult = await GetAesKey(true);
                EncryptPassword = AesCryptoService.Encrypt(password, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }
            //获取最新的AuthKey
            var result = await client.LoginExAsync(new LoginExRequest
            {
                UserName = userName,
                EncryptPassword = EncryptPassword,
                DeviceCode = deviceCode
            });
            Logger.Info($"用户登陆-登陆信息：返回内容：{JsonConvert.SerializeObject(result)}");
            //var mckey = result.AuthKey;

            if (result.LoginExResult == 0)
            {
                await SetMemberInfo(openid, userName, password, mgccAuthKey, AesResult);
                #region 老代码

                //Logger.Info($"用户登陆-登陆成功");
                //await _cacheManager.GetCache(AppConsts.Cache_MCAuthKey).SetAsync(mgccAuthKey, result.AuthKey);
                //try
                //{
                //    if (!string.IsNullOrEmpty(mckey))
                //    {
                //        _memberInfo.MemeberUserName = userName;
                //        _memberInfo.MemberPassword = password;
                //        await _cacheManager.GetCache(AppConsts.Cache_MpUserMemberKey).SetAsync(openid, _memberInfo);
                //        return result.LoginExResult;
                //    }

                //}
                //catch (Exception e)
                //{
                //    Logger.Error($"登陆接口错误：{e.Message},堆栈：{e.StackTrace}");
                //    return result.LoginExResult;

                //}
                #endregion

                return result.LoginExResult;
            }
            else
            {
                Logger.Info($"用户登陆-登陆失败：{result.LoginExResult}");
                return result.LoginExResult;
            }

        }
        [HttpPost]
        public async Task<ResetPasswordResponse> ResetPassword()
        {
            string mgccAuthKey = Request.Form["AuthKey"].ToString();
            string mobilePhone = Request.Form["Mobile"].ToString();
            string newPassword = Request.Form["NewPwd"].ToString();
            int verifyID = int.Parse(Request.Form["VerifyID"].ToString());
            string verifyCode = Request.Form["VerifyCode"].ToString();

            var AesResult = await GetAesKey();
            UserLoginSoapClient client2 = new UserLoginSoapClient(UserLoginSoapClient.EndpointConfiguration.UserLoginSoap);
            var EncryptPassword = "";
            try
            {
                EncryptPassword = AesCryptoService.Encrypt(newPassword, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }
            catch
            {
                AesResult = await GetAesKey(true);
                EncryptPassword = AesCryptoService.Encrypt(newPassword, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }
            var data = await client2.ResetPasswordAsync(new ResetPasswordRequest
            {
                AuthKey = await DefaultUserLoginAndGetMCKey(false),
                mobile = mobilePhone,
                newpwd = EncryptPassword,
                VerifyCode = verifyCode,
                VerifyID = verifyID
            });
            if (data.ResetPasswordResult != 0)
            {
                data = await client2.ResetPasswordAsync(new ResetPasswordRequest
                {
                    AuthKey = await DefaultUserLoginAndGetMCKey(),
                    mobile = mobilePhone,
                    newpwd = EncryptPassword,
                    VerifyCode = verifyCode,
                    VerifyID = verifyID
                });
            }
            var resultData = data.ResetPasswordResult == 0;
            if (resultData)
            {
                var openid = await _cacheManager.GetCache(AppConsts.Cache_AuthKey2OpenID)
                  .GetOrDefaultAsync<string, string>(mgccAuthKey);
                await SetMemberInfo(openid, data.username, newPassword, mgccAuthKey, AesResult, mobilePhone);
            }

            return data;



        }
        [HttpPost]
        public async Task<int> BindMember()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            MemberServiceSoapClient client2 = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
            var openid = await _cacheManager.GetCache(AppConsts.Cache_AuthKey2OpenID)
                .GetOrDefaultAsync<string, string>(AuthKey);
            var result = await client2.MemberBindWeChatAsync(await UserLoginAndGetMCKey(AuthKey), openid, yiliwechat, "");
            if (result == -100)
                result = await client2.MemberBindWeChatAsync(await UserLoginAndGetMCKey(AuthKey, true), openid, yiliwechat, "");
            return result;
        }
        #endregion

        #region 用户接口
        [HttpPost]
        public async Task<int> Login_GetNewMsg()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            UserLoginSoapClient client = new UserLoginSoapClient(UserLoginSoapClient.EndpointConfiguration.UserLoginSoap);
            var result = await client.Login_GetNewMsgAsync(await UserLoginAndGetMCKey(AuthKey));
            if (result == -100)
                result = await client.Login_GetNewMsgAsync(await UserLoginAndGetMCKey(AuthKey, true));
            return result;

        }
        [HttpPost]
        public async Task<int> Logout()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            UserLoginSoapClient client = new UserLoginSoapClient(UserLoginSoapClient.EndpointConfiguration.UserLoginSoap);
            return await client.LogoutAsync(await UserLoginAndGetMCKey(AuthKey));
        }
        [HttpPost]
        public async Task<bool> CheckUsername()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            string username = Request.Form["username"].ToString();
            UserLoginSoapClient client = new UserLoginSoapClient(UserLoginSoapClient.EndpointConfiguration.UserLoginSoap);
            return await client.CheckUsernameAsync(await UserLoginAndGetMCKey(AuthKey), username);


        }
        [HttpPost]
        public async Task<bool> CheckMobile()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            string mobile = Request.Form["mobile"].ToString();
            UserLoginSoapClient client = new UserLoginSoapClient(UserLoginSoapClient.EndpointConfiguration.UserLoginSoap);
            return await client.CheckMobileAsync(await UserLoginAndGetMCKey(AuthKey), mobile);
        }
        [HttpPost]
        public async Task<bool> CheckEmail()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            string email = Request.Form["email"].ToString();
            UserLoginSoapClient client = new UserLoginSoapClient(UserLoginSoapClient.EndpointConfiguration.UserLoginSoap);
            return await client.CheckEmailAsync(await UserLoginAndGetMCKey(AuthKey), email);


        }
        [HttpPost]
        public async Task<int> UserRegister()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            string mobile = Request.Form["mobile"].ToString();
            string username = Request.Form["username"].ToString();
            string pwd = Request.Form["pwd"].ToString();
            string email = Request.Form["email"].ToString();
            var AesResult = await GetAesKey();
            UserLoginSoapClient client = new UserLoginSoapClient(UserLoginSoapClient.EndpointConfiguration.UserLoginSoap);
            Logger.Info("注册传入Mgcckey:" + AuthKey);
            Logger.Info("注册传入mobile:" + mobile);
            Logger.Info("注册传入username:" + username);
            Logger.Info("注册传入pwd:" + pwd);
            var EncryptPassword = "";
            try
            {
                EncryptPassword = AesCryptoService.Encrypt(pwd, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }
            catch
            {
                AesResult = await GetAesKey(true);
                EncryptPassword = AesCryptoService.Encrypt(pwd, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }

            var result = await client.UserRegisterAsync(await UserLoginAndGetMCKey(AuthKey), mobile, username, EncryptPassword, email);
            if (result == -100)
                result = await client.UserRegisterAsync(await UserLoginAndGetMCKey(AuthKey, true), mobile, username, EncryptPassword, email);
            if (result == 0)
            {
                Logger.Info("注册成功");
                var openid = await _cacheManager.GetCache(AppConsts.Cache_AuthKey2OpenID)
                   .GetOrDefaultAsync<string, string>(AuthKey);
                await SetMemberInfo(openid, username, pwd, AuthKey, AesResult, mobilePhone: username);

                #region 老代码
                //#region 绑定openid
                //MemberServiceSoapClient client2 = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
                //var openid = await _cacheManager.GetCache(AppConsts.Cache_AuthKey2OpenID)
                //    .GetOrDefaultAsync<string, string>(AuthKey);
                //Logger.Info($"绑定时获取OpenID：{openid}");
                //var bindResult = await client2.MemberBindWeChatAsync(authkey, openid, yiliwechat, "");
                //Logger.Info($"绑定OpenID结果：{bindResult}");
                //#endregion

                //var fan = await _mpFanAppService.GetFirstOrDefault(new MpFans.Dto.GetMpFansInput { OpenID = openid });

                //#region 新增会员信息
                //var member = await _mpUserMemberAppService.Create(new MpUserMembers.Dto.MpUserMemberDto()
                //{
                //    OpenID = openid,
                //    MemeberUserName = username,
                //    MemberPassword = pwd,
                //    RegisterTime = DateTime.Now,
                //    CreationTime = DateTime.Now,
                //    IsDeleted = false,
                //    MgccAuthkey = AuthKey,
                //    Email = email,
                //    ChannelID = Convert.ToInt32(fan.ChannelID),
                //    ChannelName = fan.ChannelName,
                //    SubscribeTime = fan.SubscribeTime,
                //    IsBinding = true,
                //    BindingTime = DateTime.Now
                //});
                //await _mpUserMemberAppService.DeleteOtherSame(openid, member.Id);
                //Logger.Info("新增会员到数据库");
                //#endregion

                //#region 重置memberInfo缓存
                //var memberInfo = await _mpUserMemberAppService.GetByOpenID(openid);
                //memberInfo.MemeberUserName = username;
                //memberInfo.MemberPassword = pwd;
                //memberInfo.Email = email;
                //memberInfo.MobilePhone = mobile;
                //memberInfo.MgccAuthkey = AuthKey;
                //await _cacheManager.GetCache(AppConsts.Cache_MpUserMemberKey).SetAsync(openid, memberInfo);
                //Logger.Info("重置memberinfo缓存");
                //#endregion

                //#region 更新美池key缓存
                //authkey = await UserLoginAndGetMCKey(AuthKey, true);
                //Logger.Info($"更新美池key缓存：{authkey}");
                //#endregion

                //#region 更新粉丝信息

                //fan.MemberID = member.Id;

                //await _mpFanAppService.Update(fan);
                //#endregion
                #endregion

            }
            return result;
        }

        [HttpPost]
        public async Task<string> GetAttachmentDownloadURL()
        {
            string Guid = Request.Form["Guid"].ToString();
            UserLoginSoapClient client = new UserLoginSoapClient(UserLoginSoapClient.EndpointConfiguration.UserLoginSoap);
            return await client.GetAttachmentDownloadURLAsync(Guid);

        }
        #endregion

        #region 城市接口
        [HttpPost]
        public async Task<string> GetSubCitysBySuperJson()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            int CityID = 1;
            int.TryParse(Request.Form["CityID"].ToString(), out CityID);

            OfficialCityServiceSoapClient client = new OfficialCityServiceSoapClient(OfficialCityServiceSoapClient.EndpointConfiguration.OfficialCityServiceSoap);

            var mckey = await UserLoginAndGetMCKey(AuthKey);
            var result = await client.GetSubCitysBySuperJsonAsync(mckey, CityID);
            if (!string.IsNullOrWhiteSpace(result) && result != "null")
            {
                var AesResult = await GetAesKey();
                try
                {
                    return AesCryptoService.Decrypt(result, AesResult.CryptAESKey, AesResult.CryptAESIV);
                }
                catch
                {
                    AesResult = await GetAesKey(true);
                    return AesCryptoService.Decrypt(result, AesResult.CryptAESKey, AesResult.CryptAESIV);
                }
            }
            else
                return result;
        }
        [HttpPost]
        public async Task<string> GetCityFullName()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            int CityID = -1;
            int.TryParse(Request.Form["CityID"].ToString(), out CityID);
            OfficialCityServiceSoapClient client = new OfficialCityServiceSoapClient(OfficialCityServiceSoapClient.EndpointConfiguration.OfficialCityServiceSoap);
            return await client.GetCityFullNameAsync(await UserLoginAndGetMCKey(AuthKey), CityID);
        }
        #endregion

        #region 短信接口
        [HttpPost]
        public async Task<int> SendVerifyCodeWithParam()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            int Classify = -1;
            int.TryParse(Request.Form["Classify"].ToString(), out Classify);
            string Mobile = Request.Form["Mobile"].ToString();
            string MessageParam = Request.Form["MessageParam"].ToString();

            VCIFServiceSoapClient client = new VCIFServiceSoapClient(VCIFServiceSoapClient.EndpointConfiguration.VCIFServiceSoap);
            var result = await client.SendVerifyCodeWithParamAsync(await UserLoginAndGetMCKey(AuthKey), Classify, Mobile, MessageParam);
            if (result == -100)
                result = await client.SendVerifyCodeWithParamAsync(await UserLoginAndGetMCKey(AuthKey, true), Classify, Mobile, MessageParam);
            return result;
        }
        [HttpPost]
        public async Task<int> ReSendVerifyCode()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            int VerifyID = -1;
            int.TryParse(Request.Form["VerifyID"].ToString(), out VerifyID);
            VCIFServiceSoapClient client = new VCIFServiceSoapClient(VCIFServiceSoapClient.EndpointConfiguration.VCIFServiceSoap);
            var result = await client.ReSendVerifyCodeAsync(await UserLoginAndGetMCKey(AuthKey), VerifyID);
            if (result == -100)
                result = await client.ReSendVerifyCodeAsync(await UserLoginAndGetMCKey(AuthKey, true), VerifyID);
            return result;
        }
        [HttpPost]
        public async Task<int> VerifyCode()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            int VerifyID = -1;
            int.TryParse(Request.Form["VerifyID"].ToString(), out VerifyID);
            string Mobile = Request.Form["Mobile"].ToString();
            string Code = Request.Form["Code"].ToString();

            VCIFServiceSoapClient client = new VCIFServiceSoapClient(VCIFServiceSoapClient.EndpointConfiguration.VCIFServiceSoap);
            var result = await client.VerifyCodeAsync(await UserLoginAndGetMCKey(AuthKey), VerifyID, Mobile, Code);
            if (result == -100)
                result = await client.VerifyCodeAsync(await UserLoginAndGetMCKey(AuthKey, true), VerifyID, Mobile, Code);
            return result;
        }
        #endregion

        #region 会员接口
        [HttpPost]
        public async Task<Member> GetMyMemberInfoJson()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
            return await client.GetMyMemberInfoAsync(await UserLoginAndGetMCKey(AuthKey));
        }
        [HttpPost]
        public async Task<int> UpdateMemberInfoJson()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            string Member = Request.Form["Member"].ToString();
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
            Logger.Info("会员更新:" + AuthKey);
            var mckey = await UserLoginAndGetMCKey(AuthKey);
            var AesResult = await GetAesKey();
            var aesMember = "";
            try
            {
                aesMember = AesCryptoService.Encrypt(Member, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }
            catch
            {
                AesResult = await GetAesKey(true);
                aesMember = AesCryptoService.Encrypt(Member, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }
            Logger.Info("传入更新的会员信息：" + Member);

            var result = await client.UpdateMemberInfoJsonAsync(mckey, aesMember);
            Logger.Info($"更新的结果：{result}");


            #region 获取更新后美驰的会员信息
            var jsonStr = await client.GetMyMemberInfoJsonAsync(mckey);
            if (!string.IsNullOrWhiteSpace(jsonStr) && jsonStr != "null")
            {
                var aesResult = await GetAesKey();
                string plainJson = AesCryptoService.Decrypt(jsonStr, aesResult.CryptAESKey, aesResult.CryptAESIV);
                Logger.Info($"获取美驰的更新后的会员信息：{plainJson}");
            }
            #endregion


            var openid = await _cacheManager.GetCache(AppConsts.Cache_AuthKey2OpenID).GetOrDefaultAsync(AuthKey);
            var member = await _mpUserMemberAppService.GetByOpenID(openid.ToString());

            var newmember = JsonConvert.DeserializeObject<MeiChiMember>(Member);
            member.MemberType = newmember.MemberType;
            member.BabyName = newmember.BabyName;
            member.BabyBirthday = newmember.Birthday;
            member.MemberName = newmember.RealName;
            member.MemberIdentity = newmember.HomeRole.ToString();
            member.RegistProductName = newmember.CollectProductName;
            member.RegistProduct = newmember.CollectProduct;
            member.UsedBrand = newmember.PreBrand;
            member.FeedingMode = newmember.FeedMode;
            member.MonthConsumption = newmember.EatQuantityOneMonth;
            member.DayConsumption = newmember.EatTimesOneDay;
            member.OfficialCity = newmember.OfficialCity;
            member.Address = newmember.Address;
            member.LikeChannel = newmember.ServicePreferences.ToString();
            member.ServiceShop = newmember.serviceCMClientName;
            if (!string.IsNullOrWhiteSpace(newmember.OfficialCityName))
            {
                var areaArray = newmember.OfficialCityName.Split(' ');
                member.Province = areaArray.Length > 0 ? areaArray[0]:string.Empty;
                member.City = areaArray.Length > 1 ? areaArray[1] : string.Empty;
                member.Area = areaArray.Length > 2 ? areaArray[2] : string.Empty;
            }
            //member.ServiceShopCode = newmember.serviceCMClientCode;
            member.CRMID = newmember.CRMID;
            var fans = await _mpFanAppService.GetFirstOrDefaultByOpenID(openid.ToString());
            if (fans != null)
            {
                fans.MemberID = newmember.CRMID;
                await _mpFanAppService.Update(fans);
            }
            else
            {
                await _mpFanAppService.Create(new MpFans.Dto.MpFanDto
                {
                    OpenID = openid.ToString(),
                    IsFans = true,
                    MpID = 1,
                    MemberID = newmember.CRMID,
                    LastModificationTime = DateTime.Now
                });
            }
            await _mpUserMemberAppService.Update(member);

            return result;
        }
        [HttpPost]
        public async Task<int> SendUserRegisterMail()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
            return await client.SendUserRegisterMailAsync(await UserLoginAndGetMCKey(AuthKey));
        }
        [HttpPost]
        public async Task<int> MemberBindWeChat()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            string UserWeChat = Request.Form["UserWeChat"].ToString();
            string CompanyWeChat = Request.Form["CompanyWeChat"].ToString();
            string Remark = Request.Form["Remark"].ToString();

            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
            return await client.MemberBindWeChatAsync(await UserLoginAndGetMCKey(AuthKey), UserWeChat, CompanyWeChat, Remark);

        }
        [HttpPost]
        public async Task<string> GetMostlyProductListJson()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);

            var result = await client.GetMostlyProductListJsonAsync(await UserLoginAndGetMCKey(AuthKey));
            var AesResult = await GetAesKey();
            try
            {
                return AesCryptoService.Decrypt(result, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }
            catch
            {
                AesResult = await GetAesKey(true);
                return AesCryptoService.Decrypt(result, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }
        }


        private async Task<MpAccountTokenOutput> GetAccountToken(string Token, string ApiType)
        {
            return await _mpApiTokenAppService.GetAccountToken(new MpAccountTokenInput()
            {
                Token = Token,
                ApiType = ApiType
            });
        }

        [HttpPost]
        public async Task<int> CustomerJoinActivitySignAsync()
        {
            Logger.Info("接口活动签到---开始");

            string AuthKey = Request.Form["AuthKey"].ToString();
            string openId = Request.Form["OpenID"].ToString();
            string token = Request.Form["Token"].ToString();
            //int clientId = 0;
            int activityId = 0;
            int infoSource = 51;
            int classify = 0;
            //int.TryParse(Request.Form["ClientId"].ToString(), out clientId);
            int.TryParse(Request.Form["ActivityId"].ToString(), out activityId);
            //int.TryParse(Request.Form["InfoSource"].ToString(), out infoSource);
            int.TryParse(Request.Form["Classify"].ToString(), out classify);

            var AesResult = await GetAesKey();
            var aesMember = "";
            Logger.Info("接口活动签到---开始获取accesstoken");
            Logger.Info($"传入的AuthKey为：{AuthKey}");
            Logger.Info($"传入的OpenID为：{openId}");
            Logger.Info($"传入的ActivityId为：{activityId}");
            Logger.Info($"传入的Classify为：{classify}");
            Logger.Info($"传入的token为：{token}");
            try
            {
                var account = await GetAccountToken(token, MpApiTokenType.Nomal.ToString());
                Logger.Info($"获取的account为：{JsonConvert.SerializeObject(account)}");
                var access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret);
                Logger.Info($"接口活动签到---accesstoken为{access_token}");
                MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
                var mckey = await UserLoginAndGetMCKey(AuthKey);
                Logger.Info($"接口活动签到---美池Key为{mckey}");
                var memberInfo = await client.GetMyMemberInfoAsync(mckey);

                if (memberInfo.InfoCollectActivity == 0)
                {
                    string jsonStr = JsonConvert.SerializeObject(memberInfo);
                    aesMember = AesCryptoService.Encrypt(jsonStr, AesResult.CryptAESKey, AesResult.CryptAESIV);
                    memberInfo.InfoCollectActivity = activityId;
                    var joinresult = await client.UpdateMemberInfoJsonAsync(mckey, aesMember);
                    if (joinresult == -100)
                    {
                        mckey = await UserLoginAndGetMCKey(AuthKey, true);
                        joinresult = await client.UpdateMemberInfoJsonAsync(mckey, aesMember);
                    }
                }
                var ret = await client.CustomerJoinActivitySignAsync(mckey, memberInfo.CRMID, activityId, infoSource);
                Logger.Info($"，签到活动：{activityId}，签到返回数据：{ret}");
                if (classify == 1)//妈妈班推送
                {

                    string msg = "恭喜您签到成功！";
                    if (ret > 0)
                    {
                        msg = "恭喜您签到成功！";
                    }
                    else if (ret == 0)
                    {
                        msg = "您已经签到过本次活动了哦！";
                    }

                    if (!string.IsNullOrEmpty(memberInfo.ServicePromotorName))
                    {
                        //恭喜您签到成功！您的专属营养顾问是张三（华润万达广场店）联系电话：13312345678。
                        msg += string.Format("您的专属营养顾问是{0}（{1}）联系电话：{2}。", memberInfo.ServicePromotorName, memberInfo.ServiceCMClientName, memberInfo.ServicePromotorMobile);
                    }
                    await CustomApi.SendTextAsync(access_token, openId, msg);

                    //孕晚期推送
                    DateTime? babyBirthday = memberInfo.Birthday;

                    if (babyBirthday != null)
                    {
                        if (babyBirthday > DateTime.Now && babyBirthday < DateTime.Now.AddDays(105))
                        {

                            await CustomApi.SendImageAsync(access_token, openId, MamaBanToYun2OneMediaID);
                        }
                    }
                }
                else
                {
                    string msg = "恭喜您签到成功！";
                    if (ret > 0)
                    {
                        msg = "恭喜您签到成功！";
                    }
                    else if (ret == 0)
                    {
                        msg = "您已经签到过本次活动了哦！";
                    }
                    else if (ret == -11)
                    {
                        msg = "您当前参与的活动尚未结束，无法参加其他嘉年华活动哦！";
                    }
                    else if (ret == -12)
                    {
                        msg = "您本月参与嘉年华活动次数已达上限了哦！";
                    }

                    await CustomApi.SendTextAsync(access_token, openId, msg);


                    if (ret > 0)//仅这对成功签到
                    {
                        await CustomApi.SendImageAsync(access_token, openId, JNHToLongLineMediaID);
                    }
                }
                return ret;
            }
            catch (Exception e)
            {

                Logger.Error(e.Message);
                throw e;
            }


        }
        #endregion

        #region 积分接口
        [HttpPost]
        public async Task<decimal> GetPointsBalance()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
            var balance = await client.GetPointsBalanceAsync(await UserLoginAndGetMCKey(AuthKey));
            if (balance == -100)
                balance = await client.GetPointsBalanceAsync(await UserLoginAndGetMCKey(AuthKey, true));
            return balance;
        }
        [HttpPost]
        public async Task<string> GetPointsInfoJson()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);

            var result = await client.GetPointsInfoJsonAsync(await UserLoginAndGetMCKey(AuthKey, true));
            var AesResult = await GetAesKey();
            try
            {
                return AesCryptoService.Decrypt(result, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }
            catch
            {
                AesResult = await GetAesKey(true);
                return AesCryptoService.Decrypt(result, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }

        }
        [HttpPost]
        public async Task<AddProductPointsResponse> AddProductPoints()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            string ProductCode = Request.Form["ProductCode"].ToString();
            string Retailer = Request.Form["Retailer"].ToString();
            int Channel = -1;
            int.TryParse(Request.Form["Channel"].ToString(), out Channel);
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
            return await client.AddProductPointsAsync(new AddProductPointsRequest
            {
                AuthKey = await UserLoginAndGetMCKey(AuthKey, true),
                Retailer = Retailer,
                ProductCode = ProductCode
            });
        }

        [HttpPost]
        public async Task<string> GetPointsChangeDetailJson()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            DateTime BeginDate = DateTime.MinValue;
            DateTime EndDate = DateTime.MinValue;
            DateTime.TryParse(Request.Form["BeginDate"].ToString(), out BeginDate);
            DateTime.TryParse(Request.Form["EndDate"].ToString(), out EndDate);
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);

            var result = await client.GetPointsChangeDetailJsonAsync(await UserLoginAndGetMCKey(AuthKey), BeginDate, EndDate);
            var AesResult = await GetAesKey();
            try
            {
                return AesCryptoService.Decrypt(result, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }
            catch
            {
                AesResult = await GetAesKey(true);
                return AesCryptoService.Decrypt(result, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }

        }

        //[HttpPost]
        //public async Task<IActionResult> AddMemberPoints(string AuthKey, int Points, string Remark, string callback)
        //{
        //    MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
        //    var result = await client.AddMemberPointsAsync(UserLoginAndGetMCKey(AuthKey), Points, Remark);

        //    return Content(string.IsNullOrEmpty(callback) ? "" : string.Format("{0}({1})", callback, JsonConvert.SerializeObject(result)));

        //}
        #endregion

        #region 收货地址接口
        [HttpPost]
        public async Task<string> GetDeliveryAddressJson()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);

            var result = await client.GetDeliveryAddressJsonAsync(await UserLoginAndGetMCKey(AuthKey));
            var AesResult = await GetAesKey();
            try
            {
                return AesCryptoService.Decrypt(result, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }
            catch
            {
                AesResult = await GetAesKey(true);
                return AesCryptoService.Decrypt(result, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }

        }
        [HttpPost]
        public async Task<int> DeliveryAddressAddJson()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            string Addr = Request.Form["Addr"].ToString();
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);

            var AesResult = await GetAesKey();
            var aesAddr = "";
            try
            {
                aesAddr = AesCryptoService.Encrypt(Addr, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }
            catch
            {
                AesResult = await GetAesKey(true);
                aesAddr = AesCryptoService.Encrypt(Addr, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }

            return await client.DeliveryAddressAddJsonAsync(await UserLoginAndGetMCKey(AuthKey), aesAddr);

        }
        [HttpPost]
        public async Task<int> DeliveryAddressUpdateJson()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            string Addr = Request.Form["Addr"].ToString();

            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);

            var AesResult = await GetAesKey();
            var aesAddr = "";
            try
            {
                aesAddr = AesCryptoService.Encrypt(Addr, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }
            catch
            {
                AesResult = await GetAesKey(true);
                aesAddr = AesCryptoService.Encrypt(Addr, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }

            return await client.DeliveryAddressUpdateJsonAsync(await UserLoginAndGetMCKey(AuthKey), aesAddr);

        }
        [HttpPost]
        public async Task<int> DeliveryAddressSetDefault()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            int AddrID = -1;
            int.TryParse(Request.Form["AddrID"].ToString(), out AddrID);
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
            return await client.DeliveryAddressSetDefaultAsync(await UserLoginAndGetMCKey(AuthKey), AddrID);

        }
        [HttpPost]
        public async Task<int> DeliveryAddressDel()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            int AddrID = -1;
            int.TryParse(Request.Form["AddrID"].ToString(), out AddrID);
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
            return await client.DeliveryAddressDelAsync(await UserLoginAndGetMCKey(AuthKey), AddrID);

        }
        #endregion

        #region 积分兑换订单接口
        [HttpPost]
        public async Task<ExchangeOrder[]> GetMyExchangeOrderListJson()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
            return await client.GetMyExchangeOrderListAsync(await UserLoginAndGetMCKey(AuthKey));

        }
        [HttpPost]
        public async Task<int> ChangeExchangeOrderAddressJson()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            int OrderID = -1;
            int.TryParse(Request.Form["OrderID"].ToString(), out OrderID);
            string NewAddress = Request.Form["NewAddress"].ToString();
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
            return await client.ChangeExchangeOrderAddressJsonAsync(await UserLoginAndGetMCKey(AuthKey), OrderID, NewAddress);

        }

        [HttpPost]
        public async Task<int> SignInExchangeOrder()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            int OrderID = -1;
            int.TryParse(Request.Form["OrderID"].ToString(), out OrderID);
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
            return await client.SignInExchangeOrderAsync(await UserLoginAndGetMCKey(AuthKey), OrderID);

        }
        [HttpPost]
        public async Task<int> CancelExchangeOrderEx()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            int OrderID = -1;
            int.TryParse(Request.Form["OrderID"].ToString(), out OrderID);
            string CancelRemark = Request.Form["CancelRemark"].ToString();
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
            return await client.CancelExchangeOrderExAsync(await UserLoginAndGetMCKey(AuthKey), OrderID, CancelRemark);

        }
        [HttpPost]
        public async Task<int> ReturnExchangeOrder()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            int OrderID = -1;
            int.TryParse(Request.Form["OrderID"].ToString(), out OrderID);
            string CancelReason = Request.Form["CancelReason"].ToString();
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
            return await client.ReturnExchangeOrderAsync(await UserLoginAndGetMCKey(AuthKey), OrderID, CancelReason);

        }
        #endregion

        #region 积分订单兑换接口
        [HttpPost]
        public async Task<string> GetGiftCatalogsJson()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            int Catalog = -1;
            int.TryParse(Request.Form["Catalog"].ToString(), out Catalog);
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);

            var result = await client.GetGiftCatalogsJsonAsync(await UserLoginAndGetMCKey(AuthKey), Catalog);
            var AesResult = await GetAesKey();
            try
            {
                return AesCryptoService.Decrypt(result, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }
            catch
            {
                AesResult = await GetAesKey(true);
                return AesCryptoService.Decrypt(result, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }
        }

        [HttpPost]
        public async Task<string> GetGiftsByCatalogJson()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            int Catalog = -1;
            int.TryParse(Request.Form["Catalog"].ToString(), out Catalog);
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);

            var result = await client.GetGiftsByCatalogJsonAsync(await UserLoginAndGetMCKey(AuthKey), Catalog);
            var AesResult = await GetAesKey();
            try
            {
                return AesCryptoService.Decrypt(result, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }
            catch
            {
                AesResult = await GetAesKey(true);
                return AesCryptoService.Decrypt(result, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }
        }
        [HttpPost]
        public async Task<int> GetGiftInventory()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            int GiftID = -1;
            int.TryParse(Request.Form["GiftID"].ToString(), out GiftID);
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
            return await client.GetGiftInventoryAsync(await UserLoginAndGetMCKey(AuthKey), GiftID);

        }
        [HttpPost]
        public async Task<string> GetHotGiftsJson()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            int TopCount = -1;
            int.TryParse(Request.Form["TopCount"].ToString(), out TopCount);
            int MaxPoints = -1;
            int.TryParse(Request.Form["MaxPoints"].ToString(), out MaxPoints);
            DateTime BeginDate = DateTime.MinValue;
            DateTime.TryParse(Request.Form["BeginDate"].ToString(), out BeginDate);
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);

            var result = await client.GetHotGiftsJsonAsync(await UserLoginAndGetMCKey(AuthKey), TopCount, MaxPoints, BeginDate);
            var AesResult = await GetAesKey();
            try
            {
                return AesCryptoService.Decrypt(result, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }
            catch
            {
                AesResult = await GetAesKey(true);
                return AesCryptoService.Decrypt(result, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }

        }

        [HttpPost]
        public async Task<string> GetGiftInfoJson()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            string GiftCode = Request.Form["GiftCode"].ToString();
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);

            var result = await client.GetGiftInfoJsonAsync(await UserLoginAndGetMCKey(AuthKey), GiftCode);
            var AesResult = await GetAesKey();
            try
            {
                return AesCryptoService.Decrypt(result, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }
            catch
            {
                AesResult = await GetAesKey(true);
                return AesCryptoService.Decrypt(result, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }
        }
        [HttpPost]
        public async Task<int> CustomerExchangeOrderApplyEx()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            string Consignee = Request.Form["Consignee"].ToString();
            string Address = Request.Form["Address"].ToString();
            string Mobile = Request.Form["Mobile"].ToString();
            string AcceptRemark = Request.Form["AcceptRemark"].ToString();
            string ProductIDs = Request.Form["ProductIDs"].ToString();
            string Quantitys = Request.Form["Quantitys"].ToString();
            int OfficialCity = -1;
            int.TryParse(Request.Form["OfficialCity"].ToString(), out OfficialCity);
            int Source = -1;
            int.TryParse(Request.Form["Source"].ToString(), out Source);
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
            return await client.CustomerExchangeOrderApplyExAsync(await UserLoginAndGetMCKey(AuthKey), OfficialCity, Consignee, Address, Mobile, AcceptRemark, ProductIDs, Quantitys, Source);

        }
        [HttpPost]
        public async Task<int> ChangeExchangeOrderAddress()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();

            string Consignee = Request.Form["Consignee"].ToString();
            string Address = Request.Form["Address"].ToString();
            string Mobile = Request.Form["Mobile"].ToString();
            string AcceptRemark = Request.Form["AcceptRemark"].ToString();

            int OrderID = -1;
            int.TryParse(Request.Form["OrderID"].ToString(), out OrderID);
            int OfficialCity = -1;
            int.TryParse(Request.Form["OfficialCity"].ToString(), out OfficialCity);

            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
            return await client.ChangeExchangeOrderAddressAsync(await UserLoginAndGetMCKey(AuthKey), OrderID, OfficialCity, Consignee, Address, Mobile, AcceptRemark);

        }
        #endregion

        #region 门店接口

        [HttpPost]
        public async Task<string> GetRetailerListByOfficialCityJson()
        {
            Logger.Info("门店调用开始");
            string AuthKey = Request.Form["AuthKey"].ToString();
            int OfficialCity = 1;
            int.TryParse(Request.Form["OfficialCity"].ToString(), out OfficialCity);
            Logger.Info("城市id：" + OfficialCity);

            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);

            var mckey = await UserLoginAndGetMCKey(AuthKey);
            Logger.Info("mckey：" + mckey);
            var result = await client.GetRetailerListByOfficialCityJsonAsync(mckey, OfficialCity);

            if (!string.IsNullOrWhiteSpace(result) && result != "null")
            {
                var AesResult = await GetAesKey();
                try
                {
                    return AesCryptoService.Decrypt(result, AesResult.CryptAESKey, AesResult.CryptAESIV);
                }
                catch
                {
                    AesResult = await GetAesKey(true);
                    return AesCryptoService.Decrypt(result, AesResult.CryptAESKey, AesResult.CryptAESIV);
                }
            }
            else return result;
        }
        [HttpPost]
        public async Task<string> GetClientInfoByActivityAndOfficialCityJson()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            int Activity = 0;
            int OfficialCity = 1;
            int.TryParse(Request.Form["Activity"].ToString(), out Activity);
            int.TryParse(Request.Form["OfficialCity"].ToString(), out OfficialCity);
            var mckey = await UserLoginAndGetMCKey(AuthKey);
            yilibabyClientInfo.ClientInfoServiceSoapClient client = new yilibabyClientInfo.ClientInfoServiceSoapClient(yilibabyClientInfo.ClientInfoServiceSoapClient.EndpointConfiguration.ClientInfoServiceSoap);
            var result = await client.GetClientInfoByActivityAndOfficialCityJsonAsync(mckey, Activity, OfficialCity);
            if (!string.IsNullOrWhiteSpace(result) && result != "null")
            {
                var AesResult = await GetAesKey();
                try
                {
                    return AesCryptoService.Decrypt(result, AesResult.CryptAESKey, AesResult.CryptAESIV);
                }
                catch
                {
                    AesResult = await GetAesKey(true);
                    return AesCryptoService.Decrypt(result, AesResult.CryptAESKey, AesResult.CryptAESIV);
                }
            }
            else return result;
        }

        [HttpPost]
        public async Task<string> GetClientInfoByActivityJsonAsync()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            int Activity = 0;
            int.TryParse(Request.Form["Activity"].ToString(), out Activity);
            var mckey = await UserLoginAndGetMCKey(AuthKey);
            yilibabyClientInfo.ClientInfoServiceSoapClient client = new yilibabyClientInfo.ClientInfoServiceSoapClient(yilibabyClientInfo.ClientInfoServiceSoapClient.EndpointConfiguration.ClientInfoServiceSoap);
            var result = await client.GetClientInfoByActivityJsonAsync(mckey, Activity);
            if (!string.IsNullOrWhiteSpace(result) && result != "null")
            {
                var AesResult = await GetAesKey();
                try
                {
                    return AesCryptoService.Decrypt(result, AesResult.CryptAESKey, AesResult.CryptAESIV);
                }
                catch
                {
                    AesResult = await GetAesKey(true);
                    return AesCryptoService.Decrypt(result, AesResult.CryptAESKey, AesResult.CryptAESIV);
                }
            }
            else return result;
        }
        [HttpPost]
        public async Task<string> GetClientInfoByOfficialCityJsonAsync()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            int OfficialCity = 1;
            int.TryParse(Request.Form["OfficialCity"].ToString(), out OfficialCity);
            var mckey = await UserLoginAndGetMCKey(AuthKey);
            yilibabyClientInfo.ClientInfoServiceSoapClient client = new yilibabyClientInfo.ClientInfoServiceSoapClient(yilibabyClientInfo.ClientInfoServiceSoapClient.EndpointConfiguration.ClientInfoServiceSoap);
            var result = await client.GetClientInfoByOfficialCityJsonAsync(mckey, OfficialCity);
            if (!string.IsNullOrWhiteSpace(result) && result != "null")
            {
                var AesResult = await GetAesKey();
                try
                {
                    return AesCryptoService.Decrypt(result, AesResult.CryptAESKey, AesResult.CryptAESIV);
                }
                catch
                {
                    AesResult = await GetAesKey(true);
                    return AesCryptoService.Decrypt(result, AesResult.CryptAESKey, AesResult.CryptAESIV);
                }
            }
            else return result;
        }
        #endregion

        #region 内部接口
        /// <summary>
        /// 
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="content"></param>
        /// <param name="serviceStaff"></param>
        /// <param name="acceptSource"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> CustomerServiceAccept()
        {
            try
            {
                Logger.Info($"进入工单推送");
                string AuthKey = Request.Form["AuthKey"].ToString();
                string Topic = Request.Form["Topic"].ToString();
                string Content = Request.Form["Content"].ToString();
                string ServiceStaff = "0";
                int AcceptSource = 578;
                Logger.Info($"工单推送参数AuthKey：{AuthKey}，Topic={Topic}，Content={Content}，ServiceStaff={ServiceStaff}，AcceptSource={AcceptSource}");
                MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
                var result = await client.CustomerServiceAcceptAsync(AuthKey, Topic, Content, ServiceStaff, AcceptSource);
                Logger.Info($"工单推送调用结果：{result}");
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message + ex.StackTrace);
                throw ex;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="content"></param>
        /// <param name="serviceStaff"></param>
        /// <param name="acceptSource"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<int> SendTempleteMessage()
        {
            try
            {
                Logger.Info($"进入模板消息推送");
                string data = Request.Form["data"].ToString();
                string AuthKey = Request.Form["AuthKey"].ToString();
                Logger.Info($"模板消息推送参数 data {data} authkey {AuthKey}");
                WechatSendTemplateMsgResponse result = null;
                MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
                if (!string.IsNullOrEmpty(AuthKey))
                {
                    result = await client.WechatSendTemplateMsgAsync(new WechatSendTemplateMsgRequest()
                    {
                        AuthKey = AuthKey,
                        ReqJsonString = data,
                    });
                }
                else
                {
                    AuthKey =await DefaultUserLoginAndGetMCKey(false);
                    result = await client.WechatSendTemplateMsgAsync(new WechatSendTemplateMsgRequest()
                    {
                        AuthKey = AuthKey,
                        ReqJsonString = data,
                    });
                    if (result.WechatSendTemplateMsgResult == -100)
                    {
                        AuthKey = await DefaultUserLoginAndGetMCKey(true);
                        result = await client.WechatSendTemplateMsgAsync(new WechatSendTemplateMsgRequest()
                        {
                            AuthKey = AuthKey,
                            ReqJsonString = data,
                        });
                    }
                }
                Logger.Info($"模板消息推送结果 {JsonConvert.SerializeObject(result)}");
                return result.WechatSendTemplateMsgResult;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message + ex.StackTrace);
                throw ex;
            }

        }

        [HttpPost]
        public async Task<List<ShoppingMallPic>> GetShoppingMallPics()
        {
            var token = Request.Form["Token"].ToString();
            var nameArray = Request.Form["Names"].ToString() != "" ? Request.Form["Names"].ToString().Split(',') : null;
            List<ShoppingMallPic> result = new List<ShoppingMallPic>();
            List<MpShoppingMallPicDto> datas = null;
            if (nameArray == null)
                datas = await _mpShoppingMallPicAppService.GetListByNames(token);
            else
                datas = await _mpShoppingMallPicAppService.GetListByNames(token, nameArray);

            datas.ForEach(m =>
            {
                result.Add(new ShoppingMallPic { Name = m.Name, LinkUrl = m.LinkUrl, PicUrl = m.LocalPicUrl });
            });
            return result;
        }
        [HttpPost]
        public async Task<string> GetTextByBabyAge()
        {
            DateTime Birthday = DateTime.MinValue;
            DateTime.TryParse(Request.Form["Birthday"].ToString(), out Birthday);
            MpBabyTextDto result = null;
            TimeSpan sp = DateTime.Now.Subtract(Birthday);

            result = await _mpBabyTextAppService.GetTextByday(sp.Days);
            //var week = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(sp.Days + 1) / 7));
            //if (sp.Days > 365)
            //{
            //    if (week % 4 == 0 || week % 4 == 3)
            //        result = await _mpBabyTextAppService.GetMaxWeekText(week);
            //    else
            //        result = await _mpBabyTextAppService.GetMaxWeekText(week + 1);
            //}
            //else
            //{

            //    result = await _mpBabyTextAppService.GetFirstOrDefaultByInput(new MpBabyTexts.Dto.GetMpBabyTextsInput { BabyAge = week });
            //}

            //if (result == null)
            //    result = await _mpBabyTextAppService.GetFirstOrDefaultByInput(new MpBabyTexts.Dto.GetMpBabyTextsInput { BabyAge = -9999 });
            return result != null ? result.BabyText : null;
        }

        /// <summary>
        /// 获取母爱产品的产品信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<MpProductListDto>> GetProductInfos()
        {
            return await _mpProductInfoAppService.GetList();
        }
        /// <summary>
        /// 保存微课堂签到信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<MpCourseSignupDto> SaveWktSignupInfo()
        {

            string signModel = Request.Form["signModel"].ToString();
            Logger.Info($"反写课程签到,传入参数为:{signModel}");
            var model = JsonConvert.DeserializeObject<WKTSignModel>(signModel);
            if (model != null)
            {
                Logger.Info($"反写课程签到,序列化成功");
                //var member = await _mpUserMemberAppService.GetByOpenID(model.OpenID);
                //Logger.Info($"反写课程签到,查询到会员信息为：{JsonConvert.SerializeObject(member)}");
                try
                {
                    var resultModel = await _mpCourseSignupAppService.Create(new MpCourseSignups.Dto.MpCourseSignupDto
                    {
                        Address = model.Address,
                        BeginTime = model.BeginTime,
                        CourseID = model.CourseID,
                        CourseName = model.CourseName,
                        EndTime = model.EndTime,
                        IsConfirmed = false,
                        OpenID = model.OpenID,
                        MpID = model.MpID,
                        CRMID = -1
                    });
                    return resultModel;
                }
                catch (Exception ex)
                {
                    Logger.Info($"反写课程签到,保存保存：{ex.Message},堆栈：{ex.StackTrace}");
                    throw ex;
                }


            }
            Logger.Info($"反写课程签到,序列化失败");
            return null;
        }

        /// <summary>
        /// 获取会员信息（客服）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork(IsDisabled = true)]
        public async Task<JsonResult> GetKfMemberInfo()
        {
            int state = 1;
            string errMsg = null;
            string entity = null;
            string messageToken = Request.Form["messageToken"].ToString();
            var customer = await _customerServiceOnlineAppService.GetByMessageToken(messageToken);
            if (customer != null)
            {
                string openId = Request.Form["openId"].ToString();
                var member = await _mpUserMemberAppService.GetByOpenID(openId);
                if (member == null)
                {
                    state = 2;
                    //errMsg = "该用户不是会员";
                }
                else
                {

                    var mckey = await MemberUserLoginAndGetMCKey(member.MemeberUserName, member.MemberPassword);
                    MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
                    var balance = await client.GetPointsBalanceAsync(mckey);
                    state = 0;
                    //errMsg = "获取成功";
                    entity = JsonConvert.SerializeObject(new { member.OpenID,
                        member.MemberName,
                        member.BabyBirthday,
                        member.BabyName,
                        member.RegisterTime,
                        member.Address,
                        member.CRMID,
                        member.MobilePhone,
                        Sex = Convert.ToInt32(member.Sex) == 1 ? "男" : (Convert.ToInt32(member.Sex) == 2 ? "女" : ""),
                        member.Province,
                        member.City,
                        member.Area,
                        member.Street,
                        Balance = balance,
                        channelName =member.ChannelName });
                }
            }
            else
                errMsg = "令牌无效";
            return Json(new { state, errMsg, entity });

        }
        /// <summary>
        /// 获取订单列表（客服）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork(IsDisabled = true)]
        public async Task<JsonResult> GetKfOrderList()
        {
            int state = 1;
            string errMsg = "令牌无效";
            string entity = null;
            string messageToken = Request.Form["messageToken"].ToString();
            var customer = await _customerServiceOnlineAppService.GetByMessageToken(messageToken);
            if (customer != null)
            {
                string openId = Request.Form["openId"].ToString();
                var member = await _mpUserMemberAppService.GetByOpenID(openId);
                if (member == null)
                {
                    state = 2;
                    errMsg = "";
                }
                else
                {

                    var mckey = await MemberUserLoginAndGetMCKey(member.MemeberUserName, member.MemberPassword);
                    MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
                    try
                    {
                        var orderList = await client.GetMyExchangeOrderListAsync(mckey);
                        entity = JsonConvert.SerializeObject(orderList);
                        state = 0;
                        errMsg = "";
                    }
                    catch (Exception ex)
                    {
                        state = 2;
                        errMsg = ex.Message;
                    }
                  
                }
            }
            return Json(new { state, errMsg, entity });
        }

        /// <summary>
        /// 获取会员积分信息列表（客服）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork(IsDisabled = true)]
        public async Task<JsonResult> GetKfPointsList()
        {
            int state = 1;
            string errMsg = "令牌无效";
            string entity = null;
            string messageToken = Request.Form["messageToken"].ToString();
            var customer = await _customerServiceOnlineAppService.GetByMessageToken(messageToken);
            if (customer != null)
            {
                string openId = Request.Form["openId"].ToString();
                var member = await _mpUserMemberAppService.GetByOpenID(openId);
                if (member == null)
                {
                    state = 2;
                    errMsg = "";
                }
                else
                {

                    var mckey = await MemberUserLoginAndGetMCKey(member.MemeberUserName, member.MemberPassword);
                    MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
                    //entity = JsonConvert.SerializeObject(orderList);

                    var AesResult = await GetAesKey();
                    try
                    {

                        var result = await client.GetPointsChangeDetailJsonAsync(mckey, new DateTime(1900, 1, 1), DateTime.Now);
                        entity = AesCryptoService.Decrypt(result, AesResult.CryptAESKey, AesResult.CryptAESIV);
                        state = 0;
                        errMsg = "";
                    }
                    catch
                    {
                        try
                        {
                            AesResult = await GetAesKey(true);

                            var result = await client.GetPointsChangeDetailJsonAsync(mckey, new DateTime(1900, 1, 1), DateTime.Now);
                            entity = AesCryptoService.Decrypt(result, AesResult.CryptAESKey, AesResult.CryptAESIV);
                            state = 0;
                            errMsg = "";
                        }
                        catch (Exception ex)
                        {

                            state = 2;
                            errMsg = ex.Message;
                        }
                       
                    }

                  
                }
            }
            return Json(new { state, errMsg, entity });
        }
        /// <summary>
        /// 获取产品信息（客服）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [UnitOfWork(IsDisabled = true)]
        public async Task<JsonResult> GetKfGiftInfoJson()
        {
            int state = 1;
            string errMsg = "令牌无效";
            string entity = null;
            string messageToken = Request.Form["messageToken"].ToString();
            string GiftCode = Request.Form["GiftCode"].ToString();
            var customer = await _customerServiceOnlineAppService.GetByMessageToken(messageToken);
            if (customer != null)
            {
                string openId = Request.Form["openId"].ToString();
                var member = await _mpUserMemberAppService.GetByOpenID(openId);
                if (member == null)
                {
                    state = 2;
                    errMsg = "";
                }
                else
                {

                    var mckey = await MemberUserLoginAndGetMCKey(member.MemeberUserName, member.MemberPassword);
                    MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);

                 
                    var AesResult = await GetAesKey();
                    try
                    {
                        var result = await client.GetGiftInfoJsonAsync(mckey, GiftCode);
                        entity = AesCryptoService.Decrypt(result, AesResult.CryptAESKey, AesResult.CryptAESIV);
                        state = 0;
                        errMsg = "";
                    }
                    catch
                    {
                        try
                        {
                            AesResult = await GetAesKey(true);
                            var result = await client.GetGiftInfoJsonAsync(mckey, GiftCode);
                            entity = AesCryptoService.Decrypt(result, AesResult.CryptAESKey, AesResult.CryptAESIV);
                            state = 0;
                            errMsg = "";
                        }
                        catch (Exception ex)
                        {
                            state = 2;
                            errMsg = ex.Message;
                        }
                       
                    }
                    
                }
            }
            return Json(new { state, errMsg, entity });

            
            
        }
        #endregion

        #region 私有方法
        private async Task SetMemberInfo(string openId, string userName, string newPassword, string mgccAuthKey, ApplyAESEncryptKeyResponse AesResult, string mobilePhone = null)
        {
            var members = await _mpUserMemberAppService.GetList(new MpUserMembers.Dto.GetMpUserMembersInput { MemberUserName = userName });
            var fan = await _mpFanAppService.GetFirstOrDefaultByOpenID(openId);

            Logger.Info($"查询粉丝表信息，条件：{openId},结果：{JsonConvert.SerializeObject(fan)}");
            if (members != null && members.Count > 0)
            {
                foreach (var item in members)
                {
                    item.MemberPassword = newPassword;
                    if (!string.IsNullOrEmpty(mobilePhone))
                        item.MobilePhone = mobilePhone;
                    item.OpenID = openId;
                    item.IsBinding = true;

                    await _mpUserMemberAppService.Update(item);
                    await _cacheManager.GetCache(AppConsts.Cache_MpUserMemberKey).RemoveAsync(item.OpenID);//重置会员缓存

                }
                #region 更新粉丝信息
                await _cacheManager.GetCache(AppConsts.Cache_MpFansByOpenId).RemoveAsync(openId);
                if (fan != null)
                {
                    fan.MemberID = members[0].Id;
                    await _mpFanAppService.Update(fan);
                }
                else
                {
                    await _mpFanAppService.Create(new MpFans.Dto.MpFanDto
                    {
                        OpenID = openId,
                        IsFans = true,
                        MpID = 1,
                        MemberID = members[0].Id,
                        LastModificationTime = DateTime.Now
                    });
                }
                #endregion

            }
            else
            {
                #region 绑定openID
                var authkey = await UserLoginAndGetMCKey(mgccAuthKey, true);
                MemberServiceSoapClient client22 = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
                var bindResult = await client22.MemberBindWeChatAsync(authkey, openId, yiliwechat, "");
                if (bindResult == -100)
                {
                    authkey = await UserLoginAndGetMCKey(mgccAuthKey, true);
                    bindResult = await client22.MemberBindWeChatAsync(authkey, openId, yiliwechat, "");
                }
                #endregion

                #region 新增会员信息
                var member = await _mpUserMemberAppService.Create(new MpUserMembers.Dto.MpUserMemberDto()
                {
                    OpenID = openId,
                    MemeberUserName = userName,
                    MemberPassword = newPassword,
                    RegisterTime = DateTime.Now,
                    CreationTime = DateTime.Now,
                    IsDeleted = false,
                    MgccAuthkey = mgccAuthKey,
                    ChannelID = fan != null ? Convert.ToInt32(fan.ChannelID) : 0,
                    ChannelName = fan != null ? fan.ChannelName : "",
                    SubscribeTime = fan != null ? fan.SubscribeTime : null,
                    IsBinding = true,
                    BindingTime = DateTime.Now,
                });
                await _mpUserMemberAppService.DeleteOtherSame(openId, member.Id);

                MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
                var mcmember = await client.GetMyMemberInfoAsync(await UserLoginAndGetMCKey(mgccAuthKey));
                if (mcmember == null)
                {
                    mcmember = await client.GetMyMemberInfoAsync(await UserLoginAndGetMCKey(mgccAuthKey, true));
                }
                member.MemberName = mcmember.RealName;
                member.CRMID = mcmember.CRMID;
                member.MemberType = mcmember.MemberType;
                member.BabyName = mcmember.BabyName;
                member.BabyBirthday = mcmember.Birthday;
                member.MemberIdentity = mcmember.HomeRole.ToString();
                member.RegistProductName = mcmember.CollectProductName;
                member.RegistProduct = mcmember.CollectProduct;
                member.UsedBrand = mcmember.PreBrand;
                member.FeedingMode = mcmember.FeedMode;
                member.MonthConsumption = mcmember.EatQuantityOneMonth;
                member.DayConsumption = mcmember.EatTimesOneDay;
                member.OfficialCity = mcmember.OfficialCity;
                member.Address = mcmember.CollectProduct;
                member.LikeChannel = mcmember.ServicePreferences.ToString();
                member.ServiceShop = mcmember.ServiceCMClientName;
                member.MobilePhone = mcmember.Mobile;

                await _mpUserMemberAppService.Update(member);
                Logger.Info($"新增会员信息 {JsonConvert.SerializeObject(mcmember)}");

                #endregion

                #region 更新粉丝信息
                await _cacheManager.GetCache(AppConsts.Cache_MpFansByOpenId).RemoveAsync(openId);
                if (fan != null)
                {
                    fan.MemberID = member.Id;
                    await _mpFanAppService.Update(fan);
                }
                else
                {
                    await _mpFanAppService.Create(new MpFans.Dto.MpFanDto
                    {
                        OpenID = openId,
                        IsFans = true,
                        MpID = 1,
                        MemberID = member.Id,
                        LastModificationTime = DateTime.Now
                    });
                }
                #endregion
            }

        }
        #endregion
    }

    public class MeiChiMember
    {
        /// 会员ID
        public Guid ID = Guid.Empty;
        /// 会员姓名
        public string RealName = string.Empty;
        /// 会员级别
        public int MemberType = 0;
        /// 手机号码
        public string Mobile = string.Empty;
        /// Email
        public string Email = string.Empty;
        ///Email验证标志
        public bool EmailVerifyFlag = false;
        ///性别
        public int Sex = 1;
        /// 会员生日
        public DateTime Birthday = new DateTime(1900, 1, 1);
        /// 家庭角色
        public int HomeRole = 0;
        /// 宝宝姓名
        public string BabyName = "";
        ///爱好
        public string Hobby = "";
        /// 所属行政城市
        public int OfficialCity = 0;
        /// 家庭地址
        public string Address = string.Empty;
        ///激活时间
        public DateTime ActiveDate = new DateTime(1900, 1, 1);
        /// CRM系统ID
        public int CRMID = 0;
        /// 附件库
        public Attachment[] Atts;
        public string CollectProduct = "";
        public string CollectProductName = "";
        public int PreBrand = 0;
        /// <summary>
        /// 每月食用数量(盒)
        /// </summary>
        public decimal EatQuantityOneMonth = 0;
        /// <summary>
        /// 每天食用次数
        /// </summary>
        public int EatTimesOneDay = 0;
        /// <summary>
        /// 喂养方式，通过字典列表选择值
        /// </summary>
        public int FeedMode = 1;
        /// <summary>
        /// 偏好的服务渠道,通过字典列表选择值
        /// </summary>
        public int ServicePreferences = 0;
        /// <summary>
        /// 服务门店
        /// </summary>
        public string serviceCMClientName;
        public string serviceCMClientCode;
        public string OfficialCityName = string.Empty;
    }

    public class ShoppingMallPic
    {
        public string Name { get; set; }
        public string PicUrl { get; set; }
        public string LinkUrl { get; set; }
    }

    public class ClientInfo
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int ID { get; set; }

        public string ClientCode { get; set; }

        public string FullName { get; set; }

        public string ShortName { get; set; }

        public int OfficialCity { get; set; }

        public int OrganizeCity { get; set; }

        public string TeleNum { get; set; }

        public string Address { get; set; }

        public string PostCode { get; set; }

        public int ActiveFlag { get; set; }

        public int Supplier { get; set; }

        public int ClientType { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public int Distance { get; set; }

        public Attachment[] Atts { get; set; }
    }

    public class WKTSignModel
    {
        public int MpID { get; set; }
        public int CourseID { get; set; }
        public string OpenID { get; set; }
        public string CourseName { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Address { get; set; }
    }
}
