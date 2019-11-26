using Hangfire;
using Pb.Hangfire.Tool;
using Pb.Wechat.MpAccounts.Dto;
using Pb.Wechat.MpEvents.Dto;
using Pb.Wechat.MpFans.Dto;
using Pb.Wechat.MpGroups.Dto;
using Pb.Wechat.MpMediaVideos;
using Pb.Wechat.MpMessages.Dto;
using Pb.Wechat.TaskGroupMessages.Dto;
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
    public class WeChatSendMessageJob : IJob
    {
        public void Run()
        {
            Task.WaitAll(DoRun());
        }

        [AutomaticRetry(Attempts = 1)]
        private async Task DoRun()
        {
            IDBHelper db = new MssqlHelper(JobConfig.ConnectionStrings["YiliscrmDb"]);
            var accounts = await db.FindToListAsync<MpAccountDto>("SELECT * FROM MpAccounts WHERE ISDELETED=0 ", null, false);
            var Messages = await db.FindToListAsync<MpMessageDto>($"SELECT * FROM MpMessages WHERE ISDELETED=0 AND ISTASK=1 AND ExecTaskTime<=GETDATE() AND SendState={(int)MpMessageTaskState.Wait}", null, false);
            List<string> successList = new List<string>();

            foreach (var args in Messages)
            {
                var account = accounts.Where(m => m.Id == args.MpID).FirstOrDefault();
                args.WxMsgID = Guid.NewGuid().ToString();
                await db.ExcuteNonQueryAsync($"Update MpMessages Set SendState='{(int)MpMessageTaskState.Doing}' Where ID='{args.Id}'", null, false);//正在发送

                if (!string.IsNullOrWhiteSpace(args.GroupIds))
                {
                    if (args.IsMember == IsMemberEnum.ALL.ToString())
                    {
                        var fancount = await db.ExecuteScalarAsync("SELECT count(*) FROM MpFans WHERE ISDELETED=0 AND MpID=" + args.MpID, null, false);
                        await SendAllAsync(account, args, Convert.ToInt64(fancount));
                    }
                    else if (args.IsMember == IsMemberEnum.Tag.ToString())
                    {
                        var tagids = new List<int>();
                        var tagidstr = args.TargetID.Split(',', StringSplitOptions.RemoveEmptyEntries);
                        int tagid = -1;
                        foreach (var item in tagidstr)
                        {
                            if (int.TryParse(item, out tagid))
                                tagids.Add(tagid);
                        }
                        if (tagids.Count == 0)
                            tagids.Add(-1);
                        var _fan = await db.FindToListAsync<MpFanDto>("SELECT a.OpenID FROM MpFans a inner join MpFansTagItems b on a.Id=b.FansId WHERE a.ISDELETED=0 AND a.MpID=" + args.MpID + " AND b.TagId in (" + string.Join(",", tagids) + ")", null, false);
                        await SaveIntoGroupMessage(args, _fan.Select(m => m.OpenID));
                        await SendMessageAsync(account, args, _fan.LongCount());
                    }
                    else if (args.IsMember == IsMemberEnum.NotMember.ToString())
                    {
                        var _fan = await db.FindToListAsync<MpFanDto>("SELECT OpenID FROM MpFans WHERE ISDELETED=0 AND MemberID=0 AND MpID=" + args.MpID, null, false);
                        await SaveIntoGroupMessage(args, _fan.Select(m => m.OpenID));
                        await SendMessageAsync(account, args, _fan.LongCount());
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
                    var fancount = await db.ExecuteScalarAsync("SELECT count(*) FROM MpFans WHERE ISDELETED=0 AND MpID=" + args.MpID, null, false);
                    await SendAllAsync(account, args, Convert.ToInt64(fancount));
                }
            }
        }

        #region 将openid按10000条一组进行拆分
        /// <summary>
        /// 将openid按10000条一组进行拆分
        /// </summary>
        /// <param name="args"></param>
        /// <param name="openIds"></param>
        /// <returns></returns>
        public async Task SaveIntoGroupMessage(MpMessageDto args, IEnumerable<string> openIds)
        {
            var allCount = openIds.LongCount();
            var _count = Math.Ceiling(Convert.ToDecimal(allCount) / Convert.ToDecimal(10000));
            #region 每行10000条写入数据库,筛选会员阶段
            string insertSql = "Insert into TaskGroupMessages (GroupID,MessageID,MpID,TaskState,OpenIDs,SendCount,CreationTime,IsDeleted,LastModificationTime) Values ('{0}','{1}','{2}','{3}','{4}','{5}',GetDate(),0,GetDate())";
            for (int i = 0; i < _count; i++)
            {
                List<string> _openIds = openIds.Select(m => m).Skip(i * 10000).Take(10000).ToList();
                string opids = string.Join(",", _openIds);
                IDBHelper db = new MssqlHelper(JobConfig.ConnectionStrings["YiliscrmGroupMessage"]);
                await db.ExcuteNonQueryAsync(string.Format(insertSql, args.GroupID, args.Id, args.MpID, (int)MpMessageTaskState.Wait, opids, _openIds.Count), null, false);
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
        public async Task SendMessageAsync(MpAccountDto account, MpMessageDto args, long totalCount)
        {
            IDBHelper db = new MssqlHelper(JobConfig.ConnectionStrings["YiliscrmGroupMessage"]);
            IDBHelper yilidb = new MssqlHelper(JobConfig.ConnectionStrings["YiliscrmDb"]);
            var msgResult = await db.FindToListAsync<TaskGroupMessageDto>($"SELECT * FROM TaskGroupMessages WHERE ISDELETED=0 AND MpID='{args.MpID}' AND TaskState='{(int)MpMessageTaskState.Wait}' AND MessageID='" + args.Id + "'", null, false);

            if (JobConfig.AppSettings["Debugger"] != "true")//正式发送
            {
                string updateSql = "Update TaskGroupMessages SET TaskState='{1}',WxMsgID='{2}' WHERE ID='{0}'";
                foreach (var drItem in msgResult)
                {
                    var result = await SendByOpenIds(account, args, drItem.OpenIDs.Split(','), yilidb);
                    if (result != null && result.errcode == ReturnCode.请求成功)
                        await db.ExcuteNonQueryAsync(string.Format(updateSql, drItem.Id, (int)MpMessageTaskState.Doing, result.msg_id), null, false);
                    else
                        await db.ExcuteNonQueryAsync(string.Format(updateSql, drItem.Id, (int)MpMessageTaskState.Fail,""), null, false);
                }
            }
            else {
                foreach (var drItem in msgResult)
                {
                    await db.ExcuteNonQueryAsync($"Update TaskGroupMessages SET TaskState='{(int)MpMessageTaskState.Success}',SuccessCount='{drItem.SendCount}',FailCount=0 WHERE ID='{drItem.Id}'", null, false);
                }
                yilidb.ExcuteNonQuery($"UPDATE MpMessages SET SuccessCount=totalCount,FailCount=0,FinishDate=GetDate(),SendCount={totalCount},SendState='{(int)MpMessageTaskState.Success}' Where ID={args.Id}", null, false);
            }
        }
        
        /// <summary>
        /// 按openid群发
        /// </summary>
        /// <param name="account"></param>
        /// <param name="args"></param>
        /// <param name="openIds"></param>
        /// <returns></returns>
        private async Task<SendResult> SendByOpenIds(MpAccountDto account, MpMessageDto args, string[] openIds, IDBHelper yilidb)
        {
            SendResult result = null;

            if (args.MessageType == MpMessageType.text.ToString())
            {
                try
                {
                    result = await GroupMessageApi.SendGroupMessageByOpenIdAsync((await StaticObjects.GetAccessToken(account.TaskAccessToken)).access_token, GroupMessageType.text, args.Content, $"MpMessage{args.Id}", Senparc.Weixin.Config.TIME_OUT, openIds.ToArray());
                }
                catch
                {
                    try
                    {
                        result = await GroupMessageApi.SendGroupMessageByOpenIdAsync((await StaticObjects.GetAccessToken(account.TaskAccessToken, 1)).access_token, GroupMessageType.text, args.Content, $"MpMessage{args.Id}", Senparc.Weixin.Config.TIME_OUT, openIds.ToArray());
                    }
                    catch
                    {
                    }
                }
            }
            else if (args.MessageType == MpMessageType.video.ToString())
            {
                var videoId = int.Parse(args.VideoID.ToString());
                var video = yilidb.FindOne<MpMediaVideo>("SELECT TOP 1 * FROM MpMediaVideos WHERE ID=" + videoId, null, false);
                if (video == null)
                    throw new Exception(string.Format("视频{0}不存在", args.VideoName));
                try
                {
                    result = await GroupMessageApi.SendVideoGroupMessageByOpenIdAsync((await StaticObjects.GetAccessToken(account.TaskAccessToken)).access_token, video.Title, video.Description, args.VideoMediaID, $"MpMessage{args.Id}", Senparc.Weixin.Config.TIME_OUT, openIds.ToArray());
                }
                catch
                {
                    try
                    {
                        result = await GroupMessageApi.SendVideoGroupMessageByOpenIdAsync((await StaticObjects.GetAccessToken(account.TaskAccessToken, 1)).access_token, video.Title, video.Description, args.VideoMediaID, $"MpMessage{args.Id}", Senparc.Weixin.Config.TIME_OUT, openIds.ToArray());
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
                    result = await GroupMessageApi.SendGroupMessageByOpenIdAsync((await StaticObjects.GetAccessToken(account.TaskAccessToken)).access_token, GroupMessageType.voice, args.VoiceMediaID, $"MpMessage{args.Id}", Senparc.Weixin.Config.TIME_OUT, openIds.ToArray());
                }
                catch
                {
                    try
                    {
                        result = await GroupMessageApi.SendGroupMessageByOpenIdAsync((await StaticObjects.GetAccessToken(account.TaskAccessToken, 1)).access_token, GroupMessageType.voice, args.VoiceMediaID, $"MpMessage{args.Id}", Senparc.Weixin.Config.TIME_OUT, openIds.ToArray());
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
                    result = await GroupMessageApi.SendGroupMessageByOpenIdAsync((await StaticObjects.GetAccessToken(account.TaskAccessToken)).access_token, GroupMessageType.image, args.ImageMediaID, $"MpMessage{args.Id}", Senparc.Weixin.Config.TIME_OUT, openIds.ToArray());
                }
                catch
                {
                    try
                    {
                        result = await GroupMessageApi.SendGroupMessageByOpenIdAsync((await StaticObjects.GetAccessToken(account.TaskAccessToken, 1)).access_token, GroupMessageType.image, args.ImageMediaID, $"MpMessage{args.Id}", Senparc.Weixin.Config.TIME_OUT, openIds.ToArray());
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
                    result = await GroupMessageApi.SendGroupMessageByOpenIdAsync((await StaticObjects.GetAccessToken(account.TaskAccessToken)).access_token, GroupMessageType.mpnews, args.ArticleMediaID, $"MpMessage{args.Id}", Senparc.Weixin.Config.TIME_OUT, openIds.ToArray());
                }
                catch
                {
                    try
                    {
                        result = await GroupMessageApi.SendGroupMessageByOpenIdAsync((await StaticObjects.GetAccessToken(account.TaskAccessToken, 1)).access_token, GroupMessageType.mpnews, args.ArticleMediaID, $"MpMessage{args.Id}", Senparc.Weixin.Config.TIME_OUT, openIds.ToArray());
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
                    result = await GroupMessageApi.SendGroupMessageByOpenIdAsync((await StaticObjects.GetAccessToken(account.TaskAccessToken)).access_token, GroupMessageType.mpnews, args.ArticleGroupMediaID, $"MpMessage{args.Id}", Senparc.Weixin.Config.TIME_OUT, openIds.ToArray());
                }
                catch
                {
                    try
                    {
                        result = await GroupMessageApi.SendGroupMessageByOpenIdAsync((await StaticObjects.GetAccessToken(account.TaskAccessToken, 1)).access_token, GroupMessageType.mpnews, args.ArticleGroupMediaID, $"MpMessage{args.Id}", Senparc.Weixin.Config.TIME_OUT, openIds.ToArray());
                    }
                    catch
                    {
                    }
                }
            }
            
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
        public async Task SendAllAsync(MpAccountDto account, MpMessageDto args, long totalCount)
        {
            IDBHelper yilidb = new MssqlHelper(JobConfig.ConnectionStrings["YiliscrmDb"]);
            if (JobConfig.AppSettings["Debugger"] != "true")//正式发送
            {
                #region 发送消息
                string updateSql = "Update MpMessages SET SendState='{1}',WxMsgID='{2}' WHERE ID='{0}'";
                var result = await SendAllByGroup(account, args, yilidb);
                if (result != null && result.errcode == ReturnCode.请求成功)
                    await yilidb.ExcuteNonQueryAsync(string.Format(updateSql, args.Id, (int)MpMessageTaskState.Doing, result.msg_id), null, false);
                else
                    await yilidb.ExcuteNonQueryAsync(string.Format(updateSql, args.Id, (int)MpMessageTaskState.Fail, ""), null, false);
                #endregion
            }
            else//测试，不发送
            {
                yilidb.ExcuteNonQuery($"UPDATE MpMessages SET SuccessCount={totalCount},FailCount=0,FinishDate=GetDate(),SendCount={totalCount},SendState='(int)MpMessageTaskState.Success' Where ID={args.Id}", null, false);
            }
        }

        /// <summary>
        /// 发送全部用户
        /// </summary>
        /// <param name="account"></param>
        /// <param name="args"></param>
        /// <param name="yilidb"></param>
        /// <returns></returns>
        private async Task<SendResult> SendAllByGroup(MpAccountDto account, MpMessageDto args, IDBHelper yilidb)
        {
            SendResult result = null;

            if (args.MessageType == MpMessageType.text.ToString())
            {
                try
                {
                    result = GroupMessageApi.SendGroupMessageByGroupId((await StaticObjects.GetAccessToken(account.TaskAccessToken)).access_token, "", args.Content, GroupMessageType.text, true, clientmsgid: $"MpMessage{args.Id}");
                }
                catch
                {
                    try
                    {
                        result = GroupMessageApi.SendGroupMessageByGroupId((await StaticObjects.GetAccessToken(account.TaskAccessToken, 1)).access_token, "", args.Content, GroupMessageType.text, true, clientmsgid: $"MpMessage{args.Id}");
                    }
                    catch
                    {
                    }
                }
            }
            else if (args.MessageType == MpMessageType.video.ToString())
            {

                var videoId = int.Parse(args.VideoID.ToString());
                var video = yilidb.FindOne<MpMediaVideo>("SELECT TOP 1 * FROM MpMediaVideos WHERE ID=" + videoId, null, false);
                if (video == null)
                    throw new Exception(string.Format("视频{0}不存在", args.VideoName));
                try
                {
                    result = GroupMessageApi.SendGroupMessageByGroupId((await StaticObjects.GetAccessToken(account.TaskAccessToken)).access_token, "", args.VideoMediaID, GroupMessageType.video, true, clientmsgid: $"MpMessage{args.Id}");

                }
                catch
                {
                    try
                    {
                        result = GroupMessageApi.SendGroupMessageByGroupId((await StaticObjects.GetAccessToken(account.TaskAccessToken, 1)).access_token, "", args.VideoMediaID, GroupMessageType.video, true, clientmsgid: $"MpMessage{args.Id}");
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
                    result = GroupMessageApi.SendGroupMessageByGroupId((await StaticObjects.GetAccessToken(account.TaskAccessToken)).access_token, "", args.VoiceMediaID, GroupMessageType.voice, true, clientmsgid: $"MpMessage{args.Id}");

                }
                catch
                {
                    try
                    {
                        result = GroupMessageApi.SendGroupMessageByGroupId((await StaticObjects.GetAccessToken(account.TaskAccessToken, 1)).access_token, "", args.VoiceMediaID, GroupMessageType.voice, true, clientmsgid: $"MpMessage{args.Id}");
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
                    result = GroupMessageApi.SendGroupMessageByGroupId((await StaticObjects.GetAccessToken(account.TaskAccessToken)).access_token, "", args.ImageMediaID, GroupMessageType.image, true, clientmsgid: $"MpMessage{args.Id}");
                }
                catch
                {
                    try
                    {
                        result = GroupMessageApi.SendGroupMessageByGroupId((await StaticObjects.GetAccessToken(account.TaskAccessToken, 1)).access_token, "", args.ImageMediaID, GroupMessageType.image, true, clientmsgid: $"MpMessage{args.Id}");
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
                    result = GroupMessageApi.SendGroupMessageByGroupId((await StaticObjects.GetAccessToken(account.TaskAccessToken)).access_token, "", args.ArticleMediaID, GroupMessageType.mpnews, true, clientmsgid: $"MpMessage{args.Id}");
                }
                catch
                {
                    try
                    {
                        result = GroupMessageApi.SendGroupMessageByGroupId((await StaticObjects.GetAccessToken(account.TaskAccessToken, 1)).access_token, "", args.ArticleMediaID, GroupMessageType.mpnews, true, clientmsgid: $"MpMessage{args.Id}");
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
                    result = GroupMessageApi.SendGroupMessageByGroupId((await StaticObjects.GetAccessToken(account.TaskAccessToken)).access_token, "", args.ArticleGroupMediaID, GroupMessageType.mpnews, true, clientmsgid: $"MpMessage{args.Id}");
                }
                catch
                {
                    try
                    {
                        result = GroupMessageApi.SendGroupMessageByGroupId((await StaticObjects.GetAccessToken(account.TaskAccessToken, 1)).access_token, "", args.ArticleGroupMediaID, GroupMessageType.mpnews, true, clientmsgid: $"MpMessage{args.Id}");
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
