using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using yilibabyUser;
using Pb.Wechat.Web.Resources;
using System.Security.Cryptography;
using System;
using Abp.Runtime.Caching;
using Pb.Wechat.MpUserMembers;
using yilibabyMember;
using Pb.Wechat.Url;
using Pb.Wechat.MpBabyTexts.Dto;
using Pb.Wechat.MpBabyTexts;
using Abp.AspNetCore.Mvc.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Abp.Dependency;
using Pb.Wechat.CustomerServiceOnlines;
using Abp.Web.Models;

namespace Pb.Wechat.Web.Controllers
{
    [IgnoreAntiforgeryToken]
    [DontWrapResult]
    public class KfApiController : AbpController
    {
        private readonly ICacheManager _cacheManager;
        private readonly IIocResolver _iocResolver;
        private readonly ICustomerServiceOnlineAppService _customerServiceOnlineAppService;
        public KfApiController(ICacheManager cacheManager, IIocResolver iocResolver, ICustomerServiceOnlineAppService customerServiceOnlineAppService)
        {
            _cacheManager = cacheManager;
            _iocResolver = iocResolver;
            _customerServiceOnlineAppService = customerServiceOnlineAppService;
        }
    }
}