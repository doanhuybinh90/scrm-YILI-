using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pb.Wechat.Authorization;
using Abp.AspNetCore.Mvc.Authorization;
using Pb.Wechat.Web.Controllers;
using Abp.Application.Services.Dto;
using Pb.Wechat.MpEvents;
using Pb.Wechat.Web.Areas.AppAreaName.Models.MpEvents;
using Pb.Wechat.UserMps;
using Pb.Wechat.MpEvents.Dto;
using Newtonsoft.Json;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_ReplyManagement_MpEvents, AppPermissions.Pages_ReplyManagement_MpFocus)]
    public class MpEventsController : AbpZeroTemplateControllerBase
    {
        private readonly IMpEventAppService _mpEventAppService;
        private readonly IUserMpAppService _userMpAppService;

        public MpEventsController(IMpEventAppService mpEventAppService, IUserMpAppService userMpAppService)
        {
            _mpEventAppService = mpEventAppService;
            _userMpAppService = userMpAppService;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var mpID = await _userMpAppService.GetDefaultMpId();
            var output = await _mpEventAppService.GetModelByEventTypeAsync(MpEventType.AutoReply.ToString(), mpID);
            if (output == null)
            {
                output = new MpEventDto();
                output.MpID = mpID;
                output.EventType = MpEventType.AutoReply.ToString();
            }
            return View(output);
        }
        public async Task<IActionResult> RIndex()
        {
            var mpID = await _userMpAppService.GetDefaultMpId();
            var output = await _mpEventAppService.GetModelByEventTypeAsync(MpEventType.Subscribe.ToString(), mpID);
            if (output == null)
            {
                output = new MpEventDto();
                output.MpID = mpID;
                output.EventType = MpEventType.Subscribe.ToString();
            }
            return View(output);
        }

        public async Task<PartialViewResult> CreateOrEditModal(int id)
        {
            if (id!=0)
            {
                var output = await _mpEventAppService.Get(new EntityDto<int>(id));
                var viewModel = new CreateOrEditMpEventViewModel(output);
                return PartialView("_CreateOrEditModal", viewModel);
            }
            else
            {
                var mpID=await _userMpAppService.GetDefaultMpId();
                var model = new CreateOrEditMpEventViewModel() { MpID = mpID };
                return PartialView("_CreateOrEditModal", model);
            }

        }

        public async Task<PartialViewResult> ContentSelector()
        {
            var mpID =await _userMpAppService.GetDefaultMpId();
            var output = await _mpEventAppService.GetModelByEventTypeAsync(MpEventType.AutoReply.ToString(), mpID);
            if (output == null)
            {
                output = new MpEventDto();
                output.MpID = mpID;
                output.EventType = MpEventType.AutoReply.ToString();
            }
            
            ReplyPartialModel model = JsonConvert.DeserializeObject<ReplyPartialModel>(JsonConvert.SerializeObject(output));
            return PartialView("_ContentSelector", model);
        }
        public async Task<PartialViewResult> RContentSelector()
        {
            var mpID =await _userMpAppService.GetDefaultMpId();
            var output = await _mpEventAppService.GetModelByEventTypeAsync(MpEventType.Subscribe.ToString(), mpID);
            if (output == null)
            {
                output = new MpEventDto();
                output.MpID = mpID;
                output.EventType = MpEventType.Subscribe.ToString();
            }

            ReplyPartialModel model = JsonConvert.DeserializeObject<ReplyPartialModel>(JsonConvert.SerializeObject(output));
            return PartialView("_ContentSelector", model);
        }

        public PartialViewResult MpImageSelector()
        {
            return PartialView("_MpImageSelector");
        }
        public PartialViewResult MpArticleGroupSelector()
        {
            return PartialView("_MpArticleGroupSelector");
        }
        public PartialViewResult MpArticleSelector()
        {
            return PartialView("_MpArticleSelector");
        }
        public PartialViewResult MpVoiceSelector()
        {
            return PartialView("_MpVoiceSelector");
        }
        public PartialViewResult MpVideoSelector()
        {
            return PartialView("_MpVideoSelector");
        }
    }
}
