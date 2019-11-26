using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Pb.Wechat.Authorization;
using Pb.Wechat.Web.Controllers;
using Pb.Wechat.MpBabyTexts;
using Abp.Application.Services.Dto;
using Pb.Wechat.Web.Areas.AppAreaName.Models.MpBabyTexts;
using Pb.Wechat.UserMps;

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_MpBabyText)]
    public class MpBabyTextsController : AbpZeroTemplateControllerBase
    {
        private readonly IMpBabyTextAppService _MpBabyTextAppService;
        private readonly IUserMpAppService _userMpAppService;

        public MpBabyTextsController(IMpBabyTextAppService MpBabyTextAppService, IUserMpAppService userMpAppService)
        {
            _MpBabyTextAppService = MpBabyTextAppService;
            _userMpAppService = userMpAppService;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var mpid = await _userMpAppService.GetDefaultMpId();
            var model = new MpBabyTextViewModel() { MpID=mpid};
            return View(model);
        }
        
        public async Task<PartialViewResult> CreateOrEditModal(int id)
        {
            if (id!=0)
            {
                var output = await _MpBabyTextAppService.Get(new EntityDto<int>(id));
                var viewModel = new CreateOrEditMpBabyTextViewModel(output);

                return PartialView("_CreateOrEditModal", viewModel);
            }
            else
            {
                var mpid = await _userMpAppService.GetDefaultMpId();
                return PartialView("_CreateOrEditModal", new CreateOrEditMpBabyTextViewModel() { MpID = mpid });
            }
                
        }
    }
}
