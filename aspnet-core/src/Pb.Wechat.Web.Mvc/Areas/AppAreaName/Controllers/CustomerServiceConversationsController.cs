using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Uow;
using Microsoft.AspNetCore.Mvc;
using Pb.Wechat.Authorization;
using Pb.Wechat.CustomerServiceConversations;
using Pb.Wechat.UserMps;
using Pb.Wechat.Web.Areas.AppAreaName.Models.CustomerServiceConversations;
using Pb.Wechat.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_CustomerService_CustomerServiceConversations)]
    public class CustomerServiceConversationsController : AbpZeroTemplateControllerBase
    {
        private readonly ICustomerServiceConversationAppService _customerServiceConversationAppService;
        private readonly IUserMpAppService _userMpAppService;
        public CustomerServiceConversationsController(ICustomerServiceConversationAppService customerServiceConversationAppService, IUserMpAppService userMpAppService)
        {
            _customerServiceConversationAppService = customerServiceConversationAppService;
            _userMpAppService = userMpAppService;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var model = new CustomerServiceConversationViewModel();
            model.MpID = await _userMpAppService.GetDefaultMpId();
            return View(model);
        }
        [UnitOfWork(IsDisabled =true)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
            if (id.HasValue)
            {
                var output = await _customerServiceConversationAppService.Get(new EntityDto<long>(id.Value));
                var viewModel = new CreateOrEditCustomerServiceConversationViewModel(output);

                return PartialView("_CreateOrEditModal", viewModel);
            }
            else
            {
                return PartialView("_CreateOrEditModal", new CreateOrEditCustomerServiceConversationViewModel() { MpID=await _userMpAppService .GetDefaultMpId()});
            }
               
        }
    }
}
