using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pb.Wechat.Authorization;
using Pb.Wechat.MpSolicitudeSettings;
using Pb.Wechat.UserMps;
using Pb.Wechat.Web.Areas.AppAreaName.Models.MpSolicitudeSettings;
using Pb.Wechat.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_MpSolicitude_Settings)]
    public class MpSolicitudeSettingsController : AbpZeroTemplateControllerBase
    {
        private readonly IMpSolicitudeSettingAppService _mpSolicitudeSettingAppService;
        private readonly IUserMpAppService _userMpAppService;

        public MpSolicitudeSettingsController(IMpSolicitudeSettingAppService mpSolicitudeSettingAppService, IUserMpAppService userMpAppService)
        {
            _mpSolicitudeSettingAppService = mpSolicitudeSettingAppService;
            _userMpAppService = userMpAppService;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var mpID = await _userMpAppService.GetDefaultMpId();
            var output = await _mpSolicitudeSettingAppService.GetDefault();
            if (output == null)
            {
                output = new MpSolicitudeSettings.Dto.MpSolicitudeSettingDto();
                output.MpID = mpID;
            }
            return View(new CreateOrEditMpSolicitudeSettingViewModel(output));
        }

        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            if (id.HasValue)
            {
                var output = await _mpSolicitudeSettingAppService.Get(new EntityDto<int>(id.Value));
                var viewModel = new CreateOrEditMpSolicitudeSettingViewModel(output);

                return PartialView("_CreateOrEditModal", viewModel);
            }
            else
                return PartialView("_CreateOrEditModal", new CreateOrEditMpSolicitudeSettingViewModel());
        }
    }
}
