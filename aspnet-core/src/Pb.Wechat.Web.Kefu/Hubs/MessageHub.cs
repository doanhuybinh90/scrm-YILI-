using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Pb.Wechat.Web.Kefu.Helper;
using Pb.Wechat.Web.Kefu.Model;
using Pb.Wechat.Web.Kefu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace Pb.Wechat.Web.Kefu.Hubs
{
    public class MessageHub : Hub
    {
        //private RedisHelper redisCache = RedisHelper.GetInstance(StaticObject.RedisServer, StaticObject.RedisPort ?? 0, StaticObject.RedisPassword, StaticObject.RedisDb ?? 0);
        //private WebCacheHelper memberCache = new WebCacheHelper();
        LogHelper log = new LogHelper();
        public async Task setState(string messageToken, int state)
        {
            try
            {
                var customer = await GetCustomer(messageToken);
               
                if (customer != null)
                {
                   
                    IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
                    await db.ExcuteNonQueryAsync($"Update CustomerServiceOnlines SET OnlineState={state},ConnectId='{Context.ConnectionId}',ConnectState={(int)ConnectState.Connect} WHERE ID='{customer.Id}'", null, false);
                    customer.OnlineState = state;
                    customer.ConnectId = Context.ConnectionId;
                    customer.ConnectState = 1;

                    //memberCache.Set(messageToken, customer.OpenID, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(StaticObject.CacheDictionary[StaticObject.Cache_Kf_MessageToken2CustomerOpenId]));
                    //memberCache.Set(customer.OpenID, customer, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(StaticObject.CacheDictionary[StaticObject.Cache_Kf_OpenId2Customer]));
                    //redisCache.RemoveValue(customer.OpenID, StaticObject.Cache_OpenId2Customer);




                    await StackExchangeRedisHelper.Set(messageToken, StaticObject.Cache_Kf_MessageToken2CustomerOpenId, customer.OpenID, StaticObject.CacheDictionary[StaticObject.Cache_Kf_MessageToken2CustomerOpenId]);
                    await StackExchangeRedisHelper.Set(customer.OpenID, StaticObject.Cache_Kf_OpenId2Customer, JsonConvert.SerializeObject(customer), StaticObject.CacheDictionary[StaticObject.Cache_Kf_OpenId2Customer]);
                    await StackExchangeRedisHelper.Remove(customer.OpenID, StaticObject.Cache_OpenId2Customer);

                    var a = -1;
                    if (state == (int)OnlineState.OnLine)
                        a = (int)InOutState.online;
                    if (state == (int)OnlineState.Leave)
                        a = (int)InOutState.leave;
                    if (state == (int)OnlineState.Quit)
                        a = (int)InOutState.quit;
                    await db.ExcuteNonQueryAsync($"Insert INTO CustomerInOutLogs (CustomerId,InOutState,CreationTime) VALUES ({customer.Id},{a},GetDate())", null, false);


                }
            }
            catch (Exception ex)
            {
                log.Error("MessageHub-setState错误：",ex);
                throw;
            }
            
        }

        private async Task<CustomerServiceOnline> GetCustomer(string messageToken)
        {
            if (!string.IsNullOrEmpty(messageToken))
            {
                //获取messageToken对应的客服
                //var openid = memberCache.Get<string>(messageToken);
                var openid = await StackExchangeRedisHelper.Get(messageToken, StaticObject.Cache_Kf_MessageToken2CustomerOpenId);
                if (!string.IsNullOrEmpty(openid))
                {
                    //检查messageToken是否过期(客服1天之内可能会多次登录，识别messagetoken是否为客服最后一次登录生成)
                    //var newCustomer = memberCache.Get<CustomerServiceOnline>(openid);
                    var newCustomer = JsonConvert.DeserializeObject<CustomerServiceOnline>(await StackExchangeRedisHelper.Get(openid, StaticObject.Cache_Kf_OpenId2Customer));
                    if (newCustomer != null && newCustomer.MessageToken == messageToken)
                        return newCustomer;
                }
            }
            return null;
        }

        public override async Task OnConnected()
        {
            log.Info($"MessageHub-OnConnected {Context.ConnectionId}");
            try
            {
                IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
                var data = await db.FindOneAsync<CustomerServiceOnline>($"SELECT TOP 1 * FROM CustomerServiceOnlines WHERE ConnectId='{Context.ConnectionId}'", null, false);
                if (data != null)
                {
                    await db.ExcuteNonQueryAsync($"Insert INTO CustomerInOutLogs (CustomerId,InOutState,CreationTime) VALUES ({data.Id},{(int)InOutState.Login},GetDate())", null, false);
                    await db.ExcuteNonQueryAsync($"Update CustomerServiceOnlines SET ConnectState={(int)ConnectState.Connect} WHERE ConnectId='{Context.ConnectionId}'", null, false);
                    data.ConnectState = (int)ConnectState.Connect;
                    //memberCache.Set(data.OpenID, data, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(StaticObject.CacheDictionary[StaticObject.Cache_Kf_OpenId2Customer]));
                    //redisCache.RemoveValue(data.OpenID, StaticObject.Cache_OpenId2Customer);

                    await StackExchangeRedisHelper.Set(data.OpenID, StaticObject.Cache_Kf_OpenId2Customer, JsonConvert.SerializeObject(data), StaticObject.CacheDictionary[StaticObject.Cache_Kf_OpenId2Customer]);
                    await StackExchangeRedisHelper.Remove(data.OpenID, StaticObject.Cache_OpenId2Customer);

                }
                await base.OnConnected();
            }
            catch (Exception ex)
            {
                log.Error("MessageHub-OnConnected错误：", ex);
                throw ex;
            }
           
        }

        public override async Task OnDisconnected(bool stopCalled)
        {
            log.Info($"MessageHub-OnDisconnected {Context.ConnectionId}");
            try
            {
                IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
                var data = await db.FindOneAsync<CustomerServiceOnline>($"SELECT TOP 1 * FROM CustomerServiceOnlines WHERE ConnectId='{Context.ConnectionId}'", null, false);
                if (data != null)
                {
                    await db.ExcuteNonQueryAsync($"Insert INTO CustomerInOutLogs (CustomerId,InOutState,CreationTime) VALUES ({data.Id},{(int)InOutState.Logout},GetDate())", null, false);
                    await db.ExcuteNonQueryAsync($"Update CustomerServiceOnlines SET ConnectState={(int)ConnectState.UnConnect} WHERE ConnectId='{Context.ConnectionId}'", null, false);
                   
                    data.ConnectState = (int)ConnectState.UnConnect;
                    //memberCache.Set(data.OpenID, data, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(StaticObject.CacheDictionary[StaticObject.Cache_Kf_OpenId2Customer]));         
                    //redisCache.RemoveValue(data.OpenID, StaticObject.Cache_OpenId2Customer);

                    await StackExchangeRedisHelper.Set(data.OpenID, StaticObject.Cache_Kf_OpenId2Customer, JsonConvert.SerializeObject(data), StaticObject.CacheDictionary[StaticObject.Cache_Kf_OpenId2Customer]);
                    await StackExchangeRedisHelper.Remove(data.OpenID, StaticObject.Cache_OpenId2Customer);
                }
                await base.OnDisconnected(stopCalled);
            }
            catch (Exception ex)
            {
                log.Error("MessageHub-OnDisconnected错误：", ex);
                throw ex;
            }
            
        }

        public override async Task OnReconnected()
        {
            log.Info($"MessageHub-OnReconnected {Context.ConnectionId}");
            try
            {
                IDBHelper db = new MssqlHelper(StaticObject.ConnectionStrings["Kf"]);
                var data = await db.FindOneAsync<CustomerServiceOnline>($"SELECT TOP 1 * FROM CustomerServiceOnlines WHERE ConnectId='{Context.ConnectionId}'", null, false);
                if (data != null)
                {
                    await db.ExcuteNonQueryAsync($"Insert INTO CustomerInOutLogs (CustomerId,InOutState,CreationTime) VALUES ({data.Id},{(int)InOutState.ReLogin},GetDate())", null, false);
                    await db.ExcuteNonQueryAsync($"Update CustomerServiceOnlines SET ConnectState={(int)ConnectState.Connect} WHERE ConnectId='{Context.ConnectionId}'", null, false);
                    
                    data.ConnectState = (int)ConnectState.Connect;
                    //memberCache.Set(data.OpenID, data, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(StaticObject.CacheDictionary[StaticObject.Cache_Kf_OpenId2Customer]));
                    //redisCache.RemoveValue(data.OpenID, StaticObject.Cache_OpenId2Customer);

                    await StackExchangeRedisHelper.Set(data.OpenID, StaticObject.Cache_Kf_OpenId2Customer, JsonConvert.SerializeObject(data), StaticObject.CacheDictionary[StaticObject.Cache_Kf_OpenId2Customer]);
                    await StackExchangeRedisHelper.Remove(data.OpenID, StaticObject.Cache_OpenId2Customer);
                }
                await base.OnReconnected();
            }
            catch (Exception ex)
            {
                log.Error("MessageHub-OnReconnected错误：", ex);
                throw ex;
            }

        }
    }
}