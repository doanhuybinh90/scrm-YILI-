using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pb.Wechat.Authorization;
using Abp.AspNetCore.Mvc.Authorization;
using Pb.Wechat.Web.Controllers;
using Abp.Application.Services.Dto;
using Pb.Wechat.MpMediaArticleGroups;
using Pb.Wechat.Web.Areas.AppAreaName.Models.MpMediaArticleGroups;
using Pb.Wechat.UserMps;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_MaterialManage_MpMediaArticleGroups)]
    public class MpMediaArticleGroupsController : AbpZeroTemplateControllerBase
    {
        private readonly IMpMediaArticleGroupAppService _mpMediaArticleGroupAppService;
        private readonly IUserMpAppService _userMpAppService;

        public MpMediaArticleGroupsController(IMpMediaArticleGroupAppService mpMediaArticleGroupAppService, IUserMpAppService userMpAppService)
        {
            _mpMediaArticleGroupAppService = mpMediaArticleGroupAppService;
            _userMpAppService = userMpAppService;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var viewModel = new MpMediaArticleGroupViewModel();
            viewModel.MpID = await _userMpAppService.GetDefaultMpId();
            return View(viewModel);
        }

        public async Task<PartialViewResult> CreateOrEditModal(int id)
        {
            if (id!=0)
            {
                var output = await _mpMediaArticleGroupAppService.Get(new EntityDto<int>(id));
                var viewModel = new CreateOrEditMpMediaArticleGroupViewModel(output);
                ViewBag.GroupID = id;
                return PartialView("_CreateOrEditModal", viewModel);
            }
            else
            {
                var mpID= await _userMpAppService.GetDefaultMpId();
                var model = new CreateOrEditMpMediaArticleGroupViewModel() { MpID = mpID };
                return PartialView("_CreateOrEditModal", model);
            }

        }

        public PartialViewResult InputNickName()
        {
            return PartialView("_InputNickName");
        }

    }
}
