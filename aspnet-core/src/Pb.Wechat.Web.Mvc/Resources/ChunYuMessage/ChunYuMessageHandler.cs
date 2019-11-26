using Abp.Application.Services.Dto;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Runtime.Caching;
using Castle.Core.Logging;
using Newtonsoft.Json;
using Pb.Hangfire.Jobs;
using Pb.Wechat.CYConfigs;
using Pb.Wechat.CYConfigs.Dto;
using Pb.Wechat.CYDoctors;
using Pb.Wechat.CYDoctors.Dto;
using Pb.Wechat.CYProblemContents;
using Pb.Wechat.CYProblemContents.Dto;
using Pb.Wechat.CYProblems;
using Pb.Wechat.CYProblems.Dto;
using Pb.Wechat.MpAccessTokenClib;
using Pb.Wechat.MpAccounts;
using Pb.Wechat.MpEvents.Dto;
using Pb.Wechat.MpFans;
using Pb.Wechat.MpFans.Dto;
using Pb.Wechat.MpUserMembers;
using Pb.Wechat.Url;
using Pb.Wechat.Web.Helpers;
using Pb.Wechat.Web.Models.ChunYuModel;
using Pb.Wechat.Web.Resources.FileServers;
using Senparc.Weixin;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.Media;
using Senparc.Weixin.MP.AdvancedAPIs.User;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Resources
{
    public class ChunYuMessageHandler : IChunYuMessageHandler, ITransientDependency
    {
        private readonly ICacheManager _cacheManager;
        private readonly IChunYuService _chunYuService;
        private readonly ICYConfigAppService _cYConfigAppService;
        private readonly ICYProblemAppService _cYProblemAppService;
        private readonly ICYDoctorAppService _cYDoctorAppService;
        private readonly ICYProblemContentAppService _cYProblemContentAppService;
        private readonly IMpAccountAppService _mpAccountAppService;
        private readonly IMpFanAppService _mpFanAppService;
        private readonly IMpUserMemberAppService _mpUserMemberAppService;
        private readonly IAccessTokenContainer _accessTokenContainer;
        private readonly IFileServer _fileServer;
        private readonly IWebUrlService _webUrlService;
        private readonly IBackgroundJobManager _backgroundJobManager;
        public ILogger Logger { get; set; }

        public ChunYuMessageHandler(ICacheManager cacheManager, IAccessTokenContainer accessTokenContainer, ICYConfigAppService cYConfigAppService, IChunYuService chunYuService, ICYProblemAppService cYProblemAppService, ICYDoctorAppService cYDoctorAppService, ICYProblemContentAppService cYProblemContentAppService, IMpFanAppService mpFanAppService, IMpAccountAppService mpAccountAppService, IMpUserMemberAppService mpUserMemberAppService, IFileServer fileServer, IWebUrlService webUrlService, IBackgroundJobManager backgroundJobManager)
        {
            _accessTokenContainer = accessTokenContainer;
            _cacheManager = cacheManager;
            _chunYuService = chunYuService;
            _cYConfigAppService = cYConfigAppService;
            _cYProblemAppService = cYProblemAppService;
            _cYDoctorAppService = cYDoctorAppService;
            _cYProblemContentAppService = cYProblemContentAppService;
            _mpFanAppService = mpFanAppService;
            _mpAccountAppService = mpAccountAppService;
            _mpUserMemberAppService = mpUserMemberAppService;
            _fileServer = fileServer;
            _webUrlService = webUrlService;
            _backgroundJobManager = backgroundJobManager;
            Logger = NullLogger.Instance;
        }

        #region 公共方法
        #region 判断是否有未关闭的提问
        /// <summary>
        /// 判断是否有未关闭的提问
        /// </summary>
        /// <param name="mpid"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public async Task<bool> IsAsking(int mpid, string openid)
        {
            Logger.Info($"开始判断是否在春雨医生会话中 {openid}");
            //获取用户未关闭的提问
            var lastProblem = await GetLastProblem(openid);
            if (lastProblem == null)
            {
                Logger.Info($"没有春雨医生会话 {openid}");
                return false;
            }
            else
            {
                if (lastProblem.State == (int)CYProblemState.Closed)
                {
                    Logger.Info($"春雨医生会话已关闭 {openid}");
                    return false;
                }
            }
            Logger.Info($"正在春雨医生会话中 {openid}");
            return true;
        }
        #endregion
        
        #region 创建问题
        /// <summary>
        /// 创建问题
        /// </summary>
        /// <param name="mpid"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public async Task<IResponseMessageBase> CreateProblem(int mpid, IRequestMessageBase request)
        {
            var config = await GetCYConfig();
            if (config != null)
            {
                var faninfo = await GetFan(mpid, request.FromUserName);
                if (faninfo != null)
                {
                    //获取最后一个提问
                    var problem = await GetLastProblem(request.FromUserName);
                    //没有提问或者提问已结束，则创建新的提问
                    if (problem == null || problem.State == (int)CYProblemState.Closed)
                    {
                        await SendCustomerText(mpid, request.FromUserName, faninfo.Id, problem == null ? -1 : problem.Id, config.CreateProblemPreText);
                        var _memberInfo = await _mpUserMemberAppService.GetByOpenID(request.FromUserName);
                        //创建成功后更新数据库和缓存
                        problem = await _cYProblemAppService.Create(new CYProblemDto()
                        {
                            FansId = faninfo.Id,
                            OpenId = request.FromUserName,
                            CreationTime = DateTime.Now,
                            State = (int)CYProblemState.Prepare,
                            NickName = faninfo.NickName,
                            UserName = _memberInfo == null ? "" : _memberInfo.MemberName,
                            BabyName = _memberInfo == null ? "" : _memberInfo.BabyName,
                            Telephone = _memberInfo == null ? "" : _memberInfo.MobilePhone,
                        });
                        await _cacheManager.GetCache(AppConsts.Cache_ChunyuProblemByOpenId).SetAsync(request.FromUserName, problem);
                        await _backgroundJobManager.EnqueueAsync<SendChunYuPreCloseMessage, string>(request.FromUserName, delay: new TimeSpan(0, config.NoOperationMinute, 0));
                        await _backgroundJobManager.EnqueueAsync<SendChunYuCloseMessage, string>(request.FromUserName, delay: new TimeSpan(0, config.NoOperationCloseMinute, 0));
                    }
                    else
                    {
                        await SendCustomerText(mpid, request.FromUserName, faninfo.Id, problem.Id, config.CreateProblemPreText);
                        //更新成功后更新数据库和缓存
                        problem.LastModificationTime = DateTime.Now;
                        problem.CloseNotice = false;
                        problem = await _cYProblemAppService.Update(problem);
                        await _cacheManager.GetCache(AppConsts.Cache_ChunyuProblemByOpenId).SetAsync(request.FromUserName, problem);
                        await _backgroundJobManager.EnqueueAsync<SendChunYuPreCloseMessage, string>(request.FromUserName, delay: new TimeSpan(0, config.NoOperationMinute, 0));
                        await _backgroundJobManager.EnqueueAsync<SendChunYuCloseMessage, string>(request.FromUserName, delay: new TimeSpan(0, config.NoOperationCloseMinute, 0));
                    }
                }
            }
            return request.CreateResponseMessage<ResponseMessageNoResponse>();
        }
        #endregion

        #region 提问
        /// <summary>
        /// 提问
        /// </summary>
        /// <param name="mpid"></param>
        /// <param name="openid"></param>
        /// <param name="msgType"></param>
        /// <param name="msgContent"></param>
        /// <returns></returns>
        public async Task Ask(int mpid, string openid, string msgtype, string msgcontent)
        {
            Logger.Info($"开始提问春雨医生 {openid} {msgcontent}");
            //获取最后一个提问
            var problem = await GetLastProblem(openid);
            if (problem != null && problem.State != (int)CYProblemState.Closed)
            {
                Logger.Info($"有未关闭的春雨医生问题 {openid} {msgcontent}");
                var config = await GetCYConfig();
                if (config != null)
                {
                    Logger.Info($"获取春雨医生配置 {openid} {msgcontent}");
                    var faninfo = await GetFan(mpid, openid);
                    if (faninfo != null)
                    {
                        Logger.Info($"获取春雨医生粉丝 {openid} {msgcontent}");
                        if (msgcontent == "gb") {
                            await Close(openid);
                        }
                        else
                        {
                            if (problem.State == (int)CYProblemState.Prepare)
                            {
                                Logger.Info($"问题准备中 {openid} {msgcontent}");
                                //问题长度符合要求
                                if (msgtype.ToLower() == MpMessageType.text.ToString() && !string.IsNullOrEmpty(msgcontent) && msgcontent.Length >= config.CreateProblemPreTextLength && await LoginAsync(mpid, openid))
                                {
                                    //调用春雨医生接口，创建问题
                                    WebClient wc = new WebClient();
                                    var data = new NameValueCollection();
                                    var atime = GetTimestamp(DateTime.Now);
                                    data.Add("user_id", faninfo.Id.ToString());
                                    data.Add("partner", _chunYuService.ChunYuPartner);
                                    data.Add("content", JsonConvert.SerializeObject(new List<BaseContent>() {
                                new MetaContent(){
                                    type=CYProblemContentType.patient_meta.ToString(),
                                    age="未知",
                                    sex=faninfo.Sex=="1"?"男":faninfo.Sex=="2"?"女":"未知",
                                },
                                new TextContent(){
                                    type=CYProblemContentType.text.ToString(),
                                    text=msgcontent,
                                },
                            }));
                                    data.Add("atime", atime.ToString());
                                    data.Add("sign", GetChunYuSign(atime, faninfo.Id));
                                    var str = Encoding.UTF8.GetString(await wc.UploadValuesTaskAsync($"{_chunYuService.ChunYuBaseUrl}{_chunYuService.ChunYuCreateUrl}", data));
                                    var returndata = JsonConvert.DeserializeObject<Models.ChunYuModel.ChunYuReturnMsg>(str);

                                    Logger.Info($"创建问题sign {openid} {msgcontent} {data["sign"].ToString()}");
                                    Logger.Info($"创建问题结果 {openid} {msgcontent} {str}");
                                    if (returndata.error == 0)
                                    {
                                        problem.CYProblemId = returndata.problem_id;
                                        problem.State = (int)CYProblemState.Asking;
                                        problem.LastModificationTime = DateTime.Now;
                                        //创建成功后更新数据库和缓存
                                        problem = await _cYProblemAppService.Update(problem);
                                        await _cacheManager.GetCache(AppConsts.Cache_ChunyuProblemByOpenId).SetAsync(openid, problem);
                                        await SendCustomerText(mpid, openid, faninfo.Id, problem.Id, config.CreateProblemText);
                                        await _cYProblemContentAppService.Create(new CYProblemContentDto()
                                        {
                                            ProblemId = problem.Id,
                                            CreationTime = DateTime.Now,
                                            Type = (int)CYProblemContentType.text,
                                            SendUser = (int)CYProblemContentSender.user,
                                            Text = msgcontent,
                                        });
                                        await _backgroundJobManager.EnqueueAsync<SendChunYuPreCloseMessage, string>(openid, delay: new TimeSpan(0, config.NoOperationMinute, 0));
                                        await _backgroundJobManager.EnqueueAsync<SendChunYuCloseMessage, string>(openid, delay: new TimeSpan(0, config.NoOperationCloseMinute, 0));
                                    }
                                }
                                //问题长度不符合要求
                                else
                                {
                                    await SendCustomerText(mpid, openid, faninfo.Id, problem.Id, config.CreateProblemPreText);
                                    //更新成功后更新数据库和缓存
                                    problem.LastModificationTime = DateTime.Now;
                                    problem.CloseNotice = false;
                                    problem = await _cYProblemAppService.Update(problem);
                                    await _cacheManager.GetCache(AppConsts.Cache_ChunyuProblemByOpenId).SetAsync(openid, problem);
                                    await _backgroundJobManager.EnqueueAsync<SendChunYuPreCloseMessage, string>(openid, delay: new TimeSpan(0, config.NoOperationMinute, 0));
                                    await _backgroundJobManager.EnqueueAsync<SendChunYuCloseMessage, string>(openid, delay: new TimeSpan(0, config.NoOperationCloseMinute, 0));
                                }
                            }
                            //追问医生
                            else if (problem.State == (int)CYProblemState.Asking && (msgtype.ToLower() == MpMessageType.text.ToString() || msgtype.ToLower() == MpMessageType.image.ToString() || msgtype.ToLower() == MpMessageType.voice.ToString()))
                            {
                                if (msgtype.ToLower() == MpMessageType.image.ToString() || msgtype.ToLower() == MpMessageType.voice.ToString())
                                {
                                    var file = await GetMedia(mpid, msgcontent);
                                    if (!string.IsNullOrEmpty(file))
                                    {

                                        //调用春雨医生接口，追加问题
                                        WebClient wc = new WebClient();
                                        var data = new NameValueCollection();
                                        var atime = GetTimestamp(DateTime.Now);
                                        data.Add("user_id", faninfo.Id.ToString());
                                        data.Add("partner", _chunYuService.ChunYuPartner);
                                        data.Add("problem_id", problem.CYProblemId.ToString());
                                        data.Add("content", JsonConvert.SerializeObject(new List<BaseContent>() {
                                        new ImageOrAudioContent(){
                                            type=msgtype.ToLower() == MpMessageType.image.ToString()?CYProblemContentType.image.ToString():CYProblemContentType.audio.ToString(),
                                            file=file,
                                        },
                                    }));
                                        data.Add("atime", atime.ToString());
                                        data.Add("sign", GetChunYuSign(atime, faninfo.Id));

                                        var str = Encoding.UTF8.GetString(await wc.UploadValuesTaskAsync($"{_chunYuService.ChunYuBaseUrl}{_chunYuService.ChunYuCreateAddUrl}", data));
                                        var returndata = JsonConvert.DeserializeObject<Models.ChunYuModel.ChunYuReturnMsg>(str);
                                        if (returndata.error == 0)
                                        {
                                            problem.LastModificationTime = DateTime.Now;
                                            problem.CloseNotice = false;
                                            //创建成功后更新数据库和缓存
                                            problem = await _cYProblemAppService.Update(problem);
                                            await _cacheManager.GetCache(AppConsts.Cache_ChunyuProblemByOpenId).SetAsync(openid, problem);
                                            await _cYProblemContentAppService.Create(new CYProblemContentDto()
                                            {
                                                ProblemId = problem.Id,
                                                CreationTime = DateTime.Now,
                                                Type = (int)(msgtype == MpMessageType.image.ToString() ? CYProblemContentType.image : CYProblemContentType.audio),
                                                SendUser = (int)CYProblemContentSender.user,
                                                Text = file,
                                            });
                                            await _backgroundJobManager.EnqueueAsync<SendChunYuPreCloseMessage, string>(openid, delay: new TimeSpan(0, config.NoOperationMinute, 0));
                                            await _backgroundJobManager.EnqueueAsync<SendChunYuCloseMessage, string>(openid, delay: new TimeSpan(0, config.NoOperationCloseMinute, 0));
                                        }
                                    }
                                }
                                else
                                {
                                    //调用春雨医生接口，追加问题
                                    WebClient wc = new WebClient();
                                    var data = new NameValueCollection();
                                    var atime = GetTimestamp(DateTime.Now);
                                    data.Add("user_id", faninfo.Id.ToString());
                                    data.Add("partner", _chunYuService.ChunYuPartner);
                                    data.Add("problem_id", problem.CYProblemId.ToString());
                                    data.Add("atime", atime.ToString());
                                    data.Add("sign", GetChunYuSign(atime, faninfo.Id));
                                    data.Add("content", JsonConvert.SerializeObject(new List<BaseContent>() {
                                    new TextContent(){
                                        type=CYProblemContentType.text.ToString(),
                                        text=msgcontent,
                                    },
                                }));

                                    var str = Encoding.UTF8.GetString(await wc.UploadValuesTaskAsync($"{_chunYuService.ChunYuBaseUrl}{_chunYuService.ChunYuCreateAddUrl}", data));
                                    var returndata = JsonConvert.DeserializeObject<Models.ChunYuModel.ChunYuReturnMsg>(str);

                                    Logger.Info($"追加问题sign {openid} {msgcontent} {data["sign"].ToString()}");
                                    Logger.Info($"追加问题结果 {openid} {msgcontent} {str}");

                                    if (returndata.error == 0)
                                    {
                                        problem.LastModificationTime = DateTime.Now;
                                        problem.CloseNotice = false;
                                        //创建成功后更新数据库和缓存
                                        problem = await _cYProblemAppService.Update(problem);
                                        await _cacheManager.GetCache(AppConsts.Cache_ChunyuProblemByOpenId).SetAsync(openid, problem);
                                        await _cYProblemContentAppService.Create(new CYProblemContentDto()
                                        {
                                            ProblemId = problem.Id,
                                            CreationTime = DateTime.Now,
                                            Type = (int)CYProblemContentType.text,
                                            SendUser = (int)CYProblemContentSender.user,
                                            Text = msgcontent,
                                        });
                                        await _backgroundJobManager.EnqueueAsync<SendChunYuPreCloseMessage, string>(openid, delay: new TimeSpan(0, config.NoOperationMinute, 0));
                                        await _backgroundJobManager.EnqueueAsync<SendChunYuCloseMessage, string>(openid, delay: new TimeSpan(0, config.NoOperationCloseMinute, 0));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region 医生回答
        /// <summary>
        /// 医生回答
        /// </summary>
        /// <param name="cyproblemid"></param>
        /// <param name="fanid"></param>
        /// <param name="msgcontent"></param>
        /// <param name="doctor"></param>
        /// <param name="atime"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        public async Task Answer(DoctorReplyModel data)
        {
            Logger.Info($"医生回答 {JsonConvert.SerializeObject(data)}");
            if (!string.IsNullOrEmpty(data.content) && ValidateChunYuSign(data.atime, data.problem_id, data.sign))
            {
                Logger.Info($"医生回答验证通过 {JsonConvert.SerializeObject(data)}");
                var config = await GetCYConfig();
                if (config != null)
                {
                    Logger.Info($"医生回答拿到配置 {JsonConvert.SerializeObject(data)}");
                    var fan = await GetFan(data.user_id);
                    if (fan != null)
                    {
                        Logger.Info($"医生回答拿到粉丝信息 {JsonConvert.SerializeObject(data)}");
                        var problem = await GetLastProblem(fan.OpenID);
                        if (problem != null && problem.CYProblemId == data.problem_id)
                        {
                            Logger.Info($"医生回答拿到问题 {JsonConvert.SerializeObject(data)}");
                            var msgList = JsonConvert.DeserializeObject<List<ChunYuContent>>(data.content);
                            if (msgList != null)
                            {
                                var doctordto = await GetDoctor(data.doctor);

                                //医生首次回答，推送医生信息
                                if (!(await HasDoctorReply(problem.Id)))
                                {
                                    await SendCustomerText(fan.MpID, fan.OpenID, fan.Id, problem.Id, "", config.DoctorInTemplete, data.doctor);
                                }

                                foreach (var msg in msgList)
                                {
                                    if (msg.type == CYProblemContentType.text.ToString())
                                    {
                                        await SendCustomerText(fan.MpID, fan.OpenID, fan.Id, problem.Id, msg.text, config.AnswerTemplete, data.doctor);
                                        await _cYProblemContentAppService.Create(new CYProblemContentDto()
                                        {
                                            ProblemId = problem.Id,
                                            CreationTime = DateTime.Now,
                                            Type = (int)CYProblemContentType.text,
                                            SendUser = (int)CYProblemContentSender.doctor,
                                            Text = msg.text,
                                            DoctorId=doctordto.Id,
                                            DoctorName= doctordto.Name,
                                        });
                                    }
                                    else if (msg.type == CYProblemContentType.image.ToString())
                                    {
                                        var mediaid =await UploadMedia(fan.MpID, msg.file, CYProblemContentType.image);
                                        await SendCustomerImage(fan.MpID, fan.OpenID, mediaid);
                                        await _cYProblemContentAppService.Create(new CYProblemContentDto()
                                        {
                                            ProblemId = problem.Id,
                                            CreationTime = DateTime.Now,
                                            Type = (int)CYProblemContentType.image,
                                            SendUser = (int)CYProblemContentSender.doctor,
                                            Text = msg.file,
                                            DoctorId = doctordto.Id,
                                            DoctorName = doctordto.Name,
                                        });
                                    }
                                    else if (msg.type == CYProblemContentType.audio.ToString())
                                    {
                                        var mediaid =await UploadMedia(fan.MpID, msg.file, CYProblemContentType.audio);
                                        await SendCustomerVoice(fan.MpID, fan.OpenID, mediaid);
                                        await _cYProblemContentAppService.Create(new CYProblemContentDto()
                                        {
                                            ProblemId = problem.Id,
                                            CreationTime = DateTime.Now,
                                            Type = (int)CYProblemContentType.audio,
                                            SendUser = (int)CYProblemContentSender.doctor,
                                            Text = msg.file,
                                            DoctorId = doctordto.Id,
                                            DoctorName = doctordto.Name,
                                        });
                                    }
                                }
                                await _cacheManager.GetCache(AppConsts.Cache_ChunyuProblemFirstContent).SetAsync(problem.Id.ToString(), true);
                                problem.LastModificationTime = DateTime.Now;
                                problem.CloseNotice = false;
                                //创建成功后更新数据库和缓存
                                problem = await _cYProblemAppService.Update(problem);
                                await _cacheManager.GetCache(AppConsts.Cache_ChunyuProblemByOpenId).SetAsync(fan.OpenID, problem);
                                await _backgroundJobManager.EnqueueAsync<SendChunYuPreCloseMessage, string>(fan.OpenID, delay: new TimeSpan(0, config.NoOperationMinute, 0));
                                await _backgroundJobManager.EnqueueAsync<SendChunYuCloseMessage, string>(fan.OpenID, delay: new TimeSpan(0, config.NoOperationCloseMinute, 0));
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region 春雨医生未结束，想接入客服时的提示
        /// <summary>
        /// 春雨医生未结束，想接入客服时的提示
        /// </summary>
        /// <param name="mpid"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public async Task<IResponseMessageBase> CustomerNotice(int mpid, IRequestMessageBase request)
        {
            var config = await GetCYConfig();
            if (config != null)
            {
                var faninfo = await GetFan(mpid, request.FromUserName);
                if (faninfo != null)
                {
                    //获取最后一个提问
                    var problem = await GetLastProblem(request.FromUserName);
                    //没有提问或者提问已结束，则创建新的提问
                    if (problem != null && problem.State != (int)CYProblemState.Closed)
                    {
                        await SendCustomerText(mpid, request.FromUserName, faninfo.Id, problem.Id, "", config.CustomerAnswer);
                        //更新成功后更新数据库和缓存
                        problem.LastModificationTime = DateTime.Now;
                        problem.CloseNotice = false;
                        problem = await _cYProblemAppService.Update(problem);
                        await _cacheManager.GetCache(AppConsts.Cache_ChunyuProblemByOpenId).SetAsync(request.FromUserName, problem);
                    }
                }
            }
            return request.CreateResponseMessage<ResponseMessageNoResponse>();
        }
        #endregion

        #region 医生关闭问题
        /// <summary>
        /// 医生关闭问题
        /// </summary>
        /// <param name="cyproblemid"></param>
        /// <param name="fanid"></param>
        /// <param name="msgcontent"></param>
        /// <param name="atime"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        public async Task Close(CloseProblemModel data)
        {
            Logger.Info($"医生回答 {JsonConvert.SerializeObject(data)}");
            if (ValidateChunYuSign(data.atime, data.problem_id, data.sign))
            {
                Logger.Info($"医生回答验证通过 {JsonConvert.SerializeObject(data)}");
                var fan = await GetFan(data.user_id);
                if (fan != null)
                {
                    Logger.Info($"医生回答拿到粉丝 {JsonConvert.SerializeObject(data)}");
                    var problem = await GetLastProblem(fan.OpenID);
                    if (problem != null && problem.CYProblemId == data.problem_id)
                    {
                        Logger.Info($"医生回答拿到问题 {JsonConvert.SerializeObject(data)}");
                        problem.LastModificationTime = DateTime.Now;
                        problem.State = (int)CYProblemState.Closed;
                        //创建成功后更新数据库和缓存
                        problem = await _cYProblemAppService.Update(problem);
                        await _cacheManager.GetCache(AppConsts.Cache_ChunyuProblemByOpenId).RemoveAsync(fan.OpenID);

                        var config = await GetCYConfig();
                        if (config != null)
                        {
                            Logger.Info($"医生回答拿到配置 {JsonConvert.SerializeObject(data)}");
                            if (!string.IsNullOrEmpty(config.CloseProblemText))
                            {
                                var sendmsg = await SendCustomerText(fan.MpID, fan.OpenID, fan.Id, problem.Id, config.CloseProblemText);
                                await _cYProblemContentAppService.Create(new CYProblemContentDto()
                                {
                                    ProblemId = problem.Id,
                                    CreationTime = DateTime.Now,
                                    Type = (int)CYProblemContentType.text,
                                    SendUser = (int)CYProblemContentSender.doctor,
                                    Text = sendmsg,
                                });
                            }
                        }
                    }
                    else
                    {
                        await _cacheManager.GetCache(AppConsts.Cache_ChunyuProblemByOpenId).RemoveAsync(fan.OpenID);
                    }
                }
            }
        }
        #endregion

        #region 用户关闭问题
        /// <summary>
        /// 用户关闭问题
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public async Task<bool> Close(string openid)
        {
            Logger.Info($"开始关闭问题 {openid}");
            var fan = await GetFan(openid);
            if (fan != null)
            {
                Logger.Info($"关闭问题拿到粉丝 {openid}");
                var problem = await GetLastProblem(fan.OpenID);
                if (problem != null && problem.State != (int)CYProblemState.Closed)
                {
                    Logger.Info($"关闭问题拿到未关闭问题 {openid}");
                    if (problem.State == (int)CYProblemState.Asking)
                    {
                        Logger.Info($"关闭问题询问中 {openid}");
                        //调用春雨医生接口，创建问题
                        WebClient wc = new WebClient();
                        var data = new NameValueCollection();
                        var atime = GetTimestamp(DateTime.Now);
                        data.Add("user_id", fan.Id.ToString());
                        data.Add("partner", _chunYuService.ChunYuPartner);
                        data.Add("problem_id", (problem.CYProblemId ?? -1).ToString());
                        data.Add("atime", atime.ToString());
                        data.Add("sign", GetChunYuSign(atime, fan.Id));
                        var str = Encoding.UTF8.GetString(await wc.UploadValuesTaskAsync($"{_chunYuService.ChunYuBaseUrl}{_chunYuService.ChunYuCloseUrl}", data));
                        var returndata = JsonConvert.DeserializeObject<Models.ChunYuModel.ChunYuReturnMsg>(str);
                        Logger.Info($"关闭问题sign {openid} {data["sign"].ToString()}");
                        Logger.Info($"关闭问题结果 {openid} {str}");
                        if (returndata.error == 0)
                        {
                            problem.LastModificationTime = DateTime.Now;
                            problem.State = (int)CYProblemState.Closed;
                            //创建成功后更新数据库和缓存
                            problem = await _cYProblemAppService.Update(problem);
                            await _cacheManager.GetCache(AppConsts.Cache_ChunyuProblemByOpenId).RemoveAsync(fan.OpenID);
                            return true;
                        }
                    }
                    else
                    {
                        Logger.Info($"关闭问题准备中 {openid}");
                        problem.LastModificationTime = DateTime.Now;
                        problem.State = (int)CYProblemState.Closed;
                        //创建成功后更新数据库和缓存
                        problem = await _cYProblemAppService.Update(problem);
                        await _cacheManager.GetCache(AppConsts.Cache_ChunyuProblemByOpenId).RemoveAsync(fan.OpenID);

                        var config = await GetCYConfig();
                        if (config != null)
                        {
                            if (!string.IsNullOrEmpty(config.CloseProblemText))
                            {
                                await SendCustomerText(fan.MpID, fan.OpenID, fan.Id, problem.Id, config.CloseProblemText);
                            }
                        }
                        return true;
                    }
                }
                else
                {
                    await _cacheManager.GetCache(AppConsts.Cache_ChunyuProblemByOpenId).RemoveAsync(fan.OpenID);
                }
            }
            return false;
        }
        #endregion
        #endregion

        #region 私有方法
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

        #region 登录春雨医生
        /// <summary>
        /// 登录春雨医生
        /// </summary>
        /// <param name="mpid"></param>
        /// <param name="openid"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<bool> LoginAsync(int mpid, string openid)
        {
            var faninfo = await GetFan(mpid, openid);
            if (faninfo == null)
                return false;
            WebClient wc = new WebClient();
            var data = new NameValueCollection();
            var atime = GetTimestamp(DateTime.Now);
            data.Add("user_id", faninfo.Id.ToString());
            data.Add("password", faninfo.Id.ToString());
            data.Add("partner", _chunYuService.ChunYuPartner);
            data.Add("atime", atime.ToString());
            data.Add("sign", GetChunYuSign(atime, faninfo.Id));
            var str = Encoding.UTF8.GetString(await wc.UploadValuesTaskAsync($"{_chunYuService.ChunYuBaseUrl}{_chunYuService.ChunYuLoginUrl}", data));
            var returndata = JsonConvert.DeserializeObject<Models.ChunYuModel.ChunYuReturnMsg>(str);
            return returndata.error == 0;
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

        #region 获取医生信息
        public async Task<CYDoctorDto> GetDoctor(DoctorInfo doctorinfo)
        {
            var doctor = await _cacheManager.GetCache(AppConsts.Cache_ChunyuDoctor).GetAsync(doctorinfo.id, async (key) =>
            {
                var doctordto = await _cYDoctorAppService.GetByCyId(doctorinfo.id);
                if (doctordto == null)
                {
                    doctordto = await _cYDoctorAppService.Create(new CYDoctorDto() {
                        Clinic = doctorinfo.clinic,
                        ClinicNO = doctorinfo.clinic_no,
                        CreationTime = DateTime.Now,
                        CYId = doctorinfo.id,
                        GoodAt = doctorinfo.good_at,
                        Hospital = doctorinfo.hospital,
                        HospitalGrand = doctorinfo.hospital_grade,
                        Image = doctorinfo.image,
                        LevelTitle = doctorinfo.level_title,
                        Name = doctorinfo.name,
                        Title = doctorinfo.title,
                    });
                }
                return doctordto;
            });

            if (doctor == null)
                return null;
            return doctor as CYDoctorDto;
        }
        #endregion

        #region 是否医生首次回答
        /// <summary>
        /// 是否医生首次回答
        /// </summary>
        /// <param name="problemid"></param>
        /// <returns></returns>
        private async Task<bool> HasDoctorReply(int problemid)
        {
            //获取用户未关闭的提问
            var result = await _cacheManager.GetCache(AppConsts.Cache_ChunyuProblemFirstContent).GetAsync(problemid.ToString(), async (key) =>
            {
                return await _cYProblemContentAppService.HasDoctorReply(problemid);
            });
            return Convert.ToBoolean(result);
        }
        #endregion

        #region 发送客服文本消息
        private async Task<string> SendCustomerText(int mpid, string openid,int problemid,int fanid, string text, string templete = null, DoctorInfo doctor = null)
        {
            var account = await _mpAccountAppService.GetCache(mpid);
            if (account != null)
            {
                var textinfo = GetCustomerText(text, templete, fanid, problemid, openid, doctor);
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

        #region 发送客服图片消息
        private async Task SendCustomerImage(int mpid, string openid, string mediaid)
        {
            var account = await _mpAccountAppService.GetCache(mpid);
            if (account != null)
            {
                //根据配置信息，调用客服接口，发送问题创建后的默认回复
                try
                {
                    await CustomApi.SendImageAsync(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret), openid, mediaid);
                }
                catch (Exception ex)
                {
                    Logger.Error(string.Format("发送MpID为{0}，openid为{1}的客服图片消息{2}报错", mpid, openid, mediaid), ex);
                    await CustomApi.SendImageAsync(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true), openid, mediaid);
                }
            }
        }
        #endregion

        #region 发送客服音频消息
        private async Task SendCustomerVoice(int mpid, string openid, string mediaid)
        {
            var account = await _mpAccountAppService.GetCache(mpid);
            if (account != null)
            {
                //根据配置信息，调用客服接口，发送问题创建后的默认回复
                try
                {
                    await CustomApi.SendVoiceAsync(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret), openid, mediaid);
                }
                catch (Exception ex)
                {
                    Logger.Error(string.Format("发送MpID为{0}，openid为{1}的客服音频消息{2}报错", mpid, openid, mediaid), ex);
                    await CustomApi.SendVoiceAsync(await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true), openid, mediaid);
                }
            }
        }
        #endregion

        #region 获取客服接口回答内容
        private string GetCustomerText(string content, string templete, int fanid, int problemid,string openid, DoctorInfo doctor = null)
        {
            if (string.IsNullOrEmpty(templete))
                return content;
            templete = templete.Replace("{content}", content);
            templete = templete.Replace("{fanid}", fanid.ToString());
            templete = templete.Replace("{openid}", openid);
            templete = templete.Replace("{problemid}", problemid.ToString());
            templete = templete.Replace("{WebSiteRootAddress}", _webUrlService.GetSiteRootAddress());
            if (doctor != null)
            {
                if (!string.IsNullOrEmpty(doctor.name))
                    templete = templete.Replace("{doctorname}", doctor.name);
                if (!string.IsNullOrEmpty(doctor.title))
                    templete = templete.Replace("{doctortitle}", doctor.title);
                if (!string.IsNullOrEmpty(doctor.level_title))
                    templete = templete.Replace("{level_title}", doctor.level_title);
                if (!string.IsNullOrEmpty(doctor.clinic))
                    templete = templete.Replace("{clinic}", doctor.clinic);
                if (!string.IsNullOrEmpty(doctor.clinic_no))
                    templete = templete.Replace("{clinic_no}", doctor.clinic_no);
                if (!string.IsNullOrEmpty(doctor.hospital))
                    templete = templete.Replace("{hospital}", doctor.hospital);
                if (!string.IsNullOrEmpty(doctor.hospital_grade))
                    templete = templete.Replace("{hospital_grade}", doctor.hospital_grade);
                if (!string.IsNullOrEmpty(doctor.good_at))
                    templete = templete.Replace("{good_at}", doctor.good_at);
            }
            return templete;
        }
        #endregion

        #region 微信临时素材
        /// <summary>
        /// 获取并下载微信临时素材
        /// </summary>
        /// <param name="mpid"></param>
        /// <param name="mediaid"></param>
        /// <returns></returns>
        private async Task<string> GetMedia(int mpid, string mediaid)
        {
            var account = await _mpAccountAppService.GetCache(mpid);
            if (account != null)
            {
                try
                {
                    return await _fileServer.Download($"https://api.weixin.qq.com/cgi-bin/media/get?access_token={await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret)}&media_id={mediaid}", type: AppConsts.FileType_WxMedia);
                }
                catch (Exception ex)
                {
                    Logger.Error(string.Format("下载MpID为{0}，media_id{1}的临时素材报错", mpid, mediaid), ex);
                    return await _fileServer.Download($"https://api.weixin.qq.com/cgi-bin/media/get?access_token={await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true)}&media_id={mediaid}", type: AppConsts.FileType_WxMedia);
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
        private async Task<string> UploadMedia(int mpid, string path, CYProblemContentType type)
        {

            var account = await _mpAccountAppService.Get(new EntityDto<int>() { Id = mpid });
            if (account != null)
            {
                var wxtype = type == CYProblemContentType.image ? UploadMediaFileType.image.ToString() : UploadMediaFileType.voice.ToString();
                var retString = "";
                try
                {
                    var url = string.Format("https://api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret), wxtype);
                    retString = await HttpHelper.HttpPost(url, path, timeOut: Config.TIME_OUT);
                }
                catch (Exception ex)
                {
                    Logger.Error(string.Format("上传MpID为{0}，地址{1}的临时素材报错 {2}", mpid, path, wxtype), ex);
                    var url = string.Format("https://api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", await _accessTokenContainer.TryGetAccessTokenAsync(account.AppId, account.AppSecret, true), wxtype);
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
        #endregion
    }
}
