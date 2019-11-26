using Abp.Application.Services.Dto;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Runtime.Caching;
using Hangfire;
using Pb.Hangfire.Tool;
using Pb.Wechat.CustomerServiceConversationMsgs;
using Pb.Wechat.CustomerServiceConversations;
using Pb.Wechat.CustomerServiceConversations.Dto;
using Pb.Wechat.CustomerServiceOnlines;
using Pb.Wechat.MpAccessTokenClib;
using Pb.Wechat.MpAccounts;
using Pb.Wechat.MpAccounts.Dto;
using Pb.Wechat.MpFans;
using Pb.Wechat.Url;
using Senparc.Weixin.MP.AdvancedAPIs;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Threading.Tasks;

namespace Pb.Hangfire.Jobs
{
    public class SendKfCloseMessage : IJob
    {
      
        public void Run()
        {
            Task.WaitAll(TimeSendCloseText());
        }
        public SendKfCloseMessage() { }


        #region 长时间未操作，帮用户关闭
        /// <summary>
        /// 长时间未操作，帮用户关闭
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        [AutomaticRetry(Attempts = 1)]
        public async Task TimeSendCloseText()
        {

            IDBHelper db = new MssqlHelper(JobConfig.ConnectionStrings["Kf"]);
            IDBHelper db2 = new MssqlHelper(JobConfig.ConnectionStrings["YiliscrmDb"]);
            var accounts = await db2.FindToListAsync<MpAccountDto>("SELECT * FROM MpAccounts WHERE ISDELETED=0 ", null, false);
            WebClient wc = new WebClient();
            foreach (var item in accounts)
            {
                var state = (int)CustomerServiceConversationState.Asking;
                var allAskingList = await db.FindToListAsync<CustomerServiceConversation>($"SELECT * FROM CustomerServiceConversations WHERE MPID={item.Id} AND STATE={state}",null,false);//提问中的列表
                var defaultCustomer = await db.FindOneAsync<CustomerServiceOnline>($"SELECT TOP 1 * FROM CustomerServiceOnlines WHERE KfType='WX' AND IsDeleted=0", null, false);//获取默认微信客服
                foreach(var conversation in allAskingList)
                {
                    var fansId = Convert.ToInt32(conversation.FanId);
                    var lastMsg = await db.FindOneAsync<CustomerServiceConversationMsg>($"SELECT TOP 1 * FROM CustomerServiceConversationMsgs WHERE MPID={item.Id} AND FanId={fansId} ORDER BY CreationTime Desc",null,false);
                    if (lastMsg!=null)
                    {
                        if (DateTime.Now >= lastMsg.CreationTime.AddMinutes(30))//关闭对话
                        {

                            await db.ExcuteNonQueryAsync($"UPDATE CustomerServiceConversations SET EndTalkTime=Getdate(),State={(int)CustomerServiceConversationState.Closed} WHERE ID={conversation.Id}", null, false);

                            var values = new NameValueCollection();
                            values.Add("conversationId", conversation.Id.ToString());//会话ID
                            await wc.UploadValuesTaskAsync(JobConfig.AppSettings["AutoStopConversationUrl"], values);
                        }
                        //else if (DateTime.Now < lastMsg.CreationTime.AddMinutes(30) && DateTime.Now > lastMsg.CreationTime.AddMinutes(15))//推送即将关闭的客服消息
                        //{
                        //    if (defaultCustomer != null)//默认客服推送消息
                        //    {
                        //        //推送消息
                        //        string token = null;
                        //        var tokenApiResult = await StaticObjects.GetAccessToken(item.TaskAccessToken);
                        //        token = tokenApiResult.access_token;
                        //        try
                        //        {
                        //            await CustomApi.SendTextAsync(token, conversation.FanOpenId, "您已经很久没有和客服对话了，对话稍后将会自动关闭。", kfAccount: defaultCustomer.KfAccount);
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //            tokenApiResult = await StaticObjects.GetAccessToken(item.TaskAccessToken, 1);
                        //            token = tokenApiResult.access_token;
                        //            try
                        //            {
                        //                await CustomApi.SendTextAsync(token, conversation.FanOpenId, "您已经很久没有和客服对话了，对话稍后将会自动关闭。", kfAccount: defaultCustomer.KfAccount);
                        //            }
                        //            catch (Exception e)
                        //            {
                        //                await CustomApi.SendTextAsync(token, conversation.FanOpenId, "您已经很久没有和客服对话了，对话稍后将会自动关闭。", kfAccount: defaultCustomer.KfAccount);
                        //                throw e;
                        //            }
                        //        }
                        //    }
                        //}
                    }
                    
                }

                
                
            }

            
        }
        
        #endregion
        
    }
    
}
