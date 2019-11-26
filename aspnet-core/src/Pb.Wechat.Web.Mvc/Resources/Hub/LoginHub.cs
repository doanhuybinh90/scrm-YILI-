using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Modules;
using Microsoft.AspNetCore.SignalR;
using Abp.Runtime.Caching;
using Microsoft.AspNetCore.Mvc;

namespace Pb.Wechat.Web
{
    public class LoginHub : Hub, ITransientDependency
    {
        private readonly ICacheManager _cacheManager;
        public LoginHub(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public async Task applyToken()
        {
            var token = Guid.NewGuid().ToString();
            await _cacheManager.GetCache(AppConsts.Cache_Kf_LoginToken).SetAsync(token, Context.ConnectionId);
            await Clients.Client(Context.ConnectionId).SendAsync("getToken", token);
        }

    }
}
