using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Controllers;
using Pb.Wechat.MpApiTokens;
using Pb.Wechat.Url;
using Senparc.Weixin.MP;
using Abp.Auditing;
using Pb.Wechat.MpAccounts;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MvcExtension;
using Pb.Wechat.MpEvents;
using Pb.Wechat.MpSelfArticleGroups;
using Pb.Wechat.MpSelfArticles.Dto.MpSelfArticles;
using Pb.Wechat.MpMediaVideos;
using Abp.Runtime.Caching;
using System.IO;
using System.Text;
using Pb.Wechat.MpEventRequestMsgLogs;
using Pb.Wechat.MpKeyWordReplys;
using Pb.Wechat.MpSelfArticleGroupItems;
using Pb.Wechat.MpEventClickViewLogs;
using Pb.Wechat.MpMenus;
using Pb.Wechat.MpEventScanLogs;
using Pb.Wechat.MpMessages;
using Pb.Wechat.MpAccessTokenClib;
using Pb.Wechat.MpFans;
using Pb.Wechat.CustomerServiceResponseTexts;
using Pb.Wechat.MpUserMembers;
using Pb.Wechat.MpChannels;
using Abp.Dependency;
using Abp.Domain.Uow;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pb.Wechat.Web.Controllers
{
    public class MpCallbackController : AbpController
    {
        private readonly IMpKeyWordReplyAppService _mpKeyWordReplyAppService;
        private readonly IMpSelfArticleGroupItemAppService _mpSelfArticleGroupItemAppService;
        private readonly IMpSelfArticleGroupAppService _mpSelfArticleGroupAppService;
        private readonly IMpSelfArticleAppService _mpSelfArticleAppService;
        private readonly IMpMediaVideoAppService _mpMediaVideoAppService;
        private readonly IMpAccountAppService _mpAccountAppService;
        private readonly ICacheManager _cacheManager;
        private readonly IMpEventAppService _mpEventAppService;
        private readonly IMpEventRequestMsgLogAppService _mpEventRequestMsgLogAppService;
        private readonly IMpEventClickViewLogAppService _mpEventClickViewLogAppService;
        private readonly IMpApiTokenAppService _mpApiTokenAppService;
        private readonly IWebUrlService _webUrlService;
        private readonly IMpMenuAppService _mpMenuAppService;
        private readonly IMpMessageAppService _mpMessageAppService;
        private readonly IAccessTokenContainer _accessTokenContainer;
        private readonly IMpFanAppService _mpFanAppService;
        private readonly IMpEventScanLogAppService _mpEventScanLogAppService;
        private readonly ICustomerServiceResponseTextAppService _customerServiceResponseTextAppService;
        private readonly IMpUserMemberAppService _mpUserMemberAppService;
        private readonly IMpChannelAppService _mpChannelAppService;
        private readonly IIocResolver _iocResolver;
        private readonly IMatialFileService _matialFileService;
        private readonly IYiliBabyClubInterfaceService _yiliBabyClubInterfaceService;
        public IClientInfoProvider ClientInfoProvider { get; set; }

        public MpCallbackController(IAccessTokenContainer accessTokenContainer
            , ICacheManager cacheManager
            , IMpAccountAppService mpAccountAppService
            , IMpApiTokenAppService mpApiTokenAppService
            , IMpEventAppService mpEventAppService
            , IMpEventClickViewLogAppService mpEventClickViewLogAppService
            , IMpEventRequestMsgLogAppService mpEventRequestMsgLogAppService
            , IMpEventScanLogAppService mpEventScanLogAppService
            , IMpFanAppService mpFanAppService
            , IMpKeyWordReplyAppService mpKeyWordReplyAppService
            , IMpMessageAppService mpMessageAppService
            , IMpMenuAppService mpMenuAppService
            , IMpMediaVideoAppService mpMediaVideoAppService
            , IMpSelfArticleAppService mpSelfArticleAppService
            , IMpSelfArticleGroupAppService mpSelfArticleGroupAppService
            , IWebUrlService webUrlService
            , IMpSelfArticleGroupItemAppService mpSelfArticleGroupItemAppService
            , ICustomerServiceResponseTextAppService customerServiceResponseTextAppService
            , IMpUserMemberAppService mpUserMemberAppService
            , IMpChannelAppService mpChannelAppService
            , IIocResolver iocResolver
            , IMatialFileService matialFileService
            , IYiliBabyClubInterfaceService yiliBabyClubInterfaceService
            )
        {
            _accessTokenContainer = accessTokenContainer;
            ClientInfoProvider = NullClientInfoProvider.Instance;
            _cacheManager = cacheManager;
            _mpAccountAppService = mpAccountAppService;
            _mpApiTokenAppService = mpApiTokenAppService;
            _mpEventAppService = mpEventAppService;
            _mpEventClickViewLogAppService = mpEventClickViewLogAppService;
            _mpEventRequestMsgLogAppService = mpEventRequestMsgLogAppService;
            _mpEventScanLogAppService = mpEventScanLogAppService;
            _mpFanAppService = mpFanAppService;
            _mpKeyWordReplyAppService = mpKeyWordReplyAppService;
            _mpMenuAppService = mpMenuAppService;
            _mpMessageAppService = mpMessageAppService;
            _mpMediaVideoAppService = mpMediaVideoAppService;
            _mpSelfArticleAppService = mpSelfArticleAppService;
            _mpSelfArticleGroupAppService = mpSelfArticleGroupAppService;
            _webUrlService = webUrlService;
            _mpSelfArticleGroupItemAppService = mpSelfArticleGroupItemAppService;
            _customerServiceResponseTextAppService = customerServiceResponseTextAppService;
            _mpUserMemberAppService = mpUserMemberAppService;
            _mpChannelAppService = mpChannelAppService;
            _iocResolver = iocResolver;
            _matialFileService = matialFileService;
            _yiliBabyClubInterfaceService = yiliBabyClubInterfaceService;
        }


        #region 公众号回调
        [HttpGet]
        [ActionName("CommonMiniPost")]
        public async Task<IActionResult> Get(PostModel postModel, string echostr)
        {

            int mpId = Convert.ToInt32(Request.Query["MpID"]);
            var account = await _mpAccountAppService.Get(new Abp.Application.Services.Dto.EntityDto<int>()
            {
                Id = mpId
            });

            if (CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, account.Token))
            {
                return Content(echostr); //返回随机字符串则表示验证通过
            }
            else
            {
                return Content("failed:" + postModel.Signature + "," + Senparc.Weixin.MP.CheckSignature.GetSignature(postModel.Timestamp, postModel.Nonce, account.Token) + "。" +
                    "如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。");
            }
        }

        [HttpPost]
        [UnitOfWork(IsDisabled = true)]
        public async Task<IActionResult> CommonMiniPost(PostModel postModel)
        {
            Logger.Info("接收事件处理==========================================================================================================================================================================================================================================================================================================================================================================================================================================================================================================");

            try
            {
                int mpId = Convert.ToInt32( Request.Query["MpID"]);
                var account = await _mpAccountAppService.GetCache(mpId);
                if (account == null)
                    {
                        Logger.Info(string.Format("微信回调错误：找不到MpID为{0}的公众号", mpId));
                        return new WeixinResult("公众号或令牌不存在！");//v0.8+
                    }

                postModel.Token = account.Token;
                postModel.EncodingAESKey = account.EncodingAESKey;//根据自己后台的设置保持一致
                postModel.AppId = account.AppId;//根据自己后台的设置保持一致
                if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, postModel.Token))
                {
                    Logger.Info(string.Format("MpID为{0}的微信回调参数错误！", mpId));
                    return new WeixinResult("参数错误！");//v0.8+
                }

                string body = new StreamReader(Request.Body).ReadToEnd();
                byte[] requestData = Encoding.UTF8.GetBytes(body);
                Stream inputStream = new MemoryStream(requestData);


                var messageHandler = new CustomMessageHandler(inputStream, postModel, Logger, _cacheManager, _mpAccountAppService, mpId, _mpEventAppService, _mpMediaVideoAppService, _mpSelfArticleAppService, _mpSelfArticleGroupAppService, _mpSelfArticleGroupItemAppService, _mpEventRequestMsgLogAppService, _mpKeyWordReplyAppService, _mpEventClickViewLogAppService, _mpMenuAppService, _mpMessageAppService, _accessTokenContainer, _mpFanAppService, _mpEventScanLogAppService, _customerServiceResponseTextAppService, _mpUserMemberAppService, account, _mpChannelAppService, _iocResolver,_matialFileService,_webUrlService, _yiliBabyClubInterfaceService, 10);
                //messageHandler.UsingEcryptMessage = false;
                await messageHandler.ExecuteAsync();//执行微信处理过程
                var result = new FixWeixinBugWeixinResult(messageHandler);//v0.8+

                Logger.Error("Content:" + result.Content);
                return result;
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);

                throw e;
            }

        }

        //public Task<MpAccountDto> GetAccount(string key, string mpId)
        //{
        //    return _cacheManager.GetCache(key).GetAsync(key, async m =>
        //    {

        //        var aa = await _mpAccountAppService.Get(new Abp.Application.Services.Dto.EntityDto<string>()
        //        {
        //            Id = mpId
        //        });

        //    });
        //}
        #endregion



    }
}
