using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pb.Wechat.Authorization;
using Abp.AspNetCore.Mvc.Authorization;
using Pb.Wechat.Web.Controllers;
using Abp.Application.Services.Dto;
using Pb.Wechat.CustomerArticleGroups;
using Pb.Wechat.Web.Areas.AppAreaName.Models.CustomerArticleGroups;
using Pb.Wechat.UserMps;
using Abp.Domain.Uow;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_CustomerService_CustomerMartial_CustomerArticleGroups)]
    public class CustomerArticleGroupsController : AbpZeroTemplateControllerBase
    {
        private readonly ICustomerArticleGroupAppService _CustomerArticleGroupAppService;
        private readonly IUserMpAppService _userMpAppService;

        public CustomerArticleGroupsController(ICustomerArticleGroupAppService CustomerArticleGroupAppService, IUserMpAppService userMpAppService)
        {
            _CustomerArticleGroupAppService = CustomerArticleGroupAppService;
            _userMpAppService = userMpAppService;
        }
        // GET: /<controller>/
        [UnitOfWork(IsDisabled = true)]
        public async Task<IActionResult> Index()
        {
            var viewModel = new CustomerArticleGroupViewModel();
            viewModel.MpID = await _userMpAppService.GetDefaultMpId();
            return View(viewModel);
        }
        [UnitOfWork(IsDisabled = true)]
        public async Task<PartialViewResult> CreateOrEditModal(int id)
        {
            if (id!=0)
            {
                var output = await _CustomerArticleGroupAppService.Get(new EntityDto<int>(id));
                var viewModel = new CreateOrEditCustomerArticleGroupViewModel(output);
                ViewBag.GroupID = id;
                return PartialView("_CreateOrEditModal", viewModel);
            }
            else
            {
                var mpID= await _userMpAppService.GetDefaultMpId();
                var model = new CreateOrEditCustomerArticleGroupViewModel() { MpID = mpID };
                return PartialView("_CreateOrEditModal", model);
            }

        }
        [UnitOfWork(IsDisabled = true)]

        public PartialViewResult InputNickName()
        {
            return PartialView("_InputNickName");
        }
        [UnitOfWork(IsDisabled = true)]
        public PartialViewResult CustomerArticleGroupSelector()
        {
            return PartialView("_CustomerArticleGroupSelector");
        }

    }
}
