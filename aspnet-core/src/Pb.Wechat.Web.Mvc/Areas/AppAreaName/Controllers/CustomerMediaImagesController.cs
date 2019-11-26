using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pb.Wechat.Authorization;
using Abp.AspNetCore.Mvc.Authorization;
using Pb.Wechat.Web.Controllers;
using Abp.Application.Services.Dto;
using Pb.Wechat.CustomerMediaImages;
using Pb.Wechat.Web.Areas.AppAreaName.Models.CustomerMediaImages;
using Pb.Wechat.UserMps;
using Pb.Wechat.CustomerMediaImages.Dto;
using System.Linq;
using Abp.IO.Extensions;
using Pb.Wechat.Web.Resources.FileServers;
using System.IO;
using Pb.Wechat.Web.Resources.WxMediaHelper;
using Pb.Wechat.MpEvents.Dto;
using System;
using Pb.Wechat.Url;

using Abp.Domain.Uow;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_CustomerService_CustomerMartial_CustomerMediaImages)]
    public class CustomerMediaImagesController : AbpZeroTemplateControllerBase
    {
        private readonly ICustomerMediaImageAppService _CustomerMediaImageAppService;
        private readonly IUserMpAppService _userMpAppService;
        private readonly IWxMediaUpload _wxMediaUpload;
        private readonly IFileServer _fileServer;
        private readonly IWebUrlService _webUrlService;
        private readonly string remotingUploadUrl;
        private readonly IAppFolders _appFolders;
  
        public CustomerMediaImagesController(ICustomerMediaImageAppService CustomerMediaImageAppService,
            IUserMpAppService userMpAppService,
            IWxMediaUpload wxMediaUpload
            ,IFileServer fileServer
            , IWebUrlService webUrlService, IAppFolders appFolders
           )
        {
            _CustomerMediaImageAppService = CustomerMediaImageAppService;
            _userMpAppService = userMpAppService;
            _wxMediaUpload = wxMediaUpload;
            _fileServer = fileServer;
            _webUrlService = webUrlService;
            remotingUploadUrl = _webUrlService.GetRemotingFileUploadUrl();
            _appFolders = appFolders;

        }
        // GET: /<controller>/
        [UnitOfWork(IsDisabled = true)]
        public async Task<IActionResult> Index()
        {
           
            var viewModel = new CustomerMediaImageViewModel();
            viewModel.MpID = await _userMpAppService.GetDefaultMpId();

            return View(viewModel);
        }
        [UnitOfWork(IsDisabled = true)]
        public async Task<PartialViewResult> CreateOrEditModal(int id)
        {
            if (id != 0)
            {
                var output = await _CustomerMediaImageAppService.Get(new EntityDto<int>(id));
                var viewModel = new CreateOrEditCustomerMediaImageViewModel(output);
                return PartialView("_CreateOrEditModal", viewModel);
            }
            else
            {
                var mpID = await _userMpAppService.GetDefaultMpId();
                var model = new CreateOrEditCustomerMediaImageViewModel() { MpID = mpID, MediaID = "" };
                return PartialView("_CreateOrEditModal", model);
            }

        }

        [UnitOfWork(IsDisabled = true)]
        public PartialViewResult UploadPicModal()
        {
            return PartialView("_UploadPicModal");
        }

        /// <summary>
        /// 上传并保存微信图片素材
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(IsDisabled = true)]
        public async Task<JsonResult> SaveAndUpdate(CustomerMediaImageDto input)
        {
            CustomerMediaImageDto result = null;
            input.LastModificationTime = DateTime.Now;
            if (Request.Form.Files.Count > 0)
            {
                var profilePictureFile = Request.Form.Files.First();
                Logger.Info("开始上传文件");
                byte[] fileBytes;
                using (var stream = profilePictureFile.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                Logger.Info("文件大小:"+fileBytes.Length);
                var fileInfo = new FileInfo(profilePictureFile.FileName);
                var extra = fileInfo.Extension.Substring(fileInfo.Extension.IndexOf(".") + 1);
                string fileUrl =await _fileServer.UploadFile(fileBytes, extra, MpMessageType.image.ToString());
               
                input.FilePathOrUrl = fileUrl;
                var filename = fileUrl.Substring(fileUrl.LastIndexOf("/") + 1);
                input.MediaID =await _wxMediaUpload.UploadAndGetMediaID(input.MpID, fileUrl, MpMessageType.image);

            }

            if (input.Id == 0)
                result =await _CustomerMediaImageAppService.Create(input);
            else
                result = await _CustomerMediaImageAppService.Update(input);
            return Json(result);
        }

        
    }
}
