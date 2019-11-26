using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pb.Wechat.Authorization;
using Abp.AspNetCore.Mvc.Authorization;
using Pb.Wechat.Web.Controllers;
using Abp.Application.Services.Dto;
using Pb.Wechat.MpKeyWordReplys;
using Pb.Wechat.Web.Areas.AppAreaName.Models.MpKeyWordReplys;
using Pb.Wechat.UserMps;
using Newtonsoft.Json;
using Pb.Wechat.Web.Areas.AppAreaName.Models.MpEvents;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_ReplyManagement_MpKeyWordReplys)]
    public class MpKeyWordReplysController : AbpZeroTemplateControllerBase
    {
        private readonly IMpKeyWordReplyAppService _mpKeyWordReplyAppService;
        private readonly IUserMpAppService _userMpAppService;

        public MpKeyWordReplysController(IMpKeyWordReplyAppService mpKeyWordReplyAppService, IUserMpAppService userMpAppService)
        {
            _mpKeyWordReplyAppService = mpKeyWordReplyAppService;
            _userMpAppService = userMpAppService;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var viewModel = new MpKeyWordReplyViewModel();
            viewModel.MpID = await _userMpAppService.GetDefaultMpId();
            return View(viewModel);
        }

        public async Task<PartialViewResult> CreateOrEditModal(int id)
        {
            if (id!=0)
            {
                var output = await _mpKeyWordReplyAppService.Get(new EntityDto<int>(id));
                var viewModel = new CreateOrEditMpKeyWordReplyViewModel(output);
                return PartialView("_CreateOrEditModal", viewModel);
            }
            else
            {
                var mpID= await _userMpAppService.GetDefaultMpId();
                var model = new CreateOrEditMpKeyWordReplyViewModel() { MpID = mpID};
                return PartialView("_CreateOrEditModal", model);
            }

        }


        public async Task<PartialViewResult> ContentSelector(int id)
        {

            if (id != 0)
            {
                var output = await _mpKeyWordReplyAppService.Get(new EntityDto<int>(id));
                var viewModel = new CreateOrEditMpKeyWordReplyViewModel(output);
                ReplyPartialModel model = JsonConvert.DeserializeObject<ReplyPartialModel>(JsonConvert.SerializeObject(output));
                return PartialView("_ContentSelector", model);
            }
            else
            {
               
                return PartialView("_ContentSelector", new ReplyPartialModel());
            }

        }

    }
}
