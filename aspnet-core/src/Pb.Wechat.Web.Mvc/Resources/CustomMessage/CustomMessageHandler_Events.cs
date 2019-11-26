using System;
using System.Linq;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.Entities;
using Pb.Wechat.MpEvents.Dto;
using Senparc.Weixin.MP.AdvancedAPIs.User;
using Senparc.Weixin;
using Pb.Wechat.MpFans.Dto;
using Pb.Wechat.Web.Helpers;
using Pb.Wechat.MpEventScanLogs.Dto;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Pb.Wechat.MpEventClickViewLogs.Dto;
using Pb.Wechat.Web.Resources.MCServiceHelper;
using Pb.Wechat.MpChannels.Dto;
using Senparc.Weixin.MP.Helpers;
using Pb.Wechat.Web.Resources;
using Pb.Wechat.TaskGroupMessages;
using Pb.Wechat.MpMessages.Dto;
using Newtonsoft.Json;
using System.Collections.Generic;
using Pb.Wechat.MpSolicitudeSettings;
using Pb.Wechat.MpSolicitudeTexts;
using Pb.Wechat.MpFansTagItems;


#if NET45
using System.Web;
#else
#endif


namespace Pb.Wechat.Web
{
    /// <summary>
    /// 自定义MessageHandler
    /// </summary>
    public partial class CustomMessageHandler
    {


