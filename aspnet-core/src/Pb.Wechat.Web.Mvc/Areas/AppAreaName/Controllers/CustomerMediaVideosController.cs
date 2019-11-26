using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pb.Wechat.Authorization;
using Abp.AspNetCore.Mvc.Authorization;
using Pb.Wechat.Web.Controllers;
using Abp.Application.Services.Dto;
using Pb.Wechat.CustomerMediaVideos;
using Pb.Wechat.Web.Areas.AppAreaName.Models.CustomerMediaVideos;
using Pb.Wechat.UserMps;
using Pb.Wechat.CustomerMediaVideos.Dto;
using System.Linq;
using System.IO;
using Abp.IO.Extensions;
using Pb.Wechat.Web.Resources.WxMediaHelper;
using Pb.Wechat.Web.Resources.FileServers;
using Pb.Wechat.MpEvents.Dto;
using System;
using Abp.Domain.Uow;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_CustomerService_CustomerMartial_CustomerMediaVideos)]
    public class CustomerMediaVideosController : AbpZeroTemplateControllerBase
    {
        private readonly ICustomerMediaVideoAppService _CustomerMediaVideoAppService;
        private readonly IUserMpAppService _userMpAppService;
        private readonly IWxMediaUpload _wxMediaUpload;
        private readonly IFileServer _fileServer;
        public CustomerMediaVideosController(ICustomerMediaVideoAppService CustomerMediaVideoAppService, IUserMpAppService userMpAppService, IWxMediaUpload wxMediaUpload, IFileServer fileServer)
        {
            _CustomerMediaVideoAppService = CustomerMediaVideoAppService;
            _userMpAppService = userMpAppService;
            _wxMediaUpload = wxMediaUpload;
            _fileServer = fileServer;
        }
        // GET: /<controller>/
        [UnitOfWork(IsDisabled = true)]
        public async Task<IActionResult> Index()
        {
            var viewModel = new CustomerMediaVideoViewModel();
            viewModel.MpID =await _userMpAppService.GetDefaultMpId();
            return View(viewModel);
        }
        [UnitOfWork(IsDisabled = true)]
        public async Task<PartialViewResult> CreateOrEditModal(int id)
        {
            if (id!=0)
            {
                var output = await _CustomerMediaVideoAppService.Get(new EntityDto<int>(id));
                var viewModel = new CreateOrEditCustomerMediaVideoViewModel(output);
                return PartialView("_CreateOrEditModal", viewModel);
            }
            else
            {
                var mpID=await _userMpAppService.GetDefaultMpId();
                var model = new CreateOrEditCustomerMediaVideoViewModel() { MpID = mpID,MediaID="" };
                return PartialView("_CreateOrEditModal", model);
            }

        }

        /// <summary>
        /// 上传并保存微信素材
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(IsDisabled = true)]
        public async Task<IActionResult> SaveAndUpdate(CustomerMediaVideoDto input)
        {
            CustomerMediaVideoDto result = null;
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
                result = await _CustomerMediaVideoAppService.Create(input);
            else
                result = await _CustomerMediaVideoAppService.Update(input);
            return Json(result);
        }
    }
}
