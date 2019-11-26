using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MessageHandlers;
using Senparc.Weixin.MP.Helpers;
using Pb.Wechat.Web.Models;
using Castle.Core.Logging;
using Abp.Runtime.Caching;
using Pb.Wechat.MpAccounts;
using Pb.Wechat.MpEvents.Dto;
using Pb.Wechat.MpEvents;
using Pb.Wechat.MpMediaVideos;
using Pb.Wechat.MpSelfArticleGroups;
using Pb.Wechat.MpSelfArticles.Dto.MpSelfArticles;
using Pb.Wechat.MpSelfArticleGroupItems;
using Pb.Wechat.MpEventRequestMsgLogs;
using Pb.Wechat.MpKeyWordReplys;
using Pb.Wechat.MpEventClickViewLogs;
using Pb.Wechat.MpMenus;
using Pb.Wechat.MpEventScanLogs;
using Pb.Wechat.MpMessages;
using Pb.Wechat.MpAccessTokenClib;
using Pb.Wechat.MpFans;
using Senparc.Weixin.MP.AdvancedAPIs;
using System.Threading.Tasks;
using Pb.Wechat.MpEventRequestMsgLogs.Dto;
using Abp.Application.Services.Dto;
using Pb.Wechat.CustomerServiceResponseTexts;
using Pb.Wechat.MpUserMembers;
using Pb.Wechat.MpAccounts.Dto;
using Pb.Wechat.MpChannels;
using Abp.Dependency;
using Pb.Wechat.Web.Resources;
using Pb.Wechat.MpSecondKeyWordReplys;
using Senparc.Weixin.Context;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.MP;
using Pb.Wechat.MpKeyWordReplys.Dto;
using Pb.Wechat.Url;

#if NET45
using System.Web;
using System.Configuration;
using System.Web.Configuration;
using Senparc.Weixin.MP.Sample.CommonService.Utilities;
#else
#endif

namespace Pb.Wechat.Web
{
    public partial class CustomMessageHandler : MessageHandler<CustomMessageContext>
    {
        private readonly IAccessTokenContainer _accessTokenContainer;
        private readonly ICacheManager _cacheManager;
        private readonly IMpAccountAppService _mpAccountAppService;
        private readonly IMpEventAppService _mpEventAppService;
        private readonly IMpEventClickViewLogAppService _mpEventClickViewLogAppService;
        private readonly IMpEventRequestMsgLogAppService _mpEventRequestMsgLogAppService;
        private readonly IMpEventScanLogAppService _mpEventScanLogAppService;
        private readonly IMpFanAppService _mpFanAppService;
        private readonly IMpKeyWordReplyAppService _mpKeyWordReplyAppService;
        private readonly IMpMediaVideoAppService _mpMediaVideoAppService;
        private readonly IMpMenuAppService _mpMenuAppService;
        private readonly IMpMessageAppService _mpMessageAppService;
        private readonly IMpSelfArticleGroupItemAppService _mpSelfArticleGroupItemAppService;
        private readonly IMpSelfArticleGroupAppService _mpSelfArticleGroupAppService;
        private readonly IMpSelfArticleAppService _mpSelfArticleAppService;
        private readonly ICustomerServiceResponseTextAppService _customerServiceResponseTextAppService;
        private readonly IMpUserMemberAppService _mpUserMemberAppService;
        private readonly MpAccountDto _account;
        private readonly IMpChannelAppService _mpChannelAppService;
        private readonly IIocResolver _iocResolver;
        private readonly IWebUrlService _webUrlService;
        private readonly IMatialFileService _matialFileService;
        private readonly IYiliBabyClubInterfaceService _yiliBabyClubInterfaceService;
        private int mpId;
       
        private string domain = "";

        private string appId = "appId";
        private ILogger _logger;

        /// <summary>
        /// 模板消息集合（Key：checkCode，Value：OpenId）
        /// </summary>
        public static Dictionary<string, string> TemplateMessageCollection = new Dictionary<string, string>();

