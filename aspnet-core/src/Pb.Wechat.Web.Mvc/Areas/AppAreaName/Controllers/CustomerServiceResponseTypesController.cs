using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Pb.Wechat.Authorization;
using Pb.Wechat.Web.Controllers;
using Pb.Wechat.CustomerServiceResponseTypes;
using Abp.Application.Services.Dto;
using Pb.Wechat.Web.Areas.AppAreaName.Models.CustomerServiceResponseTypes;
using Pb.Wechat.UserMps;
using Pb.Wechat.CustomerServiceConversations.Dto;
using Pb.Wechat.CustomerServiceResponseTypes.Dto;
using System;
using System.IO;
using Pb.Wechat.Web.Resources.FileServers;
using Pb.Wechat.MpEvents.Dto;
using Pb.Wechat.Web.Resources.WxMediaHelper;
using System.Linq;
using Abp.IO.Extensions;
using Microsoft.AspNetCore.Http;

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    //[AbpMvcAuthorize(AppPermissions.Pages_CustomerService_CustomerServiceResponseType)]
    public class CustomerServiceResponseTypesController : AbpZeroTemplateControllerBase
    {
        private readonly ICustomerServiceResponseTypeAppService _CustomerServiceResponseTypeAppService;
        private readonly IUserMpAppService _userMpAppService;
        private readonly IFileServer _fileServer;
        private readonly IWxMediaUpload _wxMediaUpload;
        public CustomerServiceResponseTypesController(ICustomerServiceResponseTypeAppService CustomerServiceResponseTypeAppService, IUserMpAppService userMpAppService, IFileServer fileServer, IWxMediaUpload wxMediaUpload)
        {
            _CustomerServiceResponseTypeAppService = CustomerServiceResponseTypeAppService;
            _userMpAppService = userMpAppService;
            _fileServer = fileServer;
            _wxMediaUpload = wxMediaUpload;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var mpid = await _userMpAppService.GetDefaultMpId();
            var model = new CustomerServiceResponseTypeViewModel() { MpID=mpid};
            return View(model);
        }
        
        public async Task<PartialViewResult> CreateOrEditModal(int id)
        {
            if (id!=0)
            {
                var output = await _CustomerServiceResponseTypeAppService.Get(new EntityDto<int>(id));
                var viewModel = new CreateOrEditCustomerServiceResponseTypeViewModel(output);

                return PartialView("_CreateOrEditModal", viewModel);
            }
            else
            {
                var mpid = await _userMpAppService.GetDefaultMpId();
                return PartialView("_CreateOrEditModal", new CreateOrEditCustomerServiceResponseTypeViewModel() );
            }
                
        }
        
    }
}
