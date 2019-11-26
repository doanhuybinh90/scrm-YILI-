using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using yilibabyUser;
using Pb.Wechat.Web.Resources;
using System.Security.Cryptography;
using System;
using Abp.Runtime.Caching;
using Pb.Wechat.MpUserMembers;
using yilibabyMember;
using Pb.Wechat.Url;
using Pb.Wechat.MpBabyTexts.Dto;
using Pb.Wechat.MpBabyTexts;

namespace Pb.Wechat.Web.Controllers
{
    public class TestApiController : AbpZeroTemplateControllerBase
    {
        private static string defaultUserName = "";
        private static string defaultUserPwd = "";
        private static string deviceCode = "";
        private ICacheManager _cacheManager;
        private readonly IMpUserMemberAppService _usermember;
        private readonly IMpUserMemberAppService _mpUserMemberAppService;
        private readonly IYiliBabyClubInterfaceService _yiliBabyClubInterfaceService;
        private readonly IMpBabyTextAppService _mpBabyTextAppService;
        public TestApiController(ICacheManager cacheManager, IMpUserMemberAppService usermember, IMpUserMemberAppService mpUserMemberAppService, IYiliBabyClubInterfaceService yiliBabyClubInterfaceService, IMpBabyTextAppService mpBabyTextAppService)
        {
            _cacheManager = cacheManager;
            _usermember = usermember;
            _mpUserMemberAppService = mpUserMemberAppService;
            _yiliBabyClubInterfaceService = yiliBabyClubInterfaceService;
            defaultUserName = _yiliBabyClubInterfaceService.ApiUserName;
            defaultUserPwd = _yiliBabyClubInterfaceService.ApiPassword;
            deviceCode = _yiliBabyClubInterfaceService.ApiDeviceCode;
            _mpBabyTextAppService = mpBabyTextAppService;
    }

        private ApplyAESEncryptKeyResponse GetAesKey(bool getnew = false)
        {

            var result = _cacheManager.GetCache(AppConsts.Cache_AesKeyModal).GetOrDefaultAsync("AesKeyResult").Result;
            if (result == null || getnew)
            {
                UserLoginSoapClient client = new UserLoginSoapClient(UserLoginSoapClient.EndpointConfiguration.UserLoginSoap);
                
                RSAParameters publicKey = RsaService.GetRsaPublicKey();

                ApplyAESEncryptKeyResponse AesResult =  client.ApplyAESEncryptKeyAsync(new ApplyAESEncryptKeyRequest { DeviceCode = deviceCode, Modulus = Convert.ToBase64String(publicKey.Modulus), Exponent = Convert.ToBase64String(publicKey.Exponent) }).Result;

                 _cacheManager.GetCache(AppConsts.Cache_AesKeyModal).SetAsync("AesKeyResult", AesResult);
                return AesResult;
            }
            else
            {
                ApplyAESEncryptKeyResponse AesResult = (ApplyAESEncryptKeyResponse)result;
                return AesResult;
            }
        }

        private string UserLoginAndGetMCKey(string mgccAuthKey)
        {
            Logger.Info("进入用户登录：mgccKey为：" + mgccAuthKey);
            var userName = string.Empty;
            var userPwd = string.Empty;
            var AesResult = GetAesKey();
            var openid = _cacheManager.GetCache(AppConsts.Cache_AuthKey2OpenID).GetOrDefaultAsync(mgccAuthKey).Result;

            Logger.Info("用户OPENID为：" + openid);


            Logger.Info("使用默认用户名登录：");
            return _cacheManager.GetCache(AppConsts.Cache_MCAuthKey).Get<string, string>("DefaultMCAuthKey", c =>
            {
                UserLoginSoapClient client = new UserLoginSoapClient(UserLoginSoapClient.EndpointConfiguration.UserLoginSoap);
                var EncryptPassword = "";
                try
                {
                    EncryptPassword = AesCryptoService.Encrypt(defaultUserPwd, AesResult.CryptAESKey, AesResult.CryptAESIV);
                }
                catch
                {
                    AesResult = GetAesKey(true);
                    EncryptPassword = AesCryptoService.Encrypt(defaultUserPwd, AesResult.CryptAESKey, AesResult.CryptAESIV);
                }
                var result = client.LoginExAsync(new LoginExRequest { UserName = defaultUserName, EncryptPassword = EncryptPassword, DeviceCode = deviceCode }).Result;
                return result.AuthKey;
            });

        }