        public CustomMessageHandler(Stream inputStream, PostModel postModel, ILogger logger
            , ICacheManager cacheManager, IMpAccountAppService mpAccountAppService, int _mpId
            , IMpEventAppService mpEventAppService, IMpMediaVideoAppService mpMediaVideoAppService
            , IMpSelfArticleAppService mpSelfArticleAppService
            , IMpSelfArticleGroupAppService mpSelfArticleGroupAppService
            , IMpSelfArticleGroupItemAppService mpSelfArticleGroupItemAppService
            , IMpEventRequestMsgLogAppService mpEventRequestMsgLogAppService
            , IMpKeyWordReplyAppService mpKeyWordReplyAppService
            , IMpEventClickViewLogAppService mpEventClickViewLogAppService
            , IMpMenuAppService mpMenuAppService
            , IMpMessageAppService mpMessageAppService
            , IAccessTokenContainer accessTokenContainer
            , IMpFanAppService mpFanAppService
            , IMpEventScanLogAppService mpEventScanLogAppService
            , ICustomerServiceResponseTextAppService customerServiceResponseTextAppService
            , IMpUserMemberAppService mpUserMemberAppService
            , MpAccountDto account
            , IMpChannelAppService mpChannelAppService
            , IIocResolver iocResolver
            , IMatialFileService matialFileService
            , IWebUrlService webUrlService
            , IYiliBabyClubInterfaceService yiliBabyClubInterfaceService
            , int maxRecordCount = 0)
            : base(inputStream, postModel, maxRecordCount)
        {
            mpId = _mpId;
            _cacheManager = cacheManager;
            _logger = logger;
            _accessTokenContainer = accessTokenContainer;
            _mpAccountAppService = mpAccountAppService;
            _mpEventAppService = mpEventAppService;
            _mpEventClickViewLogAppService = mpEventClickViewLogAppService;
            _mpEventRequestMsgLogAppService = mpEventRequestMsgLogAppService;
            _mpEventScanLogAppService = mpEventScanLogAppService;
            _mpFanAppService = mpFanAppService;
            _mpKeyWordReplyAppService = mpKeyWordReplyAppService;
            _mpMediaVideoAppService = mpMediaVideoAppService;
            _mpMenuAppService = mpMenuAppService;
            _mpMessageAppService = mpMessageAppService;
            _mpSelfArticleGroupItemAppService = mpSelfArticleGroupItemAppService;
            _mpSelfArticleGroupAppService = mpSelfArticleGroupAppService;
            _mpSelfArticleAppService = mpSelfArticleAppService;
            WeixinContext.ExpireMinutes = 3;
            _customerServiceResponseTextAppService = customerServiceResponseTextAppService;
            _mpUserMemberAppService = mpUserMemberAppService;
            _iocResolver = iocResolver;
            _account = account;
            _matialFileService = matialFileService;
            _webUrlService = webUrlService;
            _mpChannelAppService = mpChannelAppService;
            _yiliBabyClubInterfaceService = yiliBabyClubInterfaceService;
            domain = _webUrlService.GetSiteRootAddress();
            if (!string.IsNullOrEmpty(postModel.AppId))
            {
                appId = postModel.AppId;//通过第三方开放平台发送过来的请求
            }

            //在指定条件下，不使用消息去重
            base.OmitRepeatedMessageFunc = requestMessage =>
            {
                var textRequestMessage = requestMessage as RequestMessageText;
                if (textRequestMessage != null && textRequestMessage.Content == "容错")
                {
                    return false;
                }
                return true;
            };
        }

        public CustomMessageHandler(RequestMessageBase requestMessage)
            : base(requestMessage)
        {
        }


        public override async Task OnExecutingAsync()
        {
            //测试MessageContext.StorageData
            if (CurrentMessageContext.StorageData == null)
            {
                CurrentMessageContext.StorageData = 0;
            }
            await base.OnExecutingAsync();
        }

        public override async Task OnExecutedAsync()
        {
            await base.OnExecutedAsync();
            CurrentMessageContext.StorageData = ((int)CurrentMessageContext.StorageData) + 1;
        }

        /// <summary>
        /// 处理文字请求
        /// </summary>
        /// <returns></returns>
        public override async Task<IResponseMessageBase> OnTextRequestAsync(RequestMessageText requestMessage)
        {
            var defaultMessage =await DefaultResponseMessageAsync(requestMessage);
            if (string.IsNullOrEmpty(requestMessage.Content))
                return defaultMessage;
            #region 记录日志
            try
            {
                var opid = requestMessage.FromUserName;
                var entitymsg = new MpEventRequestMsgLogDto();

                entitymsg.MpID = mpId;
                entitymsg.OpenID = opid;
                entitymsg.MsgType = requestMessage.MsgType.ToString();
                entitymsg.MsgId = requestMessage.MsgId.ToString();
                entitymsg.Content = requestMessage.Content;
                await _mpEventRequestMsgLogAppService.Create(entitymsg);

            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("MPID{0}记录文本回复信息出错：原因{1}", mpId, ex.Message));
            }
            #endregion

