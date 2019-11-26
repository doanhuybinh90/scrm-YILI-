using Abp.Application.Services.Dto;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Runtime.Caching;
using Hangfire;
using Newtonsoft.Json;
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
using System.Collections.Specialized;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Pb.Hangfire.Jobs
{
    public class SendChunYuCloseMessage : BackgroundJob<string>, ISingletonDependency
    {
        private readonly ICacheManager _cacheManager;
        private readonly IChunYuService _chunYuService;
        private readonly ICYConfigAppService _cYConfigAppService;
        private readonly ICYProblemAppService _cYProblemAppService;
        private readonly IMpAccountAppService _mpAccountAppService;
        private readonly IMpFanAppService _mpFanAppService;
        private readonly IWebUrlService _webUrlService;
        private readonly IAccessTokenContainer _accessTokenContainer;

        public SendChunYuCloseMessage(ICacheManager cacheManager, IAccessTokenContainer accessTokenContainer, ICYConfigAppService cYConfigAppService, IChunYuService chunYuService, ICYProblemAppService cYProblemAppService, IMpFanAppService mpFanAppService, IMpAccountAppService mpAccountAppService, IWebUrlService webUrlService)
        {

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
            Task.WaitAll(TimeSendCloseText(args ?? ""));
        }



        #region 长时间未操作，帮用户关闭
        /// <summary>
        /// 长时间未操作，帮用户关闭
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public async Task TimeSendCloseText(string openid)
        {
            var config = await GetCYConfig();
            if (config != null)
            {
                var faninfo = await GetFan(openid);
                if (faninfo != null)
                {
                    //获取最后一个提问
                    var problem = await GetLastProblem(openid);
                    //问题未关闭，且最后更新时间超过关闭时间
                    if (problem != null && problem.State != (int)CYProblemState.Closed && problem.LastModificationTime.Value.AddMinutes(config.NoOperationCloseMinute - config.NoOperationMinute) <= DateTime.Now)
                    {
                        if (problem.State == (int)CYProblemState.Asking)
                        {
                            //调用春雨医生接口，创建问题
                            WebClient wc = new WebClient();
                            var data = new NameValueCollection();
                            var atime = GetTimestamp(DateTime.Now);
                            data.Add("user_id", faninfo.Id.ToString());
                            data.Add("partner", _chunYuService.ChunYuPartner);
                            data.Add("problem_id", (problem.CYProblemId ?? -1).ToString());
                            data.Add("atime", atime.ToString());
                            data.Add("sign", GetChunYuSign(atime, faninfo.Id));
                            var str = Encoding.UTF8.GetString(await wc.UploadValuesTaskAsync($"{_chunYuService.ChunYuBaseUrl}{_chunYuService.ChunYuCloseUrl}", data));
                            var returndata = JsonConvert.DeserializeObject<ChunYuReturnMsg>(str);
                            if (returndata.error == 0)
                            {
                                problem.LastModificationTime = DateTime.Now;
                                problem.State = (int)CYProblemState.Closed;
                                //创建成功后更新数据库和缓存
                                problem = await _cYProblemAppService.Update(problem);
                                await _cacheManager.GetCache(AppConsts.Cache_ChunyuProblemByOpenId).RemoveAsync(faninfo.OpenID);
                            }
                        }
                        else
                        {
                            problem.LastModificationTime = DateTime.Now;
                            problem.State = (int)CYProblemState.Closed;
                            //创建成功后更新数据库和缓存
                            problem = await _cYProblemAppService.Update(problem);
                            await _cacheManager.GetCache(AppConsts.Cache_ChunyuProblemByOpenId).RemoveAsync(faninfo.OpenID);
                            
                            if (!string.IsNullOrEmpty(config.CloseProblemText))
                            {
                                await SendCustomerText(faninfo.MpID, openid, faninfo.Id, problem.Id, "", config.CloseProblemText);
                            }
                        }
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
                var fandto = await _mpFanAppService.GetFirstOrDefaultByOpenID(
                   openid);
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

        #region 计算春雨医生签名
        /// <summary>
        /// 计算春雨医生签名
        /// </summary>
        /// <param name="atime"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private string GetChunYuSign(long atime, int id)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(Encoding.UTF8.GetBytes(_chunYuService.ChunYuPassword + atime.ToString() + id.ToString()));
            return BitConverter.ToString(output).Replace("-", "").Substring(8, 16).ToLower();
        }

        /// <summary>
        /// 验证春雨医生签名
        /// </summary>
        /// <param name="atime"></param>
        /// <param name="id"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        private bool ValidateChunYuSign(long atime, int id, string sign)
        {
            var datetime = NewDate(atime);
            //时间戳和当前时间差别太大，则不予处理
            if (Math.Abs((DateTime.Now - datetime).Hours) > 2)
                return false;
            return sign == GetChunYuSign(atime, id);
        }
        #endregion

        #region 时间转换
        /// <summary>
        /// 获取1970-01-01至dateTime的毫秒数
        /// </summary>
        public static long GetTimestamp(DateTime dateTime)
        {
            DateTime dt1970 = new DateTime(1970, 1, 1, 8, 0, 0, 0);
            return (dateTime.Ticks - dt1970.Ticks) / 10000000;
        }
        /// <summary>
        /// 根据时间戳timestamp（单位毫秒）计算日期
        /// </summary>
        public static DateTime NewDate(long timestamp)
        {
            DateTime dt1970 = new DateTime(1970, 1, 1, 8, 0, 0, 0);
            long t = dt1970.Ticks + timestamp * 10000000;
            return new DateTime(t);
        }
        #endregion
    }
    public class ChunYuReturnMsg
    {
        public int error { get; set; }
        public string error_msg { get; set; }
        public int problem_id { get; set; }
    }
}
