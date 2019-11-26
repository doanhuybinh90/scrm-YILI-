using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Pb.Wechat.Web.Kefu;
using Pb.Wechat.Web.Kefu.Controllers;
using Pb.Wechat.Web.Kefu.Helper;
using Pb.Wechat.Web.Kefu.Hubs;
using Pb.Wechat.Web.Kefu.Model;
using Pb.Wechat.Web.Kefu.Models;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.Media;
using Senparc.Weixin.MP.Entities;

namespace Pb.Wechat.Web.Kf.Controllers
{
    public class KfApiController : Controller
    {
        //private RedisHelper redisCache = RedisHelper.GetInstance(StaticObject.RedisServer, StaticObject.RedisPort ?? 0, StaticObject.RedisPassword, StaticObject.RedisDb ?? 0);
        //private WebCacheHelper memberCache = new WebCacheHelper();
        LogHelper log = new LogHelper();

        #region 聊天接口

        /// <summary>
        /// 1.	客服扫码登录
        /// </summary>
        /// <param name="loginToken"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ScanQr(string loginToken, string openId)
        {
            Response_IncreaseCommonReply response = new Response_IncreaseCommonReply();
            var hub = GlobalHost.ConnectionManager.GetHubContext<LoginHub>();
            try
            {
                log.Info($"开始记录日志,传入参数：openId：{openId}");

                IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
                response.state = 3;
                response.errMsg = "连接失败";
                //var connectId = memberCache.Get<string>(loginToken);
                var connectId = await StackExchangeRedisHelper.Get(loginToken, StaticObject.Cache_Kf_LoginToken);

                if (connectId != null)
                {
                    string sql = "SELECT * FROM CustomerServiceOnlines WHERE ISDELETED=0 AND OpenID='" + openId + "'";
                    var model = await db.FindOneAsync<CustomerServiceOnline>(sql, null, false);

                    if (model == null)
                    {
                        response.state = 1;
                        response.errMsg = "您未绑定客服，请先绑定";
                        await hub.Clients.Client(connectId).sendLoginResult(response.state, null);
                    }
                    else
                    {
                        var msgToken = Guid.NewGuid().ToString();
                        model.OnlineState = 1;
                        model.MessageToken = msgToken;

                        //IDBHelper db2 = new MssqlHelper(StaticObject.ConnectionStrings["Default"]);
                        //var account = await db2.FindOneAsync<MpAccountDto>($"SELECT * FROM MpAccounts WHERE ISDELETED=0 AND ID={model.MpID}", null, false);
                        //if (account != null)
                        //    model.MpAccountName = account.Name;

                        log.Info($"更新了登陆实例{JsonConvert.SerializeObject(model)}");

                        await db.ExcuteNonQueryAsync($"Update CustomerServiceOnlines SET OnlineState={(int)OnlineState.OnLine},MessageToken='{msgToken}' WHERE ID='{model.Id}'", null, false);

                        //memberCache.Set(msgToken, model.OpenID, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(StaticObject.CacheDictionary[StaticObject.Cache_Kf_MessageToken2CustomerOpenId]));

                        //memberCache.Set(model.OpenID, model, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(StaticObject.CacheDictionary[StaticObject.Cache_Kf_OpenId2Customer]));

                        //redisCache.RemoveValue(model.OpenID, StaticObject.Cache_OpenId2Customer);

                        log.Info($"开始更新cache{JsonConvert.SerializeObject(model)}");
                        await StackExchangeRedisHelper.Set(msgToken, StaticObject.Cache_Kf_MessageToken2CustomerOpenId, model.OpenID, StaticObject.CacheDictionary[StaticObject.Cache_Kf_MessageToken2CustomerOpenId]);
                        log.Info($"获取cache比对{await StackExchangeRedisHelper.Get(msgToken, StaticObject.Cache_Kf_MessageToken2CustomerOpenId)}");

                        log.Info($"开始更新cache{JsonConvert.SerializeObject(model)}");
                        await StackExchangeRedisHelper.Set(model.OpenID, StaticObject.Cache_Kf_OpenId2Customer, JsonConvert.SerializeObject(model), StaticObject.CacheDictionary[StaticObject.Cache_Kf_OpenId2Customer]);
                        log.Info($"获取cache2比对{await StackExchangeRedisHelper.Get(model.OpenID, StaticObject.Cache_Kf_OpenId2Customer)}");

                        await StackExchangeRedisHelper.Remove(model.OpenID, StaticObject.Cache_OpenId2Customer);
                        response.state = 0;
                        response.errMsg = "登陆成功";

                        await hub.Clients.Client(connectId).sendLoginResult(response.state, msgToken);

                    }
                }
                else
                {
                    response.state = 2;
                    response.errMsg = "二维码过期";
                }
                return Json(response);
            }
            catch (Exception ex)
            {
                response.state = 3;
                response.errMsg = ex.Message;
                log.Error($"ScanQr接口-接口报错:", ex);
                return Json(response);

            }



        }

        /// <summary>
        /// 2. 获取登录客服的基本信息
        /// </summary>
        /// <param name="messageToken">登录接口中获取到的消息令牌</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetCustomerInfo(string messageToken)
        {
            var response = new Response_GetCustomerInfo();
            try
            {
                response.state = 1;
                var model = await GetCustomer(messageToken);
                if (model != null)
                {

                    response.autoJoin = model.AutoJoin;
                    response.autoJoinCount = model.AutoJoinCount;
                    response.headImgUrl = model.KfHeadingUrl;
                    response.nickName = model.KfNick;
                    response.openId = model.OpenID;
                    response.autoJoinReply = model.AutoJoinReply;
                    response.autoJoinReplyText = model.AutoJoinReplyText;
                    response.autoLeaveReply = model.AutoLeaveReply;
                    response.autoLeaveReplyText = model.AutoLeaveReplyText;
                    response.state = 0;
                }
                return Json(response);
            }
            catch (Exception ex)
            {
                response.state = 2;
                log.Error($"GetCustomerInfo接口-接口报错:", ex);
                return Json(response);
            }

        }
        /// <summary>
        /// 3. 获取所有在线客服信息
        /// </summary>
        /// <param name="messageToken">登录接口中获取到的消息令牌</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetAllOnlineCustomerInfo(string messageToken)
        {
            var response = new Response_GetAllOnlineCustomerInfo();
            try
            {
                response.state = 1;
                if ((await GetCustomer(messageToken)) != null)
                {

                    IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
                    string sql = $"SELECT * FROM CustomerServiceOnlines WHERE ISDELETED=0 AND OnlineState={(int)OnlineState.OnLine} AND ConnectState={(int)ConnectState.Connect} AND kfType='YL'";
                    var models = await db.FindToListAsync<CustomerServiceOnline>(sql, null, false);
                    var list = JsonConvert.SerializeObject(models.Select(m => new { openId = m.OpenID, nickName = m.KfNick, headImgUrl = m.KfHeadingUrl }).ToList());
                    response.list = list;
                    response.state = 0;
                }
                return Json(response);
            }
            catch (Exception ex)
            {
                response.state = 2;
                log.Error($"GetAllOnlineCustomerInfo接口-接口报错:", ex);
                return Json(response);
            }

        }
        /// <summary>
        /// 4. 获取通用回复信息（分页查询，按使用频率倒序排列）
        /// </summary>
        /// <param name="messageToken">登录接口中获取到的消息令牌</param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetCommonReply(string messageToken, int pageSize, int pageIndex, string searchValue = null, string typeName = null, string msgType = null)
        {
            var response = new Response_GetCommonReply();
            try
            {
                response.state = 1;
                if ((await GetCustomer(messageToken)) != null)
                {
                    string where = "";
                    if (!string.IsNullOrWhiteSpace(typeName))
                        where += $" AND CHARINDEX('{typeName}',TypeName)>0";
                    if (!string.IsNullOrWhiteSpace(msgType))
                    {
                        if (msgType == MsgType.mpnews.ToString() || msgType == MsgType.mpmultinews.ToString())
                            where += $" AND (ReponseContentType={MsgTypeToInt(MsgType.mpmultinews.ToString())} OR ReponseContentType={MsgTypeToInt(MsgType.mpnews.ToString())})";
                        else
                            where += $" AND ReponseContentType={MsgTypeToInt(msgType)}";
                    }

                    if (!string.IsNullOrWhiteSpace(searchValue))
                        where += $" AND (CHARINDEX('{searchValue}',Title)>0 OR CHARINDEX('{searchValue}',ImageName)>0  OR CHARINDEX('{searchValue}',VoiceName)>0 OR  CHARINDEX('{searchValue}',ResponseText)>0)";

                    IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
                    string sql = string.Format($"SELECT TOP {pageSize} * FROM (SELECT row_number() over (order by UseCount DESC) as rownumber,* FROM CustomerServiceResponseTexts WHERE ISDELETED=0 AND ResponseType='common' {where}) a where rownumber > {pageSize * pageIndex} and rownumber < {pageSize * (pageIndex + 1)} order by UseCount desc ");
                    var models = await db.FindToListAsync<CustomerServiceResponseText>(sql, null, false);
                    List<CommonReplyOutput> list = new List<CommonReplyOutput>();
                    list.AddRange(await GetTextList(models.ToList()));
                    list.AddRange(await GetImageList(models.ToList()));
                    list.AddRange(await GetVideoList(models.ToList()));
                    list.AddRange(await GetVoiceList(models.ToList()));
                    list.AddRange(await GetArticleGroupList(models.ToList()));
                    list.AddRange(await GetArticleList(models.ToList()));

                    response.list = JsonConvert.SerializeObject(list);
                    //if (models != null)
                    //    response.list = JsonConvert.SerializeObject(models.Select(m =>
                    //    new { type = MsgTypeToString(m.ReponseContentType),
                    //        text = m.ResponseText,
                    //        previewImgUrl = m.PreviewImgUrl,
                    //        mediaId = m.MediaId,
                    //        typeId = m.TypeId,
                    //        typeName = m.TypeName,
                    //        title = m.ReponseContentType == 1 ? m.ImageName : (m.ReponseContentType == 2 ? m.VoiceName : (m.ReponseContentType == 3 ? m.Title : "")) }).ToList());
                    response.state = 0;
                }
                return Json(response);
            }
            catch (Exception ex)
            {
                response.state = 2;
                log.Error($"GetCommonReply接口-接口报错:", ex);
                return Json(response);
            }

        }


