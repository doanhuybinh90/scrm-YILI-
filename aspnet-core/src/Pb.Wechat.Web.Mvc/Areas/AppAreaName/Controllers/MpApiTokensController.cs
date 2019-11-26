using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pb.Wechat.Authorization;
using Abp.AspNetCore.Mvc.Authorization;
using Pb.Wechat.Web.Controllers;
using Pb.Wechat.MpApiTokens;
using Abp.Application.Services.Dto;
using Pb.Wechat.Web.Areas.AppAreaName.Models.MpApiTokens;
using System;
using Pb.Wechat.UserMps;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_MpManagers_MpApiTokens)]
    public class MpApiTokensController : AbpZeroTemplateControllerBase
    {
        private readonly IMpApiTokenAppService _mpApiTokenAppService;
        private readonly IUserMpAppService _userMpAppService;

        public MpApiTokensController(IMpApiTokenAppService mpApiTokenAppService, IUserMpAppService userMpAppService)
        {
            _mpApiTokenAppService = mpApiTokenAppService;
            _userMpAppService = userMpAppService;
        }
        // GET: /<controller>/
        public async Task <IActionResult> Index(int ParentId)
        {
            var viewModel = new MpApiTokenViewModel();
            viewModel.ParentId = await _userMpAppService.GetDefaultMpId();
            return View(viewModel);
        }

        public async Task<PartialViewResult> CreateOrEditModal(int id, int parentId)
        {
            if (id!=0)
            {
                var output = await _mpApiTokenAppService.Get(new EntityDto<int>(id));
                var viewModel = new CreateOrEditMpApiTokenViewModel(output);
                return PartialView("_CreateOrEditModal", viewModel);
            }
            else
            {
                var _parentId=await _userMpAppService.GetDefaultMpId();
                var model = new CreateOrEditMpApiTokenViewModel() { ParentId = _parentId };
                model.Token = Guid.NewGuid().ToString().Substring(0, 8);
                return PartialView("_CreateOrEditModal", model);
            }

        }
    }
}
