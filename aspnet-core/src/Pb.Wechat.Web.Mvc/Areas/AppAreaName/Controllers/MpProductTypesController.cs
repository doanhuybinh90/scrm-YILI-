using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.IO.Extensions;
using Microsoft.AspNetCore.Mvc;
using Pb.Wechat.Authorization;
using Pb.Wechat.MpEvents.Dto;
using Pb.Wechat.MpProductTypes;
using Pb.Wechat.MpProductTypes.Dto;
using Pb.Wechat.Web.Areas.AppAreaName.Models.MpProductTypes;
using Pb.Wechat.Web.Controllers;
using Pb.Wechat.Web.Resources.FileServers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_MpProducts_MpProductTypes)]
    public class MpProductTypesController : AbpZeroTemplateControllerBase
    {
        private readonly IMpProductTypeAppService _mpProductTypeAppService;
        private readonly IFileServer _fileServer;

        public MpProductTypesController(IMpProductTypeAppService mpProductTypeAppService
            ,IFileServer fileServer)
        {
            _mpProductTypeAppService = mpProductTypeAppService;
            _fileServer = fileServer;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            if (id.HasValue)
            {
                var output = await _mpProductTypeAppService.Get(new EntityDto<int>(id.Value));
                var viewModel = new CreateOrEditMpProductTypeViewModel(output);

                return PartialView("_CreateOrEditModal", viewModel);
            }
            else
                return PartialView("_CreateOrEditModal", new CreateOrEditMpProductTypeViewModel());
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IActionResult> SaveAndUpdate(MpProductTypeDto input)
        {
            MpProductTypeDto result = null;
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
                result = await _mpProductTypeAppService.Create(input);
            else
                result = await _mpProductTypeAppService.Update(input);
            return Json(result);
        }
    }
}
