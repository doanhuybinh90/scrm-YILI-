using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Pb.Wechat.Authorization;
using Pb.Wechat.Web.Controllers;
using Pb.Wechat.MpChannels;
using Abp.Application.Services.Dto;
using Pb.Wechat.Web.Areas.AppAreaName.Models.MpChannels;
using Pb.Wechat.UserMps;
using Newtonsoft.Json;
using System.IO;
using Abp.Web.Models;
using Abp.Auditing;
using Pb.Wechat.MpFansTags;

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_MpManagers_MpChannels)]
    public class MpChannelsController : AbpZeroTemplateControllerBase
    {
        private readonly IMpChannelAppService _mpChannelAppService;
        private readonly IUserMpAppService _userMpAppService;
        private readonly IMpFansTagAppService _mpFansTagAppService;

        public MpChannelsController(IMpChannelAppService mpChannelAppService, IUserMpAppService userMpAppService, IMpFansTagAppService mpFansTagAppService)
        {
            _mpChannelAppService = mpChannelAppService;
            _userMpAppService = userMpAppService;
            _mpFansTagAppService = mpFansTagAppService;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var mpid = await _userMpAppService.GetDefaultMpId();
            var model = new MpChannelViewModel() { MpID=mpid};
            return View(model);
        }

        public async Task<PartialViewResult> CreateOrEditModal(int id)
        {
            if (id != 0)
            {
                var output = await _mpChannelAppService.Get(new EntityDto<int>(id));
                var viewModel = new CreateOrEditMpChannelViewModel(output);
                viewModel.Tags = await _mpFansTagAppService.GetAllTags(new MpFansTags.Dto.GetMpFansTagsInput());
                return PartialView("_CreateOrEditModal", viewModel);
            }
            else
            {
                var mpid = await _userMpAppService.GetDefaultMpId();
                var viewModel = new CreateOrEditMpChannelViewModel() { MpID = mpid };
                viewModel.Tags = await _mpFansTagAppService.GetAllTags(new MpFansTags.Dto.GetMpFansTagsInput());
                return PartialView("_CreateOrEditModal", viewModel);
            }

        }

        public async Task<PartialViewResult> ContentSelector(int id)
        {

            if (id != 0)
            {
                var output = await _mpChannelAppService.Get(new EntityDto<int>(id));
                var viewModel = new CreateOrEditMpChannelViewModel(output);
                ReplyPartialModel model = JsonConvert.DeserializeObject<ReplyPartialModel>(JsonConvert.SerializeObject(output));
                return PartialView("_ContentSelector", model);
            }
            else
            {

                return PartialView("_ContentSelector", new ReplyPartialModel());
            }

        }

        public async Task<PartialViewResult> Preview(int id)
        {
            ViewBag.ImageUrl = "";
            if (id != 0)
            {
                var output = await _mpChannelAppService.Get(new EntityDto<int>(id));
                ViewBag.ImageUrl = output.FileUrl;
            }
            return PartialView("_Preview");
        }
        [DontWrapResult]
        [DisableAuditing]
        public ActionResult DownloadFile(int id)
        {
            if (id != 0)
            {
                var output = _mpChannelAppService.Get(new EntityDto<int>(id)).Result;
                //var fileBytes = System.IO.File.ReadAllBytes(output.FilePath);

                var stream = System.IO.File.OpenRead(output.FilePath);
                return File(stream, "image/jpeg", Path.GetFileName(output.FilePath));
                //return File(fileBytes, "image/jpeg");
                //return File(new FileStream(output.FilePath, FileMode.Open), "jpeg/image");
            }
            return null;
        }
    }
}
