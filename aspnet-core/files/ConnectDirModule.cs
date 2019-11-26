using files.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace files
{
    public class ConnectDirModule : IHttpModule
    {
       
        private static string mapDirPath = System.Configuration.ConfigurationManager.AppSettings["MapDirPath"];
        private static string mapDirUserName = System.Configuration.ConfigurationManager.AppSettings
["MapDirUserName"];
        private static string mapDirPwd = System.Configuration.ConfigurationManager.AppSettings["MapDirPwd"];
        private static string mapDirRemotingName = System.Configuration.ConfigurationManager.AppSettings["MapDirRemotingName"];

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(context_BeginRequest);
        }

        void context_BeginRequest(object sender, EventArgs e)
        {
            uint state = 0;
            if (!Directory.Exists(mapDirPath))
            {
                //Logger.Info("不存在该地址");
                try
                {
                    state = WNetHelper.WNetAddConnection(mapDirUserName, mapDirPwd, mapDirRemotingName, mapDirPath);
                }
                catch (Exception ex)
                {
                    
                    throw ex;
                }

            }
        }
    }
}