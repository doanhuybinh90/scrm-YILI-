using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using System.Configuration;
using System.Web.Cors;

[assembly: OwinStartup(typeof(Pb.Wechat.Web.Kefu.Startup))]

namespace Pb.Wechat.Web.Kefu
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalHost.DependencyResolver.UseSqlServer(ConfigurationManager.ConnectionStrings["Signalr"].ConnectionString);
            app.UseCors(CorsOptions.AllowAll);
            app.Map("/signalr", map =>
            {
                var config = new HubConfiguration
                {
                    // You can enable JSONP by uncommenting this line
                    // JSONP requests are insecure but some older browsers (and some
                    // versions of IE) require JSONP to work cross domain
                    // EnableJSONP = true
                };
                config.EnableDetailedErrors = true;
                // Turns cors support on allowing everything
                // In real applications, the origins should be locked down
                map.UseCors(CorsOptions.AllowAll)
                   .RunSignalR(config);
            });
            StaticObject.RedisConnectString = ConfigurationManager.AppSettings["RedisConnect"];
            StaticObject.RedisServer = ConfigurationManager.AppSettings["RedisServer"];
            StaticObject.RedisPort = int.Parse(ConfigurationManager.AppSettings["RedisPort"]);
            StaticObject.RedisPassword = ConfigurationManager.AppSettings["RedisPassword"];
            StaticObject.RedisDb = long.Parse(ConfigurationManager.AppSettings["RedisDb"]);
            StaticObject.CacheDictionary = new System.Collections.Generic.Dictionary<string, int>();
            StaticObject.CacheDictionary.Add(StaticObject.Cache_Kf_LoginToken, int.Parse(ConfigurationManager.AppSettings["KF_LoginToken_Timeout"]));
            StaticObject.CacheDictionary.Add(StaticObject.Cache_Kf_MessageToken2CustomerOpenId, int.Parse(ConfigurationManager.AppSettings["KF_MessageToken2CustomerOpenId_Timeout"]));
            StaticObject.CacheDictionary.Add(StaticObject.Cache_Kf_OpenId2Customer, int.Parse(ConfigurationManager.AppSettings["KF_OpenId2Customer_Timeout"]));
            StaticObject.CacheDictionary.Add(StaticObject.Cache_Kf_FanOpenId2Conversation, int.Parse(ConfigurationManager.AppSettings["KF_FanOpenId2Conversation_Timeout"]));
            StaticObject.CacheDictionary.Add(StaticObject.Cache_Kf_MpFansByOpenId, int.Parse(ConfigurationManager.AppSettings["KF_MpFansByOpenId_Timeout"]));
            StaticObject.CacheDictionary.Add(StaticObject.Cache_DefaultWxKf, int.Parse(ConfigurationManager.AppSettings["DefaultWxKf_Timeout"]));
            StaticObject.ConnectionStrings = new System.Collections.Generic.Dictionary<string, string>();
            StaticObject.ConnectionStrings.Add("Default", ConfigurationManager.ConnectionStrings["Default"].ConnectionString);
            StaticObject.ConnectionStrings.Add("CY", ConfigurationManager.ConnectionStrings["CY"].ConnectionString);
            StaticObject.ConnectionStrings.Add("GroupMessage", ConfigurationManager.ConnectionStrings["GroupMessage"].ConnectionString);
            StaticObject.ConnectionStrings.Add("Kf", ConfigurationManager.ConnectionStrings["Kf"].ConnectionString);
            StaticObject.ConnectionStrings.Add("Signalr", ConfigurationManager.ConnectionStrings["Signalr"].ConnectionString);
            StaticObject.ApiTokenUrl = ConfigurationManager.AppSettings["ApiTokenUrl"];
            StaticObject.Token = ConfigurationManager.AppSettings["Token"];

            StaticObject.MapDirPath = ConfigurationManager.AppSettings["MapDirPath"];
            StaticObject.MapDirUserName = ConfigurationManager.AppSettings["MapDirUserName"];
            StaticObject.MapDirPwd = ConfigurationManager.AppSettings["MapDirPwd"];
            StaticObject.MapDirRemotingName = ConfigurationManager.AppSettings["MapDirRemotingName"];
            StaticObject.MapDirRemotingIP = ConfigurationManager.AppSettings["MapDirRemotingIP"];
            StaticObject.RemotingFileDownloadUrl = ConfigurationManager.AppSettings["RemotingFileDownloadUrl"];
            
        }
    }
}
