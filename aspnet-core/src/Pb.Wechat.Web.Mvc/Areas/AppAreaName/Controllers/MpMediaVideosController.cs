using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pb.Wechat.Authorization;
using Abp.AspNetCore.Mvc.Authorization;
using Pb.Wechat.Web.Controllers;
using Abp.Application.Services.Dto;
using Pb.Wechat.MpMediaVideos;
using Pb.Wechat.Web.Areas.AppAreaName.Models.MpMediaVideos;
using Pb.Wechat.UserMps;
using Pb.Wechat.MpMediaVideos.Dto;
using System.Linq;
using System.IO;
using Abp.IO.Extensions;
using Pb.Wechat.Web.Resources.WxMediaHelper;
using Pb.Wechat.Web.Resources.FileServers;
using Pb.Wechat.MpEvents.Dto;
using System;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_MaterialManage_MpMediaVideos)]
    public class MpMediaVideosController : AbpZeroTemplateControllerBase
    {
        private readonly IMpMediaVideoAppService _mpMediaVideoAppService;
        private readonly IUserMpAppService _userMpAppService;
        private readonly IWxMediaUpload _wxMediaUpload;
        private readonly IFileServer _fileServer;
        public MpMediaVideosController(IMpMediaVideoAppService mpMediaVideoAppService, IUserMpAppService userMpAppService, IWxMediaUpload wxMediaUpload, IFileServer fileServer)
        {
            _mpMediaVideoAppService = mpMediaVideoAppService;
            _userMpAppService = userMpAppService;
            _wxMediaUpload = wxMediaUpload;
            _fileServer = fileServer;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var viewModel = new MpMediaVideoViewModel();
            viewModel.MpID =await _userMpAppService.GetDefaultMpId();
            return View(viewModel);
        }

        public async Task<PartialViewResult> CreateOrEditModal(int id)
        {
            if (id!=0)
            {
                var output = await _mpMediaVideoAppService.Get(new EntityDto<int>(id));
                var viewModel = new CreateOrEditMpMediaVideoViewModel(output);
                return PartialView("_CreateOrEditModal", viewModel);
            }
            else
            {
                var mpID=await _userMpAppService.GetDefaultMpId();
                var model = new CreateOrEditMpMediaVideoViewModel() { MpID = mpID,MediaID="" };
                return PartialView("_CreateOrEditModal", model);
            }

        }

        /// <summary>
        /// 上传并保存微信素材
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IActionResult> SaveAndUpdate(MpMediaVideoDto input)
        {
            MpMediaVideoDto result = null;
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
                string fileUrl = await _fileServer.UploadFile(fileBytes, extra, MpMessageType.video.ToString());
                input.FilePathOrUrl = fileUrl;
                var filename = fileUrl.Substring(fileUrl.LastIndexOf("/") + 1);
                
                input.MediaID = await _wxMediaUpload.UploadVideoAndGetMediaID(input.MpID, fileUrl, MpMessageType.video,input.Title,input.Description);

            }
            if (input.Id == 0)
                result = await _mpMediaVideoAppService.Create(input);
            else
                result = await _mpMediaVideoAppService.Update(input);
            return Json(result);
        }
    }
}
