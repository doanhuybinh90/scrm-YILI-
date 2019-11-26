using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Runtime.Caching;
using Hangfire;
using Newtonsoft.Json;
using Pb.Wechat;
using Pb.Wechat.CustomerServiceConversations;
using Pb.Wechat.CustomerServiceConversations.Dto;
using Pb.Wechat.CustomerServiceOnlines;
using Pb.Wechat.CustomerServiceOnlines.Dto;
using Pb.Wechat.MpAccessTokenClib;
using Pb.Wechat.MpAccounts;
using Pb.Wechat.MpEvents.Dto;
using Pb.Wechat.MpFans;
using Pb.Wechat.MpFans.Dto;
using Pb.Wechat.Url;
using Senparc.Weixin;
using Senparc.Weixin.Helpers;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.User;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Pb.Hangfire.Jobs
{
    public class CustomerQuestionJoin : BackgroundJob<FansMessageDto>, ITransientDependency
    {
        private readonly ICacheManager _cacheManager;
        private readonly ICustomerServiceConversationAppService _customerServiceConversationAppService;
        private readonly IMpFanAppService _mpFanAppService;
        private readonly IAccessTokenContainer _accessTokenContainer;
        private readonly IMpAccountAppService _mpAccountAppService;
        private readonly ISignalrCustomerService _signalrCustomerService;
        private readonly ICustomerServiceOnlineAppService _customerServiceOnlineAppService;
        public CustomerQuestionJoin(
            ICacheManager cacheManager,
            ICustomerServiceConversationAppService customerServiceConversationAppService,
            IMpFanAppService mpFanAppService,
            IAccessTokenContainer accessTokenContainer,
            IMpAccountAppService mpAccountAppService,
            ISignalrCustomerService signalrCustomerService,
            ICustomerServiceOnlineAppService customerServiceOnlineAppService)
        {
            _cacheManager = cacheManager;
            _customerServiceConversationAppService = customerServiceConversationAppService;
            _mpFanAppService = mpFanAppService;
            _accessTokenContainer = accessTokenContainer;
            _mpAccountAppService = mpAccountAppService;
            _signalrCustomerService = signalrCustomerService;
            _customerServiceOnlineAppService = customerServiceOnlineAppService;
        }
        [AutomaticRetry(Attempts = 0)]
        public override void Execute(FansMessageDto args)
        {
            Task.WaitAll(DoExecute(args));
        }
        public async Task DoExecute(FansMessageDto args)
        {
            var faninfo = await GetFan(args.MpID, args.OpenID);
            if (faninfo != null)
            {
                Logger.Info($"开始调用客服接入机制");
                CustomerServiceOnlineDto customer = null;
                if (faninfo.LastCustomerServiceId != 0)
                    customer = await _customerServiceOnlineAppService.GetLastCustomerService(faninfo.LastCustomerServiceId);//获取末次接待客服
                if (customer == null)
                    customer = await _customerServiceOnlineAppService.GetAutoJoinCustomer(args.MpID);//获取自动分配客服
                if (customer == null)//微客服坐席已满，进入等待队列
                {
                    Logger.Info($"进入等待队列,开始创建等待会话");
                    var conversation = await _customerServiceConversationAppService.Create(new CustomerServiceConversationDto()
                    {
                        FanOpenId = args.OpenID,
                        FanId = faninfo.Id,
                        MpID = args.MpID,
                        State = (int)CustomerServiceConversationState.Wait,
                        HeadImgUrl = faninfo.HeadImgUrl,
                        NickName = faninfo.NickName
                    });
                    Logger.Info($"进入等待队列,创建等待会话成功，开始记录缓存");
                    await _cacheManager.GetCache(AppConsts.Cache_FanOpenId2Conversation).SetAsync(args.OpenID, conversation);
                    //await _cacheManager.GetCache(AppConsts.Cache_Kf_FanOpenId2Conversation).SetAsync(args.OpenID, JsonConvert.SerializeObject(conversation));
                    await _cacheManager.GetCache(AppConsts.Cache_Kf_FanOpenId2Conversation).RemoveAsync(args.OpenID);

                    #region 推送消息给客服
                    Logger.Info($"进入等待队列,创建等待会话成功，记录缓存成功，推送等待人数");
                    Logger.Info($"推送url为{_signalrCustomerService.SetUnConnectNoticeUrl}");
                    WebClient wc = new WebClient();
                    await wc.UploadValuesTaskAsync(_signalrCustomerService.SetUnConnectNoticeUrl, new System.Collections.Specialized.NameValueCollection());
                    #endregion
                }
                else
                {
                    Logger.Info($"分配到自动接入客服：{JsonConvert.SerializeObject(customer)}");
                    #region 创建客服会话
                    var conversation = await _customerServiceConversationAppService.Create(new CustomerServiceConversationDto()
                    {
                        FanOpenId = args.OpenID,
                        FanId = faninfo.Id,
                        MpID = args.MpID,
                        State = (int)CustomerServiceConversationState.Asking,
                        HeadImgUrl = faninfo.HeadImgUrl,
                        NickName = faninfo.NickName,
                        CustomerId = customer.Id,
                        CustomerOpenId = customer.OpenID,
                        StartTalkTime = DateTime.Now,
                        LastModificationTime = DateTime.Now,
                        ConversationScore = -1
                    });

                    faninfo.LastCustomerServiceId = customer.Id;
                    faninfo.LastCustomerServiceOpenId = customer.OpenID;
                    await _mpFanAppService.Update(faninfo);

                    await _cacheManager.GetCache(AppConsts.Cache_FanOpenId2Conversation).SetAsync(args.OpenID, conversation);
                    //await _cacheManager.GetCache(AppConsts.Cache_Kf_FanOpenId2Conversation).SetAsync(args.OpenID, JsonConvert.SerializeObject(conversation));
                    await _cacheManager.GetCache(AppConsts.Cache_Kf_FanOpenId2Conversation).RemoveAsync(args.OpenID);

                    WebClient wc = new WebClient();
                    var values = new NameValueCollection();
                    values.Add("messageToken", customer.MessageToken);//消息令牌
                    values.Add("fanOpenId", conversation.FanOpenId);//粉丝openid
                    values.Add("fanNickName", faninfo.NickName);//粉丝昵称
                    values.Add("fanHeadImgUrl", faninfo.HeadImgUrl);//粉丝头像
                    values.Add("replyType", ((int)CustomerServiceReplyType.AutoIn).ToString());//回复类型
                    values.Add("msgType", MpMessageType.text.ToString());//消息类型
                    values.Add("msgContent", "");//文本消息
                    var result = await wc.UploadValuesTaskAsync(_signalrCustomerService.CustomerAskUrl, values);
                    #endregion
                }
            }
        }

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
    }
}
