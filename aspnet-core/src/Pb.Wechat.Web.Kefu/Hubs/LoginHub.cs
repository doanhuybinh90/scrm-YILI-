using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Pb.Wechat.Web.Kefu.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Kefu.Hubs
{
    public class LoginHub : Hub
    {
        //private RedisHelper redisCache = RedisHelper.GetInstance(StaticObject.RedisServer, StaticObject.RedisPort ?? 0, StaticObject.RedisPassword, StaticObject.RedisDb ?? 0);
        //private WebCacheHelper memberCache = new WebCacheHelper();
        public async Task applyToken()
        {
            var token = Guid.NewGuid().ToString();

            //memberCache.Set(token, Context.ConnectionId, DateTime.Now.AddSeconds(StaticObject.CacheDictionary[StaticObject.Cache_Kf_LoginToken]), TimeSpan.Zero);

            await StackExchangeRedisHelper.Set(token, StaticObject.Cache_Kf_LoginToken, Context.ConnectionId, StaticObject.CacheDictionary[StaticObject.Cache_Kf_LoginToken]);
            await Clients.Client(Context.ConnectionId).getToken(token);
        }
    }
}
