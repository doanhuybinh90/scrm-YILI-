using Abp.Application.Services.Dto;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Runtime.Caching;
using Hangfire;
using Pb.Wechat;
using Pb.Wechat.CYConfigs;
using Pb.Wechat.CYConfigs.Dto;
using Pb.Wechat.CYProblems;
using Pb.Wechat.CYProblems.Dto;
using Pb.Wechat.MpAccessTokenClib;
using Pb.Wechat.MpAccounts;
using Pb.Wechat.MpFans;
using Pb.Wechat.MpFans.Dto;
using Pb.Wechat.Url;
using Senparc.Weixin.MP.AdvancedAPIs;
using System;
using System.Threading.Tasks;

namespace Pb.Hangfire.Jobs
{
    public class SendChunYuPreCloseMessage : BackgroundJob<string>, ISingletonDependency
    {
        private readonly ICacheManager _cacheManager;
        private readonly IChunYuService _chunYuService;
        private readonly ICYConfigAppService _cYConfigAppService;
        private readonly ICYProblemAppService _cYProblemAppService;
        private readonly IMpAccountAppService _mpAccountAppService;
        private readonly IMpFanAppService _mpFanAppService;
        private readonly IWebUrlService _webUrlService;
        private readonly IAccessTokenContainer _accessTokenContainer;

        public SendChunYuPreCloseMessage(ICacheManager cacheManager, IAccessTokenContainer accessTokenContainer, ICYConfigAppService cYConfigAppService, IChunYuService chunYuService, ICYProblemAppService cYProblemAppService, IMpFanAppService mpFanAppService, IMpAccountAppService mpAccountAppService,IWebUrlService webUrlService) {

            _accessTokenContainer = accessTokenContainer;
            _cacheManager = cacheManager;
            _chunYuService = chunYuService;
            _cYConfigAppService = cYConfigAppService;
            _cYProblemAppService = cYProblemAppService;
            _mpFanAppService = mpFanAppService;
            _mpAccountAppService = mpAccountAppService;
            _webUrlService = webUrlService;
        }
        
        [AutomaticRetry(Attempts = 1)]
        public override void Execute(string args)
        {
            Task.WaitAll(TimeSendPreCloseText(args ?? ""));
        }

        #region 长时间未操作，推送关闭提醒
        /// <summary>
        /// 长时间未操作，推送关闭提醒
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public async Task TimeSendPreCloseText(string openid)
        {
            var config = await GetCYConfig();
            if (config != null)
            {
                var faninfo = await GetFan(openid);
                if (faninfo != null)
                {
                    //获取最后一个提问
                    var problem = await GetLastProblem(openid);
                    //问题未关闭，为推送提示消息，且最后更新时间超过预提醒时间
                    if (problem != null && problem.State != (int)CYProblemState.Closed && !problem.CloseNotice && problem.LastModificationTime.Value.AddMinutes(config.NoOperationMinute) <= DateTime.Now)
                    {
                        if (!string.IsNullOrEmpty(config.NoOperationText))
                        {
                            await SendCustomerText(faninfo.MpID, openid, faninfo.Id, problem.Id, "", config.NoOperationText);
                        }
                        //更新成功后更新数据库和缓存
                        problem.CloseNotice = true;
                        problem = await _cYProblemAppService.Update(problem);
                        await _cacheManager.GetCache(AppConsts.Cache_ChunyuProblemByOpenId).SetAsync(openid, problem);
                    }
                }
            }
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
        
        #region 获取最后一个问题
        /// <summary>
        /// 获取最后一个问题
        /// </summary>
        /// <param name="mpid"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        private async Task<CYProblemDto> GetLastProblem(string openid)
        {
            //获取用户未关闭的提问
            var lastProblem = await _cacheManager.GetCache(AppConsts.Cache_ChunyuProblemByOpenId).GetAsync(openid, async (key) =>
            {
                return await _cYProblemAppService.GetUserLastProblem(openid);
            });
            return lastProblem == null ? null : lastProblem as CYProblemDto;
        }
        #endregion

        #region 获取粉丝信息
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
        #endregion
        
        #region 发送客服文本消息
        private async Task<string> SendCustomerText(int mpid, string openid, int problemid, int fanid, string text, string templete = null)
        {
            var account = await _mpAccountAppService.Get(new EntityDto<int>() { Id = mpid });
            if (account != null)
            {
                var textinfo = GetCustomerText(text, templete, fanid, problemid, openid);
                //根据配置信息，调用客服接口，发送问题创建后的默认回复
                try
                {
                    await CustomApi.SendTextAsync(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret), openid, textinfo);
                    return textinfo;
                }
                catch (Exception ex)
                {
                    Logger.Error(string.Format("发送MpID为{0}，openid为{1}的客服消息{2}报错", mpid, openid, textinfo), ex);
                    await CustomApi.SendTextAsync(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true), openid, textinfo);
                    return textinfo;
                }
            }
            return string.Empty;
        }
        #endregion
        
        #region 获取客服接口回答内容
        private string GetCustomerText(string content, string templete, int fanid, int problemid, string openid)
        {
            if (string.IsNullOrEmpty(templete))
                return content;
            templete = templete.Replace("{content}", content);
            templete = templete.Replace("{fanid}", fanid.ToString());
            templete = templete.Replace("{openid}", openid);
            templete = templete.Replace("{problemid}", problemid.ToString());
            templete = templete.Replace("{WebSiteRootAddress}", _webUrlService.GetSiteRootAddress());
            return templete;
        }
        #endregion
    }
}