        private string GetMemberAuthKey(string mgccAuthKey)
        {
            var AesResult = GetAesKey();
            var openid = _cacheManager.GetCache(AppConsts.Cache_AuthKey2OpenID).GetOrDefaultAsync(mgccAuthKey).Result;
            if (openid != null)//MgccKey没过期
            {
                var _memberInfo = _mpUserMemberAppService.GetByOpenID(openid.ToString()).Result;
                if (_memberInfo != null)//会员->用会员帐户名登陆
                {
                    Logger.Info("登录的会员信息：" + JsonConvert.SerializeObject(_memberInfo));

                    Logger.Info("会员登录:" + mgccAuthKey);
                    Logger.Info("用户名:" + _memberInfo.MemeberUserName + ",密码：" + _memberInfo.MemberPassword);
                    
                        return (string) _cacheManager.GetCache(AppConsts.Cache_MCAuthKey).Get<string, string>(mgccAuthKey, c =>
                        {

                            UserLoginSoapClient client = new UserLoginSoapClient(UserLoginSoapClient.EndpointConfiguration.UserLoginSoap);
                            var EncryptPassword = "";
                            try
                            {
                                EncryptPassword = AesCryptoService.Encrypt(_memberInfo.MemberPassword, AesResult.CryptAESKey, AesResult.CryptAESIV);
                            }
                            catch
                            {
                                AesResult = GetAesKey(true);
                                EncryptPassword = AesCryptoService.Encrypt(_memberInfo.MemberPassword, AesResult.CryptAESKey, AesResult.CryptAESIV);
                            }
                            //获取最新的AuthKey
                            var result = client.LoginExAsync(new LoginExRequest
                            {
                                UserName = _memberInfo.MemeberUserName,
                                EncryptPassword = EncryptPassword,
                                DeviceCode = deviceCode
                            }).Result;

                            return result.AuthKey;
                        });
                   
                }
            }
            return null;
        }
        public ActionResult Index()
        {


            DateTime Birthday = new DateTime(2018, 3, 28);
            TimeSpan sp = DateTime.Now.Subtract(Birthday);
            var week =Convert.ToInt32( Math.Ceiling(Convert.ToDecimal( sp.Days+1) / 7));
            MpBabyTextDto resulta = null;
          
            resulta = _mpBabyTextAppService.GetFirstOrDefaultByInput(new MpBabyTexts.Dto.GetMpBabyTextsInput { BabyAge = week }).Result;
            if (resulta == null)
                resulta = _mpBabyTextAppService.GetFirstOrDefaultByInput(new MpBabyTexts.Dto.GetMpBabyTextsInput { BabyAge = null }).Result;
            var text= resulta != null ? resulta.BabyText : null;

            //var AesResult = GetAesKey(true);




            //MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
            //var aa= client.GetPointsBalanceAsync(GetMemberAuthKey("4304bbd4857749e6be5e5c2f33166588")).Result;
            //var result= _usermember.GetList(new MpUserMembers.Dto.GetMpUserMembersInput() { MobilePhone = "15512341234" }).Result;
            //string aa = "";

            //OfficialCityServiceSoapClient client = new OfficialCityServiceSoapClient(OfficialCityServiceSoapClient.EndpointConfiguration.OfficialCityServiceSoap);
            //var mckey = UserLoginAndGetMCKey("");
            //var result =  client.GetSubCitysBySuperJsonAsync(mckey, 1).Result;
            //var aa=AesCryptoService.Decrypt(result, AesResult.CryptAESKey, AesResult.CryptAESIV);
            //var result_1 = client.GetSubCitysBySuperJsonAsync(mckey, 7).Result;
            //var aa_1 = AesCryptoService.Decrypt(result_1, AesResult.CryptAESKey, AesResult.CryptAESIV);
            //var result_2 = client.GetSubCitysBySuperJsonAsync(mckey, 180).Result;
            //var aa_2 = AesCryptoService.Decrypt(result_2, AesResult.CryptAESKey, AesResult.CryptAESIV);
            //Logger.Info(aa_2);


            //MemberServiceSoapClient client2 = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
            //var result2 = client2.GetRetailerListByOfficialCityJsonAsync("5a932747-de56-4ec3-b421-25683b46a595", 2059).Result;
            //var bb=AesCryptoService.Decrypt(result2, AesResult.CryptAESKey, AesResult.CryptAESIV);
            //Logger.Info(bb);


            //OfficialCityServiceSoapClient client = new OfficialCityServiceSoapClient(OfficialCityServiceSoapClient.EndpointConfiguration.OfficialCityServiceSoap);
            //var mckey = UserLoginAndGetMCKey("");
            //var result = client.GetSubCitysBySuperJsonAsync(mckey, 1).Result;
            //var rst=AesCryptoService.Decrypt(result, AesResult.CryptAESKey, AesResult.CryptAESIV);
            //return Content(rst);
            //MemberServiceSoapClient client = new MemberServiceSoapClient(MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);

            //var mckey = UserLoginAndGetMCKey("65d010d1e5344e819cd653e16331e748");
            //Logger.Info("mckey：" + mckey);
            //var result = client.GetRetailerListByOfficialCityJsonAsync(mckey, 48529).Result;
            //Logger.Info("回复为：" + result);

            //yilibabyVCIF.VCIFServiceSoapClient _client = new yilibabyVCIF.VCIFServiceSoapClient(yilibabyVCIF.VCIFServiceSoapClient.EndpointConfiguration.VCIFServiceSoap);
            //var bb = _client.SendVerifyCodeWithParamAsync(UserLoginAndGetMCKey(""), 3, "13764103619", "").Result;


            //yilibabyUser.UserLoginSoapClient client = new yilibabyUser.UserLoginSoapClient(yilibabyUser.UserLoginSoapClient.EndpointConfiguration.UserLoginSoap);
            //var aa = client.ResetPasswordAsync(new yilibabyUser.ResetPasswordRequest
            //{
            //    AuthKey = UserLoginAndGetMCKey(""),
            //    mobile = "13764103619",
            //    newpwd = AesCryptoService.Encrypt("123456", AesResult.CryptAESKey, AesResult.CryptAESIV),
            //    VerifyCode = "8888",
            //    VerifyID = bb
            //}).Result;


            //UserLoginSoapClient client = new UserLoginSoapClient(UserLoginSoapClient.EndpointConfiguration.UserLoginSoap);
            //var EncryptPassword = "";

            //EncryptPassword = AesCryptoService.Encrypt("123456", AesResult.CryptAESKey, AesResult.CryptAESIV);

            //////获取最新的AuthKey
            //var result = client.LoginExAsync(new LoginExRequest
            //{
            //    UserName = "13764103619",
            //    EncryptPassword = EncryptPassword,
            //    DeviceCode = deviceCode
            //}).Result;


            return View();
        }
        public ActionResult Index2()
        {
            return View();
        }
        public ActionResult Index3()
        {
            Logger.Info("进入Post");
            var obj = new { token = "123", callurl = "http://yiliscrm3.mgcc.com.cn/TestApi/Index3" };
            Logger.Info(JsonConvert.SerializeObject(obj));
            //var result = HttpPost("http://yiliscrm3.mgcc.com.cn", JsonConvert.SerializeObject(obj), UTF8Encoding.UTF8);
            string callurl = Base64Helper.EncodeBase64("http://yiliscrm3.mgcc.com.cn/TestApi/Index3");
            var result = HttpPost("http://yiliscrm3.mgcc.com.cn/MpApi/Jssdk", "token=123&callurl=http://yiliscrm3.mgcc.com.cn/TestApi/Index3", UTF8Encoding.UTF8);


            var objx = JsonConvert.DeserializeObject<apiresult>(result);
            ViewBag.appId = objx.result.appId;
            ViewBag.timestamp = objx.result.timestamp;
            ViewBag.nonceStr = objx.result.nonceStr;
            ViewBag.signature = objx.result.signature;
            ViewBag.Result = result;
            return View();
        }
        public JsonResult getoauth2()
        {

            string reurl = Base64Helper.EncodeBase64("http://yiliscrm3.mgcc.com.cn/TestApi/Index");
            return Json(reurl);
        }
        public JsonResult getoauth2userinfo()
        {

            string reurl = Base64Helper.EncodeBase64("http://yiliscrm3.mgcc.com.cn/TestApi/Index");
            return Json(reurl);
        }
        public JsonResult getJssdkJsonP()
        {

            string callurl = Base64Helper.EncodeBase64("http://yiliscrm3.mgcc.com.cn/TestApi/Index2?1=1");
            return Json(callurl);
        }
        public JsonResult getJssdk()
        {
            var obj = new { token = "123", callurl = "http://yiliscrm3.mgcc.com.cn/TestApi/Index3?1=1" };


            string callurl = Base64Helper.EncodeBase64("http://yiliscrm3.mgcc.com.cn/TestApi/Index3?1=1");

            var result = HttpPost("http://yiliscrm3.mgcc.com.cn", "token=123&", UTF8Encoding.UTF8);

            return Json(result);
        }

        public string HttpPost(string PostURL, string Data, Encoding encoding)
        {
            string retString = "";
            System.Net.HttpWebRequest request = (HttpWebRequest)WebRequest.Create(PostURL);
            byte[] postData = Encoding.GetEncoding("utf-8").GetBytes(Data);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            request.ContentLength = postData.Length;
            using (System.IO.Stream sw = request.GetRequestStream())
            {
                sw.Write(postData, 0, postData.Length);
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")))
            {
                retString = sr.ReadToEnd();
            }
            return retString;

        }

        private class apiresult
        {
            public apimodel result { get; set; }
        }
        private class apimodel
        {
            public string appId { get; set; }
            public string timestamp { get; set; }
            public string nonceStr { get; set; }
            public string signature { get; set; }
        }
    }
}