        public override async Task<IResponseMessageBase> OnTextOrEventRequestAsync(RequestMessageText requestMessage)
        {
            return null;//返回null，则继续执行OnTextRequest或OnEventRequest
        }

        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override async Task<IResponseMessageBase> OnEvent_ClickRequestAsync(RequestMessageEvent_Click requestMessage)
        {
            var account = await _mpAccountAppService.GetCache(mpId);

            var rs = await _cacheManager.GetCache(AppConsts.Cache_CallBack).GetOrDefaultAsync(string.Format("EventClickRequest_{0}_{1}", account.AppId, requestMessage.EventKey));

            var rstype = await _cacheManager.GetCache(AppConsts.Cache_CallBack).GetOrDefaultAsync(string.Format("EventClickRequest_{0}_{1}_Type", account.AppId, requestMessage.EventKey));
            #region 记录日志
            try
            {
                var opid = requestMessage.FromUserName;
                var entityclick = new MpEventClickViewLogDto();
                entityclick.MpID = mpId;
                entityclick.OpenID = opid;
                entityclick.EventKey = requestMessage.EventKey;
                entityclick.EventType = "点击";
                entityclick.MsgID = requestMessage.MsgId.ToString();
                await _mpEventClickViewLogAppService.Create(entityclick);

            }
            catch (Exception ex)
            {
                _logger.Info(string.Format("MPID{0}记录click事件出错：原因{1}", mpId, ex.Message));
            }
            #endregion

            #region 关怀回复
            await SendUserSolicitudeMsg(requestMessage);
            #endregion

            #region 推送消息
            if (rs == null || rstype == null)
            {
                if (requestMessage.EventKey == "service")
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
                else if (requestMessage.EventKey == "doctorservice")
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
                else
                {
                    var entity = await _mpMenuAppService.GetEntityByMenuKey(mpId, requestMessage.EventKey);
                    if (entity != null)
                    {
                        await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("EventClickRequest_{0}_{1}_Type", account.AppId, requestMessage.EventKey), entity.MediaType);

                        if (entity.UseSolicitude) {
                            
                        }

                        var res = await GetEntityResponse(entity.MediaType, entity.ImageMediaID, entity.Content, entity.VoiceMediaID, (entity.VideoID ?? -1).ToString(), (entity.ArticleID ?? -1).ToString(), (entity.ArticleGroupID ?? -1).ToString());
                        if (res != null)
                        {
                            await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("EventClickRequest_{0}_{1}", account.AppId, requestMessage.EventKey), res);
                            return res;
                        }
                        else
                        {
                            var none = requestMessage.CreateResponseMessage<ResponseMessageNoResponse>();
                            await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("EventClickRequest_{0}_{1}", account.AppId, requestMessage.EventKey), none);
                            return none;
                        }
                    }
                }
            }
            else
            {
                var res = await GetCacheResponse(rstype.ToString(), rs);
                if (res != null)
                    return res;
            }
            #endregion
            return requestMessage.CreateResponseMessage<ResponseMessageNoResponse>();
        }

        /// <summary>
        /// 进入事件
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override async Task<IResponseMessageBase> OnEvent_EnterRequestAsync(RequestMessageEvent_Enter requestMessage)
        {
            return null;
        }

        /// <summary>
        /// 位置事件
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override async Task<IResponseMessageBase> OnEvent_LocationRequestAsync(RequestMessageEvent_Location requestMessage)
        {
            return null;
        }

        /// <summary>
        /// 通过二维码扫描关注扫描事件
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override async Task<IResponseMessageBase> OnEvent_ScanRequestAsync(RequestMessageEvent_Scan requestMessage)
        {
            _logger.Info("通过二维码扫描关注扫描事件");
            var account = await _mpAccountAppService.GetCache(mpId);
            var rs = await _cacheManager.GetCache(AppConsts.Cache_CallBack).GetOrDefaultAsync(string.Format("ScanRequest_{0}_{1}", account.AppId, requestMessage.EventKey));
            var rstype = await _cacheManager.GetCache(AppConsts.Cache_CallBack).GetOrDefaultAsync(string.Format("ScanRequest_{0}_{1}_Type", account.AppId, requestMessage.EventKey));
            var opid = requestMessage.FromUserName;
            int sourceId = -1;
            _logger.Info("eventkey=" + requestMessage.EventKey);

            if (requestMessage.EventKey != "")
            {
                int.TryParse(requestMessage.EventKey, out sourceId);
            }

            MpChannelDto channel = null;
            //var _eventKey=

            try
            {

                var entityevent = new MpEventScanLogDto();
                entityevent.MpID = mpId;
                entityevent.OpenID = opid;
                entityevent.EventContent = requestMessage.EventKey;
                entityevent.EventType = "已关注";
                entityevent.MsgID = requestMessage.MsgId.ToString();
                await _mpEventScanLogAppService.Create(entityevent);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("MPID{0}记录扫码事件出错：原因{1}", mpId, ex.Message));
            }

            MpFanDto entityfans = null;
            #region 更新粉丝
            try
            {
                _logger.Info("进入订阅关注事件更新粉丝。");
                UserInfoJson wxinfo = null;
                try
                {
                    wxinfo = UserApi.Info(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret), opid);
                }
                catch
                {
                    _logger.Error(string.Format("获取MpID为{0}，openid为{1}的用户信息报错", mpId, opid));
                    wxinfo = UserApi.Info(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true), opid);
                }
                if (wxinfo.errcode != ReturnCode.请求成功)
                    throw new Exception(string.Format("获取MpID为{0}，openid为{1}的用户信息报错，错误编号:{2}，错误消息:{3}", mpId, opid, wxinfo.errcode, wxinfo.errmsg));


                entityfans = await _mpFanAppService.GetFirstOrDefaultByOpenID(opid);


                _logger.Info("进入订阅关注事件保存粉丝。");
                #region 保存粉丝
                if (entityfans == null)
                {
                    entityfans = new MpFanDto();

                    entityfans.City = wxinfo.city;
                    entityfans.Country = wxinfo.country;
                    entityfans.HeadImgUrl = wxinfo.headimgurl;
                    entityfans.IsFans = true;
                    entityfans.Language = wxinfo.language;
                    entityfans.MpID = mpId;
                    entityfans.NickName = wxinfo.nickname;
                    entityfans.OpenID = wxinfo.openid;
                    entityfans.Province = wxinfo.province;
                    entityfans.Remark = wxinfo.remark;
                    entityfans.Sex = wxinfo.sex.ToString();
                    entityfans.SubscribeTime = DateTimeHelper.GetDateTimeFromXml(wxinfo.subscribe_time);
                    entityfans.UnionID = wxinfo.unionid;
                    entityfans.WxGroupID = wxinfo.groupid;
                    entityfans.UpdateTime = DateTime.Now;
                    entityfans.FirstSubscribeTime = DateTime.Now;

                    if (sourceId > 110000)
                    {
                        entityfans.ChannelType = ChannelEnum.wghAc.ToString();
                    }
                    else if (sourceId > 100)
                    {
                        entityfans.ChannelType = ChannelEnum.yygw.ToString();
                    }
                    else if (!string.IsNullOrWhiteSpace(requestMessage.EventKey) && requestMessage.EventKey.IndexOf("Login|") >= 0)
                    {
                        entityfans.ChannelType = ChannelEnum.web.ToString();
                    }
                    else
                    {
                        entityfans.ChannelType = ChannelEnum.wx.ToString();
                    }
                    if (!string.IsNullOrWhiteSpace(requestMessage.EventKey))
                    {
                        _logger.Info("保存粉丝，找eventKey");
                        channel = await _mpChannelAppService.GetFirstOrDefault(new MpChannels.Dto.GetMpChannelsInput { MpID = mpId, EventKey = requestMessage.EventKey });
                        if (channel != null)
                        {
                            entityfans.ChannelID = channel.Id;
                            entityfans.ChannelName = channel.Name;
                        }
                        else
                        {
                            entityfans.ChannelID = 0;
                            entityfans.ChannelName = "公众号";
                        }
                    }
                    else
                    {
                        entityfans.ChannelID = 0;
                        entityfans.ChannelName = "公众号";
                    }
                    await _mpFanAppService.Create(entityfans);

                }
                else
                {
                    entityfans.City = wxinfo.city;
                    entityfans.Country = wxinfo.country;
                    entityfans.HeadImgUrl = wxinfo.headimgurl;
                    entityfans.IsFans = false;
                    entityfans.Language = wxinfo.language;
                    entityfans.MpID = mpId;
                    entityfans.NickName = wxinfo.nickname;
                    entityfans.OpenID = wxinfo.openid;
                    entityfans.Province = wxinfo.province;
                    entityfans.Remark = wxinfo.remark;
                    entityfans.Sex = wxinfo.sex.ToString();
                    entityfans.SubscribeTime = DateTimeHelper.GetDateTimeFromXml(wxinfo.subscribe_time);
                    entityfans.UnionID = wxinfo.unionid;
                    entityfans.WxGroupID = wxinfo.groupid;
                    entityfans.UpdateTime = DateTime.Now;
                    if (sourceId > 110000)
                    {
                        entityfans.ChannelType = ChannelEnum.wghAc.ToString();
                    }
                    else if (sourceId > 100)
                    {
                        entityfans.ChannelType = ChannelEnum.yygw.ToString();
                    }
                    else if (!string.IsNullOrWhiteSpace(requestMessage.EventKey) && requestMessage.EventKey.IndexOf("Login|") >= 0)
                    {
                        entityfans.ChannelType = ChannelEnum.web.ToString();
                    }
                    else
                    {
                        entityfans.ChannelType = ChannelEnum.wx.ToString();
                    }
                    #region 更新粉丝渠道
                    if (!string.IsNullOrWhiteSpace(requestMessage.EventKey))
                    {
                        _logger.Info($"更新粉丝渠道，找eventKey，Eventkey:{requestMessage.EventKey}");
                        channel = await _mpChannelAppService.GetFirstOrDefault(new MpChannels.Dto.GetMpChannelsInput { MpID = mpId, EventKey = requestMessage.EventKey });
                        if (channel != null)
                        {
                            entityfans.ChannelID = channel.Id;
                            entityfans.ChannelName = channel.Name;
                        }
                        else
                        {
                            entityfans.ChannelID = 0;
                            entityfans.ChannelName = "公众号";
                        }
                    }
                    else
                    {
                        entityfans.ChannelID = 0;
                        entityfans.ChannelName = "公众号";
                    }
                    await _mpFanAppService.Update(entityfans);
                }
                #endregion




                #endregion

            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("粉丝订阅更新数据库失败，原因：{0}", ex.Message));
            }
            #endregion

            #region 活动扫码/关注处理

            _logger.Info("推送处-SourceId=" + sourceId);
            if (channel == null)
            {
                _logger.Info("找不到内容");
                if (sourceId > 100)
                {
                    _logger.Info("进入美池推送。");
                    var fan = await _mpFanAppService.GetFirstOrDefaultByOpenID(opid);
                    var userMember = await _mpUserMemberAppService.GetByOpenID(opid);
                    MCServiceHandler handler = new MCServiceHandler(mpId, account, _cacheManager, _logger, _accessTokenContainer, _matialFileService, _webUrlService, _yiliBabyClubInterfaceService);
                    await handler.SanCodePushMessageByActivity(sourceId, fan, userMember, opid);
                    return null;
                }
                else
                    return null;
            }
            
            else
            {

                _logger.Info($"找到内容：{JsonConvert.SerializeObject(channel)}");
                if (!string.IsNullOrEmpty(channel.TagIds))
                {
                    var _mpFansTagItemAppService = _iocResolver.Resolve<IMpFansTagItemAppService>();
                    await _mpFansTagItemAppService.SaveFansTags(mpId, entityfans.Id, channel.TagIds);
                }
                #region 推送消息
                if (rs == null || rstype == null)
                {
                    if (requestMessage.EventKey != null && requestMessage.EventKey.Length > 0)
                    {
                        var _channelModel = await _mpChannelAppService.GetFirstOrDefault(new GetMpChannelsInput { MpID = mpId, EventKey = requestMessage.EventKey });
                        await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("ScanRequest_{0}_{1}_Type", account.AppId, requestMessage.EventKey), _channelModel.ReplyType);
                        return await GetSubscribeResponse("ScanRequest", requestMessage.EventKey, account, _channelModel.ReplyType, _channelModel.ImageMediaID, _channelModel.Content, _channelModel.VoiceMediaID, _channelModel.VideoMediaID, _channelModel.ArticleID.ToString(), _channelModel.ArticleGroupID.ToString());
                    }
                    else
                    {
                        var eventtype = MpEventType.Subscribe.ToString();
                        var entity = await _mpEventAppService.GetModelByEventTypeAsync(eventtype, mpId);
                        if (entity != null)
                        {
                            await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("ScanRequest_{0}_{1}_Type", account.AppId, requestMessage.EventKey), entity.ReplyType);
                            return await GetSubscribeResponse("ScanRequest", requestMessage.EventKey, account, entity.ReplyType, entity.ImageMediaID, entity.Content, entity.VoiceMediaID, entity.VideoMediaID, entity.ArticleID.ToString(), entity.ArticleGroupID.ToString());
                           
                        }
                        //其他回复
                        else
                            return null;
                    }

                  
                }
                else
                {

                    var rstp = rstype.ToString();
                    if (rstp == MpMessageType.image.ToString())
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
                        responseMessage.Video.MediaId = data.Video.MediaId;
                        return responseMessage;
                    }
                    else if (rstp == MpMessageType.voice.ToString())
                    {
                        var data = rs as ResponseMessageVoice;
                        var responseMessage = base.CreateResponseMessage<ResponseMessageVoice>();
                        responseMessage.Voice.MediaId = data.Voice.MediaId;
                        return responseMessage;
                    }

                    return rs as IResponseMessageBase;
                }
                #endregion

            }
            #endregion
        }



        /// <summary>
        /// 打开网页事件
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override async Task<IResponseMessageBase> OnEvent_ViewRequestAsync(RequestMessageEvent_View requestMessage)
        {
            try
            {
                var opid = requestMessage.FromUserName;
                var entityclick = new MpEventClickViewLogDto();

                entityclick.MpID = mpId;
                entityclick.OpenID = opid;
                entityclick.EventKey = requestMessage.EventKey;
                entityclick.EventType = "查看";
                entityclick.MsgID = requestMessage.MsgId.ToString();

                await _mpEventClickViewLogAppService.Create(entityclick);

                await SendUserSolicitudeMsg(requestMessage);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("MPID{0}记录click事件出错：原因{1}", mpId, ex.Message));
            }
            return null;
        }

        /// <summary>
        /// 群发完成事件
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override async Task<IResponseMessageBase> OnEvent_MassSendJobFinishRequestAsync(RequestMessageEvent_MassSendJobFinish requestMessage)
        {
            var msgid = requestMessage.MsgID.ToString();
            _logger.Info($"start masssend {msgid} {JsonConvert.SerializeObject(requestMessage)}");
            #region 更新按openid群发的结果
            var _taskGroupMessageAppService = _iocResolver.Resolve<ITaskGroupMessageAppService>();
            var task = await _taskGroupMessageAppService.GetByWxMsgId(msgid);
            if (task != null)
            {
                _logger.Info($"masssend {msgid} get task {task.Id}");
                if (requestMessage.Status == "send success")
                {
                    task.TaskState = (int)MpMessageTaskState.Success;
                    task.SuccessCount = requestMessage.SentCount;
                    task.SendCount = requestMessage.TotalCount;
                    task.FailCount = requestMessage.ErrorCount;
                    await _taskGroupMessageAppService.Update(task);
                }
                else
                {
                    task.TaskState = (int)MpMessageTaskState.Fail;
                    task.SuccessCount = requestMessage.SentCount;
                    task.SendCount = requestMessage.TotalCount;
                    task.FailCount = requestMessage.ErrorCount;
                    await _taskGroupMessageAppService.Update(task);
                }
                _logger.Info($"masssend {msgid} save task {task.Id}");
                var message = await _mpMessageAppService.Get(new EntityDto<int>() { Id = task.MessageID });
                var alltask = await _taskGroupMessageAppService.GetList(new TaskGroupMessages.Dto.GetTaskGroupMessagesInput() { MessageId = task.MessageID });
                if (alltask.All(c => c.TaskState == (int)MpMessageTaskState.Fail))
                    message.SendState = (int)MpMessageTaskState.Fail;
                else if (alltask.All(c => c.TaskState == (int)MpMessageTaskState.Fail || c.TaskState == (int)MpMessageTaskState.Success))
                    message.SendState = (int)MpMessageTaskState.Success;
                message.SuccessCount = alltask.Sum(c => c.SuccessCount);
                message.FailCount = alltask.Sum(c => c.FailCount);
                message.FinishDate = DateTime.Now;
                await _mpMessageAppService.UpdateSendState(new List<MpMessageDto>() { message });
                _logger.Info($"masssend {msgid} save task {task.Id} save message");
            }
            #endregion

            #region 更新按分组群发的结果
            else
            {
                var message = await _mpMessageAppService.GetFirstOrDefault(new MpMessages.Dto.GetMpMessagesInput { WxMsgID = msgid });
                if (message != null)
                {
                    _logger.Info($"masssend {msgid} get message {message.Id}");
                    message.State = requestMessage.Status;
                    if (requestMessage.Status == "send success")
                        message.SendState = (int)MpMessageTaskState.Success;
                    else
                        message.SendState = (int)MpMessageTaskState.Fail;
                    message.SendCount = requestMessage.TotalCount;
                    message.SuccessCount = requestMessage.SentCount;
                    message.FailCount = requestMessage.ErrorCount;
                    message.FinishDate = DateTime.Now;
                    await _mpMessageAppService.UpdateSendState(new List<MpMessageDto>() { message });
                    _logger.Info($"masssend {msgid} save message {message.Id}");
                }
            }
            #endregion
            _logger.Info($"end masssend {msgid}");
            return null;
        }

        /// <summary>
        /// 订阅（关注）事件
        /// </summary>
        /// <returns></returns>
        public override async Task<IResponseMessageBase> OnEvent_SubscribeRequestAsync(RequestMessageEvent_Subscribe requestMessage)
        {
            _logger.Info("进入订阅关注事件。EventKey=" + requestMessage.EventKey);
            var account = await _mpAccountAppService.GetCache(mpId);
            
            #region 关注Cache
            var rs= await _cacheManager.GetCache(AppConsts.Cache_CallBack).GetOrDefaultAsync(string.Format("SubscribeRequest_{0}", account.AppId));
            var rstype = await _cacheManager.GetCache(AppConsts.Cache_CallBack).GetOrDefaultAsync(string.Format("SubscribeRequest_{0}_Type", account.AppId));
            #endregion

            #region 带参二维码Cache
            var rrs = await _cacheManager.GetCache(AppConsts.Cache_CallBack).GetOrDefaultAsync(string.Format("SubscribeRequest_{0}_{1}", account.AppId, requestMessage.EventKey));
            var rrstype = await _cacheManager.GetCache(AppConsts.Cache_CallBack).GetOrDefaultAsync(string.Format("SubscribeRequest_{0}_{1}_Type", account.AppId, requestMessage.EventKey));
            #endregion

            var opid = requestMessage.FromUserName;
            #region 记录日志
            try
            {
                _logger.Info("进入订阅关注事件日志记录。");
                var entityevent = new MpEventScanLogDto();

                entityevent.MpID = mpId;
                entityevent.OpenID = opid;
                entityevent.EventContent = requestMessage.EventKey.StartsWith("qrscene_") ? requestMessage.EventKey.Substring(8) : requestMessage.EventKey; ;
                entityevent.EventType = "关注";
                entityevent.MsgID = requestMessage.MsgId.ToString();
                await _mpEventScanLogAppService.Create(entityevent);

            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("MPID{0}记录扫码事件出错：原因{1}", mpId, ex.Message));
            }
            #endregion

            int sourceId = -1;

            string[] eventKeyArray = null;
            if (requestMessage.EventKey != "")
            {
                eventKeyArray = requestMessage.EventKey.Split("_");
                if (eventKeyArray.Length == 2)
                    int.TryParse(eventKeyArray[1], out sourceId);
            }
            MpChannelDto channel = null;
            MpFanDto entityfans = null;
            #region 更新粉丝
            try
            {
                _logger.Info("进入订阅关注事件更新粉丝。");
                UserInfoJson wxinfo = null;
                try
                {
                    wxinfo = UserApi.Info(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret), opid);
                }
                catch
                {
                    _logger.Error(string.Format("获取MpID为{0}，openid为{1}的用户信息报错", mpId, opid));
                    wxinfo = UserApi.Info(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true), opid);
                }
                if (wxinfo.errcode != ReturnCode.请求成功)
                    throw new Exception(string.Format("获取MpID为{0}，openid为{1}的用户信息报错，错误编号:{2}，错误消息:{3}", mpId, opid, wxinfo.errcode, wxinfo.errmsg));


                entityfans = await _mpFanAppService.GetFirstOrDefaultByOpenID(opid );

               
                _logger.Info("进入订阅关注事件保存粉丝。");
                #region 保存粉丝
                if (entityfans == null)
                {
                    entityfans = new MpFanDto();

                    entityfans.City = wxinfo.city;
                    entityfans.Country = wxinfo.country;
                    entityfans.HeadImgUrl = wxinfo.headimgurl;
                    entityfans.IsFans = true;
                    entityfans.Language = wxinfo.language;
                    entityfans.MpID = mpId;
                    entityfans.NickName = wxinfo.nickname;
                    entityfans.OpenID = wxinfo.openid;
                    entityfans.Province = wxinfo.province;
                    entityfans.Remark = wxinfo.remark;
                    entityfans.Sex = wxinfo.sex.ToString();
                    entityfans.SubscribeTime = DateTimeHelper.GetDateTimeFromXml(wxinfo.subscribe_time);
                    entityfans.UnionID = wxinfo.unionid;
                    entityfans.WxGroupID = wxinfo.groupid;
                    entityfans.UpdateTime = DateTime.Now;
                    entityfans.FirstSubscribeTime = DateTime.Now;

                    if (sourceId > 110000)
                    {
                        entityfans.ChannelType = ChannelEnum.wghAc.ToString();
                    }
                    else if (sourceId > 100)
                    {
                        entityfans.ChannelType = ChannelEnum.yygw.ToString();
                    }
                    else if (eventKeyArray != null && eventKeyArray.Length == 2 && eventKeyArray[1].IndexOf("Login|") >= 0)
                    {
                        entityfans.ChannelType = ChannelEnum.web.ToString();
                    }
                    else
                    {
                        entityfans.ChannelType = ChannelEnum.wx.ToString();
                    }

                    if (eventKeyArray != null && eventKeyArray.Length == 2)
                    {
                        channel = await _mpChannelAppService.GetFirstOrDefault(new MpChannels.Dto.GetMpChannelsInput { MpID = mpId, EventKey = eventKeyArray[1] });
                        if (channel != null)
                        {
                            entityfans.ChannelID = channel.Id;
                            entityfans.ChannelName = channel.Name;
                        }
                        else
                        {
                            entityfans.ChannelID = 0;
                            entityfans.ChannelName = "公众号";
                        }
                    }
                    else
                    {
                        entityfans.ChannelID = 0;
                        entityfans.ChannelName = "公众号";
                    }
                    await _mpFanAppService.Create(entityfans);
                }
                else
                {
                    entityfans.City = wxinfo.city;
                    entityfans.Country = wxinfo.country;
                    entityfans.HeadImgUrl = wxinfo.headimgurl;
                    entityfans.IsFans = true;
                    entityfans.Language = wxinfo.language;
                    entityfans.MpID = mpId;
                    entityfans.NickName = wxinfo.nickname;
                    entityfans.OpenID = wxinfo.openid;
                    entityfans.Province = wxinfo.province;
                    entityfans.Remark = wxinfo.remark;
                    entityfans.Sex = wxinfo.sex.ToString();
                    entityfans.SubscribeTime = DateTimeHelper.GetDateTimeFromXml(wxinfo.subscribe_time);
                    entityfans.UnionID = wxinfo.unionid;
                    entityfans.WxGroupID = wxinfo.groupid;
                    entityfans.UpdateTime = DateTime.Now;
                    if (sourceId > 110000)
                    {
                        entityfans.ChannelType = ChannelEnum.wghAc.ToString();
                    }
                    else if (sourceId > 100)
                    {
                        entityfans.ChannelType = ChannelEnum.yygw.ToString();
                    }
                    else if (eventKeyArray != null && eventKeyArray.Length == 2 && eventKeyArray[1].IndexOf("Login|") >= 0)
                    {
                        entityfans.ChannelType = ChannelEnum.web.ToString();
                    }
                    else
                    {
                        entityfans.ChannelType = ChannelEnum.wx.ToString();
                    }
                    #region 更新粉丝渠道
                    if (eventKeyArray != null && eventKeyArray.Length == 2)
                    {
                        channel = await _mpChannelAppService.GetFirstOrDefault(new MpChannels.Dto.GetMpChannelsInput { MpID = mpId, EventKey = eventKeyArray[1] });
                        if (channel != null)
                        {
                            entityfans.ChannelID = channel.Id;
                            entityfans.ChannelName = channel.Name;
                        }
                        else
                        {
                            entityfans.ChannelID = 0;
                            entityfans.ChannelName = "公众号";
                        }
                    }
                    else
                    {
                        entityfans.ChannelID = 0;
                        entityfans.ChannelName = "公众号";
                    }
                    await _mpFanAppService.Update(entityfans);
                }

                #endregion




                #endregion

            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("粉丝订阅更新数据库失败，原因：{0}", ex.Message));
            }
            #endregion



            _logger.Info("推送处-SourceId=" + sourceId);

            _logger.Info($"进入订阅推送，此处为二维码推送");
            if (channel != null)//普通关注回复+推送带参二维码回复
            {
                if (!string.IsNullOrEmpty(channel.TagIds))
                {
                    var _mpFansTagItemAppService = _iocResolver.Resolve<IMpFansTagItemAppService>();
                    await _mpFansTagItemAppService.SaveFansTags(mpId, entityfans.Id, channel.TagIds);
                }

                var access_token = await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret);
                try
                {
                    #region 推送消息
                    if (rs == null || rstype == null)
                    {
                        var eventtype = MpEventType.Subscribe.ToString();
                        var entity = await _mpEventAppService.GetModelByEventTypeAsync(eventtype, mpId);
                        if (entity != null)
                        {

                            _logger.Info($"实体不为空");
                            await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("SubscribeRequest_{0}_Type", account.AppId), entity.ReplyType);
                            if (entity.ReplyType == MpMessageType.none.ToString())
                            {
                                var responseMessage = base.CreateResponseMessage<ResponseMessageNoResponse>();
                                await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("SubscribeRequest_{0}", account.AppId), responseMessage);

                            }
                            else if (entity.ReplyType == MpMessageType.image.ToString())
                            {
                                var responseMessage = base.CreateResponseMessage<ResponseMessageImage>();
                                responseMessage.Image.MediaId = entity.ImageMediaID;
                                await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("SubscribeRequest_{0}", account.AppId), responseMessage);
                                await CustomApi.SendImageAsync(access_token, requestMessage.FromUserName, entity.ImageMediaID);
                            }
                            else if (entity.ReplyType == MpMessageType.text.ToString())
                            {
                                var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
                                responseMessage.Content = entity.Content;
                                await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("SubscribeRequest_{0}", account.AppId), responseMessage);
                                await CustomApi.SendTextAsync(access_token, requestMessage.FromUserName, entity.Content);
                            }
                            else if (entity.ReplyType == MpMessageType.voice.ToString())
                            {
                                var responseMessage = base.CreateResponseMessage<ResponseMessageVoice>();
                                responseMessage.Voice.MediaId = entity.VoiceMediaID;
                                await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("SubscribeRequest_{0}", account.AppId), responseMessage);
                                await CustomApi.SendVoiceAsync(access_token, requestMessage.FromUserName, entity.VoiceMediaID);
                            }
                            else if (entity.ReplyType == MpMessageType.video.ToString())
                            {
                                var responseMessage = base.CreateResponseMessage<ResponseMessageVideo>();
                                int videoId = 0;
                                int.TryParse(entity.VideoMediaID, out videoId);
                                var video = await _mpMediaVideoAppService.Get(new EntityDto<int> { Id = videoId });

                                if (video == null)
                                    return null;
                                responseMessage.Video.MediaId = video.MediaID;
                                responseMessage.Video.Title = video.Title;
                                responseMessage.Video.Description = video.Description;
                                await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("SubscribeRequest_{0}", account.AppId), responseMessage);
                                await CustomApi.SendVideoAsync(access_token, requestMessage.FromUserName, video.MediaID, video.Title, video.Description);
                            }
                            else if (entity.ReplyType == MpMessageType.mpnews.ToString())
                            {
                                var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                                int articleId = 0;
                                int.TryParse(entity.ArticleID.ToString(), out articleId);
                                var article = await _mpSelfArticleAppService.Get(new EntityDto<int> { Id = articleId });
                                if (article == null)
                                    return null;
                                responseMessage.Articles.Add(new Article()
                                {
                                    Title = article.Title,
                                    Description = article.Description,
                                    Url = article.AUrl,
                                    PicUrl = article.FilePathOrUrl
                                });
                                await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("SubscribeRequest_{0}", account.AppId), responseMessage);
                                await CustomApi.SendNewsAsync(access_token, requestMessage.FromUserName, responseMessage.Articles);
                            }
                            else if (entity.ReplyType == MpMessageType.mpmultinews.ToString())
                            {
                                _logger.Info($"进入推送多图文");
                                var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                                int articleGroupId = 0;
                                int.TryParse(entity.ArticleGroupID.ToString(), out articleGroupId);

                                var groupItems = await _mpSelfArticleGroupItemAppService.GetGroupItems(articleGroupId);

                                if (groupItems == null || groupItems.Count() < 2)
                                    return null;
                                var articleIds = groupItems.Select(m => m.ArticleID).ToList();
                                _logger.Info($"图文Ids{string.Join(",", articleIds)}");
                                var articles = await _mpSelfArticleAppService.GetListByIds(articleIds);
                                foreach (var id in articleIds)
                                {
                                    var item = articles.Where(m => m.Id == id).FirstOrDefault();
                                    _logger.Info($"图文item：{item.Title},{ item.FilePathOrUrl}");
                                    responseMessage.Articles.Add(new Article()
                                    {
                                        Title = item.Title,
                                        Description = item.Description,
                                        Url = item.AUrl,
                                        PicUrl = item.FilePathOrUrl
                                    });
                                }

                                await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("SubscribeRequest_{0}", account.AppId), responseMessage);
                                await CustomApi.SendNewsAsync(access_token, requestMessage.FromUserName, responseMessage.Articles);
                            }
                            else
                            {
                                return null;
                            }
                        }

                        ////////////////////////////////////////////////////////////

                        if (eventKeyArray != null && eventKeyArray.Length > 0)
                        {

                            var _channelModel = await _mpChannelAppService.GetFirstOrDefault(new GetMpChannelsInput { MpID = mpId, EventKey = eventKeyArray[1] });
                            await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("SubscribeRequest_{0}_{1}_Type", account.AppId, eventKeyArray[1]), _channelModel.ReplyType);
                            return await GetSubscribeResponse("SubscribeRequest", eventKeyArray[1], account, _channelModel.ReplyType, _channelModel.ImageMediaID, _channelModel.Content, _channelModel.VoiceMediaID, _channelModel.VideoMediaID, _channelModel.ArticleID.ToString(), _channelModel.ArticleGroupID.ToString());
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        _logger.Info($"进入订阅推送，进入缓存推送");
                        var rstp = rstype.ToString();
                        if (rstp == MpMessageType.none.ToString())
                        {
                            var data = rs as ResponseMessageNoResponse;
                            var responseMessage = base.CreateResponseMessage<ResponseMessageNoResponse>();

                        }
                        else if (rstp == MpMessageType.image.ToString())
                        {
                            var data = rs as ResponseMessageImage;
                            var responseMessage = base.CreateResponseMessage<ResponseMessageImage>();
                            responseMessage.Image.MediaId = data.Image.MediaId;
                            await CustomApi.SendImageAsync(access_token, requestMessage.FromUserName, data.Image.MediaId);
                        }
                        else if (rstp == MpMessageType.mpmultinews.ToString())
                        {
                            var data = rs as ResponseMessageNews;
                            var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                            responseMessage.Articles.AddRange(data.Articles);
                            await CustomApi.SendNewsAsync(access_token, requestMessage.FromUserName, data.Articles);
                        }
                        else if (rstp == MpMessageType.mpnews.ToString())
                        {

                            var result = rs as ResponseMessageNews;
                            var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                            responseMessage.Articles.AddRange(result.Articles);
                            await CustomApi.SendNewsAsync(access_token, requestMessage.FromUserName, result.Articles);
                        }
                        else if (rstp == MpMessageType.text.ToString())
                        {
                            var data = rs as ResponseMessageText;
                            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
                            responseMessage.Content = data.Content;
                            await CustomApi.SendTextAsync(access_token, requestMessage.FromUserName, data.Content);

                        }
                        else if (rstp == MpMessageType.video.ToString())
                        {
                            var data = rs as ResponseMessageVideo;
                            var responseMessage = base.CreateResponseMessage<ResponseMessageVideo>();
                            responseMessage.Video.MediaId = data.Video.MediaId;
                            responseMessage.Video.Title = data.Video.Title;
                            responseMessage.Video.Description = data.Video.Description;
                            await CustomApi.SendVideoAsync(access_token, requestMessage.FromUserName, data.Video.MediaId, data.Video.Title, data.Video.Description);
                        }
                        else if (rstp == MpMessageType.voice.ToString())
                        {
                            var data = rs as ResponseMessageVoice;
                            var responseMessage = base.CreateResponseMessage<ResponseMessageVoice>();
                            responseMessage.Voice.MediaId = data.Voice.MediaId;
                            await CustomApi.SendVoiceAsync(access_token, requestMessage.FromUserName, data.Voice.MediaId);
                        }

                        if (rrs == null || rrstype == null)
                        {
                            if (eventKeyArray != null && eventKeyArray.Length > 1)
                            {

                                var _channelModel = await _mpChannelAppService.GetFirstOrDefault(new GetMpChannelsInput { MpID = mpId, EventKey = eventKeyArray[1] });
                                await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("SubscribeRequest_{0}_{1}_Type", account.AppId, eventKeyArray[1]), _channelModel.ReplyType);
                                return await GetSubscribeResponse("SubscribeRequest", eventKeyArray[1], account, _channelModel.ReplyType, _channelModel.ImageMediaID, _channelModel.Content, _channelModel.VoiceMediaID, _channelModel.VideoMediaID, _channelModel.ArticleID.ToString(), _channelModel.ArticleGroupID.ToString());
                            }
                            else
                            {
                                return base.CreateResponseMessage<ResponseMessageNoResponse>();
                            }
                        }
                        else
                        {
                            var rrstp = rrstype.ToString();
                            if (rrstp == MpMessageType.none.ToString())
                            {
                                var data = rrs as ResponseMessageNoResponse;
                                var responseMessage = base.CreateResponseMessage<ResponseMessageNoResponse>();

                            }
                            else if (rrstp == MpMessageType.image.ToString())
                            {
                                var data = rrs as ResponseMessageImage;
                                var responseMessage = base.CreateResponseMessage<ResponseMessageImage>();
                                responseMessage.Image.MediaId = data.Image.MediaId;
                                return responseMessage;
                            }
                            else if (rrstp == MpMessageType.mpmultinews.ToString())
                            {
                                var data = rrs as ResponseMessageNews;
                                var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                                responseMessage.Articles.AddRange(data.Articles);
                                return responseMessage;
                            }
                            else if (rrstp == MpMessageType.mpnews.ToString())
                            {

                                var result = rrs as ResponseMessageNews;
                                var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                                responseMessage.Articles.AddRange(result.Articles);
                                return responseMessage;
                            }
                            else if (rrstp == MpMessageType.text.ToString())
                            {
                                var data = rrs as ResponseMessageText;
                                var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
                                responseMessage.Content = data.Content;
                                return responseMessage;

                            }
                            else if (rrstp == MpMessageType.video.ToString())
                            {
                                var data = rrs as ResponseMessageVideo;
                                var responseMessage = base.CreateResponseMessage<ResponseMessageVideo>();
                                responseMessage.Video.MediaId = data.Video.MediaId;
                                responseMessage.Video.Title = data.Video.Title;
                                responseMessage.Video.Description = data.Video.Description;
                                return responseMessage;
                            }
                            else if (rrstp == MpMessageType.voice.ToString())
                            {
                                var data = rrs as ResponseMessageVoice;
                                var responseMessage = base.CreateResponseMessage<ResponseMessageVoice>();
                                responseMessage.Voice.MediaId = data.Voice.MediaId;
                                return responseMessage;
                            }
                            return base.CreateResponseMessage<ResponseMessageNoResponse>();
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                    access_token =await  _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret,true);
                    try
                    {
                       
                        #region 推送消息
                        if (rs == null || rstype == null)
                        {
                            var eventtype = MpEventType.Subscribe.ToString();
                            var entity = await _mpEventAppService.GetModelByEventTypeAsync(eventtype, mpId);
                            if (entity != null)
                            {

                                _logger.Info($"实体不为空");
                                await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("SubscribeRequest_{0}_Type", account.AppId), entity.ReplyType);
                                if (entity.ReplyType == MpMessageType.none.ToString())
                                {
                                    var responseMessage = base.CreateResponseMessage<ResponseMessageNoResponse>();
                                    await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("SubscribeRequest_{0}", account.AppId), responseMessage);

                                }
                                else if (entity.ReplyType == MpMessageType.image.ToString())
                                {
                                    var responseMessage = base.CreateResponseMessage<ResponseMessageImage>();
                                    responseMessage.Image.MediaId = entity.ImageMediaID;
                                    await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("SubscribeRequest_{0}", account.AppId), responseMessage);
                                    await CustomApi.SendImageAsync(access_token, requestMessage.FromUserName, entity.ImageMediaID);
                                }
                                else if (entity.ReplyType == MpMessageType.text.ToString())
                                {
                                    var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
                                    responseMessage.Content = entity.Content;
                                    await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("SubscribeRequest_{0}", account.AppId), responseMessage);
                                    await CustomApi.SendTextAsync(access_token, requestMessage.FromUserName, entity.Content);
                                }
                                else if (entity.ReplyType == MpMessageType.voice.ToString())
                                {
                                    var responseMessage = base.CreateResponseMessage<ResponseMessageVoice>();
                                    responseMessage.Voice.MediaId = entity.VoiceMediaID;
                                    await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("SubscribeRequest_{0}", account.AppId), responseMessage);
                                    await CustomApi.SendVoiceAsync(access_token, requestMessage.FromUserName, entity.VoiceMediaID);
                                }
                                else if (entity.ReplyType == MpMessageType.video.ToString())
                                {
                                    var responseMessage = base.CreateResponseMessage<ResponseMessageVideo>();
                                    int videoId = 0;
                                    int.TryParse(entity.VideoMediaID, out videoId);
                                    var video = await _mpMediaVideoAppService.Get(new EntityDto<int> { Id = videoId });

                                    if (video == null)
                                        return null;
                                    responseMessage.Video.MediaId = video.MediaID;
                                    responseMessage.Video.Title = video.Title;
                                    responseMessage.Video.Description = video.Description;
                                    await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("SubscribeRequest_{0}", account.AppId), responseMessage);
                                    await CustomApi.SendVideoAsync(access_token, requestMessage.FromUserName, video.MediaID, video.Title, video.Description);
                                }
                                else if (entity.ReplyType == MpMessageType.mpnews.ToString())
                                {
                                    var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                                    int articleId = 0;
                                    int.TryParse(entity.ArticleID.ToString(), out articleId);
                                    var article = await _mpSelfArticleAppService.Get(new EntityDto<int> { Id = articleId });
                                    if (article == null)
                                        return null;
                                    responseMessage.Articles.Add(new Article()
                                    {
                                        Title = article.Title,
                                        Description = article.Description,
                                        Url = article.AUrl,
                                        PicUrl = article.FilePathOrUrl
                                    });
                                    await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("SubscribeRequest_{0}", account.AppId), responseMessage);
                                    await CustomApi.SendNewsAsync(access_token, requestMessage.FromUserName, responseMessage.Articles);
                                }
                                else if (entity.ReplyType == MpMessageType.mpmultinews.ToString())
                                {
                                    _logger.Info($"进入推送多图文");
                                    var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                                    int articleGroupId = 0;
                                    int.TryParse(entity.ArticleGroupID.ToString(), out articleGroupId);

                                    var groupItems = await _mpSelfArticleGroupItemAppService.GetGroupItems(articleGroupId);

                                    if (groupItems == null || groupItems.Count() < 2)
                                        return null;
                                    var articleIds = groupItems.Select(m => m.ArticleID).ToList();
                                    _logger.Info($"图文Ids{string.Join(",", articleIds)}");
                                    var articles = await _mpSelfArticleAppService.GetListByIds(articleIds);
                                    foreach (var id in articleIds)
                                    {
                                        var item = articles.Where(m => m.Id == id).FirstOrDefault();
                                        _logger.Info($"图文item：{item.Title},{ item.FilePathOrUrl}");
                                        responseMessage.Articles.Add(new Article()
                                        {
                                            Title = item.Title,
                                            Description = item.Description,
                                            Url = item.AUrl,
                                            PicUrl = item.FilePathOrUrl
                                        });
                                    }

                                    await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("SubscribeRequest_{0}", account.AppId), responseMessage);
                                    await CustomApi.SendNewsAsync(access_token, requestMessage.FromUserName, responseMessage.Articles);
                                }
                                else
                                {
                                    return null;
                                }
                            }

                            ////////////////////////////////////////////////////////////

                            if (eventKeyArray != null && eventKeyArray.Length > 1)
                            {

                                var _channelModel = await _mpChannelAppService.GetFirstOrDefault(new GetMpChannelsInput { MpID = mpId, EventKey = eventKeyArray[1] });
                                await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("SubscribeRequest_{0}_{1}_Type", account.AppId, eventKeyArray[1]), _channelModel.ReplyType);
                                return await GetSubscribeResponse("SubscribeRequest", eventKeyArray[1], account, _channelModel.ReplyType, _channelModel.ImageMediaID, _channelModel.Content, _channelModel.VoiceMediaID, _channelModel.VideoMediaID, _channelModel.ArticleID.ToString(), _channelModel.ArticleGroupID.ToString());
                            }
                            else
                            {
                                return null;
                            }
                        }
                        else
                        {
                            _logger.Info($"进入订阅推送，进入缓存推送");
                            var rstp = rstype.ToString();
                            if (rstp == MpMessageType.none.ToString())
                            {
                                var data = rs as ResponseMessageNoResponse;
                                var responseMessage = base.CreateResponseMessage<ResponseMessageNoResponse>();

                            }
                            else if (rstp == MpMessageType.image.ToString())
                            {
                                var data = rs as ResponseMessageImage;
                                var responseMessage = base.CreateResponseMessage<ResponseMessageImage>();
                                responseMessage.Image.MediaId = data.Image.MediaId;
                                await CustomApi.SendImageAsync(account.AppId, requestMessage.FromUserName, data.Image.MediaId);
                            }
                            else if (rstp == MpMessageType.mpmultinews.ToString())
                            {
                                var data = rs as ResponseMessageNews;
                                var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                                responseMessage.Articles.AddRange(data.Articles);
                                await CustomApi.SendNewsAsync(account.AppId, requestMessage.FromUserName, data.Articles);
                            }
                            else if (rstp == MpMessageType.mpnews.ToString())
                            {

                                var result = rs as ResponseMessageNews;
                                var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                                responseMessage.Articles.AddRange(result.Articles);
                                await CustomApi.SendNewsAsync(account.AppId, requestMessage.FromUserName, result.Articles);
                            }
                            else if (rstp == MpMessageType.text.ToString())
                            {
                                var data = rs as ResponseMessageText;
                                var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
                                responseMessage.Content = data.Content;
                                await CustomApi.SendTextAsync(account.AppId, requestMessage.FromUserName, data.Content);

                            }
                            else if (rstp == MpMessageType.video.ToString())
                            {
                                var data = rs as ResponseMessageVideo;
                                var responseMessage = base.CreateResponseMessage<ResponseMessageVideo>();
                                responseMessage.Video.MediaId = data.Video.MediaId;
                                responseMessage.Video.Title = data.Video.Title;
                                responseMessage.Video.Description = data.Video.Description;
                                await CustomApi.SendVideoAsync(account.AppId, requestMessage.FromUserName, data.Video.MediaId, data.Video.Title, data.Video.Description);
                            }
                            else if (rstp == MpMessageType.voice.ToString())
                            {
                                var data = rs as ResponseMessageVoice;
                                var responseMessage = base.CreateResponseMessage<ResponseMessageVoice>();
                                responseMessage.Voice.MediaId = data.Voice.MediaId;
                                await CustomApi.SendVoiceAsync(account.AppId, requestMessage.FromUserName, data.Voice.MediaId);
                            }

                            if (rrs == null || rrstype == null)
                            {
                                if (eventKeyArray != null && eventKeyArray.Length > 1)
                                {

                                    var _channelModel = await _mpChannelAppService.GetFirstOrDefault(new GetMpChannelsInput { MpID = mpId, EventKey = eventKeyArray[1] });
                                    await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("SubscribeRequest_{0}_{1}_Type", account.AppId, eventKeyArray[1]), _channelModel.ReplyType);
                                    return await GetSubscribeResponse("SubscribeRequest", eventKeyArray[1], account, _channelModel.ReplyType, _channelModel.ImageMediaID, _channelModel.Content, _channelModel.VoiceMediaID, _channelModel.VideoMediaID, _channelModel.ArticleID.ToString(), _channelModel.ArticleGroupID.ToString());
                                }
                                else
                                {
                                    return base.CreateResponseMessage<ResponseMessageNoResponse>();
                                }
                            }
                            else
                            {
                                var rrstp = rrstype.ToString();
                                if (rrstp == MpMessageType.none.ToString())
                                {
                                    var data = rrs as ResponseMessageNoResponse;
                                    var responseMessage = base.CreateResponseMessage<ResponseMessageNoResponse>();

                                }
                                else if (rrstp == MpMessageType.image.ToString())
                                {
                                    var data = rrs as ResponseMessageImage;
                                    var responseMessage = base.CreateResponseMessage<ResponseMessageImage>();
                                    responseMessage.Image.MediaId = data.Image.MediaId;
                                    return responseMessage;
                                }
                                else if (rrstp == MpMessageType.mpmultinews.ToString())
                                {
                                    var data = rrs as ResponseMessageNews;
                                    var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                                    responseMessage.Articles.AddRange(data.Articles);
                                    return responseMessage;
                                }
                                else if (rrstp == MpMessageType.mpnews.ToString())
                                {

                                    var result = rrs as ResponseMessageNews;
                                    var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                                    responseMessage.Articles.AddRange(result.Articles);
                                    return responseMessage;
                                }
                                else if (rrstp == MpMessageType.text.ToString())
                                {
                                    var data = rrs as ResponseMessageText;
                                    var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
                                    responseMessage.Content = data.Content;
                                    return responseMessage;

                                }
                                else if (rrstp == MpMessageType.video.ToString())
                                {
                                    var data = rrs as ResponseMessageVideo;
                                    var responseMessage = base.CreateResponseMessage<ResponseMessageVideo>();
                                    responseMessage.Video.MediaId = data.Video.MediaId;
                                    responseMessage.Video.Title = data.Video.Title;
                                    responseMessage.Video.Description = data.Video.Description;
                                    return responseMessage;
                                }
                                else if (rrstp == MpMessageType.voice.ToString())
                                {
                                    var data = rrs as ResponseMessageVoice;
                                    var responseMessage = base.CreateResponseMessage<ResponseMessageVoice>();
                                    responseMessage.Voice.MediaId = data.Voice.MediaId;
                                    return responseMessage;
                                }
                                return base.CreateResponseMessage<ResponseMessageNoResponse>();
                            }
                        }
                        #endregion
                    }
                    catch (Exception e)
                    {
                        _logger.Error(e.Message);
                        return null;
                    }
                }
               
            }
            else
            {
                if (sourceId > 100)
                {
                    _logger.Info("进入美池推送。");
                    var fan = await _mpFanAppService.GetFirstOrDefaultByOpenID(opid);
                    var userMember = await _mpUserMemberAppService.GetByOpenID(opid);
                    MCServiceHandler handler = new MCServiceHandler(mpId, account, _cacheManager, _logger, _accessTokenContainer, _matialFileService, _webUrlService, _yiliBabyClubInterfaceService);
                    await handler.SanCodePushMessageByActivity(sourceId, fan, userMember, opid);
                    return base.CreateResponseMessage<ResponseMessageNoResponse>();
                }
                else if (sourceId==-1)
                {
                    #region 推送消息
                    if (rs == null || rstype == null)
                    {
                        var eventtype = MpEventType.Subscribe.ToString();
                        var entity = await _mpEventAppService.GetModelByEventTypeAsync(eventtype, mpId);
                        if (entity != null)
                        {

                            _logger.Info($"实体不为空");
                            await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("SubscribeRequest_{0}_Type", account.AppId), entity.ReplyType);
                            if (entity.ReplyType == MpMessageType.none.ToString())
                            {
                                var responseMessage = base.CreateResponseMessage<ResponseMessageNoResponse>();
                                await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("SubscribeRequest_{0}", account.AppId), responseMessage);
                                return responseMessage;
                            }
                            else if (entity.ReplyType == MpMessageType.image.ToString())
                            {
                                var responseMessage = base.CreateResponseMessage<ResponseMessageImage>();
                                responseMessage.Image.MediaId = entity.ImageMediaID;
                                await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("SubscribeRequest_{0}", account.AppId), responseMessage);
                                return responseMessage;
                            }
                            else if (entity.ReplyType == MpMessageType.text.ToString())
                            {
                                var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
                                responseMessage.Content = entity.Content;
                                await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("SubscribeRequest_{0}", account.AppId), responseMessage);
                                return responseMessage;
                            }
                            else if (entity.ReplyType == MpMessageType.voice.ToString())
                            {
                                var responseMessage = base.CreateResponseMessage<ResponseMessageVoice>();
                                responseMessage.Voice.MediaId = entity.VoiceMediaID;
                                await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("SubscribeRequest_{0}", account.AppId), responseMessage);
                                return responseMessage;
                            }
                            else if (entity.ReplyType == MpMessageType.video.ToString())
                            {
                                var responseMessage = base.CreateResponseMessage<ResponseMessageVideo>();
                                int videoId = 0;
                                int.TryParse(entity.VideoMediaID, out videoId);
                                var video = await _mpMediaVideoAppService.Get(new EntityDto<int> { Id = videoId });

                                if (video == null)
                                    return null;
                                responseMessage.Video.MediaId = video.MediaID;
                                responseMessage.Video.Title = video.Title;
                                responseMessage.Video.Description = video.Description;
                                await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("SubscribeRequest_{0}", account.AppId), responseMessage);
                                return responseMessage;
                            }
                            else if (entity.ReplyType == MpMessageType.mpnews.ToString())
                            {
                                var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                                int articleId = 0;
                                int.TryParse(entity.ArticleID.ToString(), out articleId);
                                var article = await _mpSelfArticleAppService.Get(new EntityDto<int> { Id = articleId });
                                if (article == null)
                                    return null;
                                responseMessage.Articles.Add(new Article()
                                {
                                    Title = article.Title,
                                    Description = article.Description,
                                    Url = article.AUrl,
                                    PicUrl = article.FilePathOrUrl
                                });
                                await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("SubscribeRequest_{0}", account.AppId), responseMessage);
                                return responseMessage;
                            }
                            else if (entity.ReplyType == MpMessageType.mpmultinews.ToString())
                            {
                                _logger.Info($"进入推送多图文");
                                var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                                int articleGroupId = 0;
                                int.TryParse(entity.ArticleGroupID.ToString(), out articleGroupId);

                                var groupItems = await _mpSelfArticleGroupItemAppService.GetGroupItems(articleGroupId);

                                if (groupItems == null || groupItems.Count() < 2)
                                    return null;
                                var articleIds = groupItems.Select(m => m.ArticleID).ToList();
                                _logger.Info($"图文Ids{string.Join(",", articleIds)}");
                                var articles = await _mpSelfArticleAppService.GetListByIds(articleIds);
                                foreach (var id in articleIds)
                                {
                                    var item = articles.Where(m => m.Id == id).FirstOrDefault();
                                    _logger.Info($"图文item：{item.Title},{ item.FilePathOrUrl}");
                                    responseMessage.Articles.Add(new Article()
                                    {
                                        Title = item.Title,
                                        Description = item.Description,
                                        Url = item.AUrl,
                                        PicUrl = item.FilePathOrUrl
                                    });
                                }

                                await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("SubscribeRequest_{0}", account.AppId), responseMessage);
                                return responseMessage;
                            }
                            else
                            {
                                return null;
                            }
                        }
                        else
                            return null;
                        
                    }
                    else
                    {
                        _logger.Info($"进入订阅推送，进入缓存推送");
                        var rstp = rstype.ToString();
                        if (rstp == MpMessageType.none.ToString())
                        {
                            var data = rs as ResponseMessageNoResponse;
                            var responseMessage = base.CreateResponseMessage<ResponseMessageNoResponse>();
                            return responseMessage;
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
                            responseMessage.Video.MediaId = data.Video.MediaId;
                            responseMessage.Video.Title = data.Video.Title;
                            responseMessage.Video.Description = data.Video.Description;
                            return responseMessage;
                        }
                        else if (rstp == MpMessageType.voice.ToString())
                        {
                            var data = rs as ResponseMessageVoice;
                            var responseMessage = base.CreateResponseMessage<ResponseMessageVoice>();
                            responseMessage.Voice.MediaId = data.Voice.MediaId;
                            return responseMessage;
                        }
                        else
                            return null;
                    }
                    #endregion
                }
                else
                    return base.CreateResponseMessage<ResponseMessageNoResponse>();
            }
           
            
              
            

        }

        private async Task<IResponseMessageBase> GetSubscribeResponse(string cacheHeader, string eventKey, MpAccounts.Dto.MpAccountDto account, string ReplyType, string ImageMediaID, string Content, string VoiceMediaID, string VideoMediaID, string ArticleID, string ArticleGroupID)
        {
            if (ReplyType == MpMessageType.none.ToString())
            {
                return null;
            }
            else if (ReplyType == MpMessageType.image.ToString())
            {
                var responseMessage = base.CreateResponseMessage<ResponseMessageImage>();
                responseMessage.Image.MediaId = ImageMediaID;
                await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("{2}_{0}_{1}", account.AppId, eventKey, cacheHeader), responseMessage);
                return responseMessage;
            }
            else if (ReplyType == MpMessageType.text.ToString())
            {
                var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
                responseMessage.Content = Content;
                await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("{2}_{0}_{1}", account.AppId, eventKey, cacheHeader), responseMessage);
                return responseMessage;
            }
            else if (ReplyType == MpMessageType.voice.ToString())
            {
                var responseMessage = base.CreateResponseMessage<ResponseMessageVoice>();
                responseMessage.Voice.MediaId = VoiceMediaID;
                await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("{2}_{0}_{1}", account.AppId, eventKey, cacheHeader), responseMessage);
                return responseMessage;
            }
            else if (ReplyType == MpMessageType.video.ToString())
            {
                var responseMessage = base.CreateResponseMessage<ResponseMessageVideo>();
                int videoId = 0;
                int.TryParse(VideoMediaID, out videoId);
                var video = await _mpMediaVideoAppService.Get(new EntityDto<int> { Id = videoId });

                if (video == null)
                    return null;
                responseMessage.Video.MediaId = video.MediaID;
                responseMessage.Video.Title = video.Title;
                responseMessage.Video.Description = video.Description;
                await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("{2}_{0}_{1}", account.AppId, eventKey, cacheHeader), responseMessage);
                return responseMessage;
            }
            else if (ReplyType == MpMessageType.mpnews.ToString())
            {
                var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                int articleId = 0;
                int.TryParse(ArticleID.ToString(), out articleId);
                var article = await _mpSelfArticleAppService.Get(new EntityDto<int> { Id = articleId });
                if (article == null)
                    return null;
                responseMessage.Articles.Add(new Article()
                {
                    Title = article.Title,
                    Description = article.Description,
                    Url = article.AUrl,
                    //PicUrl = string.Format("{0}{1}", domain, article.FilePathOrUrl)
                    PicUrl = article.FilePathOrUrl
                });
                await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("{2}_{0}_{1}", account.AppId, eventKey, cacheHeader), responseMessage);
                return responseMessage;
            }
            else if (ReplyType == MpMessageType.mpmultinews.ToString())
            {
                _logger.Info($"进入推送多图文");
                var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();
                int articleGroupId = 0;
                int.TryParse(ArticleGroupID.ToString(), out articleGroupId);

                var groupItems = await _mpSelfArticleGroupItemAppService.GetGroupItems(articleGroupId);

                if (groupItems == null || groupItems.Count() < 2)
                    return null;
                var articleIds = groupItems.Select(m => m.ArticleID).ToList();
                _logger.Info($"图文Ids{string.Join(",",articleIds)}");
                var articles = await _mpSelfArticleAppService.GetListByIds(articleIds);
                foreach (var id in articleIds)
                {
                    var item = articles.Where(m => m.Id == id).FirstOrDefault();
                    _logger.Info($"图文item：{item.Title},{ item.FilePathOrUrl}");
                    responseMessage.Articles.Add(new Article()
                    {
                        Title = item.Title,
                        Description = item.Description,
                        Url = item.AUrl,
                        PicUrl = item.FilePathOrUrl
                    });
                }
                   
                await _cacheManager.GetCache(AppConsts.Cache_CallBack).SetAsync(string.Format("{2}_{0}_{1}", account.AppId, eventKey, cacheHeader), responseMessage);
                return responseMessage;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 退订
        /// 实际上用户无法收到非订阅账号的消息，所以这里可以随便写。
        /// unsubscribe事件的意义在于及时删除网站应用中已经记录的OpenID绑定，消除冗余数据。并且关注用户流失的情况。
        /// </summary>
        /// <returns></returns>
        public override async Task<IResponseMessageBase> OnEvent_UnsubscribeRequestAsync(RequestMessageEvent_Unsubscribe requestMessage)
        {
            _logger.Info($"{requestMessage.FromUserName}-取消关注");
            var opid = requestMessage.FromUserName;
            var entity = await _mpFanAppService.GetFirstOrDefaultByOpenID(opid );

            var entityevent = new MpEventScanLogDto();
            entityevent.MpID = mpId;
            entityevent.OpenID = opid;


            entityevent.MsgID = requestMessage.MsgId.ToString();

            if (entity != null)//取消关注处理
            {
                entity.IsFans = false;
                entity.UpdateTime = DateTime.Now;
                await _mpFanAppService.Update(entity);
                entityevent.EventType = "取消关注";
            }

            await _mpEventScanLogAppService.Create(entityevent);
            return null;
        }

        /// <summary>
        /// 事件之扫码推事件(scancode_push)
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override async Task<IResponseMessageBase> OnEvent_ScancodePushRequestAsync(RequestMessageEvent_Scancode_Push requestMessage)
        {
            _logger.Info("事件之扫码推事件");
            return null;
        }

        /// <summary>
        /// 事件之扫码推事件且弹出“消息接收中”提示框(scancode_waitmsg)
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override async Task<IResponseMessageBase> OnEvent_ScancodeWaitmsgRequestAsync(RequestMessageEvent_Scancode_Waitmsg requestMessage)
        {
            _logger.Info("事件之扫码推事件且弹出“消息接收中”提示框");
            return null;
        }

        /// <summary>
        /// 事件之弹出拍照或者相册发图（pic_photo_or_album）
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override async Task<IResponseMessageBase> OnEvent_PicPhotoOrAlbumRequestAsync(RequestMessageEvent_Pic_Photo_Or_Album requestMessage)
        {
            return null;
        }

        /// <summary>
        /// 事件之弹出系统拍照发图(pic_sysphoto)
        /// 实际测试时发现微信并没有推送RequestMessageEvent_Pic_Sysphoto消息，只能接收到用户在微信中发送的图片消息。
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override async Task<IResponseMessageBase> OnEvent_PicSysphotoRequestAsync(RequestMessageEvent_Pic_Sysphoto requestMessage)
        {
            return null;
        }

        /// <summary>
        /// 事件之弹出微信相册发图器(pic_weixin)
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override async Task<IResponseMessageBase> OnEvent_PicWeixinRequestAsync(RequestMessageEvent_Pic_Weixin requestMessage)
        {
            return null;
        }

        /// <summary>
        /// 事件之弹出地理位置选择器（location_select）
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override async Task<IResponseMessageBase> OnEvent_LocationSelectRequestAsync(RequestMessageEvent_Location_Select requestMessage)
        {
            return null;
        }

        /// <summary>
        /// 事件之发送模板消息返回结果
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override async Task<IResponseMessageBase> OnEvent_TemplateSendJobFinishRequestAsync(RequestMessageEvent_TemplateSendJobFinish requestMessage)
        {

            return null;
        }

        private async Task SendUserSolicitudeMsg(RequestMessageEventBase requestMessage)
        {
            var usersendcache = _cacheManager.GetCache(AppConsts.Cache_SolicitudeUserSend);
            var issend = await usersendcache.GetOrDefaultAsync(requestMessage.FromUserName);
            if (issend == null || Convert.ToBoolean(issend) != true)
            {
                var member = await _mpUserMemberAppService.GetByOpenID(requestMessage.FromUserName);

                var _mpSolicitudeTextAppService = _iocResolver.Resolve<IMpSolicitudeTextAppService>();
                DateTime Birthday = member != null && member.BabyBirthday != null ? member.BabyBirthday.Value : DateTime.MinValue;
                TimeSpan sp = DateTime.Now.Subtract(Birthday);
                var textmodel =await _mpSolicitudeTextAppService.GetTextByday(sp.Days);

                if (textmodel != null && !string.IsNullOrEmpty(textmodel.SolicitudeText))
                {
                    var account = await _mpAccountAppService.GetCache(mpId);
                    try
                    {
                        await CustomApi.SendTextAsync(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret), requestMessage.FromUserName, textmodel.SolicitudeText);
                    }
                    catch
                    {
                        await CustomApi.SendTextAsync(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true), requestMessage.FromUserName, textmodel.SolicitudeText);
                    }

                    var setting = await _cacheManager.GetCache(AppConsts.Cache_SolicitudeDelayMinutes).GetAsync("Default", async key =>
                    {
                        var _mpSolicitudeSettingAppService = _iocResolver.Resolve<IMpSolicitudeSettingAppService>();
                        var settings = await _mpSolicitudeSettingAppService.GetDefault();
                        return settings != null ? settings.DelayMinutes : 2880;
                    });
                    await usersendcache.SetAsync(requestMessage.FromUserName, true, absoluteExpireTime: new TimeSpan(0, Convert.ToInt32(setting), 0));
                }
            }
        }

    }
}