        /// <summary>
        /// 5. 获取个人回复信息（分页查询，按使用频率倒序排列)
        /// </summary>
        /// <param name="messageToken"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetPrivateReply(string messageToken, int pageSize, int pageIndex)
        {
            var response = new Response_GetPrivateReply();
            try
            {
                response.state = 1;
                var model = await GetCustomer(messageToken);
                if (model != null)
                {
                    IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
                    string sql = string.Format("SELECT TOP {0} * FROM (SELECT row_number() over (order by {3} DESC) as rownumber,* FROM CustomerServicePrivateResponseTexts WHERE ISDELETED=0 AND OpenID='{4}') a where rownumber > {1} and rownumber < {2} order by {3} desc ", pageSize, pageSize * pageIndex, pageSize * (pageIndex + 1), "CreationTime", model.OpenID);
                    var models = await db.FindToListAsync<CustomerServicePrivateResponseText>(sql, null, false);

                    var countSql = $"SELECT COUNT(1) Count FROM CustomerServicePrivateResponseTexts WHERE ISDELETED=0 AND OpenID='{model.OpenID}'";
                    var countModel = await db.FindOneAsync<CountModel>(countSql, null, false);
                    response.count = countModel.Count;
                    if (models != null && models.Count>0)
                        response.list = JsonConvert.SerializeObject(models.Select(m => new { type = m.ReponseContentType, text = m.ResponseText, mediaId = m.MediaId }).ToList());
                    response.state = 0;
                }
                return Json(response);
            }
            catch (Exception ex)
            {
                response.state = 2;
                log.Error($"GetPrivateReply接口-接口报错:", ex);
                return Json(response);
            }

        }
        /// <summary>
        /// 6.	添加个人回复信息
        /// </summary>
        /// <param name="messageToken"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> AddPrivateReply(string messageToken, string text)
        {
            var response = new Response_GetPrivateReply();
            try
            {
                response.state = 1;
                var model = await GetCustomer(messageToken);
                if (model != null)
                {
                    IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
                    string sql = string.Format(@"IF NOT EXISTS(SELECT 1 FROM CustomerServicePrivateResponseTexts WHERE OpenID='{2}' AND ResponseText='{5}') 
                    INSERT INTO CustomerServicePrivateResponseTexts (CreationTime,IsDeleted,LastModificationTime,MediaId,MpID,OpenID,ResponseType,ReponseContentType,ResponseText,UseCount) Values(GetDate(),0,GetDate(),'{0}',{1},'{2}','{3}','{4}','{5}',0)", Guid.NewGuid().ToString(), model.MpID, model.OpenID, "Private", "text", text);
                    if (await db.ExcuteNonQueryAsync(sql, null, false) > 0)
                    {
                        response.state = 0;
                    }
                    else
                        response.state = 2;
                }
                return Json(response);
            }
            catch (Exception ex)
            {
                response.state = 2;
                log.Error($"AddPrivateReply接口-接口报错:", ex);
                return Json(response);
            }

        }

        /// <summary>
        /// 7.	修改个人回复信息
        /// </summary>
        /// <param name="messageToken"></param>
        /// <param name="mediaId"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> UpdatePrivateReply(string messageToken, string mediaId, string text)
        {
            var response = new Response_GetPrivateReply();
            try
            {
                response.state = 1;
                var model = await GetCustomer(messageToken);
                if (model != null)
                {
                    IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
                    string sql = string.Format(@"UPDATE CustomerServicePrivateResponseTexts SET LastModificationTime=GETDATE(),ResponseText='{0}' WHERE MediaId='{1}'", text, mediaId);
                    if (await db.ExcuteNonQueryAsync(sql, null, false) > 0)
                    {
                        response.state = 0;
                    }
                    else
                        response.state = 2;
                }
                return Json(response);
            }
            catch (Exception ex)
            {
                response.state = 2;
                log.Error($"UpdatePrivateReply接口-接口报错:", ex);
                return Json(response);
            }

        }

        /// <summary>
        /// 8.	删除个人回复信息
        /// </summary>
        /// <param name="messageToken"></param>
        /// <param name="mediaId"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeletePrivateReply(string messageToken, string mediaId, string text)
        {
            var response = new Response_GetPrivateReply();
            try
            {
                response.state = 1;
                var model = await GetCustomer(messageToken);
                if (model != null)
                {
                    IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
                    string sql = string.Format(@"UPDATE CustomerServicePrivateResponseTexts SET LastModificationTime=GETDATE(),IsDeleted=1 WHERE MediaId='{1}'", text, mediaId);
                    if (await db.ExcuteNonQueryAsync(sql, null, false) > 0)
                    {
                        response.state = 0;
                    }
                    else
                        response.state = 2;
                }
                return Json(response);
            }
            catch (Exception ex)
            {
                response.state = 2;
                log.Error($"DeletePrivateReply接口-接口报错:", ex);
                return Json(response);
            }

        }

        /// <summary>
        /// 9.	设置自动接入信息
        /// </summary>
        /// <param name="messageToken"></param>
        /// <param name="autoJoin"></param>
        /// <param name="autoJoinCount"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SetAutoJoin(string messageToken, bool autoJoin, int autoJoinCount, bool autoJoinReply, string autoJoinReplyText)
        {
            var response = new Response_Base();
            try
            {
                response.state = 1;
                var model = await GetCustomer(messageToken);
                if (model != null)
                {
                    IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
                    string sql = string.Format("UPDATE CustomerServiceOnlines SET AutoJoin={1},AutoJoinCount={2},AutoJoinReply={3},AutoJoinReplyText='{4}' WHERE ID={0}", model.Id, autoJoin ? 1 : 0, autoJoinCount, autoJoinReply ? 1 : 0, autoJoinReplyText);
                    await db.ExcuteNonQueryAsync(sql, null, false);
                    model.AutoJoin = autoJoin;
                    model.AutoJoinCount = autoJoinCount;
                    model.AutoJoinReply = autoJoinReply;
                    model.AutoJoinReplyText = autoJoinReplyText;
                    //memberCache.Set(model.OpenID, model, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(StaticObject.CacheDictionary[StaticObject.Cache_Kf_OpenId2Customer]));
                    //redisCache.RemoveValue(model.OpenID, StaticObject.Cache_OpenId2Customer);

                    await StackExchangeRedisHelper.Set(model.OpenID, StaticObject.Cache_Kf_OpenId2Customer, JsonConvert.SerializeObject(model), StaticObject.CacheDictionary[StaticObject.Cache_Kf_OpenId2Customer]);
                    await StackExchangeRedisHelper.Remove(model.OpenID, StaticObject.Cache_OpenId2Customer);
                    response.state = 0;
                }
                return Json(response);
            }
            catch (Exception ex)
            {
                response.state = 2;
                log.Error($"SetAutoJoin接口-接口报错:", ex);
                return Json(response);
            }

        }
        /// <summary>
        /// 设置离开状态自动回复信息
        /// </summary>
        /// <param name="messageToken"></param>
        /// <param name="autoLeaveReply"></param>
        /// <param name="autoLeaveReplyText"></param>
        /// <returns></returns>
        public async Task<JsonResult> SetLeaveInfo(string messageToken, bool autoLeaveReply, string autoLeaveReplyText)
        {
            var response = new Response_Base();
            try
            {
                response.state = 1;
                var model = await GetCustomer(messageToken);
                if (model != null)
                {
                    IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
                    string sql = string.Format("UPDATE CustomerServiceOnlines SET AutoLeaveReply={1},AutoLeaveReplyText='{2}' WHERE ID={0}", model.Id, autoLeaveReply ? 1 : 0, autoLeaveReplyText);
                    await db.ExcuteNonQueryAsync(sql, null, false);

                    model.AutoLeaveReply = autoLeaveReply;
                    model.AutoLeaveReplyText = autoLeaveReplyText;
                    //memberCache.Set(model.OpenID, model, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(StaticObject.CacheDictionary[StaticObject.Cache_Kf_OpenId2Customer]));
                    //redisCache.RemoveValue(model.OpenID, StaticObject.Cache_OpenId2Customer);

                    await StackExchangeRedisHelper.Set(model.OpenID, StaticObject.Cache_Kf_OpenId2Customer, JsonConvert.SerializeObject(model), StaticObject.CacheDictionary[StaticObject.Cache_Kf_OpenId2Customer]);
                    await StackExchangeRedisHelper.Remove(model.OpenID, StaticObject.Cache_OpenId2Customer);
                    response.state = 0;
                }
                return Json(response);
            }
            catch (Exception ex)
            {
                response.state = 2;
                log.Error($"SetLeaveInfo接口-接口报错:", ex);
                return Json(response);
            }

        }

        /// <summary>
        /// 10.	发送文本消息
        /// </summary>
        /// <param name="messageToken"></param>
        /// <param name="msg"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SendTextMsg(string messageToken, string msg, string openId)
        {
            Response_IncreaseCommonReply result = new Response_IncreaseCommonReply();
            try
            {
                CustomerServiceConversationDto conversation = null;
                var model = await GetCustomer(messageToken);
                if (model != null)
                {
                    conversation = await GetUserLastConversation(openId);
                    if (conversation == null)
                    {
                        result.state = 4;
                        result.errMsg = "openId无效";
                    }
                    else
                    {
                        if (conversation.CustomerId == model.Id)
                        {
                            var defaultCus = await GetDefaultWxKf();
                            var sendMsg = await SendCustomerTextMsg(openId, msg, defaultCus.KfAccount);
                            if (sendMsg.errcode == ReturnCode.请求成功)
                                result.state = 0;
                            else
                            {
                                result.state = 2;
                                result.errMsg = sendMsg.errmsg;
                            }
                        }
                        else
                        {
                            result.state = 4;
                            result.errMsg = "openId无效";
                        }
                    }
                }
                else
                    result.state = 1;

                if (result.state == 0)
                {
                    var fanEntity = await GetFanDto(openId);
                    await SaveMsgIntoDb(model.Id, fanEntity.Id, "", "", fanEntity.MpID, msg, (int)MsgType.text, (int)Sender.customer, conversation.Id);
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                result.state = 2;
                log.Error($"SendTextMsg接口-接口报错:", ex);
                return Json(result);
            }

        }

        /// <summary>
        /// 11.	发送图片消息
        /// </summary>
        /// <param name="messageToken"></param>
        /// <param name="msg"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SendImageMsg(string messageToken, string openId)
        {

            Response_SendImg result = new Response_SendImg();
            try
            {

                var file = Request.Files[0];
                string mediaId = string.Empty;
                string url = string.Empty;
                if (file != null)
                {
                    //sw.WriteLine("文件名称：" + file.FileName + "，文件后缀：" + file.FileName.Substring(file.FileName.LastIndexOf(".") + 1) + "，文件类型：" + file.ContentType);
                    var defaultCus = await GetDefaultWxKf();
                    UploadTemporaryMediaResult uploadResult = null;
                    var model = await GetCustomer(messageToken);
                    CustomerServiceConversationDto conversation = null;
                    if (model != null)
                    {
                        conversation = await GetUserLastConversation(openId);
                        if (conversation == null)
                        {
                            result.state = 4;
                            result.errMsg = "openId无效";
                        }
                        else
                        {
                            if (conversation.CustomerId == model.Id)
                            {
                                FileUploadHelper fileupload = new FileUploadHelper();
                                try
                                {
                                    //sw.WriteLine("尝试上传1");
                                    url = await fileupload.UploadFile(file.InputStream, file.FileName.Substring(file.FileName.LastIndexOf(".") + 1), file.ContentType);
                                    //sw.WriteLine("上传成功，url为" + url);
                                    //sw.WriteLine("获取微信token");
                                    var data = await GetAccessToken(StaticObject.Token);
                                    uploadResult = JsonConvert.DeserializeObject<UploadTemporaryMediaResult>(await HttpHelper.HttpPost($"https://api.weixin.qq.com/cgi-bin/media/upload?access_token={data.access_token}&type=image", url, file.FileName));
                                    mediaId = uploadResult.media_id;
                                    if (!string.IsNullOrEmpty(mediaId))
                                    {
                                        var sendResult = await WxCustomerApi.SendImageAsync(data.access_token, openId, mediaId, kfAccount: defaultCus.KfAccount);
                                        if (sendResult.errcode == ReturnCode.请求成功)
                                        {
                                            result.state = 0;
                                            result.src = url;
                                            result.errMsg = "发送成功";
                                        }
                                        else
                                        {
                                            result.state = 2;
                                            result.errMsg = sendResult.errmsg;
                                        }
                                    }
                                    else
                                    {
                                        result.state = 2;
                                        result.errMsg = uploadResult.errmsg;
                                    }

                                }
                                catch (Exception ex)
                                {
                                    //sw.WriteLine("上传1错误：" + ex.Message);
                                    try
                                    {
                                        url = await fileupload.UploadFile(file.InputStream, file.FileName.Substring(file.FileName.LastIndexOf(".") + 1), file.ContentType);
                                        var data = await GetAccessToken(StaticObject.Token, 1);
                                        uploadResult = JsonConvert.DeserializeObject<UploadTemporaryMediaResult>(await HttpHelper.HttpPost($"https://api.weixin.qq.com/cgi-bin/media/upload?access_token={data.access_token}&type=image", url, file.FileName));
                                        mediaId = uploadResult.media_id;
                                        if (!string.IsNullOrEmpty(mediaId))
                                        {
                                            var sendResult = await WxCustomerApi.SendImageAsync(data.access_token, openId, mediaId, kfAccount: defaultCus.KfAccount);
                                            if (sendResult.errcode == ReturnCode.请求成功)
                                            {
                                                result.state = 0;
                                                result.src = url;
                                                result.errMsg = "发送成功";
                                            }
                                            else
                                            {
                                                result.state = 2;
                                                result.errMsg = sendResult.errmsg;
                                            }
                                        }
                                        else
                                        {
                                            result.state = 2;
                                            result.errMsg = uploadResult.errmsg;
                                        }

                                    }
                                    catch (Exception e)
                                    {
                                        //sw.WriteLine("上传2错误：" + e.Message);
                                        result.state = 2;
                                        result.errMsg = e.Message;
                                    }
                                }
                            }
                            else
                            {
                                result.state = 4;
                                result.errMsg = "openId无效";
                            }
                        }
                    }
                    else
                        result.state = 1;

                    if (result.state == 0)
                    {
                        var fanEntity = await GetFanDto(openId);
                        await SaveMsgIntoDb(model.Id, fanEntity.Id, uploadResult != null ? uploadResult.media_id : null, "", fanEntity.MpID, url, (int)MsgType.image, (int)Sender.customer, conversation.Id);
                        //await SaveMsgIntoDb(25, 2399445, uploadResult != null ? uploadResult.media_id : null, "", 1, file.FileName, (int)MsgType.image, (int)Sender.Customer);
                    }
                }
                else
                {
                    result.state = 2;
                    result.errMsg = "文件不存在";
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                result.state = 2;
                log.Error($"SendImageMsg接口-接口报错:", ex);
                return Json(result);
            }



        }

        /// <summary>
        /// 12.	发送通用回复消息
        /// </summary>
        /// <param name="messageToken"></param>
        /// <param name="mediaId"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SendCommonReply(string messageToken, string mediaId, string openId)
        {
            Response_IncreaseCommonReply result = new Response_IncreaseCommonReply();
            try
            {
                CustomerServiceConversationDto conversation = null;
                CustomerServiceResponseText commonRpy = null;
                var model = await GetCustomer(messageToken);
                if (model != null)
                {
                    conversation = await GetUserLastConversation(openId);
                    if (conversation == null)
                    {
                        result.state = 4;
                        result.errMsg = "openId无效";
                    }
                    else
                    {
                        if (conversation.CustomerId == model.Id)
                        {
                            IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
                            string sql = $"SELECT TOP 1 * FROM CustomerServiceResponseTexts WHERE ISDELETED=0 AND ResponseType='common' AND MEDIAID='{mediaId}'";
                            commonRpy = await db.FindOneAsync<CustomerServiceResponseText>(sql, null, false);
                            if (commonRpy != null)
                            {
                                var defaultCus = await GetDefaultWxKf();
                                try
                                {
                                    var data = await GetAccessToken(StaticObject.Token);
                                    var str = await SendCommonMsg(commonRpy.ReponseContentType, openId, commonRpy, data.access_token);
                                    result.state = 0;
                                    if (result.state == 0)
                                    {
                                        var fanEntity = await GetFanDto(openId);
                                        await SaveMsgIntoDb(model.Id, fanEntity.Id, commonRpy.MediaId, commonRpy.PreviewImgUrl, fanEntity.MpID, str, commonRpy.ReponseContentType, (int)Sender.customer,conversation.Id);


                                    }
                                }
                                catch (Exception ex)
                                {

                                    try
                                    {
                                        var data = await GetAccessToken(StaticObject.Token, 1);
                                        var str = await SendCommonMsg(commonRpy.ReponseContentType, openId, commonRpy, data.access_token);
                                        result.state = 0;
                                        if (result.state == 0)
                                        {
                                            var fanEntity = await GetFanDto(openId);
                                            await SaveMsgIntoDb(model.Id, fanEntity.Id, commonRpy.MediaId, commonRpy.PreviewImgUrl, fanEntity.MpID, str, commonRpy.ReponseContentType, (int)Sender.customer,conversation.Id);


                                        }
                                    }
                                    catch (Exception e)
                                    {

                                        result.state = 2;
                                        result.errMsg = e.Message;
                                    }
                                }
                            }
                            else
                                result.state = 1;
                        }
                        else
                        {
                            result.state = 4;
                            result.errMsg = "openId无效";
                        }

                    }
                }
                else
                    result.state = 2;



                return Json(result);
            }
            catch (Exception ex)
            {
                result.state = 2;
                log.Error($"SendCommonReply接口-接口报错:", ex);
                return Json(result);
            }

        }

        /// <summary>
        /// 13.	增加通用回复消息使用次数
        /// </summary>
        /// <param name="messageToken"></param>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> IncreaseCommonReply(string messageToken, string mediaId)
        {
            var response = new Response_IncreaseCommonReply();
            try
            {
                response.state = 1;
                if ((await GetCustomer(messageToken)) != null)
                {
                    IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
                    string sql = string.Format("UPDATE CustomerServiceResponseTexts SET UseCount=UseCount+1 WHERE MediaId='{0}'", mediaId);
                    var result = await db.ExcuteNonQueryAsync(sql, null, false);
                    if (result <= 0)
                        response.state = 2;
                    else
                        response.state = 0;
                }

                if (response.state == 0)
                    response.errMsg = "执行成功";
                else if (response.state == 1)
                    response.errMsg = "令牌无效";
                else if (response.state == 2)
                    response.errMsg = "资源id无效";
                else
                    response.errMsg = "未知错误";
                return Json(response);
            }
            catch (Exception ex)
            {
                response.state = 2;
                log.Error($"IncreaseCommonReply接口-接口报错:", ex);
                return Json(response);
            }

        }

        /// <summary>
        /// 14.	将会话转给其他客服
        /// </summary>
        /// <param name="messageToken"></param>
        /// <param name="customerOpenId"></param>
        /// <param name="userOpenId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> TransferToOther(string messageToken, string customerOpenId, string userOpenId)
        {
            var response = new Response_IncreaseCommonReply();
            try
            {
                log.Error($"TransferToOther接口-开始转接客服");
                response.state = 1;
                response.errMsg = "令牌无效";
                try
                {
                    var customer = await GetCustomer(messageToken);
                    log.Error($"TransferToOther接口-获取客服信息：{JsonConvert.SerializeObject(customer)}");
                    if (customer != null)
                    {
                        var fans = await GetFanDto(userOpenId);
                        log.Error($"TransferToOther接口-获取粉丝信息：{JsonConvert.SerializeObject(fans)}");
                        if (fans == null)
                        {
                            response.state = 4;
                            response.errMsg = "用户openId无效";
                        }
                        else
                        {
                            log.Error($"TransferToOther接口-新客服OpenID：{customerOpenId}");
                            var aa = await StackExchangeRedisHelper.Get(customerOpenId, StaticObject.Cache_Kf_OpenId2Customer);
                            log.Error($"TransferToOther接口-新客服字符串：{aa}");
                            var newCustomer = JsonConvert.DeserializeObject<CustomerServiceOnline>(await StackExchangeRedisHelper.Get(customerOpenId, StaticObject.Cache_Kf_OpenId2Customer));

                            log.Error($"TransferToOther接口-获取转接的客服信息：{JsonConvert.SerializeObject(newCustomer)}");
                            if (newCustomer == null)
                            {
                                IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
                                var data = db.FindOne<CustomerServiceOnline>($"SELECT TOP 1 * FROM CustomerServiceOnlines WHERE ISDELETED=0 AND OpenID='{customerOpenId}'", null, false);
                                if (data == null)
                                {
                                    response.state = 3;
                                    response.errMsg = "客服openId无效";
                                }
                                else
                                {
                                    if (data.ConnectState == (int)ConnectState.UnConnect || data.OnlineState == (int)OnlineState.Leave || data.OnlineState == (int)OnlineState.Quit)
                                    {
                                        response.state = 2;
                                        response.errMsg = "客服不在线";
                                    }
                                    else
                                    {
                                        log.Error($"TransferToOther接口-开始正式转接");
                                        //开始转接
                                        newCustomer = await GetCustomer(data.MessageToken);
                                        var conversation = await SetUserLastConversationToNewCustomer(userOpenId, newCustomer);


                                        var hub = GlobalHost.ConnectionManager.GetHubContext<MessageHub>();
                                        log.Error($"TransferToOther接口-开始推送客服getMessage消息：新客服connectId【{newCustomer.ConnectId}】,fansOpenID【{fans.OpenID}】,NickName【{fans.NickName}】,转接类型【1】");
                                        await hub.Clients.Client(newCustomer.ConnectId).getMessage(fans.OpenID, fans.NickName, fans.HeadImgUrl, (int)CustomerServiceReplyType.Transfer, MsgType.text.ToString(), "");

                                        response.state = 0;
                                        response.errMsg = "执行成功";
                                    }
                                }
                            }
                            else
                            {
                               
                                //开始转接
                                var conversation = await SetUserLastConversationToNewCustomer(userOpenId, newCustomer);
                                var hub = GlobalHost.ConnectionManager.GetHubContext<MessageHub>();
                                await hub.Clients.Client(newCustomer.ConnectId).getMessage(fans.OpenID, fans.NickName, fans.HeadImgUrl, (int)CustomerServiceReplyType.Transfer, MsgType.text.ToString(), "");
                                response.state = 0;
                                response.errMsg = "执行成功";
                            }

                        }

                    }
                }
                catch (Exception ex)
                {

                    response.state = 5;
                    response.errMsg = ex.Message;
                }
                return Json(response);
            }
            catch (Exception ex)
            {
                response.state = 2;
                log.Error($"TransferToOther接口-接口报错:", ex);
                return Json(response);
            }

        }

        /// <summary>
        /// 15.	获取已接入用户信息
        /// </summary>
        /// <param name="messageToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetConnectedUser(string messageToken)
        {
            var response = new Response_GetAllOnlineCustomerInfo();
            try
            {
                response.state = 1;
                var customer = await GetCustomer(messageToken);
                if (customer != null)
                {
                    IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
                    string sql = $"SELECT * FROM CustomerServiceConversations WHERE State={(int)CustomerServiceConversationState.Asking} and CustomerId={customer.Id}";
                    var models = await db.FindToListAsync<CustomerServiceConversationDto>(sql, null, false);
                    if (models != null)
                    {
                        response.list = JsonConvert.SerializeObject(models.Select(m => new { openId = m.FanOpenId, nickName = m.NickName, headImgUrl = m.HeadImgUrl }).ToList());

                    }
                    response.state = 0;
                }
                return Json(response);
            }
            catch (Exception ex)
            {
                response.state = 2;
                log.Error($"GetConnectedUser接口-接口报错:", ex);
                return Json(response);
            }

        }

        /// <summary>
        /// 15.	获取待接入用户信息（分页查询，按开始等待时间顺序排列
        /// </summary>
        /// <param name="messageToken"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetWaitUser(string messageToken, int pageSize, int pageIndex)
        {
            var response = new Response_GetAllOnlineCustomerInfo();

            try
            {

                response.state = 1;
                var customer = await GetCustomer(messageToken);
                //log.Error($"GetWaitUser接口-客服实例:{JsonConvert.SerializeObject(customer)}");
                if (customer != null)
                {

                    IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
                    string sql = $"SELECT TOP {pageSize} * FROM (SELECT row_number() over (order by CreationTime) as rownumber,* FROM CustomerServiceConversations a WHERE State={(int)CustomerServiceConversationState.Wait} AND EXISTS(	SELECT * FROM (SELECT FanId,MAX(CreationTime) maxdate FROM dbo.CustomerServiceConversations b GROUP BY FanId ) b WHERE b.FanId=a.FanId AND a.CreationTime=b.maxdate)) a where rownumber > {pageSize * pageIndex} and rownumber < {pageSize * (pageIndex + 1)} order by CreationTime ";
                    var models = await db.FindToListAsync<CustomerServiceConversationDto>(sql, null, false);
                    //log.Error($"GetWaitUser接口-待接入用户数据:{JsonConvert.SerializeObject(models)}");
                    if (models != null)
                    {
                        response.list = JsonConvert.SerializeObject(models.Select(m => new { openId = m.FanOpenId, nickName = m.NickName, headImgUrl = m.HeadImgUrl, time = m.CreationTime.ToString("yyyy-MM-dd HH:mm:ss") }).ToList());

                    }
                    sql = $"SELECT count(*) FROM CustomerServiceConversations a WHERE State={(int)CustomerServiceConversationState.Wait} AND EXISTS(	SELECT * FROM (SELECT FanId,MAX(CreationTime) maxdate FROM dbo.CustomerServiceConversations b GROUP BY FanId ) b WHERE b.FanId=a.FanId AND a.CreationTime=b.maxdate)";
                    var count = await db.ExecuteScalarAsync(sql, null, false);
                    //log.Error($"GetWaitUser接口-待接入用户数量数据:{count}");
                    response.count = Convert.ToInt32(count);
                    response.state = 0;
                }
                return Json(response);
            }
            catch (Exception ex)
            {
                response.state = 2;
                log.Error($"GetWaitUser接口-接口报错:", ex);
                return Json(response);
            }


        }

        /// <summary>
        /// 16.	手动接入等待的用户
        /// </summary>
        /// <param name="messageToken"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ConnectUser(string messageToken, string openId)
        {
            var response = new Response_IncreaseCommonReply();
            try
            {
                response.state = 1;
                response.errMsg = "令牌无效";
                var customer = await GetCustomer(messageToken);
                if (customer != null)
                {
                    var conversation = await GetUserLastConversation(openId);
                    if (conversation == null)
                    {
                        response.state = 4;
                        response.errMsg = "openId无效";
                    }
                    else
                    {

                        if (conversation.State == (int)CustomerServiceConversationState.Wait)
                        {
                            IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
                            await db.ExcuteNonQueryAsync($"UPDATE CustomerServiceConversations SET CustomerId={customer.Id},CustomerOpenId='{customer.OpenID}',State={(int)CustomerServiceConversationState.Asking},StartTalkTime=GETDATE() WHERE ID={conversation.Id}", null, false);

                            conversation.CustomerId = customer.Id;
                            conversation.CustomerOpenId = customer.OpenID;
                            conversation.State = (int)CustomerServiceConversationState.Asking;
                            conversation.StartTalkTime = DateTime.Now;
                            IDBHelper db2 = new MssqlHelper(StaticObject.ConnectionStrings["Default"]);
                            await db2.ExcuteNonQueryAsync($"UPDATE MpFans SET LastCustomerServiceId={customer.Id},LastCustomerServiceOpenId='{customer.OpenID}' WHERE OPENID='{conversation.FanOpenId}'", null, false);

                            //memberCache.Set("Conversation_" + openId, conversation, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(StaticObject.CacheDictionary[StaticObject.Cache_Kf_FanOpenId2Conversation]));
                            //redisCache.RemoveValue(openId, StaticObject.Cache_FanOpenId2Conversation);

                            await StackExchangeRedisHelper.Set(openId, StaticObject.Cache_Kf_FanOpenId2Conversation, JsonConvert.SerializeObject(conversation), StaticObject.CacheDictionary[StaticObject.Cache_Kf_FanOpenId2Conversation]);
                            await StackExchangeRedisHelper.Remove(openId, StaticObject.Cache_FanOpenId2Conversation);
                            await SetUnConnectNotice();
                            response.state = 0;
                            response.errMsg = "执行成功";
                        }
                        else
                        {
                            if (conversation.State == (int)CustomerServiceConversationState.Asking && conversation.CustomerOpenId != customer.OpenID)
                            {
                                response.state = 3;
                                response.errMsg = "openId正在被其他客服接入";
                            }
                            else
                            {
                                response.state = 2;
                                response.errMsg = "openId不在等待列表中";
                            }
                        }

                    }
                }
                return Json(response);
            }
            catch (Exception ex)
            {
                response.state = 2;
                log.Error($"ConnectUser接口-接口报错:", ex);
                return Json(response);
            }

        }


        /// <summary>
        /// 16.	手动接入等待的用户
        /// </summary>
        /// <param name="messageToken"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> StopConnectUser(string messageToken, string openId)
        {
            var response = new Response_IncreaseCommonReply();
            try
            {
                response.state = 1;
                response.errMsg = "令牌无效";
                var customer = await GetCustomer(messageToken);
                if (customer != null)
                {
                    var conversation = await GetUserLastConversation(openId);
                    if (conversation == null)
                    {
                        response.state = 4;
                        response.errMsg = "openId无效";
                    }
                    else
                    {

                        if (conversation.State == (int)CustomerServiceConversationState.Asking && conversation.CustomerOpenId == customer.OpenID)
                        {
                            IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
                            await db.ExcuteNonQueryAsync($"UPDATE CustomerServiceConversations SET CustomerId={customer.Id},CustomerOpenId='{customer.OpenID}',State={(int)CustomerServiceConversationState.Closed},EndTalkTime=GETDATE() WHERE ID={conversation.Id}", null, false);

                            conversation.CustomerId = customer.Id;
                            conversation.CustomerOpenId = customer.OpenID;
                            conversation.State = (int)CustomerServiceConversationState.Closed;
                            conversation.EndTalkTime = DateTime.Now;
                            //memberCache.Set("Conversation_" + openId, conversation, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(StaticObject.CacheDictionary[StaticObject.Cache_Kf_FanOpenId2Conversation]));
                            //redisCache.RemoveValue(openId, StaticObject.Cache_FanOpenId2Conversation);
                            var defaultCus = await GetDefaultWxKf();
                            await SendCustomerTextMsg(openId, "您的会话已被客服关闭。", defaultCus.KfAccount);

                            await StackExchangeRedisHelper.Remove(openId, StaticObject.Cache_Kf_FanOpenId2Conversation);
                            await StackExchangeRedisHelper.Remove(openId, StaticObject.Cache_FanOpenId2Conversation);

                            await SetUnConnectNotice();

                            response.state = 0;
                            response.errMsg = "执行成功";
                        }
                        else
                        {
                            if (conversation.State == (int)CustomerServiceConversationState.Asking && conversation.CustomerOpenId != customer.OpenID)
                            {
                                response.state = 3;
                                response.errMsg = "openId正在被其他客服接入";
                            }
                            else
                            {
                                response.state = 2;
                                response.errMsg = "openId不在会话中";
                            }
                        }

                    }
                }
                return Json(response);
            }
            catch (Exception ex)
            {
                response.state = 2;
                log.Error($"StopConnectUser接口-接口报错:", ex);
                return Json(response);
            }

        }

        /// <summary>
        /// 17.	历史聊天记录（分页查询，按发送时间倒序排列）
        /// </summary>
        /// <param name="messageToken"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> GetMsgRecord(string messageToken, int pageSize, long? startId, string openId, DateTime? startTime, DateTime? endTime, string content)
        {
            var response = new Response_GetAllOnlineCustomerInfo();
            try
            {
                response.state = 1;
                var customer = await GetCustomer(messageToken);
                if (customer != null)
                {
                    var user = await GetFanDto(openId);
                    if (user != null)
                    {

                        IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
                        var extsearch = "";
                        if (startId != null)
                            extsearch += $" AND ID<{startId.Value}";
                        if (startTime != null)
                            extsearch += $" AND CreationTime >='{new DateTime(startTime.Value.Year, startTime.Value.Month, startTime.Value.Day, 0, 0, 0)}'";
                        if (endTime != null)
                            extsearch += $" AND CreationTime <='{new DateTime(endTime.Value.Year, endTime.Value.Month, endTime.Value.Day, 23, 59, 59)}'";
                        if (!string.IsNullOrWhiteSpace(content))
                            extsearch += $" AND CHARINDEX('{content}',MsgContent)>0";
                        string sql = $"SELECT TOP {pageSize} * FROM  CustomerServiceConversationMsgs WHERE FanId={user.Id}{extsearch} order by CreationTime desc ";
                        var models = await db.FindToListAsync<CustomerServiceConversationMsgDto>(sql, null, false);
                        if(models==null || models.Count<=0)
                        {
                            response.list = null;
                            response.state = 0;
                        }
                        else
                        {
                            string kfSql = $"SELECT * FROM CustomerServiceOnlines WHERE id IN ({string.Join(",", models.Select(m => m.CustomerId).ToList())})";
                            var customers = await db.FindToListAsync<CustomerServiceOnline>(kfSql, null, false);
                            if (models != null)
                            {
                                List<ConversationMsgHistory> list = new List<ConversationMsgHistory>();
                                foreach (var item in models)
                                {
                                    ConversationMsgHistory m = new ConversationMsgHistory();
                                    m.Id = item.Id;
                                    m.msgContent = item.MsgContent;
                                    m.msgType = MsgTypeToString(item.MsgType);
                                    m.senderType = SenderToString(item.Sender);
                                    m.sendTime = item.CreationTime.ToString("yyyy-MM-dd HH:mm:ss");
                                    if (m.senderType == Sender.customer.ToString())
                                    {
                                        var cusId = Convert.ToInt32(item.CustomerId);
                                        var em = customers.Where(t => t.Id == cusId).FirstOrDefault();
                                        if (em != null)
                                        {
                                            m.customerOpenId = em.OpenID;
                                            m.customerNickName = em.KfNick;
                                            m.customerHeadImgUrl = em.KfHeadingUrl;
                                        }
                                    }
                                    list.Add(m);
                                }
                                response.list = JsonConvert.SerializeObject(list);
                                //response.list = JsonConvert.SerializeObject(models.Select(m => new { senderType = SenderToString(m.Sender), Id = m.Id, msgType = MsgTypeToString(m.MsgType), msgContent = m.MsgContent, sendTime = m.CreationTime.ToString("yyyy-MM-dd HH:mm:ss") }).ToList());

                            }

                            response.state = 0;
                        }
                       
                    }
                }
                return Json(response);
            }
            catch (Exception ex)
            {
                response.state = 2;
                log.Error($"GetMsgRecord接口-接口报错:", ex);
                return Json(response);
            }

        }

        /// <summary>
        /// 获取所有类型列表
        /// </summary>
        /// <param name="messageToken"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetCommonReplyType(string messageToken)
        {
            var response = new Response_GetAllOnlineCustomerInfo();
            try
            {
                response.state = 1;
                var customer = await GetCustomer(messageToken);
                if (customer != null)
                {
                    IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
                    string sql = $"SELECT * FROM CustomerServiceResponseTypes WHERE ISDELETED=0";
                    var models = await db.FindToListAsync<CommonReplyType>(sql, null, false);
                    if (models != null)
                    {
                        response.list = JsonConvert.SerializeObject(models);

                    }
                    response.state = 0;
                }
                return Json(response);
            }
            catch (Exception ex)
            {
                response.state = 2;
                log.Error($"GetCommonReplyType接口-接口报错:", ex);
                return Json(response);
            }

        }

        /// <summary>
        /// 顾客提问
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Ask(string messageToken, string fanOpenId, string fanNickName, string fanHeadImgUrl, int replyType, string msgType, string msgContent)
        {
            try
            {
                var customer = await GetCustomer(messageToken);
                if (customer != null)
                {
                    var hub = GlobalHost.ConnectionManager.GetHubContext<MessageHub>();
                    await hub.Clients.Client(customer.ConnectId).getMessage(fanOpenId, fanNickName, fanHeadImgUrl, replyType, msgType, msgContent);
                    return "发送成功";
                }
                return "令牌无效";
            }
            catch (Exception ex)
            {
                log.Error($"Ask接口-接口报错:", ex);
                return ex.Message;
            }
        }

        /// <summary>
        /// 通知未接入用户数量
        /// </summary>
        /// <returns></returns>
        public async Task SetUnConnectNotice()
        {
            try
            {
                var hub = GlobalHost.ConnectionManager.GetHubContext<MessageHub>();
                IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
                var result = await db.FindOneAsync<CountModel>("SELECT Count(1) Count FROM CustomerServiceConversations a WHERE  EXISTS(	SELECT * FROM (SELECT FanId,MAX(CreationTime) maxdate FROM dbo.CustomerServiceConversations b GROUP BY FanId ) b WHERE b.FanId=a.FanId AND a.CreationTime=b.maxdate) AND State=" + (int)CustomerServiceConversationState.Wait, null, false);
                var userCount = result.Count;
                await hub.Clients.All.unConnectedNotice(userCount);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        /// <summary>
        /// 通知自动结束会话
        /// </summary>
        /// <returns></returns>
        public async Task AutoStopConversation()
        {
            var conversationId = Request.Form["conversationId"].ToString();
            var FansClose = Request.Form["FansClose"].ToString();
            string sql = $"SELECT * FROM dbo.CustomerServiceConversations WHERE Id ={conversationId}";
            IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
            var conList = await db.FindToListAsync<CustomerServiceConversationDto>(sql, null, false);
            var hub = GlobalHost.ConnectionManager.GetHubContext<MessageHub>();
            var defaultCus = await GetDefaultWxKf();
            foreach (var item in conList)
            {
                var customer = await GetCustomerByOpenId(item.CustomerOpenId);
                if (customer != null)
                    await hub.Clients.Client(customer.ConnectId).autoStopConversation(item.FanOpenId);
                if (FansClose == "1")
                    await SendCustomerTextMsg(item.FanOpenId, "您的会话已关闭。", defaultCus.KfAccount);
                else
                    await SendCustomerTextMsg(item.FanOpenId, "您已经很久没有和客服对话了，当前会话已关闭。", defaultCus.KfAccount);
                await StackExchangeRedisHelper.Remove(item.FanOpenId, StaticObject.Cache_Kf_FanOpenId2Conversation);
                await StackExchangeRedisHelper.Remove(item.FanOpenId, StaticObject.Cache_FanOpenId2Conversation);
            }
            await SetUnConnectNotice();
        }

        /// <summary>
        /// 获取当前客服今日接待和回复数
        /// </summary>
        /// <param name="messageToken"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetCustomerServiceLog(string messageToken)
        {
            var response = new Response_CustomerServiceLog();
            try
            {
                response.state = 1;
                var customer = await GetCustomer(messageToken);
                if (customer != null)
                {
                    IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
                    string sql = $"SELECT COUNT(DISTINCT FanId) TodayServiceCount,COUNT(1) TodayReplyMsgCount FROM dbo.CustomerServiceConversationMsgs WHERE MpID={customer.MpID} AND CustomerId={customer.Id} AND Sender={(int)Sender.customer} AND YEAR(CreationTime)=YEAR(GETDATE()) AND MONTH(CreationTime)=MONTH(GETDATE()) AND DAY(CreationTime)=DAY(GETDATE())";
                    var models = await db.FindOneAsync<CustomerServiceLog>(sql, null, false);
                    if (models != null)
                    {
                        response.TodayReplyMsgCount = models.TodayReplyMsgCount;
                        response.TodayServiceCount = models.TodayServiceCount;
                    }
                    response.state = 0;
                }
                return Json(response);
            }
            catch (Exception ex)
            {
                log.Error($"GetCustomerServiceLog接口-接口报错:", ex);
                response.state = 2;
                return Json(response);
            }

        }

        /// <summary>
        /// 获取聊天会话列表（去重）
        /// </summary>
        /// <param name="messageToken"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetDateRangeConversationList(string messageToken, DateTime? startDate = null)
        {
            var response = new Response_GetAllOnlineCustomerInfo();
            try
            {
                response.state = 1;
                var customer = await GetCustomer(messageToken);
                if (customer != null)
                {
                    var startTime = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0)).ToString("yyyy-MM-dd HH:mm:ss");
                    if (startDate != null)
                        startTime = (new DateTime(startDate.Value.Year, startDate.Value.Month, startDate.Value.Day, 0, 0, 0)).ToString("yyyy-MM-dd HH:mm:ss");
                    var endTime = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59)).ToString("yyyy-MM-dd HH:mm:ss");
                    IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
                    string sql = $"SELECT * FROM CustomerServiceConversations WHERE ID IN (SELECT MAX(Id) maxid FROM dbo.CustomerServiceConversations WHERE CreationTime >= '{startTime}' AND CreationTime<= '{endTime}' AND CustomerOpenId = '{customer.OpenID}' AND MpID = {customer.MpID} GROUP BY FanId ) ORDER BY State ASC,CreationTime DESC";
                    var result = await db.FindToListAsync<TodayConversation>(sql, null, false);
                    if (result != null)
                    {
                        response.list = JsonConvert.SerializeObject(result);
                        response.count = result.Count;
                        response.state = 0;
                    }


                }
                return Json(response);
            }
            catch (Exception ex)
            {
                response.state = 2;
                log.Error($"GetTodayConversationList接口-接口报错:", ex);
                return Json(response);
            }
        }
        /// <summary>
        /// 评价会话
        /// </summary>
        /// <param name="conversationId"></param>
        /// <param name="conversationScore"></param>
        /// <returns></returns>
        public async Task<JsonResult> SetConversationScore(int conversationId, int conversationScore)
        {
            var response = new Response_IncreaseCommonReply();
            try
            {
                IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
                await db.ExcuteNonQueryAsync($"UPDATE CustomerServiceConversations SET ConversationScore={conversationScore} WHERE ID={conversationId}", null, false);
                response.state = 0;
                response.errMsg = "操作成功";
                return Json(response);
            }
            catch (Exception ex)
            {
                response.state = 2;
                response.errMsg = ex.Message;
                log.Error($"SetConversationScore接口-接口报错:", ex);
                return Json(response);
            }
        }

        /// <summary>
        /// 模糊查询通用回复和自定义回复内容
        /// </summary>
        /// <param name="messageToken"></param>
        /// <param name="searchValue"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public async Task<JsonResult> SearchQuicklyReply(string messageToken, string searchValue, int pageSize, int pageIndex)
        {
            var response = new Response_GetAllOnlineCustomerInfo();
            try
            {
                response.state = 1;
                var customer = await GetCustomer(messageToken);
                if (customer != null)
                {
                    IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
                    string sql = $"SELECT TOP {pageSize} * FROM (SELECT ROW_NUMBER() OVER(ORDER BY UseCount DESC) AS rowNumber,* FROM(SELECT CASE WHEN ReponseContentType = 0 THEN 'text' WHEN ReponseContentType = 1 THEN 'image' WHEN ReponseContentType = 2 THEN 'voice' WHEN ReponseContentType = 3 THEN 'video' WHEN ReponseContentType = 4 THEN 'mpnews' ELSE '' END Type, ResponseText Text, PreviewImgUrl, mediaId, TypeId, TypeName, CASE WHEN ReponseContentType = 1 THEN ImageName WHEN ReponseContentType = 2 THEN VoiceName WHEN ReponseContentType = 3 THEN Title ELSE '' END Title, 'common' ReplyType, UseCount FROM dbo.CustomerServiceResponseTexts WHERE IsDeleted = 0 AND ResponseType = 'common' AND((CHARINDEX('{searchValue}', Title) > 0  AND ReponseContentType = 3) OR(CHARINDEX('{searchValue}', Description) > 0  AND ReponseContentType = 3) OR(CHARINDEX('{searchValue}', ResponseText) > 0 AND ReponseContentType = 0) OR(CHARINDEX('{searchValue}', ImageName) > 0 AND ReponseContentType = 1) OR(CHARINDEX('{searchValue}', VoiceName) > 0 AND ReponseContentType = 2) OR CHARINDEX('{searchValue}', TypeName) > 0) AND MpID = {customer.MpID} UNION ALL SELECT ReponseContentType Type, ResponseText Text,'' PreviewImgUrl,MediaId,0 TypeId,'' TypeName,'' Title,'private' ReplyType,0 UseCount FROM dbo.CustomerServicePrivateResponseTexts WHERE CHARINDEX('{searchValue}', ResponseText) > 0 AND OpenID = '{customer.OpenID}' AND IsDeleted = 0 AND MpID = {customer.MpID} ) T ) a WHERE a.rowNumber > {pageSize * pageIndex} AND a.rowNumber < {pageSize * (pageIndex + 1)}";
                    var result = await db.FindToListAsync<SearchQuickReply>(sql, null, false);
                    if (result != null)
                    {
                        response.list = JsonConvert.SerializeObject(result);
                        response.count = result.Count;
                    }

                    response.state = 0;
                }
                return Json(response);
            }
            catch (Exception ex)
            {
                response.state = 2;
                log.Error($"SearchQuicklyReply接口-接口报错:", ex);
                return Json(response);
            }

        }
        /// <summary>
        /// 获取个人历史统计信息
        /// </summary>
        /// <param name="messageToken"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetSelfCustomerReport(string messageToken, string startDate, string endDate)
        {
            Response_SelfCustomerReport response = new Response_SelfCustomerReport();
            try
            {
                response.state = 1;
                var customer = await GetCustomer(messageToken);
                if (customer != null)
                {
                    IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
                    var _beginDate = Convert.ToDateTime(startDate);
                    var _endDate = Convert.ToDateTime(endDate);
                    var mBeginDate = new DateTime(_beginDate.Year, _beginDate.Month, _beginDate.Day, 0, 0, 0);
                    var mEndDate = new DateTime(_endDate.Year, _endDate.Month, _endDate.Day, 23, 59, 59);
                    var dataList = await db.FindToListAsync<CustomerDailyReportDto>($"SELECT * FROM CustomerServiceReports WHERE ReportDate>='{mBeginDate.ToString("yyyy-MM-dd HH:mm:ss")}' AND ReportDate<='{mEndDate.ToString("yyyy-MM-dd HH:mm:ss")}' AND CustomerId={customer.Id}", null, false);
                    CustomerSelfReport report = new CustomerSelfReport();
                    if (dataList!=null && dataList.Count>0)
                    {
                        report.TotalMsgCount = dataList.Sum(m => m.ServiceMsgCount);
                        report.TotalOnLineTime = Convert.ToDouble(dataList.Sum(m => m.OnlineTime));
                        report.TotalReceiveCount = dataList.Sum(m => m.ReceiveCount);
                        report.TotalServiceCount = dataList.Sum(m => m.ServiceCount);
                        var totalScoreCount = dataList.Sum(m => m.ScoreCount);
                        if (totalScoreCount != 0)
                            report.TotalAvgScore = Math.Round(Convert.ToDouble(dataList.Sum(m => m.TotalScore) / totalScoreCount), 2);
                        else
                            report.TotalAvgScore = 0;
                        report.DailyAvgScore = Math.Round(Convert.ToDouble(dataList.Average(m => m.AvgScore)), 2);
                        report.DailyMsgCount = Math.Round(Convert.ToDouble(dataList.Average(m => m.ServiceMsgCount)), 2);
                        report.DailyOnLineTime = Math.Round(Convert.ToDouble(dataList.Average(m => m.OnlineTime)), 2);
                        report.DailyReceiveCount = Math.Round(Convert.ToDouble(dataList.Average(m => m.ReceiveCount)), 2);
                        report.DailyServiceCount = Math.Round(Convert.ToDouble(dataList.Average(m => m.ServiceCount)), 2);
                        report.list = dataList.Select(m => new CustomerSelfReportDetail
                        {
                            AvgScore = m.AvgScore,
                            Date = m.ReportDate.ToString("yyyy-MM-dd"),
                            MsgCount = m.ServiceMsgCount,
                            OnLineTime = m.OnlineTime,
                            ReceiveCount = m.ReceiveCount,
                            ServiceCount = m.ServiceCount
                        }).ToList();

                    }


                    //var startTime = Convert.ToDateTime(startDate).ToString("yyyy-MM-dd HH:mm:ss");
                    //var endTime = Convert.ToDateTime(endDate).ToString("yyyy-MM-dd HH:mm:ss");

                    //string msgSql = $"SELECT * FROM CustomerServiceConversationMsgs WHERE CreationTime>='{startTime}' AND CreationTime<='{endTime}' AND Sender={(int)CustomerServiceMsgSender.customer} AND MpID={customer.MpID} AND CustomerId={customer.Id}";
                    //var msgList = await db.FindToListAsync<CustomerServiceConversationMsgDto>(msgSql, null, false);


                    //string conSql = $"SELECT * FROM CustomerServiceConversations WHERE CustomerId={customer.Id} AND MpID={customer.MpID} AND CreationTime>='{startTime}' AND CreationTime<='{endTime}'";
                    //var conList = await db.FindToListAsync<CustomerServiceConversationDto>(conSql, null, false);

                    //string inoutLogSql = $"SELECT * FROM CustomerInOutLogs WHERE CustomerId={customer.Id} AND CreationTime>='{startTime}' AND CreationTime<='{endTime}'";
                    //var inoutList = await db.FindToListAsync<LogInout>(inoutLogSql, null, false);




                    //List<CustomerSelfReportDetail> dList = new List<CustomerSelfReportDetail>();

                    //var days = Convert.ToDateTime(endDate).Subtract(Convert.ToDateTime(startDate)).Days;
                    //for (int i = 0; i < days; i++)
                    //{
                    //    CustomerSelfReportDetail item = new CustomerSelfReportDetail();
                    //    var date = Convert.ToDateTime(startDate).AddDays(i);
                    //    item.Date = date.ToString("yyyy-MM-dd");
                    //    var _startTime = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                    //    var _endTime = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
                    //    item.MsgCount = msgList.Where(m => m.CreationTime >= _startTime && m.CreationTime <= _endTime).Count();
                    //    item.ServiceCount = conList.Where(m => m.CreationTime >= _startTime && m.CreationTime <= _endTime).Count();
                    //    item.ReceiveCount = conList.Where(m => m.CreationTime >= _startTime && m.CreationTime <= _endTime).Select(m => m.FanId).Distinct().Count();
                    //    var source = conList.Where(m => m.CreationTime >= _startTime && m.CreationTime <= _endTime && m.ConversationScore > -1).ToList();
                    //    if (source.Count > 0)
                    //        item.AvgScore = source.Average(m => m.ConversationScore);

                    //    var inoutDetails = inoutList.Where(m => m.CreationTime >= _startTime && m.CreationTime <= _endTime).OrderBy(m => m.CreationTime).ToList();
                    //    double mins = 0;
                    //    int selCount = 0;
                    //    if (inoutDetails.Count % 2 != 0)
                    //        selCount = inoutDetails.Count - 1;
                    //    DateTime? _sD = null;
                    //    DateTime? _eD = null;
                    //    for (int j = 0; j < selCount; j++)
                    //    {
                    //        if (j % 2 == 0)
                    //            _sD = inoutDetails[j].CreationTime;
                    //        else
                    //        {
                    //            _eD = inoutDetails[j].CreationTime;
                    //            mins = _eD.Value.Subtract(_sD.Value).TotalMinutes;
                    //            _sD = null;
                    //            _eD = null;
                    //        }
                    //    }
                    //    item.OnLineTime = Math.Round(Convert.ToDouble(mins / 60), 2);

                    //    dList.Add(item);

                    //}

                    //report.list = dList;
                    //if (dList.Count > 0)
                    //{
                    //    report.DailyAvgScore = dList.Average(m => m.AvgScore);
                    //    report.DailyMsgCount = dList.Average(m => m.MsgCount);
                    //    report.DailyOnLineTime = dList.Average(m => m.OnLineTime);
                    //    report.DailyReceiveCount = dList.Average(m => m.ReceiveCount);
                    //    report.DailyServiceCount = dList.Average(m => m.ServiceCount);
                    //    report.TotalMsgCount = dList.Sum(m => m.MsgCount);
                    //    report.TotalOnLineTime = dList.Sum(m => m.OnLineTime);
                    //    report.TotalReceiveCount = dList.Sum(m => m.ReceiveCount);
                    //    report.TotalServiceCount = dList.Sum(m => m.ServiceCount);
                    //}

                    //var tCount = conList.Where(m => m.ConversationScore > -1).ToList();
                    //if (tCount.Count > 0)
                    //    report.TotalAvgScore = tCount.Average(m => m.ConversationScore);

                    response.NickName = customer.KfNick;
                    response.reportData = report;
                    response.state = 0;
                }
                return Json(response);
            }
            catch (Exception ex)
            {

                response.state = 2;
                log.Error($"GetSelfCustomerReport接口-接口报错:", ex);
                return Json(response);
            }

        }

        #region 私有方法
        private async Task<WxJsonResult> SendCustomerTextMsg(string openId,string msg,string KfAccount="") {
            try
            {
                var data = await GetAccessToken(StaticObject.Token);
                return await CustomApi.SendTextAsync(data.access_token, openId, msg, kfAccount: KfAccount);
            }
            catch
            {

                try
                {
                    var data = await GetAccessToken(StaticObject.Token, 1);
                    return await CustomApi.SendTextAsync(data.access_token, openId, msg, kfAccount: KfAccount);
                }
                catch (Exception e)
                {
                    return new WxJsonResult()
                    {
                        errcode = ReturnCode.页面路径错误,
                        errmsg = e.Message
                    };
                }
            }
        }

        #region 通用回复内容获取
        public async Task<List<CommonReplyOutput>> GetTextList(List<CustomerServiceResponseText> sources)
        {
            var imageType = (int)CustomerServiceMsgType.text;
            var martialIds = sources.Where(m => m.ReponseContentType == imageType).Select(m => m.Id).ToList();
            if (martialIds != null && martialIds.Count > 0)
            {
                IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
                string sql = $"SELECT mediaId,ResponseText text,'text' type,typeId,typeName From CustomerServiceResponseTexts WHERE ID IN ({string.Join(",", martialIds)})";
                var result = await db.FindToListAsync<CommonReplyOutput>(sql, null, false);
                return result.ToList();
            }
            return new List<CommonReplyOutput>();
        }
        private async Task<List<CommonReplyOutput>> GetImageList(List<CustomerServiceResponseText> sources)
        {
            var imageType = (int)CustomerServiceMsgType.image;
            var martialIds = sources.Where(m => m.ReponseContentType == imageType).Select(m => m.MartialId).ToList();
            if (martialIds != null && martialIds.Count > 0)
            {
                IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
                string sql = $"SELECT mediaId,'image' type,FilePathOrUrl previewImgUrl,typeId,typeName,name title From CustomerMediaImages WHERE ID IN ({string.Join(",", martialIds)})";
                var result = await db.FindToListAsync<CommonReplyOutput>(sql, null, false);
                return result.ToList();
            }
            return new List<CommonReplyOutput>();
        }
        private async Task<List<CommonReplyOutput>> GetVoiceList(List<CustomerServiceResponseText> sources)
        {
            var imageType = (int)CustomerServiceMsgType.voice;
            var martialIds = sources.Where(m => m.ReponseContentType == imageType).Select(m => m.MartialId).ToList();
            if (martialIds != null && martialIds.Count > 0)
            {
                IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
                string sql = $"SELECT mediaId,'voice' type,FilePathOrUrl previewImgUrl,typeId,typeName, title,description From CustomerMediaVoices WHERE ID IN ({string.Join(",", martialIds)})";
                var result = await db.FindToListAsync<CommonReplyOutput>(sql, null, false);
                return result.ToList();
            }
            return new List<CommonReplyOutput>();
        }
        private async Task<List<CommonReplyOutput>> GetVideoList(List<CustomerServiceResponseText> sources)
        {
            var imageType = (int)CustomerServiceMsgType.video;
            var martialIds = sources.Where(m => m.ReponseContentType == imageType).Select(m => m.MartialId).ToList();
            if (martialIds != null && martialIds.Count > 0)
            {
                IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
                string sql = $"SELECT mediaId,'video' type,FilePathOrUrl previewImgUrl,typeId,typeName, title,description From CustomerMediaVideos WHERE ID IN ({string.Join(",", martialIds)})";
                var result = await db.FindToListAsync<CommonReplyOutput>(sql, null, false);
                return result.ToList();
            }
            return new List<CommonReplyOutput>();
        }
        private async Task<List<CommonReplyOutput>> GetArticleList(List<CustomerServiceResponseText> sources)
        {
            var imageType = (int)CustomerServiceMsgType.mpnews;
            var martialIds = sources.Where(m => m.ReponseContentType == imageType).Select(m => m.MartialId).ToList();
            if (martialIds != null && martialIds.Count > 0)
            {
                IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
                string sql = $"SELECT mediaId,'mpnews' type,FilePathOrUrl previewImgUrl,typeId,typeName, title,description,aurl linkUrl From CustomerArticles WHERE ID IN ({string.Join(",", martialIds)})";
                var result = await db.FindToListAsync<CommonReplyOutput>(sql, null, false);
                return result.ToList();
            }
            return new List<CommonReplyOutput>();
        }
        private async Task<List<CommonReplyOutput>> GetArticleGroupList(List<CustomerServiceResponseText> sources)
        {
            var imageType = (int)CustomerServiceMsgType.mpmultinews;
            var martialIds = sources.Where(m => m.ReponseContentType == imageType).Select(m => m.MartialId).ToList();
            if (martialIds != null && martialIds.Count > 0)
            {
                List<CommonReplyOutput> list = new List<CommonReplyOutput>();
                IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
                string sql = $"SELECT Id,mediaId,'mpmultinews' type,typeId,typeName,name title From CustomerArticleGroups WHERE ID IN ({string.Join(",", martialIds)})";
                var data = await db.FindToListAsync<ExtraArticleGroup>(sql, null, false);
                foreach (var item in data)
                {
                    var model = new CommonReplyOutput();
                    model.title = item.title;
                    model.mediaId = item.mediaId;
                    model.type = CustomerServiceMsgType.mpmultinews.ToString();
                    model.typeId = item.typeId;
                    model.typeName = item.typeName;
                    string dSql = $"SELECT CustomerArticles.title,CustomerArticles.description,CustomerArticles.FilePathOrUrl previewImgUrl,CustomerArticles.AUrl linkUrl FROM dbo.CustomerArticles JOIN dbo.CustomerArticleGroupItems ON CustomerArticleGroupItems.ArticleID = CustomerArticles.Id WHERE GROUPID={item.Id} ORDER BY SortIndex";
                    var ddata = await db.FindToListAsync<ArticleGroup>(dSql, null, false);
                    model.articleGroups = ddata.ToList();
                    list.Add(model);
                }
                return list;
            }
            return new List<CommonReplyOutput>();
        }
        #endregion

        /// <summary>
        /// 检测messageToken并获取客服信息
        /// </summary>
        /// <param name="messageToken"></param>
        /// <returns></returns>
        private async Task<CustomerServiceOnline> GetCustomer(string messageToken)
        {

            if (!string.IsNullOrEmpty(messageToken))
            {
                //获取messageToken对应的客服
                //var openid = memberCache.Get<string>(messageToken);
                var openid = await StackExchangeRedisHelper.Get(messageToken, StaticObject.Cache_Kf_MessageToken2CustomerOpenId);
                var customer = await GetCustomerByOpenId(openid);
                if (customer != null && customer.MessageToken == messageToken)
                    return customer;
            }
            return null;
        }
        /// <summary>
        /// 检测messageToken并获取客服信息
        /// </summary>
        /// <param name="messageToken"></param>
        /// <returns></returns>
        private async Task<CustomerServiceOnline> GetCustomerByOpenId(string openId)
        {
            if (!string.IsNullOrEmpty(openId))
            {
                //检查messageToken是否过期(客服1天之内可能会多次登录，识别messagetoken是否为客服最后一次登录生成)
                //var newCustomer = memberCache.Get<CustomerServiceOnline>(openid);
                var newCustomerStr = await StackExchangeRedisHelper.Get(openId, StaticObject.Cache_Kf_OpenId2Customer);
                CustomerServiceOnline newCustomer = null;
                if (!string.IsNullOrWhiteSpace(newCustomerStr))
                    newCustomer = JsonConvert.DeserializeObject<CustomerServiceOnline>(newCustomerStr);
                else
                {
                    IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
                    newCustomer = await db.FindOneAsync<CustomerServiceOnline>($"SELECT TOP 1 * FROM CustomerServiceOnlines WHERE OpenID='{openId}' and IsDeleted=0", null, false);
                }
                if (newCustomer != null)
                    return newCustomer;
            }
            return null;
        }

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <returns></returns>
        private static async Task<ApiTokenResult> GetAccessToken(string token, int getnewtoken = 0)
        {
            ApiTokenResult accessTokenResult = null;
            var url = StaticObject.ApiTokenUrl;
            var handler = new HttpClientHandler() { AutomaticDecompression = System.Net.DecompressionMethods.None };
            using (HttpClient httpclient = new HttpClient(handler))
            {
                httpclient.BaseAddress = new Uri(url);
                var content = new FormUrlEncodedContent(new Dictionary<string, string>() { { "token", token }, { "getnewtoken", getnewtoken.ToString() } });

                var response = await httpclient.PostAsync(url, content);

                string responseString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<AbpResultModel>(responseString);
                accessTokenResult = result.result;
            }
            return accessTokenResult;
        }

        /// <summary>
        /// 保存聊天信息
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="fanId"></param>
        /// <param name="mediaId"></param>
        /// <param name="mediaUrl"></param>
        /// <param name="mpId"></param>
        /// <param name="msgContent"></param>
        /// <param name="msgType"></param>
        /// <param name="sender"></param>
        /// <returns></returns>
        private async Task SaveMsgIntoDb(int? customerId, int? fanId, string mediaId, string mediaUrl, int mpId, string msgContent, int? msgType, int sender,long conversationId)
        {
            IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
            string sql = $"INSERT INTO  CustomerServiceConversationMsgs (CreationTime,CustomerId,FanId,MediaId,MediaUrl,MpID,MsgContent,MsgType,Sender,ConversationId) VALUES (GETDATE(),{customerId},{fanId},'{mediaId}','{mediaUrl}',{mpId},'{msgContent}','{msgType}',{sender},{conversationId})";
            await db.ExcuteNonQueryAsync(sql, null, false);
        }

        /// <summary>
        /// 获取粉丝信息
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        private async Task<MpFanDto> GetFanDto(string openId)
        {
            //var fandto = memberCache.Get<MpFanDto>("Fans_" + openId);
            var fanStr = await StackExchangeRedisHelper.Get(openId, StaticObject.Cache_Kf_MpFansByOpenId);
            MpFanDto fandto = null;
            if (!string.IsNullOrWhiteSpace(fanStr))
                fandto = JsonConvert.DeserializeObject<MpFanDto>(fanStr);
            if (fandto == null)
            {
                IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Default"]);
                string sql = $"SELECT TOP 1 * FROM MpFans WHERE ISDELETED=0 AND OPENID='{openId}'";
                fandto = await db.FindOneAsync<MpFanDto>(sql, null, false);
                //memberCache.Set("Fans_" + openId, fandto, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(StaticObject.CacheDictionary[StaticObject.Cache_Kf_MpFansByOpenId]));
                await StackExchangeRedisHelper.Set(openId, StaticObject.Cache_Kf_MpFansByOpenId, JsonConvert.SerializeObject(fandto), StaticObject.CacheDictionary[StaticObject.Cache_Kf_MpFansByOpenId]);

            }
            return fandto;
        }

        /// <summary>
        /// 发送客服信息(利用微信客服账号发送)
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="openId"></param>
        /// <param name="model"></param>
        /// <param name="accesstoken"></param>
        /// <returns></returns>
        private async Task<string> SendCommonMsg(int? msgType, string openId, CustomerServiceResponseText model, string accesstoken)
        {
            WxJsonResult result = null;
            var defaultCus = await GetDefaultWxKf();
            if (msgType == (int)MsgType.text)
            {
                result = await WxCustomerApi.SendTextAsync(accesstoken, openId, model.ResponseText, kfAccount: defaultCus.KfAccount);
                if (result.errcode == ReturnCode.请求成功)
                    return model.ResponseText;
                else
                    return null;
            }
            else if (msgType == (int)MsgType.image)
            {
                result = await WxCustomerApi.SendImageAsync(accesstoken, openId, model.MediaId, kfAccount: defaultCus.KfAccount);
                if (result.errcode == ReturnCode.请求成功)
                    return model.PreviewImgUrl;
                else
                    return null;
            }

            else if (msgType == (int)MsgType.video)
            {
                result = await WxCustomerApi.SendVideoAsync(accesstoken, openId, model.MediaId, model.Title, model.Description, kfAccount: defaultCus.KfAccount);
                if (result.errcode == ReturnCode.请求成功)
                    return model.PreviewImgUrl;
                else
                    return null;
            }

            else if (msgType == (int)MsgType.voice)
            {
                result = await WxCustomerApi.SendVoiceAsync(accesstoken, openId, model.MediaId, kfAccount: defaultCus.KfAccount);
                if (result.errcode == ReturnCode.请求成功)
                    return model.PreviewImgUrl;
                else
                    return null;
            }

            else if (msgType == (int)MsgType.mpnews)
            {
                IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
                string sql = $"SELECT top 1 * FROM CustomerArticles WHERE ISDELETED=0 AND MEDIAID='{model.MediaId}'";
                var article = await db.FindOneAsync<CustomerArticle>(sql, null, false);
                if (article == null)
                    return null;
                else
                {
                    List<Article> list = new List<Article> {
                        new Article
                        {
                            Title = article.Title,
                        Description = article.Description,
                        Url = article.AUrl,
                        PicUrl = article.FilePathOrUrl
                        }

                    };
                    result = await WxCustomerApi.SendNewsAsync(accesstoken, openId, list, kfAccount: defaultCus.KfAccount);
                    if (result.errcode == ReturnCode.请求成功)
                        return JsonConvert.SerializeObject(list);
                    else
                        return null;
                }

            }
            else if (msgType == (int)MsgType.mpmultinews)
            {
                IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
                string sql = $"SELECT top 1 * FROM CustomerArticleGroups WHERE ISDELETED=0 AND MEDIAID='{model.MediaId}'";
                var articlegroup = await db.FindOneAsync<CustomerArticleGroup>(sql, null, false);
                if (articlegroup == null)
                    return null;
                else
                {
                    var dSql = $"SELECT CustomerArticles.* FROM dbo.CustomerArticles JOIN dbo.CustomerArticleGroupItems ON CustomerArticleGroupItems.ArticleID = CustomerArticles.Id WHERE GroupID={articlegroup.Id} ORDER BY SortIndex";
                    var articles = await db.FindToListAsync<CustomerArticle>(dSql, null, false);
                    if (articles == null)
                        return null;
                    else
                    {
                        List<Article> list = new List<Article>();
                        foreach (var item in articles)
                        {
                            list.Add(new Article
                            {
                                Title = item.Title,
                                Description = item.Description,
                                Url = item.AUrl,
                                PicUrl = item.FilePathOrUrl
                            });
                        }
                        result = await WxCustomerApi.SendNewsAsync(accesstoken, openId, list, kfAccount: defaultCus.KfAccount);
                        if (result.errcode == ReturnCode.请求成功)
                            return JsonConvert.SerializeObject(list);
                        else
                            return null;
                    };
                }
            }
            else
                return null;
        }



        /// <summary>
        /// 获取首选默认微信账号
        /// </summary>
        /// <returns></returns>
        private async Task<CustomerServiceOnline> GetDefaultWxKf()
        {
            //var cus = memberCache.Get<CustomerServiceOnline>(StaticObject.Cache_DefaultWxKf);
            var cusStr = await StackExchangeRedisHelper.Get(StaticObject.Cache_DefaultWxKf, StaticObject.Cache_DefaultWxKf);

            CustomerServiceOnline cus = null;
            if (!string.IsNullOrWhiteSpace(cusStr))
                cus = JsonConvert.DeserializeObject<CustomerServiceOnline>(cusStr);
            if (cus == null)
            {
                IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
                string sql = $"SELECT TOP 1 * FROM CustomerServiceOnlines WHERE ISDELETED=0 AND KfType='{KFType.WX.ToString()}'";
                cus = await db.FindOneAsync<CustomerServiceOnline>(sql, null, false);
                //memberCache.Set(StaticObject.Cache_DefaultWxKf, cus, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(StaticObject.CacheDictionary[StaticObject.Cache_DefaultWxKf]));
                await StackExchangeRedisHelper.Set(StaticObject.Cache_DefaultWxKf, StaticObject.Cache_DefaultWxKf, JsonConvert.SerializeObject(cus), StaticObject.CacheDictionary[StaticObject.Cache_DefaultWxKf]);
            }
            return cus;
        }

        /// <summary>
        /// 转接最后一个会话至新客服
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        private async Task<CustomerServiceConversationDto> SetUserLastConversationToNewCustomer(string fansOpenId, CustomerServiceOnline newCustomer)
        {
            var resultStr = await StackExchangeRedisHelper.Get(fansOpenId, StaticObject.Cache_Kf_FanOpenId2Conversation);
            CustomerServiceConversationDto result = null;
            if (!string.IsNullOrWhiteSpace(resultStr))
                result = JsonConvert.DeserializeObject<CustomerServiceConversationDto>(resultStr);
            IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
            if (result == null)
            {
                result = await db.FindOneAsync<CustomerServiceConversationDto>($"SELECT TOP 1 * FROM CustomerServiceConversations WHERE FanOpenId='{fansOpenId}' ORDER BY CreationTime DESC", null, false);
                await db.ExcuteNonQueryAsync($"UPDATE CustomerServiceConversations SET CustomerId={newCustomer.Id},CustomerOpenId='{newCustomer.OpenID}' WHERE ID={result.Id}", null, false);
                result.CustomerId = newCustomer.Id;
                result.CustomerOpenId = newCustomer.OpenID;
                //memberCache.Set("Conversation_" + fansOpenId, result, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(StaticObject.CacheDictionary[StaticObject.Cache_Kf_FanOpenId2Conversation]));
                //redisCache.RemoveValue(fansOpenId, StaticObject.Cache_FanOpenId2Conversation);
                await StackExchangeRedisHelper.Set(fansOpenId, StaticObject.Cache_Kf_FanOpenId2Conversation, JsonConvert.SerializeObject(result), StaticObject.CacheDictionary[StaticObject.Cache_Kf_FanOpenId2Conversation]);
                await StackExchangeRedisHelper.Remove(fansOpenId, StaticObject.Cache_FanOpenId2Conversation);
            }
            else
            {
                result.CustomerId = newCustomer.Id;
                result.CustomerOpenId = newCustomer.OpenID;
                await db.ExcuteNonQueryAsync($"UPDATE CustomerServiceConversations SET CustomerId={newCustomer.Id},CustomerOpenId='{newCustomer.OpenID}' WHERE ID={result.Id}", null, false);
                //memberCache.Set("Conversation_" + fansOpenId, result);
                //redisCache.RemoveValue(fansOpenId, StaticObject.Cache_FanOpenId2Conversation);
                await StackExchangeRedisHelper.Set(fansOpenId, StaticObject.Cache_Kf_FanOpenId2Conversation, JsonConvert.SerializeObject(result));
                await StackExchangeRedisHelper.Remove(fansOpenId, StaticObject.Cache_FanOpenId2Conversation);
            }


            return result;
        }
        /// <summary>
        /// 获取客户最后一个会话
        /// </summary>
        /// <param name="fansOpenId"></param>
        /// <returns></returns>
        private async Task<CustomerServiceConversationDto> GetUserLastConversation(string fansOpenId)
        {
            var resultStr = await StackExchangeRedisHelper.Get(fansOpenId, StaticObject.Cache_Kf_FanOpenId2Conversation);

            CustomerServiceConversationDto result = null;
            //if (!memberCache.IsSet("Conversation_" + fansOpenId))
            //{
            if (string.IsNullOrWhiteSpace(resultStr))
            {

                IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
                result = await db.FindOneAsync<CustomerServiceConversationDto>($"SELECT TOP 1 * FROM CustomerServiceConversations WHERE FanOpenId='{fansOpenId}' ORDER BY CreationTime DESC", null, false);

                //memberCache.Set("Conversation_" + fansOpenId, result, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(StaticObject.CacheDictionary[StaticObject.Cache_Kf_FanOpenId2Conversation]));
                await StackExchangeRedisHelper.Set(fansOpenId, StaticObject.Cache_Kf_FanOpenId2Conversation, JsonConvert.SerializeObject(result), StaticObject.CacheDictionary[StaticObject.Cache_Kf_FanOpenId2Conversation]);

            }
            else
            {
                //return memberCache.Get<CustomerServiceConversationDto>("Conversation_" + fansOpenId);
                return JsonConvert.DeserializeObject<CustomerServiceConversationDto>(resultStr);
            }
            return result;
        }

        /// <summary>
        /// 获取发送者枚举文本
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        private string SenderToString(int sender)
        {
            if (sender == (int)Sender.user)
                return Sender.user.ToString();
            else if (sender == (int)Sender.customer)
                return Sender.customer.ToString();
            else
                return null;
        }
        /// <summary>
        /// 获取信息类型枚举文本
        /// </summary>
        /// <param name="msgType"></param>
        /// <returns></returns>
        private string MsgTypeToString(int? msgType)
        {
            if (msgType == (int)MsgType.text)
                return MsgType.text.ToString();
            else if (msgType == (int)MsgType.image)
                return MsgType.image.ToString();
            else if (msgType == (int)MsgType.mpnews)
                return MsgType.mpnews.ToString();
            else if (msgType == (int)MsgType.video)
                return MsgType.video.ToString();
            else if (msgType == (int)MsgType.voice)
                return MsgType.voice.ToString();
            else
                return null;
        }

        private int MsgTypeToInt(string msgType)
        {
            if (msgType == MsgType.text.ToString())
                return (int)MsgType.text;
            else if (msgType == MsgType.image.ToString())
                return (int)MsgType.image;
            else if (msgType == MsgType.mpnews.ToString())
                return (int)MsgType.mpnews;
            else if (msgType == MsgType.video.ToString())
                return (int)MsgType.video;
            else if (msgType == MsgType.voice.ToString())
                return (int)MsgType.voice;
            else if (msgType == MsgType.mpmultinews.ToString())
                return (int)MsgType.mpmultinews;
            else
                return -1;
        }
        #endregion

        #endregion

        #region 设置接口
        //public async Task AaddCustomer(string kfNick)
        //{
        //    var file = Request.Files[0];
        //    FileUploadHelper fileupload = new FileUploadHelper();

        //    var url = await fileupload.UploadFile(file.InputStream, file.FileName.Substring(file.FileName.LastIndexOf(".") + 1), file.ContentType);
        //}
        #endregion

    }


}