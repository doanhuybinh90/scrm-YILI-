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

namespace Pb.Wechat.Web.Controllers
{
    [IgnoreAntiforgeryToken]
    public class MeiChiController : AbpController
    {

        private static string defaultUserName = "";
        private static string defaultUserPwd = "";
        private static string deviceCode = "";
        private static string yiliwechat = "";
        private string MamaBanToYun2OneMediaID = "";
        private string JNHToLongLineMediaID = "";
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
        public MeiChiController(ICacheManager cacheManager, IMpUserMemberAppService mpUserMemberAppService, IMpBabyTextAppService mpBabyTextAppService, IMpShoppingMallPicAppService mpShoppingMallPicAppService, IMpFanAppService mpFanAppService, IAccessTokenContainer accessTokenContainer, IMpApiTokenAppService mpApiTokenAppService, IMatialFileService matialFileService, IYiliBabyClubInterfaceService yiliBabyClubInterfaceService, IMpProductInfoAppService mpProductInfoAppService, IMpCourseSignupAppService mpCourseSignupAppService, ICustomerServiceOnlineAppService customerServiceOnlineAppService)
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
        }
        #region 注册
        public IActionResult RegistFromWX()
        {
            return View();
        }
        public IActionResult RegistFromApp()
        {
            return View();
        }

        public IActionResult Mall()
        {
            return View();
        }

        public IActionResult Mother()
        {
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
        private async Task<ApplyAESEncryptKeyResponse> GetDefaultAesKey(bool getnew = false)
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
            var AesResult = await GetDefaultAesKey();
            try
            {
                EncryptPassword = AesCryptoService.Encrypt(defaultUserPwd, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }
            catch
            {
                AesResult = await GetDefaultAesKey(true);
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
        
        [HttpPost]
        public async Task<ResetPasswordResponse> ResetPassword()
        {
            string mgccAuthKey = Request.Form["AuthKey"].ToString();
            string mobilePhone = Request.Form["Mobile"].ToString();
            string newPassword = Request.Form["NewPwd"].ToString();
            int verifyID = int.Parse(Request.Form["VerifyID"].ToString());
            string verifyCode = Request.Form["VerifyCode"].ToString();
            string AesKey = Request.Form["AesKey"].ToString();
            string Iv = Request.Form["Iv"].ToString();
            
            UserLoginSoapClient client2 = new UserLoginSoapClient(UserLoginSoapClient.EndpointConfiguration.UserLoginSoap);
            var EncryptPassword = "";
            EncryptPassword = AesCryptoService.Encrypt(newPassword, AesKey, Iv);
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
                await SetMemberInfo(openid, data.username, newPassword, mgccAuthKey, AesKey, Iv, mobilePhone);
            }

            return data;



        }
        #endregion

        #region 用户接口
        [HttpPost]
        public async Task<int> Login_GetNewMsg()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            UserLoginSoapClient client = new UserLoginSoapClient(UserLoginSoapClient.EndpointConfiguration.UserLoginSoap);
            var result = await client.Login_GetNewMsgAsync(AuthKey);
            if (result == -100)
                result = await client.Login_GetNewMsgAsync(AuthKey);
            return result;

        }
        [HttpPost]
        public async Task<int> Logout()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            UserLoginSoapClient client = new UserLoginSoapClient(UserLoginSoapClient.EndpointConfiguration.UserLoginSoap);
            return await client.LogoutAsync(AuthKey);
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
            string AesKey = Request.Form["AesKey"].ToString();
            string Iv = Request.Form["Iv"].ToString();
            int CityID = 1;
            int.TryParse(Request.Form["CityID"].ToString(), out CityID);

            OfficialCityServiceSoapClient client = new OfficialCityServiceSoapClient(OfficialCityServiceSoapClient.EndpointConfiguration.OfficialCityServiceSoap);
            
            var result = await client.GetSubCitysBySuperJsonAsync(AuthKey, CityID);
            if (!string.IsNullOrWhiteSpace(result) && result != "null")
            {
                return AesCryptoService.DecryptText(result, AesKey, Iv);
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
            return await client.GetCityFullNameAsync(AuthKey, CityID);
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
            var result = await client.SendVerifyCodeWithParamAsync(AuthKey, Classify, Mobile, MessageParam);
            return result;
        }
        [HttpPost]
        public async Task<int> ReSendVerifyCode()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            int VerifyID = -1;
            int.TryParse(Request.Form["VerifyID"].ToString(), out VerifyID);
            VCIFServiceSoapClient client = new VCIFServiceSoapClient(VCIFServiceSoapClient.EndpointConfiguration.VCIFServiceSoap);
            var result = await client.ReSendVerifyCodeAsync(AuthKey, VerifyID);
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
            var result = await client.VerifyCodeAsync(AuthKey, VerifyID, Mobile, Code);
            return result;
        }
        #endregion

