using Abp.Application.Services.Dto;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Runtime.Caching;
using Castle.Core.Logging;
using Newtonsoft.Json;
using Pb.Hangfire;
using Pb.Hangfire.Jobs;
using Pb.Wechat.CustomerServiceConversationMsgs;
using Pb.Wechat.CustomerServiceConversationMsgs.Dto;
using Pb.Wechat.CustomerServiceConversations;
using Pb.Wechat.CustomerServiceConversations.Dto;
using Pb.Wechat.CustomerServiceOnlines;
using Pb.Wechat.CustomerServiceOnlines.Dto;
using Pb.Wechat.CustomerServiceResponseTexts;
using Pb.Wechat.CYConfigs;
using Pb.Wechat.CYConfigs.Dto;
using Pb.Wechat.MpAccessTokenClib;
using Pb.Wechat.MpAccounts;
using Pb.Wechat.MpEvents.Dto;
using Pb.Wechat.MpFans;
using Pb.Wechat.MpFans.Dto;
using Pb.Wechat.Url;
using Pb.Wechat.Web.Helpers;
using Pb.Wechat.Web.Resources.FileServers;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.CustomService;
using Senparc.Weixin.MP.AdvancedAPIs.Media;
using Senparc.Weixin.MP.AdvancedAPIs.User;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Resources.KeFuMessage
{
    public class SelfKeFuMessageHandler : IKeFuMessageHandler, ITransientDependency
    {
        private readonly IMpAccountAppService _mpAccountAppService;
        private readonly IMpFanAppService _mpFanAppService;
        private readonly IAccessTokenContainer _accessTokenContainer;
        private readonly ICYConfigAppService _cYConfigAppService;
        private readonly ICacheManager _cacheManager;
        private readonly IFileServer _fileServer;
        private readonly IWebUrlService _webUrlService;
        private readonly ISignalrCustomerService _signalrCustomerService;
        private readonly ICustomerServiceOnlineAppService _customerServiceOnlineAppService;
        private readonly ICustomerServiceResponseTextAppService _customerServiceResponseTextAppService;
        private readonly ICustomerServiceConversationAppService _customerServiceConversationAppService;
        private readonly ICustomerServiceConversationMsgAppService _customerServiceConversationMsgAppService;
        private readonly IBackgroundJobManager _backgroundJobManager;
        public ILogger Logger { get; set; }

        public SelfKeFuMessageHandler(ICacheManager cacheManager, ICYConfigAppService cYConfigAppService,
            IMpAccountAppService mpAccountAppService, IAccessTokenContainer accessTokenContainer,
            ICustomerServiceResponseTextAppService customerServiceResponseTextAppService,
            ICustomerServiceConversationAppService customerServiceConversationAppService,
            ICustomerServiceConversationMsgAppService customerServiceConversationMsgAppService,
            IMpFanAppService mpFanAppService, ISignalrCustomerService signalrCustomerService,
            ICustomerServiceOnlineAppService customerServiceOnlineAppService,
            IFileServer fileServer, IWebUrlService webUrlService,
            IBackgroundJobManager backgroundJobManager)
        {
            _accessTokenContainer = accessTokenContainer;
            _mpAccountAppService = mpAccountAppService;
            _mpFanAppService = mpFanAppService;
            _cacheManager = cacheManager;
            _signalrCustomerService = signalrCustomerService;
            _cYConfigAppService = cYConfigAppService;
            _customerServiceResponseTextAppService = customerServiceResponseTextAppService;
            _customerServiceConversationAppService = customerServiceConversationAppService;
            _customerServiceConversationMsgAppService = customerServiceConversationMsgAppService;
            _fileServer = fileServer;
            _webUrlService = webUrlService;
            _customerServiceOnlineAppService = customerServiceOnlineAppService;
            Logger = NullLogger.Instance;
            _backgroundJobManager = backgroundJobManager;
        }

        public async Task<bool> IsAsking(int mpid, string openid)
        {
            var conversation = await GetUserLastConversation(openid);

            return conversation != null && conversation.State != (int)CustomerServiceConversationState.Closed;
        }
        [UnitOfWork(IsDisabled = true)]
        public async Task<IResponseMessageBase> InCustomer(int mpid, IRequestMessageBase request)
        {
            Logger.Info($"开始接入客服 {request.FromUserName}");
            
            if (await IsAsking(mpid, request.FromUserName))
            {
                Logger.Info($"接入客服 正在会话中无需接入 {request.FromUserName}");
                return request.CreateResponseMessage<ResponseMessageNoResponse>();
            }
            var text = await _customerServiceResponseTextAppService.GetCustomerDefaultResponseString(mpid);
            await SendCustomerText(mpid, request.FromUserName, text);
            //推送接入问候语
            if (await _customerServiceResponseTextAppService.CheckIsWorkingTime(mpid))
            {
                Logger.Info($"开始接入客服 工作时间 {request.FromUserName}");
                FansMessageDto msgDto = new FansMessageDto { MpID = mpid, OpenID = request.FromUserName };

                _backgroundJobManager.Enqueue<CustomerQuestionJoin, FansMessageDto>(msgDto);//接入机制

                #region 原代码
                //                var faninfo = await GetFan(mpid, request.FromUserName); 
                //                if (faninfo != null)
                //                {
                //                    Logger.Info($"开始调用客服接入机制");
                //#warning 此处需要增加自动接入客服的机制，目前全部丢人待接入列表

                //                    var customer = await _customerServiceOnlineAppService.GetAutoJoinCustomer(mpid);
                //                    if (customer == null)//微客服坐席已满，进入等待队列
                //                    {
                //                        Logger.Info($"进入等待队列,开始创建等待会话");
                //                        var conversation = await _customerServiceConversationAppService.Create(new CustomerServiceConversationDto()
                //                        {
                //                            FanOpenId = request.FromUserName,
                //                            FanId = faninfo.Id,
                //                            MpID = mpid,
                //                            State = (int)CustomerServiceConversationState.Wait,
                //                            HeadImgUrl = faninfo.HeadImgUrl,
                //                            NickName = faninfo.NickName
                //                        });
                //                        Logger.Info($"进入等待队列,创建等待会话成功，开始记录缓存");
                //                        await _cacheManager.GetCache(AppConsts.Cache_FanOpenId2Conversation).SetAsync(request.FromUserName, conversation);
                //                        await _cacheManager.GetCache(AppConsts.Cache_Kf_FanOpenId2Conversation).RemoveAsync(request.FromUserName);

                //                        #region 推送消息给客服???
                //                        Logger.Info($"进入等待队列,创建等待会话成功，记录缓存成功，推送等待人数");
                //                        WebClient wc = new WebClient();
                //                        await wc.UploadValuesTaskAsync(_signalrCustomerService.SetUnConnectNoticeUrl, null);
                //                        #endregion
                //                    }
                //                    else
                //                    {
                //                        Logger.Info($"分配到自动接入客服：{JsonConvert.SerializeObject(customer)}");
                //                        #region 创建客服会话
                //                        var conversation = await _customerServiceConversationAppService.Create(new CustomerServiceConversationDto()
                //                        {
                //                            FanOpenId = request.FromUserName,
                //                            FanId = faninfo.Id,
                //                            MpID = mpid,
                //                            State = (int)CustomerServiceConversationState.Asking,
                //                            HeadImgUrl = faninfo.HeadImgUrl,
                //                            NickName = faninfo.NickName,
                //                            CustomerId = customer.Id,
                //                            CustomerOpenId = customer.OpenID,
                //                            StartTalkTime = DateTime.Now,
                //                            LastModificationTime = DateTime.Now
                //                        });
                //                        await _cacheManager.GetCache(AppConsts.Cache_FanOpenId2Conversation).SetAsync(request.FromUserName, conversation);
                //                        await _cacheManager.GetCache(AppConsts.Cache_Kf_FanOpenId2Conversation).RemoveAsync(request.FromUserName);
                //                        #endregion
                //                    }
                //                }
                #endregion

                return request.CreateResponseMessage<ResponseMessageNoResponse>();
            }
            else
            {
                Logger.Info($"开始接入客服 休息时间 {request.FromUserName}");
                return request.CreateResponseMessage<ResponseMessageNoResponse>();
            }
        }

        public async Task<IResponseMessageBase> ChunYuNotice(int mpid, IRequestMessageBase request)
        {
            var config = await GetCYConfig();
            if (config != null)
            {
                var response = request.CreateResponseMessage<ResponseMessageText>();
                response.Content = config.CustomerTaklingAnswer;
                return response;
            }
            return request.CreateResponseMessage<ResponseMessageNoResponse>();
        }

        public async Task Ask(int mpid, string openid, string msgtype, string msgcontent, string format = null)
        {
            Logger.Info($"用户客服提问，进入Ask：MpID【{mpid}】OPENID【{openid}】");
            var faninfo = await GetFan(mpid, openid);
            if (faninfo != null && !string.IsNullOrEmpty(msgcontent))
            {
                Logger.Info($"用户客服提问，存在粉丝信息");
                var conversation = await GetUserLastConversation(openid);

                if (conversation != null)
                {
                    Logger.Info($"用户客服提问，存在会话信息");
                    var customer = await GetCustomer(conversation.CustomerOpenId, mpid);
                    if (customer != null)
                    {
                        Logger.Info($"用户客服提问 {faninfo.OpenID} {msgtype}");
                        if (msgcontent == "gb")
                        {
                            await Close(conversation);
                        }
                        else
                        {
                            var medisurl = "";
                            if (msgtype.ToLower() != MpMessageType.text.ToString())
                            {
                                medisurl = await GetMedia(mpid, msgcontent, msgtype, format);//非文本消息的素材url
                                Logger.Error($"临时素材本地URL：{medisurl}");
                            }
                            if (conversation.State == (int)CustomerServiceConversationState.Wait)
                            {

                                await SendCustomerText(mpid, openid, await _customerServiceResponseTextAppService.GetWaitResponseString(mpid));

                                await _customerServiceConversationMsgAppService.Create(new CustomerServiceConversationMsgDto()
                                {
                                    CustomerId = customer.Id,
                                    FanId = faninfo.Id,
                                    MpID = mpid,
                                    MsgContent = msgtype.ToLower() == MpMessageType.text.ToString() ? msgcontent : "",
                                    MediaUrl = msgtype.ToLower() != MpMessageType.text.ToString() ? medisurl : "",
                                    MsgType = GetMsgType(msgtype),
                                    Sender = (int)CustomerServiceMsgSender.user
                                });
                            }
                            else if (conversation.State == (int)CustomerServiceConversationState.Asking)
                            {


                                WebClient wc = new WebClient();
                                var values = new NameValueCollection();
                                values.Add("messageToken", customer.MessageToken);//消息令牌
                                values.Add("fanOpenId", conversation.FanOpenId);//粉丝openid
                                values.Add("fanNickName", faninfo.NickName);//粉丝昵称
                                values.Add("fanHeadImgUrl", faninfo.HeadImgUrl);//粉丝头像
                                values.Add("replyType", ((int)CustomerServiceReplyType.Common).ToString());//回复类型
                                values.Add("msgType", msgtype);//消息类型
                                if (msgtype.ToLower() != MpMessageType.text.ToString())
                                {
                                    values.Add("msgContent", medisurl);
                                }
                                else
                                    values.Add("msgContent", msgcontent);//文本消息
                                Logger.Info($"用户客服提问 开始发消息给客服 {faninfo.OpenID}");
                                var result = await wc.UploadValuesTaskAsync(_signalrCustomerService.CustomerAskUrl, values);
                                Logger.Info($"用户客服提问 开始发消息给客服结果 {faninfo.OpenID} {Encoding.UTF8.GetString(result)}");



                                await _customerServiceConversationMsgAppService.Create(new CustomerServiceConversationMsgDto()
                                {
                                    CustomerId = customer.Id,
                                    FanId = faninfo.Id,
                                    MpID = mpid,
                                    MsgContent = msgtype.ToLower() == MpMessageType.text.ToString() ? msgcontent : "",
                                    MediaUrl = msgtype.ToLower() != MpMessageType.text.ToString() ? medisurl : "",
                                    MsgType = GetMsgType(msgtype),
                                    Sender = (int)CustomerServiceMsgSender.user
                                });
                            }
                        }
                    }
                }
                else
                    Logger.Info($"找不到会话信息");
            }
            else
                Logger.Info($"找不到粉丝");
        }

        #region 用户关闭问题
        /// <summary>
        /// 用户关闭问题
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        private async Task<bool> Close(CustomerServiceConversationDto conversation)
        {
            if (conversation != null)
            {
                Logger.Info($"开始关闭问题 {conversation.Id}");
                conversation.EndTalkTime = DateTime.Now;
                conversation.State = (int)CustomerServiceConversationState.Closed;
                await _customerServiceConversationAppService.Update(conversation);

                WebClient wc = new WebClient();
                var values = new NameValueCollection();
                values.Add("conversationId", conversation.Id.ToString());//会话ID
                values.Add("FansClose", "1");//会话ID
                await wc.UploadValuesTaskAsync(JobConfig.AppSettings["AutoStopConversationUrl"], values);
                Logger.Info($"成功关闭问题 {conversation.Id}");
            }
            return false;
        }
        #endregion

        #region 获取春雨配置
        /// <summary>
        /// 获取春雨配置
        /// </summary>
        /// <returns></returns>
        private async Task<CYConfigDto> GetCYConfig()
        {
            var result = await _cacheManager.GetCache(AppConsts.Cache_ChunyuConfig).GetAsync(AppConsts.Cache_ChunyuConfig, async (key) =>
            {
                return await _cYConfigAppService.GetConfig();
            });
            return result == null ? null : result as CYConfigDto;
        }
        #endregion

        #region 发送客服文本消息
        private async Task SendCustomerText(int mpid, string openid, string text)
        {
            var account = await _mpAccountAppService.GetCache(mpid);
            if (account != null)
            {
                //根据配置信息，调用客服接口，发送问题创建后的默认回复
                try
                {
                    await CustomApi.SendTextAsync(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret), openid, text);
                }
                catch
                {
                    await CustomApi.SendTextAsync(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true), openid, text);
                }
            }
        }
        #endregion

        #region 获取最后一个会话
        /// <summary>
        /// 获取最后一个会话
        /// </summary>
        /// <param name="mpid"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        private async Task<CustomerServiceConversationDto> GetUserLastConversation(string openid)
        {
            //获取用户未关闭的会话
            var lastConversation = await _cacheManager.GetCache(AppConsts.Cache_FanOpenId2Conversation).GetAsync(openid, async (key) =>
            {
                return await _customerServiceConversationAppService.GetUserLastConversation(openid);
            });
            return lastConversation == null ? null : lastConversation as CustomerServiceConversationDto;
        }
        #endregion

        #region 获取客服信息
        /// <summary>
        /// 获取客服信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CustomerServiceOnlineDto> GetCustomer(string openid, int mpid)
        {

            var customer = await _cacheManager.GetCache(AppConsts.Cache_OpenId2Customer).GetAsync(openid, async (key) =>
            {
                return await _customerServiceOnlineAppService.GetFirstOrDefault(new GetCustomerServiceOnlinesInput()
                {
                    MpID = mpid,
                    OpenID = openid
                });
            });
            if (customer == null)
                return null;
            return customer as CustomerServiceOnlineDto;

        }
        #endregion

        #region 获取粉丝信息
        public async Task<MpFanDto> GetFan(int mpid, string openid)
        {
            var fan = await _cacheManager.GetCache(AppConsts.Cache_MpFansByOpenId).GetAsync(openid, async (key) =>
            {
                var fandto = await _mpFanAppService.GetFirstOrDefaultByOpenID(openid);
                if (fandto == null)
                {
                    fandto = await AddFan(mpid, openid);
                }
                if (fandto != null)
                    await _cacheManager.GetCache(AppConsts.Cache_MpFansByUserId).SetAsync(fandto.Id.ToString(), fandto);
                return fandto;
            });

            if (fan == null)
                return null;
            return fan as MpFanDto;
        }
        public async Task<MpFanDto> GetFan(string openid)
        {
            var fan = await _cacheManager.GetCache(AppConsts.Cache_MpFansByOpenId).GetAsync(openid, async (key) =>
            {
                var fandto = await _mpFanAppService.GetFirstOrDefaultByOpenID(openid);
                if (fandto != null)
                    await _cacheManager.GetCache(AppConsts.Cache_MpFansByUserId).SetAsync(fandto.Id.ToString(), fandto);
                return fandto;
            });

            if (fan == null)
                return null;
            return fan as MpFanDto;
        }
        public async Task<MpFanDto> GetFan(int userid)
        {
            var fan = await _cacheManager.GetCache(AppConsts.Cache_MpFansByUserId).GetAsync(userid.ToString(), async (key) =>
            {
                var fandto = await _mpFanAppService.Get(new EntityDto<int>() { Id = userid });
                if (fandto != null)
                    await _cacheManager.GetCache(AppConsts.Cache_MpFansByOpenId).SetAsync(fandto.OpenID, fandto);
                return fandto;
            });
            if (fan == null)
                return null;
            return fan as MpFanDto;
        }
        #endregion

        #region 添加粉丝到数据库
        /// <summary>
        /// 添加粉丝到数据库
        /// </summary>
        /// <param name="mpid"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        private async Task<MpFanDto> AddFan(int mpid, string openid)
        {
            var account = await _mpAccountAppService.GetCache(mpid);
            if (account == null)
                return null;
            try
            {

                UserInfoJson wxinfo = null;
                try
                {
                    wxinfo = UserApi.Info(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret), openid);
                }
                catch
                {
                    Logger.Error(string.Format("获取MpID为{0}，openid为{1}的用户信息报错", mpid, openid));
                    wxinfo = UserApi.Info(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true), openid);
                }
                if (wxinfo.errcode != ReturnCode.请求成功)
                    throw new Exception(string.Format("获取MpID为{0}，openid为{1}的用户信息报错，错误编号:{2}，错误消息:{3}", mpid, openid, wxinfo.errcode, wxinfo.errmsg));

                var entityfans = new MpFanDto();
                entityfans.City = wxinfo.city;
                entityfans.Country = wxinfo.country;
                entityfans.HeadImgUrl = wxinfo.headimgurl;
                entityfans.IsFans = true;
                entityfans.Language = wxinfo.language;
                entityfans.MpID = mpid;
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
                entityfans.ChannelID = 0;
                entityfans.ChannelName = "公众号";
                return await _mpFanAppService.Create(entityfans);
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("粉丝订阅更新数据库失败，原因：{0}", ex.Message));
            }
            return null;
        }
        #endregion

        #region 微信临时素材
        /// <summary>
        /// 获取并下载微信临时素材
        /// </summary>
        /// <param name="mpid"></param>
        /// <param name="mediaid"></param>
        /// <returns></returns>
        private async Task<string> GetMedia(int mpid, string mediaid, string msgtype, string format = null)
        {
            var extra = string.Empty;
            if (msgtype == "Image")
                extra = "jpg";
            if (msgtype == "Voice")
                extra = format;
            if (msgtype == "Video")
                extra = "mp4";
            var account = await _mpAccountAppService.GetCache(mpid);
            WebClient client = new WebClient();
            if (account != null)
            {
                try
                {
                    Logger.Error("进入素材下载");
                    Logger.Error($"临时素材发送地址URL：https://api.weixin.qq.com/cgi-bin/media/get?access_token={await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret)}&media_id={mediaid}");

                    var mediaData = await client.DownloadDataTaskAsync($"https://api.weixin.qq.com/cgi-bin/media/get?access_token={await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret)}&media_id={mediaid}"); ;
                    return await _fileServer.UploadFile(mediaData, extra, AppConsts.FileType_WxMedia);
                    //return await _fileServer.Download($"https://api.weixin.qq.com/cgi-bin/media/get?access_token={await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret)}&media_id={mediaid}", type: AppConsts.FileType_WxMedia);
                }
                catch (Exception ex)
                {
                    Logger.Error(string.Format("下载MpID为{0}，media_id{1}的临时素材报错", mpid, mediaid), ex);
                    var mediaData = await client.DownloadDataTaskAsync($"https://api.weixin.qq.com/cgi-bin/media/get?access_token={await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret)}&media_id={mediaid}"); ;
                    return await _fileServer.UploadFile(mediaData, extra, AppConsts.FileType_WxMedia);
                    //return await _fileServer.Download($"https://api.weixin.qq.com/cgi-bin/media/get?access_token={await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true)}&media_id={mediaid}", type: AppConsts.FileType_WxMedia);
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 上传微信临时素材
        /// </summary>
        /// <param name="mpid"></param>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private async Task<string> UploadMedia(int mpid, string path, MpMessageType type)
        {

            var account = await _mpAccountAppService.Get(new EntityDto<int>() { Id = mpid });
            if (account != null)
            {
                var retString = "";
                try
                {
                    var url = string.Format("https://api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret), type.ToString());
                    retString = await HttpHelper.HttpPost(url, path, timeOut: Config.TIME_OUT);
                }
                catch (Exception ex)
                {
                    Logger.Error(string.Format("上传MpID为{0}，地址{1}的临时素材报错 {2}", mpid, path, type.ToString()), ex);
                    var url = string.Format("https://api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true), type.ToString());
                    retString = await HttpHelper.HttpPost(url, path, timeOut: Config.TIME_OUT);
                }
                var returndata = JsonConvert.DeserializeObject<UploadTemporaryMediaResult>(retString);
                if (returndata.errcode == ReturnCode.请求成功)
                    return returndata.media_id;
                else
                    Logger.Error(string.Format("上传MpID为{0}，地址{1}的临时素材报错：{2}", mpid, path, JsonConvert.SerializeObject(returndata)));
            }
            return string.Empty;
        }
        #endregion



        private int GetMsgType(string msgType)
        {
            msgType = msgType.ToLower();
            if (msgType == CustomerServiceMsgType.text.ToString())
                return (int)CustomerServiceMsgType.text;
            if (msgType == CustomerServiceMsgType.image.ToString())
                return (int)CustomerServiceMsgType.image;
            if (msgType == CustomerServiceMsgType.voice.ToString())
                return (int)CustomerServiceMsgType.voice;
            if (msgType == CustomerServiceMsgType.video.ToString())
                return (int)CustomerServiceMsgType.video;
            if (msgType == CustomerServiceMsgType.mpnews.ToString())
                return (int)CustomerServiceMsgType.mpnews;
            if (msgType == CustomerServiceMsgType.mpmultinews.ToString())
                return (int)CustomerServiceMsgType.mpmultinews;
            return -1;
        }
    }
}
