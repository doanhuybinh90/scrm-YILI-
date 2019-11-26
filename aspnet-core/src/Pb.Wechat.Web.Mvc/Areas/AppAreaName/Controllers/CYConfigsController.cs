using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pb.Wechat.Authorization;
using Pb.Wechat.CYConfigs;
using Pb.Wechat.Web.Areas.AppAreaName.Models.CYConfigs;
using Pb.Wechat.Web.Controllers;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_CY_Problems)]
    public class CYConfigsController : AbpZeroTemplateControllerBase
    {
        private readonly ICYConfigAppService _cYConfigAppService;

        public CYConfigsController(ICYConfigAppService cYConfigAppService)
        {
            _cYConfigAppService = cYConfigAppService;
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
                var output = await _cYConfigAppService.Get(new EntityDto<int>(id.Value));
                var viewModel = new CreateOrEditCYConfigViewModel(output);

                return PartialView("_CreateOrEditModal", viewModel);
            }
            else
                return PartialView("_CreateOrEditModal", new CreateOrEditCYConfigViewModel());
        }
    }
}