        #region 会员接口
        [HttpPost]
        public async Task<Member> GetMyMemberInfoJson()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
            return await client.GetMyMemberInfoAsync(AuthKey);
        }
        [HttpPost]
        public async Task<int> UpdateMemberInfoJson()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            string AesKey = Request.Form["AesKey"].ToString();
            string Iv = Request.Form["Iv"].ToString();
            string Member = Request.Form["Member"].ToString();
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
            Logger.Info("会员更新:" + AuthKey);
            var mckey = AuthKey;
            var aesMember = AesCryptoService.Encrypt(Member, AesKey, Iv);
            Logger.Info("传入更新的会员信息：" + Member);

            var result = await client.UpdateMemberInfoJsonAsync(mckey, aesMember);
            Logger.Info($"更新的结果：{result}");


            #region 获取更新后美驰的会员信息
            var jsonStr = await client.GetMyMemberInfoJsonAsync(mckey);
            if (!string.IsNullOrWhiteSpace(jsonStr) && jsonStr != "null")
            {
                string plainJson = AesCryptoService.DecryptText(jsonStr, AesKey, Iv);
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
                member.Province = areaArray.Length > 0 ? areaArray[0] : string.Empty;
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
            return await client.SendUserRegisterMailAsync(AuthKey);
        }
        [HttpPost]
        public async Task<int> MemberBindWeChat()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            string UserWeChat = Request.Form["UserWeChat"].ToString();
            string CompanyWeChat = Request.Form["CompanyWeChat"].ToString();
            string Remark = Request.Form["Remark"].ToString();

            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
            return await client.MemberBindWeChatAsync(AuthKey, UserWeChat, CompanyWeChat, Remark);

        }
        [HttpPost]
        public async Task<string> GetMostlyProductListJson()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            string AesKey = Request.Form["AesKey"].ToString();
            string Iv = Request.Form["Iv"].ToString();
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);

            var result = await client.GetMostlyProductListJsonAsync(AuthKey);
            return AesCryptoService.DecryptText(result, AesKey, Iv);
        }


        private async Task<MpAccountTokenOutput> GetAccountToken(string Token, string ApiType)
        {
            return await _mpApiTokenAppService.GetAccountToken(new MpAccountTokenInput()
            {
                Token = Token,
                ApiType = ApiType
            });
        }
        #endregion

        #region 积分接口
        [HttpPost]
        public async Task<decimal> GetPointsBalance()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
            var balance = await client.GetPointsBalanceAsync(AuthKey);
            return balance;
        }
        [HttpPost]
        public async Task<string> GetPointsInfoJson()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            string AesKey = Request.Form["AesKey"].ToString();
            string Iv = Request.Form["Iv"].ToString();
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);

            var result = await client.GetPointsInfoJsonAsync(AuthKey);
            return AesCryptoService.DecryptText(result, AesKey, Iv);
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
                AuthKey = AuthKey,
                Retailer = Retailer,
                ProductCode = ProductCode
            });
        }

        [HttpPost]
        public async Task<string> GetPointsChangeDetailJson()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            string AesKey = Request.Form["AesKey"].ToString();
            string Iv = Request.Form["Iv"].ToString();
            DateTime BeginDate = DateTime.MinValue;
            DateTime EndDate = DateTime.MinValue;
            DateTime.TryParse(Request.Form["BeginDate"].ToString(), out BeginDate);
            DateTime.TryParse(Request.Form["EndDate"].ToString(), out EndDate);
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);

            var result = await client.GetPointsChangeDetailJsonAsync(AuthKey, BeginDate, EndDate);
            return AesCryptoService.DecryptText(result, AesKey, Iv);
        }
        #endregion

        #region 收货地址接口
        [HttpPost]
        public async Task<string> GetDeliveryAddressJson()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            string AesKey = Request.Form["AesKey"].ToString();
            string Iv = Request.Form["Iv"].ToString();
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);

            var result = await client.GetDeliveryAddressJsonAsync(AuthKey);
            return AesCryptoService.DecryptText(result, AesKey, Iv);

        }
        [HttpPost]
        public async Task<int> DeliveryAddressAddJson()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            string AesKey = Request.Form["AesKey"].ToString();
            string Iv = Request.Form["Iv"].ToString();
            string Addr = Request.Form["Addr"].ToString();
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
            
            var aesAddr = AesCryptoService.Encrypt(Addr, AesKey, Iv);

            return await client.DeliveryAddressAddJsonAsync(AuthKey, aesAddr);

        }
        [HttpPost]
        public async Task<int> DeliveryAddressUpdateJson()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            string AesKey = Request.Form["AesKey"].ToString();
            string Iv = Request.Form["Iv"].ToString();
            string Addr = Request.Form["Addr"].ToString();

            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
            
            var aesAddr = AesCryptoService.Encrypt(Addr, AesKey, Iv);
            return await client.DeliveryAddressUpdateJsonAsync(AuthKey, aesAddr);

        }
        [HttpPost]
        public async Task<int> DeliveryAddressSetDefault()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            int AddrID = -1;
            int.TryParse(Request.Form["AddrID"].ToString(), out AddrID);
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
            return await client.DeliveryAddressSetDefaultAsync(AuthKey, AddrID);

        }
        [HttpPost]
        public async Task<int> DeliveryAddressDel()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            int AddrID = -1;
            int.TryParse(Request.Form["AddrID"].ToString(), out AddrID);
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
            return await client.DeliveryAddressDelAsync(AuthKey, AddrID);

        }
        #endregion

        #region 积分兑换订单接口
        [HttpPost]
        public async Task<ExchangeOrder[]> GetMyExchangeOrderListJson()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
            return await client.GetMyExchangeOrderListAsync(AuthKey);

        }
        [HttpPost]
        public async Task<int> ChangeExchangeOrderAddressJson()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            int OrderID = -1;
            int.TryParse(Request.Form["OrderID"].ToString(), out OrderID);
            string NewAddress = Request.Form["NewAddress"].ToString();
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
            return await client.ChangeExchangeOrderAddressJsonAsync(AuthKey, OrderID, NewAddress);

        }

        [HttpPost]
        public async Task<int> SignInExchangeOrder()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            int OrderID = -1;
            int.TryParse(Request.Form["OrderID"].ToString(), out OrderID);
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
            return await client.SignInExchangeOrderAsync(AuthKey, OrderID);

        }
        [HttpPost]
        public async Task<int> CancelExchangeOrderEx()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            int OrderID = -1;
            int.TryParse(Request.Form["OrderID"].ToString(), out OrderID);
            string CancelRemark = Request.Form["CancelRemark"].ToString();
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
            return await client.CancelExchangeOrderExAsync(AuthKey, OrderID, CancelRemark);

        }
        [HttpPost]
        public async Task<int> ReturnExchangeOrder()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            int OrderID = -1;
            int.TryParse(Request.Form["OrderID"].ToString(), out OrderID);
            string CancelReason = Request.Form["CancelReason"].ToString();
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
            return await client.ReturnExchangeOrderAsync(AuthKey, OrderID, CancelReason);

        }
        #endregion

        #region 积分订单兑换接口
        [HttpPost]
        public async Task<string> GetGiftCatalogsJson()
        {
            //string AuthKey = Request.Form["AuthKey"].ToString();
            //string AesKey = Request.Form["AesKey"].ToString();
            //string Iv = Request.Form["Iv"].ToString();
            int Catalog = -1;
            int.TryParse(Request.Form["Catalog"].ToString(), out Catalog);
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);

            string result = "";
            try
            {
                result = await client.GetGiftCatalogsJsonAsync(await DefaultUserLoginAndGetMCKey(), Catalog);
            }
            catch {
                result = await client.GetGiftCatalogsJsonAsync(await DefaultUserLoginAndGetMCKey(true), Catalog);
            }
            var AesResult = await GetDefaultAesKey();
            try
            {
                return AesCryptoService.Decrypt(result, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }
            catch
            {
                AesResult = await GetDefaultAesKey(true);
                return AesCryptoService.Decrypt(result, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }
        }

        [HttpPost]
        public async Task<string> GetGiftsByCatalogJson()
        {
            int Catalog = -1;
            int.TryParse(Request.Form["Catalog"].ToString(), out Catalog);
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);

            string result = "";
            try
            {
                result = await client.GetGiftsByCatalogJsonAsync(await DefaultUserLoginAndGetMCKey(), Catalog);
            }
            catch
            {
                result = await client.GetGiftsByCatalogJsonAsync(await DefaultUserLoginAndGetMCKey(true), Catalog);
            }
            var AesResult = await GetDefaultAesKey();
            try
            {
                return AesCryptoService.Decrypt(result, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }
            catch
            {
                AesResult = await GetDefaultAesKey(true);
                return AesCryptoService.Decrypt(result, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }
        }
        [HttpPost]
        public async Task<int> GetGiftInventory()
        {
            int GiftID = -1;
            int.TryParse(Request.Form["GiftID"].ToString(), out GiftID);
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);

            int result = 0;
            try
            {
                result = await client.GetGiftInventoryAsync(await DefaultUserLoginAndGetMCKey(), GiftID);
            }
            catch
            {
                result = await client.GetGiftInventoryAsync(await DefaultUserLoginAndGetMCKey(true), GiftID);
            }
            return result;
        }
        [HttpPost]
        public async Task<string> GetHotGiftsJson()
        {
            int TopCount = -1;
            int.TryParse(Request.Form["TopCount"].ToString(), out TopCount);
            int MaxPoints = -1;
            int.TryParse(Request.Form["MaxPoints"].ToString(), out MaxPoints);
            DateTime BeginDate = DateTime.MinValue;
            DateTime.TryParse(Request.Form["BeginDate"].ToString(), out BeginDate);
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);

            string result = "";
            try
            {
                result = await client.GetHotGiftsJsonAsync(await DefaultUserLoginAndGetMCKey(), TopCount, MaxPoints, BeginDate);
            }
            catch
            {
                result = await client.GetHotGiftsJsonAsync(await DefaultUserLoginAndGetMCKey(true), TopCount, MaxPoints, BeginDate);
            }
            var AesResult = await GetDefaultAesKey();
            try
            {
                return AesCryptoService.Decrypt(result, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }
            catch
            {
                AesResult = await GetDefaultAesKey(true);
                return AesCryptoService.Decrypt(result, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }
        }

        [HttpPost]
        public async Task<string> GetGiftInfoJson()
        {
            string GiftCode = Request.Form["GiftCode"].ToString();
            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);

            string result = "";
            try
            {
                result = await client.GetGiftInfoJsonAsync(await DefaultUserLoginAndGetMCKey(), GiftCode);
            }
            catch
            {
                result = await client.GetGiftInfoJsonAsync(await DefaultUserLoginAndGetMCKey(true), GiftCode);
            }
            var AesResult = await GetDefaultAesKey();
            try
            {
                return AesCryptoService.Decrypt(result, AesResult.CryptAESKey, AesResult.CryptAESIV);
            }
            catch
            {
                AesResult = await GetDefaultAesKey(true);
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
            return await client.CustomerExchangeOrderApplyExAsync(AuthKey, OfficialCity, Consignee, Address, Mobile, AcceptRemark, ProductIDs, Quantitys, Source);

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
            return await client.ChangeExchangeOrderAddressAsync(AuthKey, OrderID, OfficialCity, Consignee, Address, Mobile, AcceptRemark);

        }
        #endregion

        #region 门店接口

        [HttpPost]
        public async Task<string> GetRetailerListByOfficialCityJson()
        {
            Logger.Info("门店调用开始");
            string AuthKey = Request.Form["AuthKey"].ToString();
            string AesKey = Request.Form["AesKey"].ToString();
            string Iv = Request.Form["Iv"].ToString();
            int OfficialCity = 1;
            int.TryParse(Request.Form["OfficialCity"].ToString(), out OfficialCity);
            Logger.Info("城市id：" + OfficialCity);

            MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
            
            var result = await client.GetRetailerListByOfficialCityJsonAsync(AuthKey, OfficialCity);

            if (!string.IsNullOrWhiteSpace(result) && result != "null")
            {
                return AesCryptoService.DecryptText(result, AesKey, Iv);
            }
            else return result;
        }
        [HttpPost]
        public async Task<string> GetClientInfoByActivityAndOfficialCityJson()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            string AesKey = Request.Form["AesKey"].ToString();
            string Iv = Request.Form["Iv"].ToString();
            int Activity = 0;
            int OfficialCity = 1;
            int.TryParse(Request.Form["Activity"].ToString(), out Activity);
            int.TryParse(Request.Form["OfficialCity"].ToString(), out OfficialCity);
            yilibabyClientInfo.ClientInfoServiceSoapClient client = new yilibabyClientInfo.ClientInfoServiceSoapClient(yilibabyClientInfo.ClientInfoServiceSoapClient.EndpointConfiguration.ClientInfoServiceSoap);
            var result = await client.GetClientInfoByActivityAndOfficialCityJsonAsync(AuthKey, Activity, OfficialCity);
            if (!string.IsNullOrWhiteSpace(result) && result != "null")
            {
                return AesCryptoService.DecryptText(result, AesKey, Iv);
            }
            else return result;
        }

        [HttpPost]
        public async Task<string> GetClientInfoByActivityJsonAsync()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            string AesKey = Request.Form["AesKey"].ToString();
            string Iv = Request.Form["Iv"].ToString();
            int Activity = 0;
            int.TryParse(Request.Form["Activity"].ToString(), out Activity);
            yilibabyClientInfo.ClientInfoServiceSoapClient client = new yilibabyClientInfo.ClientInfoServiceSoapClient(yilibabyClientInfo.ClientInfoServiceSoapClient.EndpointConfiguration.ClientInfoServiceSoap);
            var result = await client.GetClientInfoByActivityJsonAsync(AuthKey, Activity);
            if (!string.IsNullOrWhiteSpace(result) && result != "null")
            {
                return AesCryptoService.DecryptText(result, AesKey, Iv);
            }
            else return result;
        }
        [HttpPost]
        public async Task<string> GetClientInfoByOfficialCityJsonAsync()
        {
            string AuthKey = Request.Form["AuthKey"].ToString();
            string AesKey = Request.Form["AesKey"].ToString();
            string Iv = Request.Form["Iv"].ToString();
            int OfficialCity = 1;
            int.TryParse(Request.Form["OfficialCity"].ToString(), out OfficialCity);
            yilibabyClientInfo.ClientInfoServiceSoapClient client = new yilibabyClientInfo.ClientInfoServiceSoapClient(yilibabyClientInfo.ClientInfoServiceSoapClient.EndpointConfiguration.ClientInfoServiceSoap);
            var result = await client.GetClientInfoByOfficialCityJsonAsync(AuthKey, OfficialCity);
            if (!string.IsNullOrWhiteSpace(result) && result != "null")
            {
                return AesCryptoService.DecryptText(result, AesKey, Iv);
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
                Logger.Info($"工单推送美驰key：{AuthKey}");
                MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
                var result = await client.CustomerServiceAcceptAsync(AuthKey, Topic, Content, ServiceStaff, AcceptSource);
                if (result == -100)
                {
                    result = await client.CustomerServiceAcceptAsync(AuthKey, Topic, Content, ServiceStaff, AcceptSource);
                }
                Logger.Info($"工单推送调用结果：{result}");
                return result;
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
        public async Task<MpCourseSignups.Dto.MpCourseSignupDto> SaveWktSignupInfo()
        {

            string signModel = Request.Form["signModel"].ToString();
            Logger.Info($"反写课程签到,传入参数为:{signModel}");
            var model = JsonConvert.DeserializeObject<WKTSignModel>(signModel);
            if (model != null)
            {
                Logger.Info($"反写课程签到,序列化成功");
                var member = await _mpUserMemberAppService.GetByOpenID(model.OpenID);
                Logger.Info($"反写课程签到,查询到会员信息为：{JsonConvert.SerializeObject(member)}");
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
                        CRMID = member.CRMID
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
        #endregion

        #region 私有方法
        private async Task SetMemberInfo(string openId, string userName, string newPassword, string authKey, string AesKey, string Iv, string mobilePhone = null)
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
                MemberServiceSoapClient client22 = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
                var bindResult = await client22.MemberBindWeChatAsync(authKey, openId, yiliwechat, "");
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
                    ChannelID = fan != null ? Convert.ToInt32(fan.ChannelID) : 0,
                    ChannelName = fan != null ? fan.ChannelName : "",
                    SubscribeTime = fan != null ? fan.SubscribeTime : null,
                    IsBinding = true,
                    BindingTime = DateTime.Now,
                });
                await _mpUserMemberAppService.DeleteOtherSame(openId, member.Id);

                MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
                var mcmember = await client.GetMyMemberInfoAsync(authKey);
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
}
