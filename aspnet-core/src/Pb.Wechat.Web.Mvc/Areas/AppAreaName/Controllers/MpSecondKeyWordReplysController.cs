using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pb.Wechat.Authorization;
using Pb.Wechat.MpKeyWordReplys;
using Pb.Wechat.MpSecondKeyWordReplys;
using Pb.Wechat.UserMps;
using Pb.Wechat.Web.Areas.AppAreaName.Models.MpEvents;
using Pb.Wechat.Web.Areas.AppAreaName.Models.MpSecondKeyWordReplys;
using Pb.Wechat.Web.Controllers;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_ReplyManagement_MpKeyWordReplys)]
    public class MpSecondKeyWordReplysController : AbpZeroTemplateControllerBase
    {
        private readonly IMpSecondKeyWordReplyAppService _mpSecondKeyWordReplyAppService;
        private readonly IMpKeyWordReplyAppService _mpKeyWordReplyAppService;
        private readonly IUserMpAppService _userMpAppService;

        public MpSecondKeyWordReplysController(IMpSecondKeyWordReplyAppService mpSecondKeyWordReplyAppService, IMpKeyWordReplyAppService mpKeyWordReplyAppService, IUserMpAppService userMpAppService)
        {
            _mpSecondKeyWordReplyAppService = mpSecondKeyWordReplyAppService;
            _mpKeyWordReplyAppService = mpKeyWordReplyAppService;
            _userMpAppService = userMpAppService;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index(int? ParentId)
        {
            var viewModel = new MpSecondKeyWordReplyViewModel();
            if (ParentId.HasValue)
            {
                var keyword = await _mpKeyWordReplyAppService.Get(new EntityDto<int>() { Id = ParentId.Value });
                if (keyword != null)
                {
                    viewModel.ParentId = ParentId;
                    viewModel.KeyWord = keyword.KeyWord;
                }
            }
            viewModel.MpID = await _userMpAppService.GetDefaultMpId();
            return View(viewModel);
        }

        public async Task<PartialViewResult> CreateOrEditModal(int? id, int parentId, int mpId)
        {
            if (id.HasValue)
            {
                var output = await _mpSecondKeyWordReplyAppService.Get(new EntityDto<int>(id.Value));
                var viewModel = new CreateOrEditMpSecondKeyWordReplyViewModel(output);

                return PartialView("_CreateOrEditModal", viewModel);
            }
            else
                return PartialView("_CreateOrEditModal", new CreateOrEditMpSecondKeyWordReplyViewModel() { ParentId = parentId, MpID = mpId });
        }


        public async Task<PartialViewResult> ContentSelector(int id)
        {

            if (id != 0)
            {
                var output = await _mpSecondKeyWordReplyAppService.Get(new EntityDto<int>(id));
                var viewModel = new CreateOrEditMpSecondKeyWordReplyViewModel(output);
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
