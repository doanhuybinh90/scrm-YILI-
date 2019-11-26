using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pb.Wechat.Authorization;
using Abp.AspNetCore.Mvc.Authorization;
using Pb.Wechat.Web.Controllers;
using Abp.Application.Services.Dto;
using Pb.Wechat.MpProductInfos;
using Pb.Wechat.Web.Areas.AppAreaName.Models.MpProductInfos;
using Pb.Wechat.UserMps;
using Microsoft.AspNetCore.Hosting;
using System;
using Pb.Wechat.MpProductInfos.Dto;
using System.Linq;
using System.IO;
using Pb.Wechat.MpEvents.Dto;
using Pb.Wechat.Web.Resources.WxMediaHelper;
using Pb.Wechat.Web.Resources.FileServers;
using Abp.IO.Extensions;
using Pb.Wechat.MpMediaImages;
using Pb.Wechat.MpMediaImages.Dto;
using Pb.Wechat.Url;
using Pb.Wechat.MpAccounts;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_MpProducts_MpProductInfos)]
    public class MpProductInfosController : AbpZeroTemplateControllerBase
    {
        private readonly IMpProductInfoAppService _MpProductInfoAppService;
        private readonly IUserMpAppService _userMpAppService;
        private readonly IHostingEnvironment _host = null;
        private readonly IWxMediaUpload _wxMediaUpload;
        private readonly IFileServer _fileServer;
        private readonly IMpMediaImageAppService _mpMediaImageAppService;
        private readonly IWebUrlService _webUrlService;
        private readonly IMatialFileService _matialFileService;
        private readonly IMpAccountAppService _mpAccountAppService;
        public MpProductInfosController(IMpProductInfoAppService MpProductInfoAppService,
            IUserMpAppService userMpAppService,
            IHostingEnvironment host,
            IWxMediaUpload wxMediaUpload,
            IFileServer fileServer
            , IMpMediaImageAppService mpMediaImageAppService
            ,IWebUrlService webUrlService
            , IMatialFileService matialFileService
            , IMpAccountAppService mpAccountAppService)
            
        {
            _MpProductInfoAppService = MpProductInfoAppService;
            _userMpAppService = userMpAppService;
            _host = host;
            _wxMediaUpload = wxMediaUpload;
            _fileServer = fileServer;
            _mpMediaImageAppService = mpMediaImageAppService;
            _webUrlService = webUrlService;
            _matialFileService = matialFileService;
            _mpAccountAppService = mpAccountAppService;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var viewModel = new MpProductInfoViewModel();
            return View(viewModel);
        }

        public async Task<PartialViewResult> CreateOrEditModal(int id)
        {
            if (id != 0)
            {
                var output = await _MpProductInfoAppService.Get(new EntityDto<int>(id));
                var viewModel = new CreateOrEditMpProductInfoViewModel(output);
                return PartialView("_CreateOrEditModal", viewModel);
            }
            else
            {

                var model = new CreateOrEditMpProductInfoViewModel();
               
                return PartialView("_CreateOrEditModal", model);
            }

        }
               
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IActionResult> SaveAndUpdate(MpProductInfoDto input)
        {
            MpProductInfoDto result = null;
            //input.LastModificationTime = DateTime.Now;
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
                string fileUrl = await _fileServer.UploadFile(fileBytes, extra, MpMessageType.mpnews.ToString());
                input.FilePathOrUrl = fileUrl;
                var filename = fileUrl.Substring(fileUrl.LastIndexOf("/") + 1);
              
            }

            if (input.Id == 0)
                result = await _MpProductInfoAppService.Create(input);
            else
                result = await _MpProductInfoAppService.Update(input);
            return Json(result);
        }
    }
}
