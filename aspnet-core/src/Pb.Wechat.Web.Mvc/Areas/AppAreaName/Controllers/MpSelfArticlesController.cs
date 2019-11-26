using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pb.Wechat.Authorization;
using Abp.AspNetCore.Mvc.Authorization;
using Pb.Wechat.Web.Controllers;
using Abp.Application.Services.Dto;
using Pb.Wechat.Web.Areas.AppAreaName.Models.MpSelfArticles;
using Pb.Wechat.UserMps;
using Pb.Wechat.MpSelfArticles.Dto.MpSelfArticles;
using Pb.Wechat.MpSelfArticles.Dto;
using System;
using System.Linq;
using System.IO;
using Pb.Wechat.MpEvents.Dto;
using Abp.IO.Extensions;
using Pb.Wechat.Web.Resources.FileServers;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_ReplyManagement_MpSelfArticles)]
    public class MpSelfArticlesController : AbpZeroTemplateControllerBase
    {
        private readonly IMpSelfArticleAppService _mpSelfArticleAppService;
        private readonly IUserMpAppService _userMpAppService;
        private readonly IFileServer _fileServer;
        public MpSelfArticlesController(IMpSelfArticleAppService mpSelfArticleAppService, IUserMpAppService userMpAppService,
            IFileServer fileServer)
        {
            _mpSelfArticleAppService = mpSelfArticleAppService;
            _userMpAppService = userMpAppService;
            _fileServer = fileServer;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var viewModel = new MpSelfArticleViewModel();
            viewModel.MpID = await _userMpAppService.GetDefaultMpId();
            return View(viewModel);
        }

        public async Task<PartialViewResult> CreateOrEditModal(int id)
        {
            if (id!=0)
            {
                var output = await _mpSelfArticleAppService.Get(new EntityDto<int>(id));
                var viewModel = new CreateOrEditMpSelfArticleViewModel(output);
                return PartialView("_CreateOrEditModal", viewModel);
            }
            else
            {
                var mpID= await _userMpAppService.GetDefaultMpId();
                var model = new CreateOrEditMpSelfArticleViewModel() { MpID = mpID };
                return PartialView("_CreateOrEditModal", model);
            }

        }

        public PartialViewResult MpSelfArticleSelector()
        {
            return PartialView("_MpSelfArticleSelector");
        }

        /// <summary>
        /// 上传并保存微信图片素材
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IActionResult> SaveAndUpdate(MpSelfArticleDto input)
        {
            MpSelfArticleDto result = null;
            input.LastModificationTime = DateTime.Now;
            if (Request.Form.Files.Count > 0)
            {
                Logger.Info($"回复图文上传");
                var profilePictureFile = Request.Form.Files.First();

                byte[] fileBytes;
                using (var stream = profilePictureFile.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var fileInfo = new FileInfo(profilePictureFile.FileName);
                var extra = fileInfo.Extension.Substring(fileInfo.Extension.IndexOf(".") + 1);
                string fileUrl = await _fileServer.UploadFile(fileBytes, extra, MpMessageType.image.ToString());
                Logger.Info($"回复图文url：{fileUrl}");
                input.FilePathOrUrl = fileUrl;
                var filename = fileUrl.Substring(fileUrl.LastIndexOf("/") + 1);

            }
            Logger.Info($"回复类：{JsonConvert.SerializeObject(input)}");
            if (input.Id == 0)
                result = await _mpSelfArticleAppService.Create(input);
            else
                result = await _mpSelfArticleAppService.Update(input);
            return Json(result);
        }
    }
}
