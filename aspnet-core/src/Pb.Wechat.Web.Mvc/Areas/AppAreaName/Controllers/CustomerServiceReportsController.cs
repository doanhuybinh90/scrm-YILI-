using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pb.Wechat.Authorization;
using Pb.Wechat.CustomerServiceReports;
using Pb.Wechat.UserMps;
using Pb.Wechat.Web.Areas.AppAreaName.Models.CustomerServiceReports;
using Pb.Wechat.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_CustomerService_CustomerServiceReports)]
    public class CustomerServiceReportsController : AbpZeroTemplateControllerBase
    {
        private readonly ICustomerServiceReportAppService _CustomerServiceReportAppService;
        private readonly IUserMpAppService _userMpAppService;
        public CustomerServiceReportsController(ICustomerServiceReportAppService CustomerServiceReportAppService, IUserMpAppService userMpAppService)
        {
            _CustomerServiceReportAppService = CustomerServiceReportAppService;
            _userMpAppService = userMpAppService;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var model = new CustomerServiceReportViewModel { MpID=await _userMpAppService .GetDefaultMpId()};
            return View(model);
        }

        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
            if (id.HasValue)
            {
                var output = await _CustomerServiceReportAppService.Get(new EntityDto<long>(id.Value));
                var viewModel = new CreateOrEditCustomerServiceReportViewModel(output);

                return PartialView("_CreateOrEditModal", viewModel);
            }
            else
                return PartialView("_CreateOrEditModal", new CreateOrEditCustomerServiceReportViewModel() { MpID=await _userMpAppService.GetDefaultMpId() });
        }
    }
}
