using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pb.Wechat.Authorization;
using Pb.Wechat.MpFansTags;
using Pb.Wechat.UserMps;
using Pb.Wechat.Web.Areas.AppAreaName.Models.MpFansTags;
using Pb.Wechat.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_MpUserManagement_MpFansTags)]
    public class MpFansTagsController : AbpZeroTemplateControllerBase
    {
        private readonly IMpFansTagAppService _mpFansTagAppService;
        private readonly IUserMpAppService _userMpAppService;

        public MpFansTagsController(IMpFansTagAppService mpFansTagAppService, IUserMpAppService userMpAppService)
        {
            _mpFansTagAppService = mpFansTagAppService;
            _userMpAppService = userMpAppService;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            if (id.HasValue && id != 0)
            {
                var output = await _mpFansTagAppService.Get(new EntityDto<int>(id.Value));
                var viewModel = new CreateOrEditMpFansTagViewModel(output);
                return PartialView("_CreateOrEditModal", viewModel);
            }
            else
            {
                var mpID = await _userMpAppService.GetDefaultMpId();
                var model = new CreateOrEditMpFansTagViewModel() { MpID = mpID };
                return PartialView("_CreateOrEditModal", model);
            }
        }
    }
}