            _logger.Info($"开始处理文本消息 {requestMessage.FromUserName} {requestMessage.Content}");
            #region 春雨医生逻辑处理
            var chunyuHandler = _iocResolver.Resolve<IChunYuMessageHandler>();
            if (await chunyuHandler.IsAsking(mpId, requestMessage.FromUserName))
            {
                _logger.Info($"春雨医生正在会话 {requestMessage.FromUserName} {requestMessage.Content}");
                await chunyuHandler.Ask(mpId, requestMessage.FromUserName, requestMessage.MsgType.ToString(), requestMessage.Content);
                return requestMessage.CreateResponseMessage<ResponseMessageNoResponse>();
            }
            #endregion

            #region 客服逻辑处理
            _logger.Info($"春雨医生没有会话 {requestMessage.FromUserName} {requestMessage.Content}");
            var kefuHandler = _iocResolver.Resolve<IKeFuMessageHandler>();
            if (await kefuHandler.IsAsking(mpId, requestMessage.FromUserName))
            {
                _logger.Info($"客服正在会话 {requestMessage.FromUserName} {requestMessage.Content}");
                await kefuHandler.Ask(mpId, requestMessage.FromUserName, requestMessage.MsgType.ToString(), requestMessage.Content);
                return requestMessage.CreateResponseMessage<ResponseMessageNoResponse>();
            }
            _logger.Info($"客服没有会话 {requestMessage.FromUserName} {requestMessage.Content}");
            #endregion

            var account =await _mpAccountAppService.GetCache(mpId);
            var rs =await _cacheManager.GetCache(AppConsts.Cache_CallBack).GetOrDefaultAsync(string.Format("TextRequest_{0}_{1}", account.AppId, requestMessage.Content));
            var rstype =await _cacheManager.GetCache(AppConsts.Cache_CallBack).GetOrDefaultAsync(string.Format("TextRequest_{0}_{1}_Type", account.AppId, requestMessage.Content));
            var rsid = await _cacheManager.GetCache(AppConsts.Cache_CallBack).GetOrDefaultAsync(string.Format("TextRequest_{0}_{1}_Id", account.AppId, requestMessage.Content));
            var firstkey = await _cacheManager.GetCache(AppConsts.Cache_FirstKeyWordReply).GetOrDefaultAsync(requestMessage.FromUserName);


