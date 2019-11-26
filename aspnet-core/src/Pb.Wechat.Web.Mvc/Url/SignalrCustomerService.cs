using Abp.Dependency;
using Microsoft.AspNetCore.Hosting;
using Pb.Wechat.Url;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Url
{
    public class SignalrCustomerService : SignalrCustomerServiceBase, ISignalrCustomerService, ITransientDependency
    {
        public SignalrCustomerService(
            IHostingEnvironment hostingEnvironment) :
            base(hostingEnvironment)
        {
        }

        public override string CustomerAskUrlFormatKey => "App:SignalrCustomer:CustomerAskUrl";
        public override string SetUnConnectNoticeUrlFormatKey => "App:SignalrCustomer:SetUnConnectNoticeUrl";
    }
}
