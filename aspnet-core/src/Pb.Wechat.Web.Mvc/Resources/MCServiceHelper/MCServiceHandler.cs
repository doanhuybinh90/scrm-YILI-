using Abp.Runtime.Caching;
using Castle.Core.Logging;
using Newtonsoft.Json;
using Pb.Wechat.MpAccessTokenClib;
using Pb.Wechat.MpAccounts.Dto;
using Pb.Wechat.MpFans.Dto;
using Pb.Wechat.MpUserMembers.Dto;
using Pb.Wechat.Url;
using Pb.Wechat.Web.Models.MCServiceModel;
using Senparc.Weixin.MP.AdvancedAPIs;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using yilibabyUser;

namespace Pb.Wechat.Web.Resources.MCServiceHelper
{
    public class MCServiceHandler
    {
     
        private static string defaultUserName = "";
        private static string defaultUserPwd = "";
        private static string deviceCode = "";
        private static string yiliwechat = "";
        yilibabyWCIF.WCIFServiceSoapClient client = new yilibabyWCIF.WCIFServiceSoapClient(yilibabyWCIF.WCIFServiceSoapClient.EndpointConfiguration.WCIFServiceSoap);
        private string MamaBanToYun2OneMediaID = "";
        private string JNHToLongLineMediaID = "";
        private string wxOnlineUrl = "";
        private string auth2Url ="";
        private readonly IAccessTokenContainer _accessTokenContainer;
        private readonly ICacheManager _cacheManager;
        private readonly ILogger _logger;
        private readonly int mpid;
        private readonly MpAccountDto _account;
        private readonly IMatialFileService _matialFileService;
        private readonly IWebUrlService _webUrlService;
        private readonly IYiliBabyClubInterfaceService _yiliBabyClubInterfaceService;
        public MCServiceHandler(int mpId,
            MpAccountDto account,
            ICacheManager cacheManager,
            ILogger logger,
            IAccessTokenContainer accessTokenContainer
            , IMatialFileService matialFileService
            , IWebUrlService webUrlService
            , IYiliBabyClubInterfaceService yiliBabyClubInterfaceService)
        {
            _cacheManager = cacheManager;
            _logger = logger;
            mpid = mpId;
            _account = account;
            _accessTokenContainer = accessTokenContainer;
            _matialFileService = matialFileService;
            _webUrlService = webUrlService;
            _yiliBabyClubInterfaceService = yiliBabyClubInterfaceService;

            MamaBanToYun2OneMediaID = _matialFileService.MaMaBanToYun2One;
            JNHToLongLineMediaID = _matialFileService.JNHToLongLine;
            wxOnlineUrl = _webUrlService.GetSiteRootAddress()+_matialFileService.WxOnlineUrl;
            auth2Url = _webUrlService.GetSiteRootAddress() + _matialFileService.Auth2Url;
            
            defaultUserName = _yiliBabyClubInterfaceService.ApiUserName;
            defaultUserPwd = _yiliBabyClubInterfaceService.ApiPassword;
            deviceCode = _yiliBabyClubInterfaceService.ApiDeviceCode;
            yiliwechat = _yiliBabyClubInterfaceService.Yiliwechat;

        }

