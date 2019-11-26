using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pb.Wechat.Authorization;
using Abp.AspNetCore.Mvc.Authorization;
using Pb.Wechat.Web.Controllers;
using Abp.Application.Services.Dto;
using Pb.Wechat.MpMediaVoices;
using Pb.Wechat.Web.Areas.AppAreaName.Models.MpMediaVoices;
using Pb.Wechat.UserMps;
using Pb.Wechat.MpMediaVoices.Dto;
using System;
using System.Linq;
using System.IO;
using Abp.IO.Extensions;
using Pb.Wechat.MpEvents.Dto;
using Pb.Wechat.Web.Resources.WxMediaHelper;
using Pb.Wechat.Web.Resources.FileServers;
using Abp.UI;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_MaterialManage_MpMediaVoices)]
    public class MpMediaVoicesController : AbpZeroTemplateControllerBase
    {
        private readonly IMpMediaVoiceAppService _mpMediaVoiceAppService;
        private readonly IUserMpAppService _userMpAppService;
        private readonly IWxMediaUpload _wxMediaUpload;
        private readonly IFileServer _fileServer;
        public MpMediaVoicesController(IMpMediaVoiceAppService mpMediaVoiceAppService, IUserMpAppService userMpAppService, IWxMediaUpload wxMediaUpload, IFileServer fileServer)
        {
            _mpMediaVoiceAppService = mpMediaVoiceAppService;
            _userMpAppService = userMpAppService;
            _wxMediaUpload = wxMediaUpload;
            _fileServer = fileServer;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var viewModel = new MpMediaVoiceViewModel();
            viewModel.MpID =await _userMpAppService.GetDefaultMpId();
            return View(viewModel);
        }

        public async Task<PartialViewResult> CreateOrEditModal(int id)
        {
            if (id!=0)
            {
                var output = await _mpMediaVoiceAppService.Get(new EntityDto<int>(id));
                var viewModel = new CreateOrEditMpMediaVoiceViewModel(output);
                return PartialView("_CreateOrEditModal", viewModel);
            }
            else
            {
                var mpID=await _userMpAppService.GetDefaultMpId();
                var model = new CreateOrEditMpMediaVoiceViewModel() { MpID = mpID,MediaID="" };
                return PartialView("_CreateOrEditModal", model);
            }

        }

        /// <summary>
        /// 上传并保存微信素材
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IActionResult> SaveAndUpdate(MpMediaVoiceDto input)
        {
           
            MpMediaVoiceDto result = null;
            input.LastModificationTime = DateTime.Now;
            if (Request.Form.Files.Count > 0)
            {
                var profilePictureFile = Request.Form.Files.First();
                byte[] fileBytes;
                using (var stream = profilePictureFile.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var fileInfo = new FileInfo(profilePictureFile.FileName);
                var extra = fileInfo.Extension.Substring(fileInfo.Extension.IndexOf(".") + 1);
                string fileUrl = await _fileServer.UploadFile(fileBytes, extra, MpMessageType.voice.ToString());
                input.FilePathOrUrl = fileUrl;
                var filename = fileUrl.Substring(fileUrl.LastIndexOf("/") + 1);

                input.MediaID = await _wxMediaUpload.UploadAndGetMediaID(input.MpID, fileUrl, MpMessageType.voice);
                if (string.IsNullOrWhiteSpace(input.MediaID))
                    throw new UserFriendlyException("对不起，请检查您提交的音频文件的时长。");
            }
            if (input.Id == 0)
                result = await _mpMediaVoiceAppService.Create(input);
            else
                result = await _mpMediaVoiceAppService.Update(input);
            return Json(result);
        }
    }
}
