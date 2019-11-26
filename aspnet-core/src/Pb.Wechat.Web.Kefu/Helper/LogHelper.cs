using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Pb.Wechat.Web.Kefu.Helper
{
    public class LogHelper
    {
        #region 静态参数
        private static log4net.ILog Instance = null;
        private static string DefaultName = "log";
        private static string path = null;
        #endregion

        #region 构造函数
        public LogHelper()
        {
            path = AppDomain.CurrentDomain.BaseDirectory + "Log4Net.config";
            log4net.Config.XmlConfigurator.Configure(new FileInfo(path));
            Instance = log4net.LogManager.GetLogger(DefaultName);
        }
        public LogHelper(string configPath)
        {
            path = configPath;
            log4net.Config.XmlConfigurator.Configure(new FileInfo(path));
            Instance = log4net.LogManager.GetLogger(DefaultName);
        }
        public LogHelper(string configPath, string logNode)
        {
            path = configPath;
            DefaultName = logNode;
            log4net.Config.XmlConfigurator.Configure(new FileInfo(path));
            Instance = log4net.LogManager.GetLogger(DefaultName);
        }
        #endregion

        #region 写入日志
        public void Error(string text)
        {
            if (Instance.IsErrorEnabled)
                Instance.Error(text);
        }

        public void Info(string text)
        {
            if (Instance.IsInfoEnabled)
                Instance.Info(text);
        }

        public void Debug(string text)
        {
            if (Instance.IsDebugEnabled)
                Instance.Debug(text);
        }

        public void Fatal(string text)
        {
            if (Instance.IsFatalEnabled)
                Instance.Fatal(text);
        }

        public void Warn(string text)
        {
            if (Instance.IsWarnEnabled)
                Instance.Warn(text);
        }


        public void Error(string text, Exception ex)
        {
            if (Instance.IsErrorEnabled)
                Instance.Error(text, ex);
        }

        public void Info(string text, Exception ex)
        {
            if (Instance.IsInfoEnabled)
                Instance.Info(text, ex);
        }

        public void Debug(string text, Exception ex)
        {
            if (Instance.IsDebugEnabled)
                Instance.Debug(text, ex);
        }

        public void Fatal(string text, Exception ex)
        {
            if (Instance.IsFatalEnabled)
                Instance.Fatal(text, ex);
        }

        public void Warn(string text, Exception ex)
        {
            if (Instance.IsWarnEnabled)
                Instance.Warn(text, ex);
        }
        #endregion
    }
}