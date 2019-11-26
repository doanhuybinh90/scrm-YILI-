using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Pb.Wechat.Authorization;
using Pb.Wechat.Web.Controllers;
using Pb.Wechat.CustomerServiceWorkTimes;
using Abp.Application.Services.Dto;
using Pb.Wechat.Web.Areas.AppAreaName.Models.CustomerServiceWorkTimes;
using Pb.Wechat.UserMps;

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_CustomerService_CustomerServiceWorkTime)]
    public class CustomerServiceWorkTimesController : AbpZeroTemplateControllerBase
    {
        private readonly ICustomerServiceWorkTimeAppService _CustomerServiceWorkTimeAppService;
        private readonly IUserMpAppService _userMpAppService;

        public CustomerServiceWorkTimesController(ICustomerServiceWorkTimeAppService CustomerServiceWorkTimeAppService, IUserMpAppService userMpAppService)
        {
            _CustomerServiceWorkTimeAppService = CustomerServiceWorkTimeAppService;
            _userMpAppService = userMpAppService;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var mpid = await _userMpAppService.GetDefaultMpId();
            var model = new CustomerServiceWorkTimeViewModel() { MpID=mpid};
            return View(model);
        }
        
        public async Task<PartialViewResult> CreateOrEditModal(int id)
        {
            if (id!=0)
            {
                var output = await _CustomerServiceWorkTimeAppService.Get(new EntityDto<int>(id));
                var viewModel = new CreateOrEditCustomerServiceWorkTimeViewModel(output);

                return PartialView("_CreateOrEditModal", viewModel);
            }
            else
            {
                var mpid = await _userMpAppService.GetDefaultMpId();
                return PartialView("_CreateOrEditModal", new CreateOrEditCustomerServiceWorkTimeViewModel() { MpID = mpid });
            }
                
        }
    }
}
