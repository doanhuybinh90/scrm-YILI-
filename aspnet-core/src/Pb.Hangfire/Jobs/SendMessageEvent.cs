using Abp.Dependency;
using Castle.Core.Logging;
using Pb.Hangfire.Tool;
using Pb.Wechat.MpAccessTokenClib;
using Pb.Wechat.MpAccounts;
using Pb.Wechat.MpEvents.Dto;
using Pb.Wechat.MpFans;
using Pb.Wechat.MpGroups;
using Pb.Wechat.MpMediaVideos;
using Pb.Wechat.MpMessages;
using Pb.Wechat.MpMessages.Dto;
using Senparc.Weixin;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.GroupMessage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pb.Hangfire.Jobs
{
    public class SendMessageEvent : ITransientDependency
    {
        private readonly ILogger LogWriter;
        private readonly IMpMessageAppService _mpMessageAppService;
        private readonly IMpGroupAppService _mpGroupAppService;
        private readonly IMpAccountAppService _mpAccountAppService;
        private readonly IMpFanAppService _mpFanAppService;
        private readonly IMpMediaVideoAppService _mpMediaVideoAppService;
        private readonly IAccessTokenContainer _accessTokenContainer;
        public SendMessageEvent(ILogger logger, IMpMessageAppService mpMessageAppService, IMpGroupAppService mpGroupAppService, IMpAccountAppService mpAccountAppService, IMpFanAppService mpFanAppService, IMpMediaVideoAppService mpMediaVideoAppService, IAccessTokenContainer accessTokenContainer)
        {
            LogWriter = logger;
            _mpMessageAppService = mpMessageAppService;
            _mpGroupAppService = mpGroupAppService;
            _mpAccountAppService = mpAccountAppService;
            _mpFanAppService = mpFanAppService;
            _mpMediaVideoAppService = mpMediaVideoAppService;
            _accessTokenContainer = accessTokenContainer;
        }
        public void SendMessage()
        {
            SendResult result = null;
            var Messages = _mpMessageAppService.GetNotYetSendContent().Result;
            List<MpMessageDto> successList = new List<MpMessageDto>();
            if (Messages != null && Messages.Count > 0)
            {
               
                foreach (var Message in Messages)
                {
                    var account=_mpAccountAppService.GetCache(Message.MpID).Result;

                    List<int> groupIds = new List<int>() { Message.GroupID ?? -1 };
                    var groups = _mpGroupAppService.GetListByIds(Message.MpID, groupIds).Result;

                    #region 修改后代码-按OPenID发送
                    #region 获取粉丝数据
                    var fans = _mpFanAppService.GetAllByMpIdAndGroupId(Message.MpID, groupIds).Result.Where(m => m.IsFans == true);

                    if (!string.IsNullOrEmpty(Message.Sex))
                    {
                        var sexes = Message.Sex.Split(',');
                        fans = fans.Where(c => sexes.Contains(c.Sex)).ToList();
                    }
                    if (!string.IsNullOrEmpty(Message.Country))
                    {
                        fans = fans.Where(c => Message.Country == c.Country).ToList();
                    }
                    if (!string.IsNullOrEmpty(Message.Province))
                    {
                        fans = fans.Where(c => Message.Province == c.Province).ToList();
                    }
                    if (!string.IsNullOrEmpty(Message.City))
                    {
                        var cities = Message.City.Split(',');
                        fans = fans.Where(c => cities.Contains(c.City)).ToList();
                    }
                    #endregion

                    #region 发送消息
                    if (Message.MessageType == MpMessageType.text.ToString())
                    {
                        try
                        {
                            result = GroupMessageApi.SendGroupMessageByOpenId(_accessTokenContainer.TryGetAccessTokenAsync(account.AppId,account.AppSecret).Result, GroupMessageType.text, Message.Content, null, Senparc.Weixin.Config.TIME_OUT, fans.Select(c => c.OpenID).ToArray());
                        }
                        catch
                        {
                            
                            result = GroupMessageApi.SendGroupMessageByOpenId(_accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret,true).Result, GroupMessageType.text, Message.Content, null, Senparc.Weixin.Config.TIME_OUT, fans.Select(c => c.OpenID).ToArray());
                        }
                        if (result.errcode != ReturnCode.请求成功)
                            result = GroupMessageApi.SendGroupMessageByOpenId(_accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret).Result, GroupMessageType.text, Message.Content, null, Senparc.Weixin.Config.TIME_OUT, fans.Select(c => c.OpenID).ToArray());
                    }
                    else if (Message.MessageType == MpMessageType.video.ToString())
                    {
                        var videoId = int.Parse(Message.VideoID.ToString());
                        var video = _mpMediaVideoAppService.Get(new Abp.Application.Services.Dto.EntityDto<int> { Id = videoId }).Result;
                        if (video == null)
                            throw new Exception(string.Format("视频{0}不存在", Message.VideoName));
                        try
                        {
                            result = GroupMessageApi.SendVideoGroupMessageByOpenId(_accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret).Result, video.Title, video.Description, Message.VideoMediaID, null, Senparc.Weixin.Config.TIME_OUT, fans.Select(c => c.OpenID).ToArray());
                        }
                        catch
                        {
                            
                            result = GroupMessageApi.SendVideoGroupMessageByOpenId(_accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret,true).Result, video.Title, video.Description, Message.VideoMediaID, null, Senparc.Weixin.Config.TIME_OUT, fans.Select(c => c.OpenID).ToArray());
                        }
                        if (result.errcode != ReturnCode.请求成功)
                            result = GroupMessageApi.SendVideoGroupMessageByOpenId(_accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret).Result, video.Title, video.Description, Message.VideoMediaID, null, Senparc.Weixin.Config.TIME_OUT, fans.Select(c => c.OpenID).ToArray());
                    }
                    else if (Message.MessageType == MpMessageType.voice.ToString())
                    {
                        try
                        {
                            result = GroupMessageApi.SendGroupMessageByOpenId(_accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret).Result, GroupMessageType.voice, Message.VoiceMediaID, null, Senparc.Weixin.Config.TIME_OUT, fans.Select(c => c.OpenID).ToArray());
                        }
                        catch
                        {
                           
                            result = GroupMessageApi.SendGroupMessageByOpenId(_accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret,true).Result, GroupMessageType.voice, Message.VoiceMediaID, null, Senparc.Weixin.Config.TIME_OUT, fans.Select(c => c.OpenID).ToArray());
                        }
                        if (result.errcode != ReturnCode.请求成功)
                            result = GroupMessageApi.SendGroupMessageByOpenId(_accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret).Result, GroupMessageType.voice, Message.VoiceMediaID, null, Senparc.Weixin.Config.TIME_OUT, fans.Select(c => c.OpenID).ToArray());
                    }
                    else if (Message.MessageType == MpMessageType.image.ToString())
                    {
                        try
                        {
                            result = GroupMessageApi.SendGroupMessageByOpenId(_accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret).Result, GroupMessageType.image, Message.ImageMediaID, null, Senparc.Weixin.Config.TIME_OUT, fans.Select(c => c.OpenID).ToArray());
                        }
                        catch
                        {

                            result = GroupMessageApi.SendGroupMessageByOpenId(_accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret,true).Result, GroupMessageType.image, Message.ImageMediaID, null, Senparc.Weixin.Config.TIME_OUT, fans.Select(c => c.OpenID).ToArray());
                        }
                        if (result.errcode != ReturnCode.请求成功)
                            result = GroupMessageApi.SendGroupMessageByOpenId(_accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret).Result, GroupMessageType.image, Message.ImageMediaID, null, Senparc.Weixin.Config.TIME_OUT, fans.Select(c => c.OpenID).ToArray());
                    }
                    else if (Message.MessageType == MpMessageType.mpnews.ToString())
                    {
                        try
                        {
                            result = GroupMessageApi.SendGroupMessageByOpenId(_accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret).Result, GroupMessageType.mpnews, Message.ArticleMediaID, null, Senparc.Weixin.Config.TIME_OUT, fans.Select(c => c.OpenID).ToArray());
                        }
                        catch
                        {
                       
                            result = GroupMessageApi.SendGroupMessageByOpenId(_accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret,true).Result, GroupMessageType.mpnews, Message.ArticleMediaID, null, Senparc.Weixin.Config.TIME_OUT, fans.Select(c => c.OpenID).ToArray());
                        }
                        if (result.errcode != ReturnCode.请求成功)
                            result = GroupMessageApi.SendGroupMessageByOpenId(_accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret).Result, GroupMessageType.mpnews, Message.ArticleMediaID, null, Senparc.Weixin.Config.TIME_OUT, fans.Select(c => c.OpenID).ToArray());
                    }
                    else if (Message.MessageType == MpMessageType.mpmultinews.ToString())
                    {
                        try
                        {
                            result = GroupMessageApi.SendGroupMessageByOpenId(_accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret).Result, GroupMessageType.mpnews, Message.ArticleGroupMediaID, null, Senparc.Weixin.Config.TIME_OUT, fans.Select(c => c.OpenID).ToArray());
                        }
                        catch
                        {
                           
                            result = GroupMessageApi.SendGroupMessageByOpenId(_accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret,true).Result, GroupMessageType.mpnews, Message.ArticleGroupMediaID, null, Senparc.Weixin.Config.TIME_OUT, fans.Select(c => c.OpenID).ToArray());
                        }
                        if (result.errcode != ReturnCode.请求成功)
                            result = GroupMessageApi.SendGroupMessageByOpenId(_accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret).Result, GroupMessageType.mpnews, Message.ArticleGroupMediaID, null, Senparc.Weixin.Config.TIME_OUT, fans.Select(c => c.OpenID).ToArray());
                    }
                    #endregion
                    #endregion

                    #region 修改前代码
                    ////按分组发送
                    //if (new string[] { Message.Country, Message.Province, Message.City, Message.Sex }.All(c => string.IsNullOrEmpty(c)))
                    //{

                    //    #region 发送全部分组
                    //    if (string.IsNullOrEmpty(Message.GroupIDs) || groups.Any(c => c.Name == "全部"))
                    //    {
                    //        if (Message.MessageType == MpMessageType.text.ToString())
                    //        {
                    //            try
                    //            {
                    //                result = GroupMessageApi.SendGroupMessageByGroupId(accessTokenResult.access_token, "", Message.Content, GroupMessageType.text, true);
                    //            }
                    //            catch (Exception ex)
                    //            {
                    //                accessTokenResult = GetAccessToken();
                    //                result = GroupMessageApi.SendGroupMessageByGroupId(accessTokenResult.access_token, "", Message.Content, GroupMessageType.text, true);
                    //            }
                    //            if (result.errcode != ReturnCode.请求成功)
                    //                result = GroupMessageApi.SendGroupMessageByGroupId(accessTokenResult.access_token, "", Message.Content, GroupMessageType.text, true);
                    //        }
                    //        else if (Message.MessageType == MpMessageType.video.ToString())
                    //        {
                    //            try
                    //            {
                    //                result = GroupMessageApi.SendGroupMessageByGroupId(accessTokenResult.access_token, "", Message.VideoMediaID, GroupMessageType.video, true);
                    //            }
                    //            catch (Exception ex)
                    //            {
                    //                accessTokenResult = GetAccessToken();
                    //                result = GroupMessageApi.SendGroupMessageByGroupId(accessTokenResult.access_token, "", Message.VideoMediaID, GroupMessageType.video, true);
                    //            }
                    //            if (result.errcode != ReturnCode.请求成功)
                    //                result = GroupMessageApi.SendGroupMessageByGroupId(accessTokenResult.access_token, "", Message.VideoMediaID, GroupMessageType.video, true);
                    //        }
                    //        else if (Message.MessageType == MpMessageType.voice.ToString())
                    //        {
                    //            try
                    //            {
                    //                result = GroupMessageApi.SendGroupMessageByGroupId(accessTokenResult.access_token, "", Message.VoiceMediaID, GroupMessageType.voice, true);
                    //            }
                    //            catch (Exception ex)
                    //            {
                    //                accessTokenResult = GetAccessToken();
                    //                result = GroupMessageApi.SendGroupMessageByGroupId(accessTokenResult.access_token, "", Message.VoiceMediaID, GroupMessageType.voice, true);
                    //            }
                    //            if (result.errcode != ReturnCode.请求成功)
                    //                result = GroupMessageApi.SendGroupMessageByGroupId(accessTokenResult.access_token, "", Message.VoiceMediaID, GroupMessageType.voice, true);
                    //        }
                    //        else if (Message.MessageType == MpMessageType.image.ToString())
                    //        {
                    //            try
                    //            {
                    //                result = GroupMessageApi.SendGroupMessageByGroupId(accessTokenResult.access_token, "", Message.ImageMediaID, GroupMessageType.image, true);
                    //            }
                    //            catch (Exception ex)
                    //            {
                    //                accessTokenResult = GetAccessToken();
                    //                result = GroupMessageApi.SendGroupMessageByGroupId(accessTokenResult.access_token, "", Message.ImageMediaID, GroupMessageType.image, true);
                    //            }
                    //            if (result.errcode != ReturnCode.请求成功)
                    //                result = GroupMessageApi.SendGroupMessageByGroupId(accessTokenResult.access_token, "", Message.ImageMediaID, GroupMessageType.image, true);
                    //        }
                    //        else if (Message.MessageType == MpMessageType.mpnews.ToString())
                    //        {
                    //            try
                    //            {
                    //                result = GroupMessageApi.SendGroupMessageByGroupId(accessTokenResult.access_token, "", Message.ArticleMediaID, GroupMessageType.mpnews, true);
                    //            }
                    //            catch (Exception ex)
                    //            {
                    //                accessTokenResult = GetAccessToken();
                    //                result = GroupMessageApi.SendGroupMessageByGroupId(accessTokenResult.access_token, "", Message.ArticleMediaID, GroupMessageType.mpnews, true);
                    //            }
                    //            if (result.errcode != ReturnCode.请求成功)
                    //                result = GroupMessageApi.SendGroupMessageByGroupId(accessTokenResult.access_token, "", Message.ArticleMediaID, GroupMessageType.mpnews, true);
                    //        }
                    //        else if (Message.MessageType == MpMessageType.mpmultinews.ToString())
                    //        {
                    //            try
                    //            {
                    //                result = GroupMessageApi.SendGroupMessageByGroupId(accessTokenResult.access_token, "", Message.ArticleGroupMediaID, GroupMessageType.mpnews, true);
                    //            }
                    //            catch (Exception ex)
                    //            {
                    //                accessTokenResult = GetAccessToken();
                    //                result = GroupMessageApi.SendGroupMessageByGroupId(accessTokenResult.access_token, "", Message.ArticleGroupMediaID, GroupMessageType.mpnews, true);
                    //            }

                    //            if (result.errcode != ReturnCode.请求成功)
                    //                result = GroupMessageApi.SendGroupMessageByGroupId(accessTokenResult.access_token, "", Message.ArticleGroupMediaID, GroupMessageType.mpnews, true);
                    //        }
                    //    }
                    //    #endregion

                    //    #region 发送特定分组
                    //    else
                    //    {
                    //        foreach (var groupid in groups.Where(c => c.WxGroupID != null).Select(c => c.WxGroupID.ToString()))
                    //        {
                    //            if (Message.MessageType == MpMessageType.text.ToString())
                    //            {
                    //                try
                    //                {
                    //                    result = GroupMessageApi.SendGroupMessageByGroupId(accessTokenResult.access_token, groupid, Message.Content, GroupMessageType.text, true);
                    //                }
                    //                catch (Exception ex)
                    //                {
                    //                    accessTokenResult = GetAccessToken();
                    //                    result = GroupMessageApi.SendGroupMessageByGroupId(accessTokenResult.access_token, groupid, Message.Content, GroupMessageType.text, true);
                    //                }
                    //                if (result.errcode != ReturnCode.请求成功)
                    //                    result = GroupMessageApi.SendGroupMessageByGroupId(accessTokenResult.access_token, groupid, Message.Content, GroupMessageType.text, true);
                    //            }
                    //            else if (Message.MessageType == MpMessageType.video.ToString())
                    //            {
                    //                try
                    //                {
                    //                    result = GroupMessageApi.SendGroupMessageByGroupId(accessTokenResult.access_token, groupid, Message.VideoMediaID, GroupMessageType.video, true);
                    //                }
                    //                catch (Exception ex)
                    //                {
                    //                    accessTokenResult = GetAccessToken();
                    //                    result = GroupMessageApi.SendGroupMessageByGroupId(accessTokenResult.access_token, groupid, Message.VideoMediaID, GroupMessageType.video, true);
                    //                }
                    //                if (result.errcode != ReturnCode.请求成功)
                    //                    result = GroupMessageApi.SendGroupMessageByGroupId(accessTokenResult.access_token, groupid, Message.VideoMediaID, GroupMessageType.video, true);
                    //            }
                    //            else if (Message.MessageType == MpMessageType.voice.ToString())
                    //            {
                    //                try
                    //                {
                    //                    result = GroupMessageApi.SendGroupMessageByGroupId(accessTokenResult.access_token, groupid, Message.VoiceMediaID, GroupMessageType.voice, true);
                    //                }
                    //                catch (Exception ex)
                    //                {
                    //                    accessTokenResult = GetAccessToken();
                    //                    result = GroupMessageApi.SendGroupMessageByGroupId(accessTokenResult.access_token, groupid, Message.VoiceMediaID, GroupMessageType.voice, true);
                    //                }
                    //                if (result.errcode != ReturnCode.请求成功)
                    //                    result = GroupMessageApi.SendGroupMessageByGroupId(accessTokenResult.access_token, groupid, Message.VoiceMediaID, GroupMessageType.voice, true);
                    //            }
                    //            else if (Message.MessageType == MpMessageType.image.ToString())
                    //            {
                    //                try
                    //                {
                    //                    result = GroupMessageApi.SendGroupMessageByGroupId(accessTokenResult.access_token, groupid, Message.ImageMediaID, GroupMessageType.image, true);
                    //                }
                    //                catch (Exception ex)
                    //                {
                    //                    accessTokenResult = GetAccessToken();
                    //                    result = GroupMessageApi.SendGroupMessageByGroupId(accessTokenResult.access_token, groupid, Message.ImageMediaID, GroupMessageType.image, true);
                    //                }
                    //                if (result.errcode != ReturnCode.请求成功)
                    //                    result = GroupMessageApi.SendGroupMessageByGroupId(accessTokenResult.access_token, groupid, Message.ImageMediaID, GroupMessageType.image, true);
                    //            }
                    //            else if (Message.MessageType == MpMessageType.mpnews.ToString())
                    //            {
                    //                try
                    //                {
                    //                    result = GroupMessageApi.SendGroupMessageByGroupId(accessTokenResult.access_token, groupid, Message.ArticleMediaID, GroupMessageType.mpnews, true);
                    //                }
                    //                catch (Exception ex)
                    //                {
                    //                    accessTokenResult = GetAccessToken();
                    //                    result = GroupMessageApi.SendGroupMessageByGroupId(accessTokenResult.access_token, groupid, Message.ArticleMediaID, GroupMessageType.mpnews, true);
                    //                }
                    //                if (result.errcode != ReturnCode.请求成功)
                    //                    result = GroupMessageApi.SendGroupMessageByGroupId(accessTokenResult.access_token, groupid, Message.ArticleMediaID, GroupMessageType.mpnews, true);
                    //            }
                    //            else if (Message.MessageType == MpMessageType.mpmultinews.ToString())
                    //            {
                    //                try
                    //                {
                    //                    result = GroupMessageApi.SendGroupMessageByGroupId(accessTokenResult.access_token, groupid, Message.ArticleGroupMediaID, GroupMessageType.mpnews, true);
                    //                }
                    //                catch (Exception ex)
                    //                {
                    //                    accessTokenResult = GetAccessToken();
                    //                    result = GroupMessageApi.SendGroupMessageByGroupId(accessTokenResult.access_token, groupid, Message.ArticleGroupMediaID, GroupMessageType.mpnews, true);
                    //                }

                    //                if (result.errcode != ReturnCode.请求成功)
                    //                    result = GroupMessageApi.SendGroupMessageByGroupId(accessTokenResult.access_token, groupid, Message.ArticleGroupMediaID, GroupMessageType.mpnews, true);
                    //            }
                    //        }
                    //    }
                    //    #endregion
                    //}
                    ////按openid发送
                    //else
                    //{
                    //    #region 过滤粉丝
                    //    //var fans = entities.Set<MpFans>().Where(c => c.MpID == Message.MpID && c.IsFans == "1");
                    //    var fans = _mpFanAppService.GetAllByMpId(Message.MpID).Result.Where(m => m.IsFans == "1").ToList();
                    //    if (!string.IsNullOrEmpty(Message.GroupIDs) && !groups.Any(c => c.Name == "全部"))
                    //    {
                    //        fans = fans.Where(c => groupIds.Contains(c.GroupID)).ToList();
                    //    }
                    //    if (!string.IsNullOrEmpty(Message.Sex))
                    //    {
                    //        var sexes = Message.Sex.Split(',');
                    //        fans = fans.Where(c => sexes.Contains(c.Sex)).ToList();
                    //    }
                    //    if (!string.IsNullOrEmpty(Message.Country))
                    //    {
                    //        fans = fans.Where(c => Message.Country == c.Country).ToList();
                    //    }
                    //    if (!string.IsNullOrEmpty(Message.Province))
                    //    {
                    //        fans = fans.Where(c => Message.Province == c.Province).ToList();
                    //    }
                    //    if (!string.IsNullOrEmpty(Message.City))
                    //    {
                    //        var cities = Message.City.Split(',');
                    //        fans = fans.Where(c => cities.Contains(c.City)).ToList();
                    //    }
                    //    #endregion

                    //    #region 发送消息
                    //    if (Message.MessageType == MpMessageType.text.ToString())
                    //    {
                    //        try
                    //        {
                    //            result = GroupMessageApi.SendGroupMessageByOpenId(accessTokenResult.access_token, GroupMessageType.text, Message.Content, null, Senparc.Weixin.Config.TIME_OUT, fans.Select(c => c.OpenID).ToArray());
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            accessTokenResult = GetAccessToken();
                    //            result = GroupMessageApi.SendGroupMessageByOpenId(accessTokenResult.access_token, GroupMessageType.text, Message.Content, null, Senparc.Weixin.Config.TIME_OUT, fans.Select(c => c.OpenID).ToArray());
                    //        }
                    //        if (result.errcode != ReturnCode.请求成功)
                    //            result = GroupMessageApi.SendGroupMessageByOpenId(accessTokenResult.access_token, GroupMessageType.text, Message.Content, null, Senparc.Weixin.Config.TIME_OUT, fans.Select(c => c.OpenID).ToArray());
                    //    }
                    //    else if (Message.MessageType == MpMessageType.video.ToString())
                    //    {
                    //        //var video = entities.Set<MpMediaVideo>().Where(c => c.MpID == Message.MpID && c.ID == Message.VideoID).FirstOrDefault();
                    //        var videoId = int.Parse(Message.VideoID.ToString());
                    //        var video = _mpMediaVideoAppService.Get(new Abp.Application.Services.Dto.EntityDto<int> { Id = videoId }).Result;
                    //        if (video == null)
                    //            throw new Exception(string.Format("视频{0}不存在", Message.VideoName));
                    //        try
                    //        {
                    //            result = GroupMessageApi.SendVideoGroupMessageByOpenId(accessTokenResult.access_token, video.Title, video.Description, Message.VideoMediaID, null, Senparc.Weixin.Config.TIME_OUT, fans.Select(c => c.OpenID).ToArray());
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            accessTokenResult = GetAccessToken();
                    //            result = GroupMessageApi.SendVideoGroupMessageByOpenId(accessTokenResult.access_token, video.Title, video.Description, Message.VideoMediaID, null, Senparc.Weixin.Config.TIME_OUT, fans.Select(c => c.OpenID).ToArray());
                    //        }
                    //        if (result.errcode != ReturnCode.请求成功)
                    //            result = GroupMessageApi.SendVideoGroupMessageByOpenId(accessTokenResult.access_token, video.Title, video.Description, Message.VideoMediaID, null, Senparc.Weixin.Config.TIME_OUT, fans.Select(c => c.OpenID).ToArray());
                    //    }
                    //    else if (Message.MessageType == MpMessageType.voice.ToString())
                    //    {
                    //        try
                    //        {
                    //            result = GroupMessageApi.SendGroupMessageByOpenId(accessTokenResult.access_token, GroupMessageType.voice, Message.VoiceMediaID, null, Senparc.Weixin.Config.TIME_OUT, fans.Select(c => c.OpenID).ToArray());
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            accessTokenResult = GetAccessToken();
                    //            result = GroupMessageApi.SendGroupMessageByOpenId(accessTokenResult.access_token, GroupMessageType.voice, Message.VoiceMediaID, null, Senparc.Weixin.Config.TIME_OUT, fans.Select(c => c.OpenID).ToArray());
                    //        }
                    //        if (result.errcode != ReturnCode.请求成功)
                    //            result = GroupMessageApi.SendGroupMessageByOpenId(accessTokenResult.access_token, GroupMessageType.voice, Message.VoiceMediaID, null, Senparc.Weixin.Config.TIME_OUT, fans.Select(c => c.OpenID).ToArray());
                    //    }
                    //    else if (Message.MessageType == MpMessageType.image.ToString())
                    //    {
                    //        try
                    //        {
                    //            result = GroupMessageApi.SendGroupMessageByOpenId(accessTokenResult.access_token, GroupMessageType.image, Message.ImageMediaID, null, Senparc.Weixin.Config.TIME_OUT, fans.Select(c => c.OpenID).ToArray());
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            accessTokenResult = GetAccessToken();
                    //            result = GroupMessageApi.SendGroupMessageByOpenId(accessTokenResult.access_token, GroupMessageType.image, Message.ImageMediaID, null, Senparc.Weixin.Config.TIME_OUT, fans.Select(c => c.OpenID).ToArray());
                    //        }
                    //        if (result.errcode != ReturnCode.请求成功)
                    //            result = GroupMessageApi.SendGroupMessageByOpenId(accessTokenResult.access_token, GroupMessageType.image, Message.ImageMediaID, null, Senparc.Weixin.Config.TIME_OUT, fans.Select(c => c.OpenID).ToArray());
                    //    }
                    //    else if (Message.MessageType == MpMessageType.mpnews.ToString())
                    //    {
                    //        try
                    //        {
                    //            result = GroupMessageApi.SendGroupMessageByOpenId(accessTokenResult.access_token, GroupMessageType.mpnews, Message.ArticleMediaID, null, Senparc.Weixin.Config.TIME_OUT, fans.Select(c => c.OpenID).ToArray());
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            accessTokenResult = GetAccessToken();
                    //            result = GroupMessageApi.SendGroupMessageByOpenId(accessTokenResult.access_token, GroupMessageType.mpnews, Message.ArticleMediaID, null, Senparc.Weixin.Config.TIME_OUT, fans.Select(c => c.OpenID).ToArray());
                    //        }
                    //        if (result.errcode != ReturnCode.请求成功)
                    //            result = GroupMessageApi.SendGroupMessageByOpenId(accessTokenResult.access_token, GroupMessageType.mpnews, Message.ArticleMediaID, null, Senparc.Weixin.Config.TIME_OUT, fans.Select(c => c.OpenID).ToArray());
                    //    }
                    //    else if (Message.MessageType == MpMessageType.mpmultinews.ToString())
                    //    {
                    //        try
                    //        {
                    //            result = GroupMessageApi.SendGroupMessageByOpenId(accessTokenResult.access_token, GroupMessageType.mpnews, Message.ArticleGroupMediaID, null, Senparc.Weixin.Config.TIME_OUT, fans.Select(c => c.OpenID).ToArray());
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            accessTokenResult = GetAccessToken();
                    //            result = GroupMessageApi.SendGroupMessageByOpenId(accessTokenResult.access_token, GroupMessageType.mpnews, Message.ArticleGroupMediaID, null, Senparc.Weixin.Config.TIME_OUT, fans.Select(c => c.OpenID).ToArray());
                    //        }
                    //        if (result.errcode != ReturnCode.请求成功)
                    //            result = GroupMessageApi.SendGroupMessageByOpenId(accessTokenResult.access_token, GroupMessageType.mpnews, Message.ArticleGroupMediaID, null, Senparc.Weixin.Config.TIME_OUT, fans.Select(c => c.OpenID).ToArray());
                    //    }
                    //    #endregion
                    //}
                    #endregion

                    if (result != null)
                    {
                        Message.WxMsgID = result.msg_id;
                        GetSendResult sendresult = null;
                        try
                        {
                            sendresult = GroupMessageApi.GetGroupMessageResult(_accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret).Result, result.msg_id);
                        }
                        catch
                        {
                            sendresult = GroupMessageApi.GetGroupMessageResult(_accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret,true).Result, result.msg_id);
                        }
                        if (result.errcode != ReturnCode.请求成功)
                            sendresult = GroupMessageApi.GetGroupMessageResult(_accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret).Result, result.msg_id);
                        Message.State = sendresult.msg_status;
                        Message.FinishDate = DateTime.Now;

                        //if (result.errcode == ReturnCode.请求成功)
                        successList.Add(Message);

                    }

                }

                if (successList.Count > 0)
                {
                    _mpMessageAppService.UpdateSendState(successList);
                }
            }
        }
        
    }
}
