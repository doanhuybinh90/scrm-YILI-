using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pb.Wechat.Authorization;
using Pb.Wechat.MpSolicitudeTexts;
using Pb.Wechat.UserMps;
using Pb.Wechat.Web.Areas.AppAreaName.Models.MpSolicitudeTexts;
using Pb.Wechat.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_MpSolicitude_Texts)]
    public class MpSolicitudeTextsController : AbpZeroTemplateControllerBase
    {
        private readonly IMpSolicitudeTextAppService _mpSolicitudeTextAppService;
        private readonly IUserMpAppService _userMpAppService;

        public MpSolicitudeTextsController(IMpSolicitudeTextAppService mpSolicitudeTextAppService, IUserMpAppService userMpAppService)
        {
            _mpSolicitudeTextAppService = mpSolicitudeTextAppService;
            _userMpAppService = userMpAppService;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var mpid = await _userMpAppService.GetDefaultMpId();
            var model = new CreateOrEditMpSolicitudeTextViewModel() { MpID = mpid };
            return View(model);
        }

        public async Task<PartialViewResult> CreateOrEditModal(int id)
        {
            if (id != 0)
            {
                var output = await _mpSolicitudeTextAppService.Get(new EntityDto<int>(id));
                var viewModel = new CreateOrEditMpSolicitudeTextViewModel(output);

                return PartialView("_CreateOrEditModal", viewModel);
            }
            else
            {
                var mpid = await _userMpAppService.GetDefaultMpId();
                return PartialView("_CreateOrEditModal", new CreateOrEditMpSolicitudeTextViewModel() { MpID = mpid });
            }
        }
    }
}
