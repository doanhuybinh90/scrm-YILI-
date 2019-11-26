using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Pb.Wechat.Authorization;
using Pb.Wechat.Web.Controllers;
using Pb.Wechat.MpMenus;
using Abp.Application.Services.Dto;
using Pb.Wechat.Web.Areas.AppAreaName.Models.MpMenus;
using Pb.Wechat.UserMps;
using Newtonsoft.Json;

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_MpMenus)]
    public class MpMenusController : AbpZeroTemplateControllerBase
    {
        private readonly IMpMenuAppService _mpMenuAppService;
        private readonly IUserMpAppService _userMpAppService;
        public MpMenusController(IMpMenuAppService mpMenuAppService, IUserMpAppService userMpAppService)
        {
            _mpMenuAppService = mpMenuAppService;
            _userMpAppService = userMpAppService;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public async Task<PartialViewResult> CreateOrEditModal(int id)
        {
            if (id != 0)
            {
                var output = await _mpMenuAppService.Get(new EntityDto<int>(id));
                var viewModel = new CreateOrEditMpMenuViewModel(output);

                return PartialView("_CreateOrEditModal", viewModel);
            }
            else
            {
                var model = new CreateOrEditMpMenuViewModel();
                model.MpID = await _userMpAppService.GetDefaultMpId();
                return PartialView("_CreateOrEditModal", model);
            }
                
        }

        public async Task<PartialViewResult> ContentSelector(int id)
        {
            var mpID = await _userMpAppService.GetDefaultMpId();
            ReplyPartialModel model = new ReplyPartialModel();
            if (id != 0)
            {
                var output = await _mpMenuAppService.Get(new EntityDto<int> { Id = id });
                model = JsonConvert.DeserializeObject<ReplyPartialModel>(JsonConvert.SerializeObject(output));
            }

            return PartialView("_ContentSelector", model);
        }

        public async Task<PartialViewResult> CreateTreeModal(int? parentId)
        {
            var mpID = await _userMpAppService.GetDefaultMpId();
            var model = new CreateOrEditMpMenuViewModel() { MpID = mpID, ParentID = parentId ?? 0,Type="click" };
            return PartialView("_CreateTreeModal", model);
        }

        
    }
}
