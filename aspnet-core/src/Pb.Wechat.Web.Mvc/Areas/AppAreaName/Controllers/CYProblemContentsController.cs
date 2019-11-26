using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pb.Wechat.Authorization;
using Pb.Wechat.CYProblemContents;
using Pb.Wechat.Web.Areas.AppAreaName.Models.CYProblemContents;
using Pb.Wechat.Web.Controllers;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_CY_Problems)]
    public class CYProblemContentsController : AbpZeroTemplateControllerBase
    {
        private readonly ICYProblemContentAppService _cyProblemContentAppService;

        public CYProblemContentsController(ICYProblemContentAppService cyProblemContentAppService)
        {
            _cyProblemContentAppService = cyProblemContentAppService;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            if (id.HasValue)
            {
                var output = await _cyProblemContentAppService.Get(new EntityDto<int>(id.Value));
                var viewModel = new CreateOrEditCYProblemContentViewModel(output);

                return PartialView("_CreateOrEditModal", viewModel);
            }
            else
                return PartialView("_CreateOrEditModal", new CreateOrEditCYProblemContentViewModel());
        }
    }
}
