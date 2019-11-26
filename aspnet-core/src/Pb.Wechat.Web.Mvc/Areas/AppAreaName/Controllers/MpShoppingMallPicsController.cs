using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Pb.Wechat.Authorization;
using Pb.Wechat.Web.Controllers;
using Pb.Wechat.MpShoppingMallPics;
using Abp.Application.Services.Dto;
using Pb.Wechat.Web.Areas.AppAreaName.Models.MpShoppingMallPics;
using Pb.Wechat.UserMps;
using Pb.Wechat.MpShoppingMallPics.Dto;
using System;
using System.Linq;
using System.IO;
using Abp.IO.Extensions;
using Pb.Wechat.Web.Resources.FileServers;

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_MpShoppingMallPic)]
    public class MpShoppingMallPicsController : AbpZeroTemplateControllerBase
    {
        private readonly IMpShoppingMallPicAppService _MpShoppingMallPicAppService;
        private readonly IUserMpAppService _userMpAppService;
        private readonly IFileServer _fileServer;
        public MpShoppingMallPicsController(IMpShoppingMallPicAppService MpShoppingMallPicAppService,
            IUserMpAppService userMpAppService,
            IFileServer fileServer)
        {
            _MpShoppingMallPicAppService = MpShoppingMallPicAppService;
            _userMpAppService = userMpAppService;
            _fileServer = fileServer;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var mpid = await _userMpAppService.GetDefaultMpId();
            var model = new MpShoppingMallPicViewModel() { MpID=mpid};
            return View(model);
        }
        
        public async Task<PartialViewResult> CreateOrEditModal(int id)
        {
            if (id!=0)
            {
                var output = await _MpShoppingMallPicAppService.Get(new EntityDto<int>(id));
                var viewModel = new CreateOrEditMpShoppingMallPicViewModel(output);

                return PartialView("_CreateOrEditModal", viewModel);
            }
            else
            {
                var mpid = await _userMpAppService.GetDefaultMpId();
                return PartialView("_CreateOrEditModal", new CreateOrEditMpShoppingMallPicViewModel() { MpID = mpid });
            }
                
        }

        /// <summary>
        /// 上传并保存微信图片素材
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IActionResult> SaveAndUpdate(MpShoppingMallPicDto input)
        {
            MpShoppingMallPicDto result = null;
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
                string fileUrl = await _fileServer.UploadFile(fileBytes, extra, "MpShoppingMallPic");
                input.LocalPicUrl = fileUrl;
                var filename = fileUrl.Substring(fileUrl.LastIndexOf("/") + 1);
            }
            if (input.Id == 0)
                result = await _MpShoppingMallPicAppService.Create(input);
            else
                result = await _MpShoppingMallPicAppService.Update(input);
            return Json(result);
        }
    }
}
