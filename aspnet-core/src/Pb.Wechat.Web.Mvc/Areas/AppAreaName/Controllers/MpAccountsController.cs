using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Pb.Wechat.Authorization;
using Pb.Wechat.Web.Controllers;
using Pb.Wechat.MpAccounts;
using Abp.Application.Services.Dto;
using Pb.Wechat.Web.Areas.AppAreaName.Models.MpAccounts;

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_MpManagers_MpAccounts)]
    public class MpAccountsController : AbpZeroTemplateControllerBase
    {
        private readonly IMpAccountAppService _mpAccountAppService;

        public MpAccountsController(IMpAccountAppService mpAccountAppService)
        {
            _mpAccountAppService = mpAccountAppService;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<PartialViewResult> CreateOrEditModal(int id)
        {
            if (id!=0)
            {
                var output = await _mpAccountAppService.Get(new EntityDto<int>(id));
                var viewModel = new CreateOrEditMpAccountViewModel(output);

                return PartialView("_CreateOrEditModal", viewModel);
            }
            else
                return PartialView("_CreateOrEditModal", new CreateOrEditMpAccountViewModel());
        }
    }
}
