using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pb.Wechat.Authorization;
using Pb.Wechat.CustomerServiceConversationMsgs;
using Pb.Wechat.Web.Areas.AppAreaName.Models.CustomerServiceConversationMsgs;
using Pb.Wechat.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_CustomerService_CustomerServiceOnline)]
    public class CustomerServiceConversationMsgsController : AbpZeroTemplateControllerBase
    {
        private readonly ICustomerServiceConversationMsgAppService _customerServiceConversationMsgAppService;

        public CustomerServiceConversationMsgsController(ICustomerServiceConversationMsgAppService customerServiceConversationMsgAppService)
        {
            _customerServiceConversationMsgAppService = customerServiceConversationMsgAppService;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
            if (id.HasValue)
            {
                var output = await _customerServiceConversationMsgAppService.Get(new EntityDto<long>(id.Value));
                var viewModel = new CreateOrEditCustomerServiceConversationMsgViewModel(output);

                return PartialView("_CreateOrEditModal", viewModel);
            }
            else
                return PartialView("_CreateOrEditModal", new CreateOrEditCustomerServiceConversationMsgViewModel());
        }
    }
}
