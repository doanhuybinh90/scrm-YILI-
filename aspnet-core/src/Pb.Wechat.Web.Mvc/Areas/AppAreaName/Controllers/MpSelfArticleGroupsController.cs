using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pb.Wechat.Authorization;
using Abp.AspNetCore.Mvc.Authorization;
using Pb.Wechat.Web.Controllers;
using Abp.Application.Services.Dto;
using Pb.Wechat.MpSelfArticleGroups;
using Pb.Wechat.Web.Areas.AppAreaName.Models.MpSelfArticleGroups;
using Pb.Wechat.UserMps;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_ReplyManagement_MpSelfArticleGroups)]
    public class MpSelfArticleGroupsController : AbpZeroTemplateControllerBase
    {
        private readonly IMpSelfArticleGroupAppService _mpSelfArticleGroupAppService;
        private readonly IUserMpAppService _userMpAppService;

        public MpSelfArticleGroupsController(IMpSelfArticleGroupAppService mpSelfArticleGroupAppService, IUserMpAppService userMpAppService)
        {
            _mpSelfArticleGroupAppService = mpSelfArticleGroupAppService;
            _userMpAppService = userMpAppService;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var viewModel = new MpSelfArticleGroupViewModel();
            viewModel.MpID = await _userMpAppService.GetDefaultMpId();
            return View(viewModel);
        }

        public async Task<PartialViewResult> CreateOrEditModal(int id)
        {
            if (id!=0)
            {
                var output = await _mpSelfArticleGroupAppService.Get(new EntityDto<int>(id));
                var viewModel = new CreateOrEditMpSelfArticleGroupViewModel(output);
                ViewBag.GroupID = id;
                return PartialView("_CreateOrEditModal", viewModel);
            }
            else
            {
                var mpID= await _userMpAppService.GetDefaultMpId();
                var model = new CreateOrEditMpSelfArticleGroupViewModel() { MpID = mpID };
                return PartialView("_CreateOrEditModal", model);
            }

        }

        public PartialViewResult InputNickName()
        {
            return PartialView("_InputNickName");
        }

        public PartialViewResult MpSelfArticleGroupSelector()
        {
            return PartialView("_MpSelfArticleGroupSelector");
        }

    }
}