            #region 短时间内输入过一级回复，则查询当次输入是否属于二级回复
            if (firstkey != null)
            {
                var keyid = Convert.ToInt32(firstkey);
                var srs = await _cacheManager.GetCache(AppConsts.Cache_CallBack).GetOrDefaultAsync(string.Format("TextRequest_{0}_{1}_{2}_Second", account.AppId, keyid, requestMessage.Content));
                var srstype = await _cacheManager.GetCache(AppConsts.Cache_CallBack).GetOrDefaultAsync(string.Format("TextRequest_{0}_{1}_{2}_Second_Type", account.AppId, keyid, requestMessage.Content));
                if (srs == null || srstype == null)
                {
                    var mpSecondKeyWordReplyAppService = _iocResolver.Resolve<IMpSecondKeyWordReplyAppService>();
                    var reply = await mpSecondKeyWordReplyAppService.GetEntityByKeyWordAsync(requestMessage.Content, keyid);
                    if (reply != null)
                    {
                        await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("TextRequest_{0}_{1}_{2}_Second_Type", account.AppId, keyid, requestMessage.Content), reply.ReplyType);
                        var res = await GetEntityResponse(reply.ReplyType, reply.ImageMediaID, reply.Content, reply.VoiceMediaID, (reply.VideoID ?? -1).ToString(), (reply.ArticleID ?? -1).ToString(), (reply.ArticleGroupID ?? -1).ToString());
                        if (res != null)
                        {
                            await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("TextRequest_{0}_{1}_{2}_Second", account.AppId, keyid, requestMessage.Content), res);
                            return res;
                        }
                        else
                        {
                            return requestMessage.CreateResponseMessage<ResponseMessageNoResponse>();
                        }
                    }
                }
                else
                {
                    var res =await GetCacheResponse(srstype.ToString(), srs);
                    if (res != null)
                        return res;
                }
            }
            #endregion

            #region 没有命中二级回复，则查询一级回复
            if (rs == null || rstype == null || rsid == null)
            {
                //关键字回复
                var entity =await _mpKeyWordReplyAppService.GetEntityByKeyWordAsync(requestMessage.Content, mpId);
                if (entity != null)
                {
                    await _cacheManager.GetCache(AppConsts.Cache_FirstKeyWordReply).SetAsync(requestMessage.FromUserName, entity.Id);
                    await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("TextRequest_{0}_{1}_Type", account.AppId, requestMessage.Content), entity.ReplyType);
                    await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("TextRequest_{0}_{1}_Id", account.AppId, requestMessage.Content), entity.Id);
                    var res = await GetEntityResponse(entity.ReplyType, entity.ImageMediaID, entity.Content, entity.VoiceMediaID, (entity.VideoID ?? -1).ToString(), (entity.ArticleID ?? -1).ToString(), (entity.ArticleGroupID ?? -1).ToString(), requestMessage);
                    if (res != null)
                    {
                        await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("TextRequest_{0}_{1}", account.AppId, requestMessage.Content), res);
                        return res;
                    }
                    else
                    {
                        var none = requestMessage.CreateResponseMessage<ResponseMessageNoResponse>();
                        await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("TextRequest_{0}_{1}", account.AppId, requestMessage.Content), none);
                        return none;
                    }
                }
                //其他回复
                else
                    return defaultMessage;
            }
            else
            {
                var replyType=(string)await _cacheManager.GetCache(AppConsts.Cache_CallBack).GetOrDefaultAsync(string.Format("TextRequest_{0}_{1}_Type", account.AppId, requestMessage.Content));
                if (replyType == ReplyMsgType.kf.ToString())
                {
                    var kefuMessageHandler = _iocResolver.Resolve<IKeFuMessageHandler>();
                    return await kefuMessageHandler.InCustomer(mpId, requestMessage);
                }
                else
                {
                    var res = await GetCacheResponse(rstype.ToString(), rs, requestMessage);
                    if (res != null)
                    {
                        await _cacheManager.GetCache(AppConsts.Cache_FirstKeyWordReply).SetAsync(requestMessage.FromUserName, Convert.ToInt32(rsid));
                        return res;
                    }
                }
               
            }
            #endregion
            return defaultMessage;
        }

        /// <summary>
        /// 处理位置请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override async Task<IResponseMessageBase> OnLocationRequestAsync(RequestMessageLocation requestMessage)
        {
            return await DefaultResponseMessageAsync(requestMessage);
        }

        public override async Task<IResponseMessageBase> OnShortVideoRequestAsync(RequestMessageShortVideo requestMessage)
        {
            //记录短视频回复信息
            try
            {
                var opid = requestMessage.FromUserName;
                var entitymsg = new MpEventRequestMsgLogDto();

                entitymsg.MpID = mpId;
                entitymsg.OpenID = opid;
                entitymsg.MsgType = requestMessage.MsgType.ToString();
                entitymsg.MsgId = requestMessage.MsgId.ToString();
                entitymsg.MediaId = requestMessage.MediaId;
                await _mpEventRequestMsgLogAppService.Create(entitymsg);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("MPID{0}记录短视频回复信息出错：原因{1}", mpId, ex.Message));
            }


            #region 客服逻辑处理
            var kefuHandler = _iocResolver.Resolve<IKeFuMessageHandler>();
            if (await kefuHandler.IsAsking(mpId, requestMessage.FromUserName))
            {
                await kefuHandler.Ask(mpId, requestMessage.FromUserName, requestMessage.MsgType.ToString(), requestMessage.MediaId);
                return requestMessage.CreateResponseMessage<ResponseMessageNoResponse>();
            }
            #endregion

            return await DefaultResponseMessageAsync(requestMessage);
        }

        /// <summary>
        /// 处理图片请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override async Task<IResponseMessageBase> OnImageRequestAsync(RequestMessageImage requestMessage)
        {
            try
            {
                var opid = requestMessage.FromUserName;
                var entitymsg = new MpEventRequestMsgLogDto();

                entitymsg.MpID = mpId;
                entitymsg.OpenID = opid;
                entitymsg.MsgType = requestMessage.MsgType.ToString();
                entitymsg.MsgId = requestMessage.MsgId.ToString();
                entitymsg.MediaId = requestMessage.MediaId;
                await _mpEventRequestMsgLogAppService.Create(entitymsg);
                

                #region 春雨医生逻辑处理
                var chunyuHandler = _iocResolver.Resolve<IChunYuMessageHandler>();
                if (await chunyuHandler.IsAsking(mpId, requestMessage.FromUserName))
                {
                    await chunyuHandler.Ask(mpId, requestMessage.FromUserName, requestMessage.MsgType.ToString(), requestMessage.MediaId);
                    return requestMessage.CreateResponseMessage<ResponseMessageNoResponse>();
                }
                #endregion
                
                #region 客服逻辑处理
                var kefuHandler = _iocResolver.Resolve<IKeFuMessageHandler>();
                if (await kefuHandler.IsAsking(mpId, requestMessage.FromUserName))
                {
                    await kefuHandler.Ask(mpId, requestMessage.FromUserName, requestMessage.MsgType.ToString(), requestMessage.MediaId);
                    return requestMessage.CreateResponseMessage<ResponseMessageNoResponse>();
                }
                #endregion
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("MPID{0}记录图片回复信息出错：原因{1}", mpId, ex.Message));
            }
            return await DefaultResponseMessageAsync(requestMessage);
        }

        /// <summary>
        /// 处理语音请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override async Task<IResponseMessageBase> OnVoiceRequestAsync(RequestMessageVoice requestMessage)
        {
            try
            {
                var opid = requestMessage.FromUserName;
                var entitymsg = new MpEventRequestMsgLogDto();
                
                entitymsg.MpID = mpId;
                entitymsg.OpenID = opid;
                entitymsg.MsgType = requestMessage.MsgType.ToString();
                entitymsg.MsgId = requestMessage.MsgId.ToString();
                entitymsg.MediaId = requestMessage.MediaId;
                await _mpEventRequestMsgLogAppService.Create(entitymsg);
                
                #region 春雨医生逻辑处理
                var chunyuHandler = _iocResolver.Resolve<IChunYuMessageHandler>();
                if (await chunyuHandler.IsAsking(mpId, requestMessage.FromUserName))
                {
                    await chunyuHandler.Ask(mpId, requestMessage.FromUserName, requestMessage.MsgType.ToString(), requestMessage.MediaId);
                    return requestMessage.CreateResponseMessage<ResponseMessageNoResponse>();
                }
                #endregion
                
                #region 客服逻辑处理
                var kefuHandler = _iocResolver.Resolve<IKeFuMessageHandler>();
                if (await kefuHandler.IsAsking(mpId, requestMessage.FromUserName))
                {
                    await kefuHandler.Ask(mpId, requestMessage.FromUserName, requestMessage.MsgType.ToString(), requestMessage.MediaId, requestMessage.Format);
                    return requestMessage.CreateResponseMessage<ResponseMessageNoResponse>();
                }
                #endregion

            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("MPID{0}记录语音回复信息出错：原因{1}", mpId, ex.Message));
            }
            return await DefaultResponseMessageAsync(requestMessage);
        }
        /// <summary>
        /// 处理视频请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override async Task<IResponseMessageBase> OnVideoRequestAsync(RequestMessageVideo requestMessage)
        {
            try
            {
                var opid = requestMessage.FromUserName;
                var entitymsg = new MpEventRequestMsgLogDto();

                entitymsg.MpID = mpId;
                entitymsg.OpenID = opid;
                entitymsg.MsgType = requestMessage.MsgType.ToString();
                entitymsg.MsgId = requestMessage.MsgId.ToString();
                entitymsg.MediaId = requestMessage.MediaId;
                await _mpEventRequestMsgLogAppService.Create(entitymsg);

            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("MPID{0}记录视频回复信息出错：原因{1}", mpId, ex.Message));
            }


            #region 客服逻辑处理
            var kefuHandler = _iocResolver.Resolve<IKeFuMessageHandler>();
            if (await kefuHandler.IsAsking(mpId, requestMessage.FromUserName))
            {
                await kefuHandler.Ask(mpId, requestMessage.FromUserName, requestMessage.MsgType.ToString(), requestMessage.MediaId);
                return requestMessage.CreateResponseMessage<ResponseMessageNoResponse>();
            }
            #endregion

            return await DefaultResponseMessageAsync(requestMessage);
        }


        /// <summary>
        /// 处理链接消息请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override async Task<IResponseMessageBase> OnLinkRequestAsync(RequestMessageLink requestMessage)
        {
            try
            {
                var opid = requestMessage.FromUserName;
                var entitymsg = new MpEventRequestMsgLogDto();
                entitymsg.MpID = mpId;
                entitymsg.OpenID = opid;
                entitymsg.MsgType = requestMessage.MsgType.ToString();
                entitymsg.MsgId = requestMessage.MsgId.ToString();
                entitymsg.Title = requestMessage.Title;
                entitymsg.Description = requestMessage.Description;
                entitymsg.AUrl = requestMessage.Url;
                await _mpEventRequestMsgLogAppService.Create(entitymsg);
            }
            catch (Exception ex)
            {

                _logger.Error(string.Format("MPID{0}记录链接回复信息出错：原因{1}", mpId, ex.Message));
            }
            return await DefaultResponseMessageAsync(requestMessage);

        }

        /// <summary>
        /// 处理事件请求（这个方法一般不用重写，这里仅作为示例出现。除非需要在判断具体Event类型以外对Event信息进行统一操作
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override async Task<IResponseMessageBase> OnEventRequestAsync(IRequestMessageEventBase requestMessage)
        {
           
            var eventResponseMessage =await base.OnEventRequestAsync(requestMessage);//对于Event下属分类的重写方法，见：CustomerMessageHandler_Events.cs

            //TODO: 对Event信息进行统一操作
            return eventResponseMessage;
        }

        public override async Task<IResponseMessageBase> DefaultResponseMessageAsync(IRequestMessageBase requestMessage)
        {
            /* 所有没有被处理的消息会默认返回这里的结果，
             * 因此，如果想把整个微信请求委托出去（例如需要使用分布式或从其他服务器获取请求），
             * 只需要在这里统一发出委托请求，如：
             * var responseMessage = MessageAgent.RequestResponseMessage(agentUrl, agentToken, RequestDocument.ToString());
             * return responseMessage;
             */
             
            var account =await _mpAccountAppService.GetCache(mpId);
            var rs =await _cacheManager.GetCache(AppConsts.Cache_CallBack).GetOrDefaultAsync(string.Format("DefaultRequest_{0}", account.AppId));
            var rstype = await _cacheManager.GetCache(AppConsts.Cache_CallBack).GetOrDefaultAsync(string.Format("DefaultRequest_{0}_Type", account.AppId));
            if (rs == null || rstype == null)
            {
                var eventtype = MpEventType.AutoReply.ToString();
                var entity =await _mpEventAppService.GetModelByEventTypeAsync(eventtype, mpId);

                if (entity != null)
                {
                    await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("DefaultRequest_{0}_Type", account.AppId), entity.ReplyType);
                    var res = await GetEntityResponse(entity.ReplyType, entity.ImageMediaID, entity.Content, entity.VoiceMediaID, (entity.VideoID ?? -1).ToString(), (entity.ArticleID ?? -1).ToString(), (entity.ArticleGroupID ?? -1).ToString());
                    if (res != null)
                    {
                        await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("DefaultRequest_{0}", account.AppId), res);
                        return res;
                    }
                    else
                    {
                        var none = requestMessage.CreateResponseMessage<ResponseMessageNoResponse>();
                        await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("DefaultRequest_{0}", account.AppId), none);
                        return none;
                    }
                }
            }
            else
            {
                var res =await GetCacheResponse(rstype.ToString(), rs);
                if (res != null)
                    return res;
            }
            return base.CreateResponseMessage<ResponseMessageNoResponse>();
        }


        public override async Task<IResponseMessageBase> OnUnknownTypeRequestAsync(RequestMessageUnknownType requestMessage)
        {
            /*
             * 此方法用于应急处理SDK没有提供的消息类型，
             * 原始XML可以通过requestMessage.RequestDocument（或this.RequestDocument）获取到。
             * 如果不重写此方法，遇到未知的请求类型将会抛出异常（v14.8.3 之前的版本就是这么做的）
             */
            var msgType = MsgTypeHelper.GetRequestMsgTypeString(requestMessage.RequestDocument);
            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "未知消息类型：" + msgType;
            return responseMessage;
        }

        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            return null;
        }

        #region 获取实体中的回复
        /// <summary>
        /// 获取实体中的回复
        /// </summary>
        /// <param name="type"></param>
        /// <param name="imageMediaID"></param>
        /// <param name="content"></param>
        /// <param name="voiceMediaID"></param>
        /// <param name="videoID"></param>
        /// <param name="articleID"></param>
        /// <param name="articleGroupID"></param>
        /// <returns></returns>
        private async Task<IResponseMessageBase> GetEntityResponse(string type, string imageMediaID,string content,string voiceMediaID,string videoID,string articleID,string articleGroupID,RequestMessageText requestMessage=null) {
            if (type == ReplyMsgType.kf.ToString())
            {
                #region 春雨医生逻辑处理
                var chunyuHandler = _iocResolver.Resolve<IChunYuMessageHandler>();
                //如果春雨医生未结束，先引导用户主动关闭春雨医生会话
                if (await chunyuHandler.IsAsking(mpId, requestMessage.FromUserName))
                {
                    return await chunyuHandler.CustomerNotice(mpId, requestMessage);
                }
                #endregion

                #region 客服逻辑处理
                var kefuMessageHandler = _iocResolver.Resolve<IKeFuMessageHandler>();
                return await kefuMessageHandler.InCustomer(mpId, requestMessage);
                #endregion
            }
            else if (type == ReplyMsgType.doctor.ToString())
            {
                #region 客服逻辑处理
                var kefuMessageHandler = _iocResolver.Resolve<IKeFuMessageHandler>();
                if (await kefuMessageHandler.IsAsking(mpId, requestMessage.FromUserName))
                {
                    return await kefuMessageHandler.ChunYuNotice(mpId, requestMessage);
                }
                #endregion

                #region 春雨医生逻辑处理
                var chunyuHandler = _iocResolver.Resolve<IChunYuMessageHandler>();
                return await chunyuHandler.CreateProblem(mpId, requestMessage);
                #endregion
            }
            else if (type == ReplyMsgType.none.ToString())
            {
                return base.CreateResponseMessage<ResponseMessageNoResponse>();
            }
            else if (type == ReplyMsgType.image.ToString())
            {
                var responseMessage = base.CreateResponseMessage<ResponseMessageImage>();
                responseMessage.Image.MediaId = imageMediaID;
                return responseMessage;
            }
            else if (type == ReplyMsgType.text.ToString())
            {
                var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
                responseMessage.Content = content;
                return responseMessage;
            }
            else if (type == ReplyMsgType.voice.ToString())
            {
                var responseMessage = base.CreateResponseMessage<ResponseMessageVoice>();
                responseMessage.Voice.MediaId = voiceMediaID;
                return responseMessage;
            }
            else if (type == ReplyMsgType.video.ToString())
            {
                var responseMessage = base.CreateResponseMessage<ResponseMessageVideo>();
                int vedioId = 0;
                int.TryParse(videoID, out vedioId);
                var video = await _mpMediaVideoAppService.Get(new EntityDto<int> { Id = vedioId });

                if (video != null)
                {
                    responseMessage.Video.MediaId = video.MediaID;
                    responseMessage.Video.Title = video.Title;
                    responseMessage.Video.Description = video.Description;
                    return responseMessage;
                }
                else
                {
                    return base.CreateResponseMessage<ResponseMessageNoResponse>();
                }
            }
            else if (type == ReplyMsgType.mpnews.ToString())
            {
                var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                int articleId = 0;
                int.TryParse(articleID, out articleId);
                var article = await _mpSelfArticleAppService.Get(new EntityDto<int> { Id = articleId });
                if (article != null)
                {
                    responseMessage.Articles.Add(new Article()
                    {
                        Title = article.Title,
                        Description = article.Description,
                        Url = article.AUrl,
                        PicUrl = article.FilePathOrUrl
                    });
                    return responseMessage;
                }
                else
                {
                    return base.CreateResponseMessage<ResponseMessageNoResponse>();
                }
            }
            else if (type == ReplyMsgType.mpmultinews.ToString())
            {
                var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                int articleGroupId = 0;
                int.TryParse(articleGroupID, out articleGroupId);
                var groupItems = await _mpSelfArticleGroupItemAppService.GetGroupItems(articleGroupId);

                if (!(groupItems == null || groupItems.Count() < 2))
                {
                    var articleIds = groupItems.Select(m => m.ArticleID).ToList();
                    var articles = await _mpSelfArticleAppService.GetListByIds(articleIds);
                    foreach (var id in articleIds)
                    {
                        var item = articles.Where(m => m.Id == id).FirstOrDefault();
                        responseMessage.Articles.Add(new Article()
                        {
                            Title = item.Title,
                            Description = item.Description,
                            Url = item.AUrl,
                            PicUrl = item.FilePathOrUrl
                        });
                    }
                       
                    return responseMessage;
                }
                else
                {
                    return base.CreateResponseMessage<ResponseMessageNoResponse>();
                }
            }
            else
                return null;
        }
        #endregion

        #region 获取缓存中的回复
        /// <summary>
        /// 获取缓存中的回复
        /// </summary>
        /// <param name="rstp"></param>
        /// <param name="rs"></param>
        /// <returns></returns>
        private async Task<IResponseMessageBase> GetCacheResponse(string rstp, object rs, RequestMessageText requestMessage = null) {
            if (rstp == ReplyMsgType.kf.ToString())
            {
                #region 春雨医生逻辑处理
                var chunyuHandler = _iocResolver.Resolve<IChunYuMessageHandler>();
                //如果春雨医生未结束，先引导用户主动关闭春雨医生会话
                if (await chunyuHandler.IsAsking(mpId, requestMessage.FromUserName))
                {
                    return await chunyuHandler.CustomerNotice(mpId, requestMessage);
                }
                #endregion

                #region 客服逻辑处理
                var kefuMessageHandler = _iocResolver.Resolve<IKeFuMessageHandler>();
                return await kefuMessageHandler.InCustomer(mpId, requestMessage);
                #endregion
            }
            else if (rstp == ReplyMsgType.doctor.ToString())
            {
                #region 客服逻辑处理
                var kefuMessageHandler = _iocResolver.Resolve<IKeFuMessageHandler>();
                if (await kefuMessageHandler.IsAsking(mpId, requestMessage.FromUserName))
                {
                    return await kefuMessageHandler.ChunYuNotice(mpId, requestMessage);
                }
                #endregion

                #region 春雨医生逻辑处理
                var chunyuHandler = _iocResolver.Resolve<IChunYuMessageHandler>();
                return await chunyuHandler.CreateProblem(mpId, requestMessage);
                #endregion
            }
            else if (rstp == MpMessageType.image.ToString())
            {
                var data = rs as ResponseMessageImage;
                var responseMessage = base.CreateResponseMessage<ResponseMessageImage>();
                responseMessage.Image.MediaId = data.Image.MediaId;
                return responseMessage;
            }
            else if (rstp == MpMessageType.mpmultinews.ToString())
            {
                var data = rs as ResponseMessageNews;
                var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                responseMessage.Articles.AddRange(data.Articles);
                return responseMessage;
            }

            else if (rstp == MpMessageType.mpnews.ToString())
            {
                var result = rs as ResponseMessageNews;
                var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                responseMessage.Articles.AddRange(result.Articles);
                return responseMessage;
            }
            else if (rstp == MpMessageType.text.ToString())
            {
                var data = rs as ResponseMessageText;
                var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
                responseMessage.Content = data.Content;
                return responseMessage;
            }
            else if (rstp == MpMessageType.video.ToString())
            {
                var data = rs as ResponseMessageVideo;
                var responseMessage = base.CreateResponseMessage<ResponseMessageVideo>();
                responseMessage.Video.Description = data.Video.Description;
                responseMessage.Video.MediaId = data.Video.MediaId;
                responseMessage.Video.Title = data.Video.Title;
                return responseMessage;
            }
            else if (rstp == MpMessageType.voice.ToString())
            {
                var data = rs as ResponseMessageVoice;
                var responseMessage = base.CreateResponseMessage<ResponseMessageVoice>();
                responseMessage.Voice.MediaId = data.Voice.MediaId;
                return responseMessage;
            }
            else if (rstp == MpMessageType.none.ToString())
            {
                var responseMessage = base.CreateResponseMessage<ResponseMessageNoResponse>();
                return responseMessage;
            }
            return null;
        }
        #endregion
    }

}
