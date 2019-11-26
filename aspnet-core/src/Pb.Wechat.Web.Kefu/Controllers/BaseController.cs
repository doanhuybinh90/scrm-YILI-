using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pb.Wechat.Web.Kefu.Controllers
{
    public abstract class BaseController<T> : Controller where T : Hub
    {
        public IHubConnectionContext<dynamic> Clients { get; set; }
        public IGroupManager Groups { get; set; }
        public BaseController()
        {
            var gh = GlobalHost.ConnectionManager.GetHubContext<T>();
            Clients = gh.Clients;
            Groups = gh.Groups;
        }
    }
}