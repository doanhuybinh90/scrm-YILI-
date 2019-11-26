using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pb.Wechat.Authorization;
using Abp.AspNetCore.Mvc.Authorization;
using Pb.Wechat.Web.Controllers;
using Pb.Wechat.MpFans;
using Pb.Wechat.Web.Areas.AppAreaName.Models.MpFans;
using Pb.Wechat.UserMps;
using Microsoft.AspNetCore.Hosting;
using Pb.Wechat.MpGroups;
using Pb.Hangfire.Jobs;
using Abp.BackgroundJobs;
using Pb.Wechat.MpAccounts;
using Pb.Wechat.MpGroups.Dto;
using Pb.Wechat.MpFans.Dto;
using System;
using Newtonsoft.Json;
using Abp.Runtime.Caching;
using yilibabyUser;
using Pb.Wechat.Web.Resources;
using Pb.Wechat.Url;
using System.Security.Cryptography;
using Abp.Web.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_MpUserManagement_MpFans)]
    public class MpFansController : AbpZeroTemplateControllerBase
    {
        private readonly IMpFanAppService _mpFanAppService;
        private readonly IUserMpAppService _userMpAppService;
        private readonly IHostingEnvironment _host = null;
        private readonly IMpGroupAppService _mpGroupAppService;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly IMpAccountAppService _mpAccountService;
        private readonly ICacheManager _cacheManager;
        private static string defaultUserName = "";
        private static string defaultUserPwd = "";
        private static string deviceCode = "";
        private readonly IYiliBabyClubInterfaceService _yiliBabyClubInterfaceService;
        public MpFansController(IMpFanAppService mpFanAppService, IUserMpAppService userMpAppService, IHostingEnvironment host, IMpGroupAppService mpGroupAppService, IBackgroundJobManager backgroundJobManager, IMpAccountAppService mpAccountService, ICacheManager cacheManager, IYiliBabyClubInterfaceService yiliBabyClubInterfaceService)
        {
            _mpFanAppService = mpFanAppService;
            _userMpAppService = userMpAppService;
            _host = host;
            _mpGroupAppService = mpGroupAppService;
            _backgroundJobManager = backgroundJobManager;
            _mpAccountService = mpAccountService;
            _cacheManager = cacheManager;
            _yiliBabyClubInterfaceService = yiliBabyClubInterfaceService;
            defaultUserName = _yiliBabyClubInterfaceService.ApiUserName;
            defaultUserPwd = _yiliBabyClubInterfaceService.ApiPassword;
            deviceCode = _yiliBabyClubInterfaceService.ApiDeviceCode;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var viewModel = new MpFanViewModel();
            //viewModel.Groups = await _mpGroupAppService.GetList();
            viewModel.MpID =await _userMpAppService.GetDefaultMpId();
            viewModel.Token = (await _mpAccountService.Get(new Abp.Application.Services.Dto.EntityDto<int> { Id = viewModel.MpID })).TaskAccessToken;
            return View(viewModel);
        }

        //public async Task<PartialViewResult> CreateOrEditModal(int id)
        //{
        //    if (id!=0)
        //    {
        //        var output = await _mpFanAppService.Get(new EntityDto<int>(id));
        //        var viewModel = new CreateOrEditMpFanViewModel(output);
        //        return PartialView("_CreateOrEditModal", viewModel);
        //    }
        //    else
        //    {
        //        var mpID=_userMpAppService.GetDefaultMpId().Result;
        //        var model = new CreateOrEditMpFanViewModel() { MpID = mpID };
        //        return PartialView("_CreateOrEditModal", model);
        //    }

        //}

        public PartialViewResult ImageSelectorModal()
        {
            return PartialView("_ImageSelector");
        }

        public PartialViewResult InputNickName()
        {
            return PartialView("_InputNickName");
        }
        public PartialViewResult SelectorModal(string isrenew)
        {
            ViewBag.isRenew = isrenew;
            return PartialView("_SelectorModal");
        }



        public async Task UpdateFans()
        {
            await _backgroundJobManager.EnqueueAsync<WechatGroupRefreshNowJob, bool>(false);
        }

        [DontWrapResult]
        public async Task<IActionResult> GetPrms(int groupId)
        {
            var item = await _mpGroupAppService.GetItem(groupId);
            var _MpID = await _userMpAppService.GetDefaultMpId();
            if (item != null)
            {
                if (item.IsMember == IsMemberEnum.ALL.ToString())
                {
                    return Json(new { type = "ALL", openIds = "" });
                }
                else if (item.IsMember == IsMemberEnum.NotMember.ToString())
                {
                    return Json(new { type = "NotMember", openIds = "" });
                }
                else
                {
                    yilibabyMember.MemberServiceSoapClient client = new yilibabyMember.MemberServiceSoapClient(yilibabyMember.MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
                    var authKey = await DefaultLoginAndGetMCKey();

                    if (item.MotherType == MotherType.ALL.ToString())
                    {
                        item.BeginBabyBirthday = new DateTime(1900, 1, 1);
                        item.EndBabyBirthday = new DateTime(1900, 1, 1);
                    }
                    else if (item.MotherType == MotherType.UnPregnant.ToString())
                    {
                        DateTime date = DateTime.Now.AddDays(280);
                        item.BeginBabyBirthday = date;
                        item.EndBabyBirthday = new DateTime(1900, 1, 1);
                    }
                    else if (item.MotherType == MotherType.Pregnant.ToString())
                    {
                        DateTime date = DateTime.Now.AddDays(280);
                        item.BeginBabyBirthday = DateTime.Now;
                        item.EndBabyBirthday = date;
                    }
                    else if (item.MotherType == MotherType.One.ToString())
                    {
                        DateTime date = DateTime.Now.AddDays(-180);
                        item.BeginBabyBirthday = date;
                        item.EndBabyBirthday = DateTime.Now;
                    }
                    else if (item.MotherType == MotherType.Two.ToString())
                    {
                        DateTime date = DateTime.Now.AddDays(-365);
                        DateTime date2 = DateTime.Now.AddDays(-180);
                        item.BeginBabyBirthday = date;
                        item.EndBabyBirthday = date2;
                    }
                    else if (item.MotherType == MotherType.Three.ToString())
                    {
                        DateTime date = DateTime.Now.AddDays(-730);
                        DateTime date2 = DateTime.Now.AddDays(-365);
                        item.BeginBabyBirthday = date;
                        item.EndBabyBirthday = date2;
                    }
                    else if (item.MotherType == MotherType.Four.ToString())
                    {
                        DateTime date = DateTime.Now.AddDays(-730);
                        item.BeginBabyBirthday = new DateTime(1900, 1, 1);
                        item.EndBabyBirthday = date;
                    }

                    var obj = new
                    {
                        BaySex = item.BaySex,
                        OrganizeCity = !string.IsNullOrWhiteSpace(item.OrganizeCity) ? item.OrganizeCity.Split(',') : null,
                        OfficialCity = !string.IsNullOrWhiteSpace(item.OfficialCity) ? item.OfficialCity.Split(',') : null,
                        LastBuyProduct = !string.IsNullOrWhiteSpace(item.LastBuyProduct) ? item.LastBuyProduct.Split(',') : null,
                        MemberCategory = !string.IsNullOrWhiteSpace(item.MemberCategory) ? item.MemberCategory.Split(',') : null,
                        BeginBabyBirthday = item.BeginBabyBirthday,
                        EndBabyBirthday = item.EndBabyBirthday,
                        BeginPointsBalance = item.BeginPointsBalance,
                        EndPointsBalance = item.EndPointsBalance
                    };
                    var str = JsonConvert.SerializeObject(obj);

                    var openIds = await client.FindWeChatMemberByAdvConditionAsync(authKey, str);
                    return Json(new { type = "Member", openIds = openIds != null ? string.Join(",", openIds) : "" });
                    
                }
            }
            else
                return Json("");
        }

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

    }
}
