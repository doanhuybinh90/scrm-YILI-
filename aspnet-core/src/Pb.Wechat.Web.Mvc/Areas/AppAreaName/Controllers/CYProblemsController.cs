using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pb.Wechat.Authorization;
using Pb.Wechat.CYProblems;
using Pb.Wechat.Web.Areas.AppAreaName.Models.CYProblems;
using Pb.Wechat.Web.Controllers;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_CY_Problems)]
    public class CYProblemsController : AbpZeroTemplateControllerBase
    {
        private readonly ICYProblemAppService _cyProblemAppService;

        public CYProblemsController(ICYProblemAppService cyProblemAppService)
        {
            _cyProblemAppService = cyProblemAppService;
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
                var output = await _cyProblemAppService.Get(new EntityDto<int>(id.Value));
                var viewModel = new CreateOrEditCYProblemViewModel(output);

                return PartialView("_CreateOrEditModal", viewModel);
            }
            else
                return PartialView("_CreateOrEditModal", new CreateOrEditCYProblemViewModel());
        }
    }
}