        public async Task SanCodePushMessageByActivity(int source, MpFanDto fan, MpUserMemberDto userMember, string openId, bool isSubscribe = true)
        {
            try
            {
                if (source > 110000)//网格化活动扫码签到（妈妈班/嘉年华）
                {

                    _logger.Info("进入网格化推送,sourceID:" + source);
                    string activityName = "本次";
                    int activityID = source - 110000;
                    _logger.Info("进入活动接口判定=================================================");
                    ActivityInfo activity = await GetActivity(activityID);

                    if (activity != null)
                    {
                       
                        _logger.Info("活动接口判定结果为：" + JsonConvert.SerializeObject(activity));
                        if (fan != null && fan.MemberID > 0)//已绑定用户
                        {
                            _logger.Info("开始获取会员authkey");
                            string authKey = await GetUserMemberAuthKey(userMember.MemeberUserName, userMember.MemberPassword);//获取会员AuthKey
                            _logger.Info("获取会员authkey成功：" + authKey);
                            _logger.Info("开始获取会员信息，调用美驰接口");
                            MemberTemp serviceMember = await GetServiceMember(authKey);
                            _logger.Info("被动接口-获取会员信息为：" + JsonConvert.SerializeObject(serviceMember));
                            int _crmid = Convert.ToInt32(userMember.CRMID);
                            if (activity.Classify == 1)//妈妈班  1
                            {
                                _logger.Info("妈妈班活动");
                                _logger.Info($"妈妈班活动会员信息：{JsonConvert.SerializeObject(serviceMember)}");
                                if (serviceMember != null && !string.IsNullOrWhiteSpace(serviceMember.ServiceCMClientCode))//已绑定门店
                                {
                                    _logger.Info("已绑定门店");
                                    if (serviceMember.InfoCollectActivity == 0)
                                    {
                                        _logger.Info("开始更新会员信息，调用美驰接口：活动id：" + activityID);
                                        serviceMember.InfoCollectActivity = activityID;
                                        await UpdateMemberInfoJson(authKey, serviceMember);
                                        _logger.Info("更新成功");
                                    }
                                    _logger.Info("开始活动签到");
                                    int ret = await JoinActivitySign(_crmid, activityID, 51, userMember);
                                    _logger.Info($"，签到活动：{activityID}，签到返回数据：{ret}");
                                    string msg = "恭喜您签到成功！";
                                    if (ret > 0)
                                    {
                                        msg = "恭喜您签到成功！";
                                    }
                                    else if (ret == 0)
                                    {
                                        msg = "您已经签到过本次活动了哦！";
                                    }
                                    _logger.Info(msg);

                                    if (!string.IsNullOrEmpty(serviceMember.ServicePromotorName))
                                    {
                                        //恭喜您签到成功！您的专属营养顾问是张三（华润万达广场店）联系电话：13312345678。
                                        msg += string.Format("您的专属营养顾问是{0}（{1}）联系电话：{2}。", serviceMember.ServicePromotorName, serviceMember.ServiceCMClientName, serviceMember.ServicePromotorMobile);
                                    }
                                    if (isSubscribe)
                                    {

                                        _logger.Info("开始推送消息，美驰接口推送");
                                        await SendMessage(openId, msg, false);
                                    }
                                    else
                                    {
                                        await SendCustomerMsg(openId, msg);

                                    }

                                    //孕晚期推送
                                    DateTime? babyBirthday = userMember.BabyBirthday;
                                    if (babyBirthday != null)
                                    {
                                        if (babyBirthday > DateTime.Now && babyBirthday < DateTime.Now.AddDays(105))
                                        {
                                            _logger.Info("开始固定推送");
                                            await SendCustomerImage(openId, MamaBanToYun2OneMediaID);
                                            //推送固定

                                        }
                                    }
                                }
                                else
                                {
                                    _logger.Info("未绑定门店");
                                    activityName = activity.Topic;
                                    _logger.Info("活动标题：" + activityName);
                                    //string _str = "欢迎参加" + activityName + "活动，完成签到后将有专属营养顾问为您服务，并有礼品相赠哦~~\n";
                                    string _str = "欢迎参加" + activityName + "活动，完成签到后将有专属营养顾问为您服务，并有礼品相赠哦~~请点此进行签到！";

                                    ///////////////////////////有疑问，需要更换地址
                                    var reurl = Base64Helper.EncodeBase64($"{wxOnlineUrl}?qd=inmm");
                                    string _url = $"{auth2Url}?token={_account.TaskAccessToken}&needRegister=1&reurl={reurl}&wghhd=inmm&ActivityId={activityID}&Classify={activity.Classify}&DefaultClient={activity.DefaultClient}&DefaultClientCode={activity.DefaultClientCode}&DefaultClientFullName={activity.DefaultClientFullName}&OfficialCity={activity.OfficialCity}&response_type=code&scope=snsapi_base&state=state#wechat_redirect";

                                    await SendLinkMessage(openId, _url, _str, false);
                                }
                            }
                            else //嘉年华  2
                            {
                                _logger.Info("嘉年华活动");

                                if (serviceMember != null && serviceMember.InfoCollectActivity == 0)
                                {
                                    _logger.Info("开始更新会员信息，调用美驰接口：活动id：" + activityID);
                                    serviceMember.InfoCollectActivity = activityID;
                                    await UpdateMemberInfoJson(authKey, serviceMember);
                                    _logger.Info("更新成功");
                                }
                                _logger.Info("开始活动签到");
                                int ret = await JoinActivitySign(_crmid, activityID, 51, userMember);
                                string msg = "恭喜您签到成功！";
                                if (ret > 0)
                                {
                                    msg = "恭喜您签到成功！";
                                }
                                else if (ret == 0)
                                {
                                    msg = "您已经签到过本次活动了哦！";
                                }
                                else if (ret == -11)
                                {
                                    msg = "您当前参与的活动尚未结束，无法参加其他嘉年华活动哦！";
                                }
                                else if (ret == -12)
                                {
                                    msg = "您本月参与嘉年华活动次数已达上限了哦！";
                                }
                                _logger.Info(msg);
                                if (isSubscribe)
                                {
                                    _logger.Info("开始推送消息，美驰接口推送");
                                    await SendMessage(openId, msg, false);
                                }
                                else
                                {
                                    await SendCustomerMsg(openId, msg);
                                    //wt.SimpleTextSender(requestMsg.FromUserName, msg);
                                }

                                if (ret > 0)//仅这对成功签到
                                {
                                    await SendCustomerImage(openId, JNHToLongLineMediaID);
                                    //wt.SendImageMsg(requestMsg.FromUserName, JNHToLongLineMediaID);
                                }
                            }
                        }
                        else//未绑定用户
                        {
                            _logger.Info("MCServiceHander活动接口--非会员入口");
                            _logger.Info("=====================================================================================================================================================================================================");
                            string _url = "";
                            _logger.Info($"活动类型：{activity.Classify}");

                            string _str = "请点此注册签到！";
                            if (activity.Classify == 1) //妈妈班  1
                            {
                                var reurl = Base64Helper.EncodeBase64($"{wxOnlineUrl}?qd=unmm");
                                _url = $"{auth2Url}?token={_account.TaskAccessToken}&needRegister=1&reurl={reurl}&wghhd=unmm&ActivityId={activityID}&Classify={activity.Classify}&DefaultClient={activity.DefaultClient}&DefaultClientCode={activity.DefaultClientCode}&DefaultClientFullName={activity.DefaultClientFullName}&OfficialCity={activity.OfficialCity}&response_type=code&scope=snsapi_base&state=state#wechat_redirect";
                                //欢迎参加朝阳妇幼保健院妈妈班（emp中录入的活动名称）活动，完成签到后将有专属营养顾问为您服务，并有礼品相赠哦~~请点此进行签到！
                                activityName = activity.Topic;
                                //_str = "欢迎参加" + activityName + "活动，完成签到后将有专属营养顾问为您服务，并有礼品相赠哦~~\n";
                                //_str += "<a href='" + _url + "'>请点此进行签到！</a>";
                                _str = "欢迎参加" + activityName + "活动，完成签到后将有专属营养顾问为您服务，并有礼品相赠哦~~请点此进行签到！";
                            }
                            else //嘉年华  2
                            {
                                var reurl = Base64Helper.EncodeBase64($"{wxOnlineUrl}?qd=injnh");
                                _url = $"{auth2Url}?token={_account.TaskAccessToken}&needRegister=1&reurl={reurl}&wghhd=injnh&ActivityId={activityID}&Classify={activity.Classify}&DefaultClient={activity.DefaultClient}&DefaultClientCode={activity.DefaultClientCode}&DefaultClientFullName={activity.DefaultClientFullName}&OfficialCity={activity.OfficialCity}&response_type=code&scope=snsapi_base&state=state#wechat_redirect";
                                //欢迎参加大润发金领冠嘉年华（emp中录入的活动名称）活动，完成签到后有机会获得礼品哦~~请点此进行签到！
                                activityName = activity.Topic;
                                //_str = "欢迎参加" + activityName + "活动，完成签到后有机会获得礼品哦~~\n";
                                //_str += "<a href='" + _url + "'>请点此进行签到！</a>";
                                _str = "欢迎参加" + activityName + "活动，完成签到后有机会获得礼品哦~~请点此进行签到！";

                                _logger.Info($"反推地址：{_url}");
                            }

                            await SendLinkMessage(openId, _url, _str, false);

                        }
                    }
                    else
                    {
                        _logger.Info("活动接口判定结果为空值");
                    }
                }
                else if (source > 100) //营养顾问帮注册扫码
                {
                    _logger.Info("进入营销顾问推送");
                    if (fan != null && fan.MemberID > 0)
                    {
                        _logger.Info("会员推送");
                        await SendCustomerMsg(openId, "亲爱的" + fan.NickName + "，您已经绑定会员啦，不要重复注册哦~");

                    }
                    else
                    {

                        try
                        {
                            _logger.Info("非会员推送，开始生成二维码");
                            string result = await client.GetPromotorInfoAsync(source);//根据顾问ID生成的二维码
                            _logger.Info("二维码内容：" + result);
                            if (!string.IsNullOrEmpty(result))
                            {
                                _logger.Info("有结果，结果为：" + result);
                                string[] _retlist = result.Split(new char[] { '|' });
                                string _assigntype = _retlist[2];
                                _logger.Info("_assigntype内容：" + _assigntype);
                                if (_assigntype == "2")
                                {
                                    _logger.Info("2的推送");
                                    string _str = "亲爱的会员，" + _retlist[1] + "营养顾问正在帮您注册/绑定会员，请继续扫描营养顾问APP“绑定码”或点此完善您的信息，以便获得六大会员专属服务！";
                                    var reurl = Base64Helper.EncodeBase64($"{wxOnlineUrl}?qd=app");
                                    string _url = $"{auth2Url}?token={_account.TaskAccessToken}&needRegister=1&reurl={reurl}&response_type=code&scope=snsapi_base&state=state#wechat_redirect";

                                    await SendLinkMessage(openId, _url, _str, false);
                                }
                                else if (_assigntype == "3")
                                {
                                    _logger.Info("3的推送");
                                    string _str = "亲爱的会员，营养代表(" + _retlist[1] + ")正在帮您注册/绑定会员，点此完善您的信息，以便获得六大会员专属服务！";
                                    var reurl = Base64Helper.EncodeBase64($"{wxOnlineUrl}?qd=app");
                                    string _url = $"{auth2Url}?token={_account.TaskAccessToken}&needRegister=1&reurl={reurl}&response_type=code&scope=snsapi_base&state=state#wechat_redirect";
                                    await SendLinkMessage(openId, _url, _str, false);
                                }
                            }
                            _logger.Info("取结果结束。");
                        }
                        catch (Exception eex)
                        {
                            _logger.Error($"错误原因：{eex.Message}，错误堆栈：{eex.StackTrace}");
                            throw eex;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"活动扫码/关注处理异常:错误信息:{ex.Message},错误堆栈：{ex.StackTrace}");
            }

        }

        #region 获取活动类型
        /// <summary>
        /// 获取活动类型
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public async Task<ActivityInfo> GetActivity(int activityid)
        {
            _logger.Info("活动id为：" + activityid);
            string cachekeyname_activity = "ActivityInfo_" + activityid.ToString();
            ActivityInfo activi = (ActivityInfo)await _cacheManager.GetCache(AppConsts.Cache_ActivityInfo).GetOrDefaultAsync(cachekeyname_activity);
            if (activi == null)
            {
                _logger.Info("没有缓存，继续进入：" + activityid);
                string jsonStr = string.Empty;
                string authKey = await GetDefaultAuthKey();//获取默认账户AuthKey
                _logger.Info("获取默认账户authkey：" + authKey);
                yilibabyActivity.ActivityServiceSoapClient activity = new yilibabyActivity.ActivityServiceSoapClient(yilibabyActivity.ActivityServiceSoapClient.EndpointConfiguration.ActivityServiceSoap12);
                {
                    _logger.Info($"WCF地址{activity.Endpoint.Address.ToString()}");


                    jsonStr = await activity.GetActivityInfoJsonAsync(authKey, activityid);
                    if (string.IsNullOrWhiteSpace(jsonStr))
                        jsonStr = activity.GetActivityInfoJsonAsync(authKey, activityid).Result;
                }

                if (!string.IsNullOrWhiteSpace(jsonStr) && jsonStr != "null")
                {
                    var aesResult = await GetAesKey();
                    _logger.Info("获取默认账户解码key：" + aesResult);
                    string plainJson = AesCryptoService.Decrypt(jsonStr, aesResult.CryptAESKey, aesResult.CryptAESIV);
                    _logger.Info("解码内容：" + plainJson);
                    activi = JsonConvert.DeserializeObject<ActivityInfo>(plainJson);
                    if (activi != null && activi.ID > 0)
                    {
                        _logger.Info("保存活动缓存");
                        //缓存数据对象
                        await _cacheManager.GetCache(AppConsts.Cache_ActivityInfo).SetAsync(cachekeyname_activity, activi);
                    }
                }
            }


            return activi;

        }
        #endregion

        #region 登录以及凭证获取
        /// <summary>
        /// 获取AES回执
        /// </summary>
        /// <returns></returns>
        private async Task<ApplyAESEncryptKeyResponse> GetAesKey(bool getnew = false)
        {
            var result = await _cacheManager.GetCache(AppConsts.Cache_AesKeyModal).GetOrDefaultAsync("AesKeyResult");
            if (result == null || getnew)
            {
                UserLoginSoapClient client = new UserLoginSoapClient(UserLoginSoapClient.EndpointConfiguration.UserLoginSoap);
                
                RSAParameters publicKey = RsaService.GetRsaPublicKey();

                ApplyAESEncryptKeyResponse AesResult = await client.ApplyAESEncryptKeyAsync(new ApplyAESEncryptKeyRequest { DeviceCode = deviceCode, Modulus = Convert.ToBase64String(publicKey.Modulus), Exponent = Convert.ToBase64String(publicKey.Exponent) });

                await _cacheManager.GetCache(AppConsts.Cache_AesKeyModal).SetAsync("AesKeyResult", AesResult);
                return AesResult;
            }
            else
            {
                ApplyAESEncryptKeyResponse AesResult = (ApplyAESEncryptKeyResponse)result;
                return AesResult;
            }
        }
        /// <summary>
        /// 获取默认账户AuthKey
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetDefaultAuthKey()
        {
            var userName = string.Empty;
            var userPwd = string.Empty;
            var AesResult = await GetAesKey();
            return await _cacheManager.GetCache(AppConsts.Cache_MCAuthKey).GetAsync<string, string>("DefaultMCAuthKey", async c =>
            {
                UserLoginSoapClient client = new UserLoginSoapClient(UserLoginSoapClient.EndpointConfiguration.UserLoginSoap);
                var EncryptPassword = "";
                try
                {
                    EncryptPassword = AesCryptoService.Encrypt(defaultUserPwd, AesResult.CryptAESKey, AesResult.CryptAESIV);
                }
                catch
                {
                    AesResult = await GetAesKey(true);
                    EncryptPassword = AesCryptoService.Encrypt(defaultUserPwd, AesResult.CryptAESKey, AesResult.CryptAESIV);
                }
                var result = await client.LoginExAsync(new LoginExRequest { UserName = defaultUserName, EncryptPassword = EncryptPassword, DeviceCode = deviceCode });
                return result.AuthKey;
            });
        }
        /// <summary>
        /// 获取会员AuthKey
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<string> GetUserMemberAuthKey(string userName, string password)
        {
            var AesResult = await GetAesKey();
            return (string)await _cacheManager.GetCache(AppConsts.Cache_MCAuthKey).GetAsync<string, string>($"{userName}.Activity", async c =>
            {
                UserLoginSoapClient client = new UserLoginSoapClient(UserLoginSoapClient.EndpointConfiguration.UserLoginSoap);
                var EncryptPassword = "";
                try
                {
                    EncryptPassword = AesCryptoService.Encrypt(password, AesResult.CryptAESKey, AesResult.CryptAESIV);
                }
                catch
                {
                    AesResult = await GetAesKey(true);
                    EncryptPassword = AesCryptoService.Encrypt(password, AesResult.CryptAESKey, AesResult.CryptAESIV);
                }
                //获取最新的AuthKey
                var result = await client.LoginExAsync(new LoginExRequest
                {
                    UserName = userName,
                    EncryptPassword = EncryptPassword,
                    DeviceCode = deviceCode
                });
                return result.AuthKey;
            });
        }
        #endregion

        #region 会员信息
        /// <summary>
        /// 获取会员信息
        /// </summary>
        /// <param name="authKey"></param>
        /// <returns></returns>
        public async Task<MemberTemp> GetServiceMember(string authKey)
        {
            string jsonStr = string.Empty;

            yilibabyMember.MemberServiceSoapClient client = new yilibabyMember.MemberServiceSoapClient(yilibabyMember.MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
            {
                jsonStr = await client.GetMyMemberInfoJsonAsync(authKey);
            }

            MemberTemp member = null;
            if (!string.IsNullOrWhiteSpace(jsonStr) && jsonStr != "null")
            {
                var aesResult = await GetAesKey();
                string plainJson = AesCryptoService.Decrypt(jsonStr, aesResult.CryptAESKey, aesResult.CryptAESIV);

                member = JsonConvert.DeserializeObject<MemberTemp>(plainJson);
            }

            return member;
        }
        /// <summary>
        /// 更新会员信息
        /// </summary>
        /// <param name="authKey"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public async Task<bool> UpdateMemberInfoJson(string authKey, MemberTemp member)
        {
            int result = -1;
            DateTime startTime = DateTime.Now;
            //string authKey = "";
            try
            {
                yilibabyMember.MemberServiceSoapClient client = new yilibabyMember.MemberServiceSoapClient(yilibabyMember.MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);

                var aesResult = await GetAesKey();
                string jsonStr = JsonConvert.SerializeObject(member);
                jsonStr = AesCryptoService.Encrypt(jsonStr, aesResult.CryptAESKey, aesResult.CryptAESIV);
                result = await client.UpdateMemberInfoJsonAsync(authKey, jsonStr);

                if (result >= 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"更新会员资料出错,错误信息：{ex.Message},错误堆栈：{ex.StackTrace}");

                return false;
            }
            finally
            {
                _logger.Info($"MemberService.UpdateMemberInfoJson信息：开始时间{startTime},终止时间：{DateTime.Now}，AuthKey：{authKey}，回复值：{result}");
                //LogInterfaceMan.AddLog("MemberService.UpdateMemberInfoJson", 0, startTime, DateTime.Now, "MemberName:" + ";Password:" + ";AuthKey:" + authKey, "", result.ToString(), "", "", result == 0 ? true : false);
            }
        }
        #endregion

        #region 消息推送
        /// <summary>
        /// 推送WCF消息
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="message"></param>
        /// <param name="callIF"></param>
        /// <returns></returns>
        private async Task<int> SendMessage(string openid, string message, bool callIF = true)
        {
            if (callIF)
            {

                return await client.SendMessageAsync(openid, "text", message, "1");
            }
            else
            {

                await SendCustomerMsg(openid, message);
                return 0;
            }
        }
        /// <summary>
        /// 推送客服消息
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendCustomerMsg(string openid, string message)
        {
            var access_token = await _accessTokenContainer.TryGetAccessTokenAsync(_account.AppId, _account.AppSecret);
            try
            {
                await CustomApi.SendTextAsync(access_token, openid, message);
            }
            catch
            {
                access_token = await _accessTokenContainer.TryGetAccessTokenAsync(_account.AppId, _account.AppSecret,true);
                await CustomApi.SendTextAsync(access_token, openid, message);
            }
           
        }
        /// <summary>
        /// 推送客服图片
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="mediaID"></param>
        /// <returns></returns>
        public async Task SendCustomerImage(string openid, string mediaID)
        {
            var access_token = await _accessTokenContainer.TryGetAccessTokenAsync(_account.AppId, _account.AppSecret);

           
            try
            {
                await CustomApi.SendImageAsync(access_token, openid, mediaID);
            }
            catch
            {
                access_token = await _accessTokenContainer.TryGetAccessTokenAsync(_account.AppId, _account.AppSecret, true);
                await CustomApi.SendImageAsync(access_token, openid, mediaID);
            }
        }
        /// <summary>
        /// 发送链接信息
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="message"></param>
        /// <param name="linkurl"></param>
        /// <param name="linkname"></param>
        /// <returns></returns>
        public async Task SendLinkMsg(string openid, string message, string linkurl, string linkname)
        {
            var _msg = message.Replace(linkname, "<a href='" + linkurl + "'>" + linkname + "</a>");
            _logger.Info("推送内容为：" + _msg);
            await SendCustomerMsg(openid, _msg);
        }

        private async Task<int> SendLinkMessage(string openid, string url, string message, bool callIF = true)
        {
            _logger.Info("进入链接推送");
            if (callIF)
            {
                _logger.Info("进入美池WEBSERVICE推送");
                yilibabyWCIF.WCIFServiceSoapClient client = new yilibabyWCIF.WCIFServiceSoapClient(yilibabyWCIF.WCIFServiceSoapClient.EndpointConfiguration.WCIFServiceSoap);
                return await client.SendMessageAsync(openid, "text", "<a href='" + url + "'>" + message + "</a>", "1");
            }
            else
            {
                _logger.Info("主动微信客服推送");
                await SendLinkMsg(openid, message, url, message);
                return 0;
            }
        }
        #endregion

        #region 活动签到
        /// <summary>
        /// 现场活动签到
        /// </summary>
        /// <param name="activityid"></param>
        /// <param name="clientid"></param>
        /// <param name="infoSource"></param>
        /// <returns></returns>
        public async Task<int> JoinActivitySign(int clientid, int activityid, int infoSource, MpUserMemberDto userMember = null)
        {
            int result = -1;
            DateTime startTime = DateTime.Now;
            string authKey = "";
            try
            {
                yilibabyMember.MemberServiceSoapClient client = new yilibabyMember.MemberServiceSoapClient(yilibabyMember.MemberServiceSoapClient.EndpointConfiguration.MemberServiceSoap);
                {
                    if (userMember != null)
                        authKey = await GetUserMemberAuthKey(userMember.MemeberUserName, userMember.MemberPassword);
                    else
                        authKey = await GetDefaultAuthKey();
                    result = await client.CustomerJoinActivitySignAsync(authKey, clientid, activityid, infoSource);

                }
                return result;
            }
            catch (Exception ex)
            {


                _logger.Error(ex.Message + ex.StackTrace);
                return result;
            }
            finally
            {
                _logger.Info("MemberService.JoinActivitySign-----------");
                //LogInterfaceMan.AddLog("MemberService.JoinActivitySign", 0, startTime, DateTime.Now, "MemberName:" + ";Password:" + ";AuthKey:" + authKey, "", result.ToString(), "", "", result == 0 ? true : false);
            }
        }
        #endregion
    }
}
