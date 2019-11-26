using Microsoft.AspNetCore.Hosting;
using Pb.Wechat.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pb.Wechat.Web.Url
{
    public abstract class SignalrCustomerServiceBase
    {
        public abstract string CustomerAskUrlFormatKey { get; }
        public abstract string SetUnConnectNoticeUrlFormatKey { get; }
        public string CustomerAskUrl => _hostingEnvironment.GetAppConfiguration()[CustomerAskUrlFormatKey] ?? "";
        public string SetUnConnectNoticeUrl => _hostingEnvironment.GetAppConfiguration()[SetUnConnectNoticeUrlFormatKey] ?? "";
        private readonly IHostingEnvironment _hostingEnvironment;

        public SignalrCustomerServiceBase(
            IHostingEnvironment hostingEnvironment
        )
        {
            _hostingEnvironment = hostingEnvironment;
        }
    }
}
