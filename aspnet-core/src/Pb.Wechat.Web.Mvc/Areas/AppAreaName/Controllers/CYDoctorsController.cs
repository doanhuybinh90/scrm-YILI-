using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pb.Wechat.Authorization;
using Pb.Wechat.CYDoctors;
using Pb.Wechat.Web.Areas.AppAreaName.Models.CYDoctors;
using Pb.Wechat.Web.Controllers;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_CY_Doctors)]
    public class CYDoctorsController : AbpZeroTemplateControllerBase
    {
        private readonly ICYDoctorAppService _cyDoctorAppService;

        public CYDoctorsController(ICYDoctorAppService cyDoctorAppService)
        {
            _cyDoctorAppService = cyDoctorAppService;
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
                var output = await _cyDoctorAppService.Get(new EntityDto<int>(id.Value));
                var viewModel = new CreateOrEditCYDoctorViewModel(output);

                return PartialView("_CreateOrEditModal", viewModel);
            }
            else
                return PartialView("_CreateOrEditModal", new CreateOrEditCYDoctorViewModel());
        }
    }
}
