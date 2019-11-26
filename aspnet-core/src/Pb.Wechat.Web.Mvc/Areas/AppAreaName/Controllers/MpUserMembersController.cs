using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Pb.Wechat.Authorization;
using Pb.Wechat.Web.Controllers;
using Pb.Wechat.MpUserMembers;
using Pb.Wechat.Web.Areas.AppAreaName.Models.MpUserMembers;
using Pb.Wechat.UserMps;

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_MpUserMembers)]
    public class MpUserMembersController : AbpZeroTemplateControllerBase
    {
        private readonly IMpUserMemberAppService _mpUserMemberAppService;
        private readonly IUserMpAppService _userMpAppService;
        public MpUserMembersController(IMpUserMemberAppService mpUserMemberAppService, IUserMpAppService userMpAppService)
        {
            _mpUserMemberAppService = mpUserMemberAppService;
            _userMpAppService = userMpAppService;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var viewModel = new MpUserMemberViewModel();
            viewModel.MpID = await _userMpAppService.GetDefaultMpId();
            return View(viewModel);
        }

        public async Task<PartialViewResult> CreateOrEditModal(string openid)
        {
            if (!string.IsNullOrWhiteSpace(openid))
            {
                var output = await _mpUserMemberAppService.GetByOpenID(openid);
                var viewModel = new CreateOrEditMpUserMemberViewModel(output);

                return PartialView("_CreateOrEditModal", viewModel);
            }
            else
                return PartialView("_CreateOrEditModal", new CreateOrEditMpUserMemberViewModel());
        }
    }
}
