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
using Pb.Wechat.MpFansTagItems;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pb.Wechat.Web.Controllers
{
    public class UserApiController : AbpController
    {
        private readonly IMpApiTokenAppService _mpApiTokenAppService;
        private readonly IMpFanAppService _mpFanAppService;
        private readonly IMpFansTagItemAppService _mpFansTagItemAppService;
        public IClientInfoProvider ClientInfoProvider { get; set; }

        public UserApiController(IMpApiTokenAppService mpApiTokenAppService
            , IMpFanAppService mpFanAppService
            , IMpFansTagItemAppService mpFansTagItemAppService)
        {
            _mpApiTokenAppService = mpApiTokenAppService;
            _mpFanAppService = mpFanAppService;
            _mpFansTagItemAppService = mpFansTagItemAppService;
        }

        [HttpPost]
        public async Task<IActionResult> SetUserTags(string token, string openids, string tags)
        {
            var account = await GetAccountToken(token, MpApiTokenType.Nomal.ToString());
            if (account == null)
            {
                Logger.Info($"token为“{token}”的设置标签失败，原因：公众号或令牌不存在，Url：{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}");
                return Content("公众号或令牌不存在");
            }
            if (!string.IsNullOrEmpty(openids) && !string.IsNullOrEmpty(tags))
            {
                var fans = await _mpFanAppService.GetAll(new MpFans.Dto.GetMpFansInput()
                {
                    OpenIDs = openids,
                    GroupSearch = true,
                    MaxResultCount = 1000,
                });
                var outfans = fans.Items.Where(c => c.MpID != account.MpId);
                if (outfans.Count() > 0)
                {
                    return Json(new
                    {
                        result = false,
                        msg = $"openid为{outfans.First().OpenID}的粉丝不属于当前公众号",
                    });
                }
                else
                {
                    foreach (var item in fans.Items)
                    {
                        await _mpFansTagItemAppService.SaveFansTags(account.MpId, item.Id, tags);
                    }
                    return Json(new
                    {
                        result = true
                    });
                }
            }
            return Json(new
            {
                result = false
            });
        }

        private async Task<MpAccountTokenOutput> GetAccountToken(string Token, string ApiType)
        {
            return await _mpApiTokenAppService.GetAccountToken(new MpAccountTokenInput()
            {
                Token = Token,
                ApiType = ApiType
            });
        }
    }
}
