using Abp.BackgroundJobs;
using Abp.Dependency;
using Castle.Core.Logging;
using Hangfire;
using Newtonsoft.Json;
using Pb.Hangfire.Tool;
using Pb.Wechat.MpAccessTokenClib;
using Pb.Wechat.MpAccounts;
using Pb.Wechat.MpAccounts.Dto;
using Pb.Wechat.MpEvents.Dto;
using Pb.Wechat.MpFans;
using Pb.Wechat.MpGroups;
using Pb.Wechat.MpGroups.Dto;
using Pb.Wechat.MpMediaVideos;
using Pb.Wechat.MpMessages;
using Pb.Wechat.MpMessages.Dto;
using Pb.Wechat.MpUserMembers;
using Pb.Wechat.TaskGroupMessages;
using Pb.Wechat.TaskGroupMessages.Dto;
using Pb.Wechat.Url;
using Senparc.Weixin;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.GroupMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Hangfire.Jobs
{
    public class SendMessageImmediatelyJob : BackgroundJob<MpMessageDto>, ITransientDependency
    {
        private readonly ILogger LogWriter;
        private readonly IMpMessageAppService _mpMessageAppService;
        private readonly IMpGroupAppService _mpGroupAppService;
        private readonly IMpFanAppService _mpFanAppService;
        private readonly IMpMediaVideoAppService _mpMediaVideoAppService;
        private readonly IMpAccountAppService _mpAccountAppService;
        private readonly IAccessTokenContainer _accessTokenContainer;
        private readonly ITaskGroupMessageAppService _taskGroupMessageAppService;
        private readonly IMpUserMemberAppService _userMemberAppService;
        public SendMessageImmediatelyJob(ILogger logger,
            IMpMessageAppService mpMessageAppService,
            IMpGroupAppService mpGroupAppService,
            IMpFanAppService mpFanAppService,
            IMpMediaVideoAppService mpMediaVideoAppService,
            IMpAccountAppService mpAccountAppService,
            IAccessTokenContainer accessTokenContainer,
            ITaskGroupMessageAppService taskGroupMessageAppService,
            IMpUserMemberAppService userMemberAppService)
        {
            LogWriter = logger;
            _mpMessageAppService = mpMessageAppService;
            _mpGroupAppService = mpGroupAppService;
            _mpFanAppService = mpFanAppService;
            _mpMediaVideoAppService = mpMediaVideoAppService;
            _mpAccountAppService = mpAccountAppService;
            _accessTokenContainer = accessTokenContainer;
            _taskGroupMessageAppService = taskGroupMessageAppService;
            _userMemberAppService = userMemberAppService;
        }

        [AutomaticRetry(Attempts = 1)]
        public override void Execute(MpMessageDto args)
        {
            Task.WaitAll(DoExecute(args));
        }

        private async Task DoExecute(MpMessageDto args)
        {
            LogWriter.Info(string.Format("群发消息开始 {0}", args.Id));
            var account = await _mpAccountAppService.GetCache(args.MpID);
            #region 初始化数据
            args.SendState = (int)MpMessageTaskState.Doing;
           
            await _mpMessageAppService.UpdateSendState(new List<MpMessageDto> { args });
            #endregion

            #region 获取美池OpenID
            List<string> OpenIDs = new List<string>();
            if (!string.IsNullOrEmpty(args.GroupIds))
            {
                if (args.IsMember == IsMemberEnum.ALL.ToString())
                {
                    await SendAllAsync(account, args, await _mpFanAppService.FilterCountAsync(args.MpID));
                }
                else if (args.IsMember == IsMemberEnum.Tag.ToString())
                {
                    OpenIDs = _mpFanAppService.FilterTagDatas(args.MpID, args.TargetID);
                    await SaveIntoGroupMessage(args, OpenIDs);
                    await SendMessageAsync(account, args, OpenIDs.LongCount());
                }
                else if (args.IsMember == IsMemberEnum.NotMember.ToString())
                {
                    OpenIDs =_mpFanAppService.FilterNotMemberDatas(args.MpID);
                    await SaveIntoGroupMessage(args, OpenIDs);
                    await SendMessageAsync(account, args, OpenIDs.LongCount());
                }
                else
                {
                    var messageResult = await StaticObjects.GetMessageResponse(account.Token, account.Id, args.Id);
                    await SaveIntoGroupMessage(args, messageResult.OpenIDs);
                    await SendMessageAsync(account, args, messageResult.OpenIDs.LongCount());
              
                }
            }
            else
            {
                await SendAllAsync(account, args, await _mpFanAppService.FilterCountAsync(args.MpID));
            }
            #endregion
        }

        #region 将openid按10000条一组进行拆分
        /// <summary>
        /// 将openid按10000条一组进行拆分
        /// </summary>
        /// <param name="args"></param>
        /// <param name="openIds"></param>
        /// <returns></returns>
        private async Task SaveIntoGroupMessage(MpMessageDto args, IEnumerable<string> openIds)
        {
            var allCount = openIds.LongCount();
            var _count = Math.Ceiling(Convert.ToDecimal(allCount) / Convert.ToDecimal(10000));
            #region 每行10000条写入数据库,筛选会员阶段
            for (int i = 0; i < _count; i++)
            {
                List<string> _openIds = openIds.Select(m => m).Skip(i * 10000).Take(10000).ToList();
                string opids = string.Join(",", _openIds);
                await _taskGroupMessageAppService.Create(new TaskGroupMessageDto()
                {
                    GroupID = args.GroupID ?? -1,
                    MessageID = args.Id,
                    MpID = args.MpID,
                    TaskState = (int)MpMessageTaskState.Wait,
                    OpenIDs = opids,
                    SendCount = _openIds.Count,
                });
            }
            #endregion
        }
        #endregion

        #region 按openid逐组群发
        /// <summary>
        /// 按openid逐组群发
        /// </summary>
        /// <param name="account"></param>
        /// <param name="args"></param>
        /// <param name="taskId"></param>
        /// <param name="totalCount"></param>
        private async Task SendMessageAsync(MpAccountDto account, MpMessageDto args, long totalCount)
        {
            var msgResult = await _taskGroupMessageAppService.GetList(new Wechat.TaskGroupMessages.Dto.GetTaskGroupMessagesInput { MpID = args.MpID, TaskState = (int)MpMessageTaskState.Wait, MessageId = args.Id });

            if (JobConfig.AppSettings["Debugger"] != "true")//正式发送
            {
                LogWriter.Info("群发消息开始 准备开始");
                foreach (var drItem in msgResult)
                {
                    var result =await SendByOpenIds(account, args, drItem.OpenIDs.Split(','));
                    LogWriter.Info($"群发消息开始 开始反写状态 {JsonConvert.SerializeObject(args)} {JsonConvert.SerializeObject(drItem)}");
                    if (result != null && result.errcode == ReturnCode.请求成功)
                    {
                        LogWriter.Info($"群发消息 请求返回成功 {JsonConvert.SerializeObject(result)}");
                        drItem.TaskState = (int)MpMessageTaskState.Doing;
                        drItem.WxMsgID = result.msg_id;
                        await _taskGroupMessageAppService.Update(drItem);
                    }
                    else
                    {
                        LogWriter.Info($"群发消息 请求返回失败 {JsonConvert.SerializeObject(result)}");
                        drItem.TaskState = (int)MpMessageTaskState.Fail;
                        await _taskGroupMessageAppService.Update(drItem);
                    }
                }
            }
            else
            {
                foreach (var drItem in msgResult)
                {
                    drItem.TaskState = (int)MpMessageTaskState.Success;
                    drItem.SuccessCount = drItem.SendCount;
                    drItem.FailCount = 0;
                    await _taskGroupMessageAppService.Update(drItem);
                }
                args.SuccessCount = totalCount;
                args.FailCount = 0;
                args.FinishDate = DateTime.Now;
                args.SendCount = totalCount;
                args.SendState = (int)MpMessageTaskState.Success;
                await _mpMessageAppService.UpdateSendState(new List<MpMessageDto>() { args });
            }
        }

        /// <summary>
        /// 按openid群发
        /// </summary>
        /// <param name="account"></param>
        /// <param name="args"></param>
        /// <param name="openIds"></param>
        /// <returns></returns>
        private async Task<SendResult> SendByOpenIds(MpAccountDto account, MpMessageDto args,string[] openIds)
        {
            SendResult result = null;


            if (args.MessageType == MpMessageType.text.ToString())
            {
                try
                {
                    LogWriter.Info("群发消息开始 openid开始");
                    result = GroupMessageApi.SendGroupMessageByOpenId(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret), GroupMessageType.text, args.Content, $"MpMessage{args.Id}", Senparc.Weixin.Config.TIME_OUT, openIds);
       
                    LogWriter.Info("群发消息开始 openid结束");
                }
                catch (Exception ex)
                {
                    LogWriter.Error("群发消息开始 第一次报错", ex);
                    try
                    {
                        LogWriter.Info("群发消息开始 openid开始2");
                        result = GroupMessageApi.SendGroupMessageByOpenId(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true), GroupMessageType.text, args.Content, $"MpMessage{args.Id}", Senparc.Weixin.Config.TIME_OUT, openIds);
          
                        LogWriter.Info("群发消息开始 openid结束2");
                    }
                    catch (Exception ex2)
                    {
                        LogWriter.Error("群发消息开始 第二次报错", ex2);
                    }
                }
            }
            else if (args.MessageType == MpMessageType.video.ToString())
            {
                var videoId = int.Parse(args.VideoID.ToString());
                var video = await _mpMediaVideoAppService.Get(new Abp.Application.Services.Dto.EntityDto<int> { Id = videoId });
                if (video == null)
                    throw new Exception(string.Format("视频{0}不存在", args.VideoName));
                try
                {
                    result = GroupMessageApi.SendVideoGroupMessageByOpenId(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret), video.Title, video.Description, args.VideoMediaID, $"MpMessage{args.Id}", Senparc.Weixin.Config.TIME_OUT, openIds);
       
                }
                catch
                {
                    try
                    {
                        result = GroupMessageApi.SendVideoGroupMessageByOpenId(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true), video.Title, video.Description, args.VideoMediaID, $"MpMessage{args.Id}", Senparc.Weixin.Config.TIME_OUT, openIds);
          
                    }
                    catch
                    {
                    }
                }
            }
            else if (args.MessageType == MpMessageType.voice.ToString())
            {
                try
                {
                    result = GroupMessageApi.SendGroupMessageByOpenId(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret), GroupMessageType.voice, args.VoiceMediaID, $"MpMessage{args.Id}", Senparc.Weixin.Config.TIME_OUT, openIds);
           
                }
                catch
                {
                    try
                    {
                        result = GroupMessageApi.SendGroupMessageByOpenId(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true), GroupMessageType.voice, args.VoiceMediaID, $"MpMessage{args.Id}", Senparc.Weixin.Config.TIME_OUT, openIds);
                      
                    }
                    catch
                    {
                    }
                }
            }
            else if (args.MessageType == MpMessageType.image.ToString())
            {
                try
                {
                    result = GroupMessageApi.SendGroupMessageByOpenId(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret), GroupMessageType.image, args.ImageMediaID, $"MpMessage{args.Id}", Senparc.Weixin.Config.TIME_OUT, openIds);
               
                }
                catch
                {
                    try
                    {
                        result = GroupMessageApi.SendGroupMessageByOpenId(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true), GroupMessageType.image, args.ImageMediaID, $"MpMessage{args.Id}", Senparc.Weixin.Config.TIME_OUT, openIds);
             
                    }
                    catch
                    {
                    }
                }
            }
            else if (args.MessageType == MpMessageType.mpnews.ToString())
            {
                try
                {
                    result = GroupMessageApi.SendGroupMessageByOpenId(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret), GroupMessageType.mpnews, args.ArticleMediaID, $"MpMessage{args.Id}", Senparc.Weixin.Config.TIME_OUT, openIds);
                
                }
                catch
                {
                    try
                    {
                        result = GroupMessageApi.SendGroupMessageByOpenId(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true), GroupMessageType.mpnews, args.ArticleMediaID, $"MpMessage{args.Id}", Senparc.Weixin.Config.TIME_OUT, openIds);
            
                    }
                    catch
                    {
                    }
                }
            }
            else if (args.MessageType == MpMessageType.mpmultinews.ToString())
            {
                try
                {
                    result = GroupMessageApi.SendGroupMessageByOpenId(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret), GroupMessageType.mpnews, args.ArticleGroupMediaID, $"MpMessage{args.Id}", Senparc.Weixin.Config.TIME_OUT, openIds);
                
                }
                catch
                {
                    try
                    {
                        result = GroupMessageApi.SendGroupMessageByOpenId(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true), GroupMessageType.mpnews, args.ArticleGroupMediaID, $"MpMessage{args.Id}", Senparc.Weixin.Config.TIME_OUT, openIds);

                    }
                    catch
                    {
                    }
                }
            }
            await _mpMessageAppService.UpdateSendState(new List<MpMessageDto> { args });
            return result;
        }
        #endregion

        #region 发送全部用户
        /// <summary>
        /// 发送全部用户
        /// </summary>
        /// <param name="account"></param>
        /// <param name="args"></param>
        /// <param name="totalCount"></param>
        private async Task SendAllAsync(MpAccountDto account, MpMessageDto args, long totalCount)
        {
            Logger.Info($"hangfileDebug {System.Configuration.ConfigurationManager.AppSettings["hangfileDebug"]}");
            if (JobConfig.AppSettings["Debugger"] != "true")//正式发送
            {
                #region 发送消息
                LogWriter.Info("群发消息开始 准备开始");
                var result=await SendAllByGroup(account, args);
                LogWriter.Info("群发消息开始 开始反写状态");
                if (result != null && result.errcode == ReturnCode.请求成功)
                {
                    LogWriter.Info($"群发消息 请求返回成功 {JsonConvert.SerializeObject(result)}");
                    args.SendState = (int)MpMessageTaskState.Doing;
                    args.WxMsgID = result.msg_id;
                    await _mpMessageAppService.UpdateSendState(new List<MpMessageDto>() { args });
                }
                else
                {
                    LogWriter.Info($"群发消息 请求返回失败 {JsonConvert.SerializeObject(result)} {JsonConvert.SerializeObject(args)}");
                    args.SendState = (int)MpMessageTaskState.Fail;
                    await _mpMessageAppService.UpdateSendState(new List<MpMessageDto>() { args });
                    LogWriter.Info($"群发消息 请求返回失败 更新状态结束 {JsonConvert.SerializeObject(args)}");
                }
                #endregion
            }
            else//测试，不发送
            {
                args.SuccessCount = totalCount;
                args.FailCount = 0;
                args.FinishDate = DateTime.Now;
                args.SendCount = totalCount;
                args.SendState = (int)MpMessageTaskState.Success;//发送成功
                await _mpMessageAppService.TaskUpdate(args);
            }
        }

        /// <summary>
        /// 发送全部用户
        /// </summary>
        /// <param name="account"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private async Task<SendResult> SendAllByGroup(MpAccountDto account, MpMessageDto args)
        {
            SendResult result = null;

            if (args.MessageType == MpMessageType.text.ToString())
            {
                try
                {
                    LogWriter.Info("群发消息开始 全部开始");
                    result = GroupMessageApi.SendGroupMessageByGroupId(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret), "", args.Content, GroupMessageType.text, true, clientmsgid: $"MpMessage{args.Id}");
                    LogWriter.Info("群发消息开始 全部结束");
                }
                catch(Exception ex)
                {
                    LogWriter.Error("群发消息开始 第一次报错", ex);
                    try
                    {
                        LogWriter.Info("群发消息开始 全部开始2");
                        result = GroupMessageApi.SendGroupMessageByGroupId(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true), "", args.Content, GroupMessageType.text, true, clientmsgid: $"MpMessage{args.Id}");
                        LogWriter.Info("群发消息开始 全部结束2");
                    }
                    catch (Exception ex2)
                    {
                        LogWriter.Error("群发消息开始 第二次报错", ex2);
                    }
                }
            }
            else if (args.MessageType == MpMessageType.video.ToString())
            {
                var videoId = int.Parse(args.VideoID.ToString());
                var video = await _mpMediaVideoAppService.Get(new Abp.Application.Services.Dto.EntityDto<int> { Id = videoId });
                if (video == null)
                    throw new Exception(string.Format("视频{0}不存在", args.VideoName));
                try
                {
                    result = GroupMessageApi.SendGroupMessageByGroupId(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret), "", args.VideoMediaID, GroupMessageType.video, true, clientmsgid: $"MpMessage{args.Id}");
                }
                catch
                {
                    try
                    {
                        result = GroupMessageApi.SendGroupMessageByGroupId(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true), "", args.VideoMediaID, GroupMessageType.video, true, clientmsgid: $"MpMessage{args.Id}");
                    }
                    catch
                    {
                    }
                }
            }
            else if (args.MessageType == MpMessageType.voice.ToString())
            {
                try
                {
                    result = GroupMessageApi.SendGroupMessageByGroupId(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret), "", args.VoiceMediaID, GroupMessageType.voice, true, clientmsgid: $"MpMessage{args.Id}");
                }
                catch
                {
                    try
                    {
                        result = GroupMessageApi.SendGroupMessageByGroupId(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true), "", args.VoiceMediaID, GroupMessageType.voice, true, clientmsgid: $"MpMessage{args.Id}");
                    }
                    catch
                    {
                    }
                }
            }
            else if (args.MessageType == MpMessageType.image.ToString())
            {
                try
                {
                    result = GroupMessageApi.SendGroupMessageByGroupId(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret), "", args.ImageMediaID, GroupMessageType.image, true, clientmsgid: $"MpMessage{args.Id}");
                }
                catch
                {
                    try
                    {
                        result = GroupMessageApi.SendGroupMessageByGroupId(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true), "", args.ImageMediaID, GroupMessageType.image, true, clientmsgid: $"MpMessage{args.Id}");
                    }
                    catch
                    {
                    }
                }
            }
            else if (args.MessageType == MpMessageType.mpnews.ToString())
            {
                try
                {
                    result = GroupMessageApi.SendGroupMessageByGroupId(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret), "", args.ArticleMediaID, GroupMessageType.mpnews, true, clientmsgid: $"MpMessage{args.Id}");
                }
                catch
                {
                    try
                    {
                        result = GroupMessageApi.SendGroupMessageByGroupId(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true), "", args.ArticleMediaID, GroupMessageType.mpnews, true, clientmsgid: $"MpMessage{args.Id}");
                    }
                    catch
                    {
                    }
                }
            }
            else if (args.MessageType == MpMessageType.mpmultinews.ToString())
            {
                try
                {
                    result = GroupMessageApi.SendGroupMessageByGroupId(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret), "", args.ArticleGroupMediaID, GroupMessageType.mpnews, true, clientmsgid: $"MpMessage{args.Id}");
                }
                catch
                {
                    try
                    {
                        result = GroupMessageApi.SendGroupMessageByGroupId(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true), "", args.ArticleGroupMediaID, GroupMessageType.mpnews, true, clientmsgid: $"MpMessage{args.Id}");
                    }
                    catch
                    {
                    }
                }
            }
           
            return result;
        }
        #endregion
    }
}
