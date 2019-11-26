using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pb.Wechat.Authorization;
using Abp.AspNetCore.Mvc.Authorization;
using Pb.Wechat.Web.Controllers;
using Abp.Application.Services.Dto;
using Pb.Wechat.Web.Areas.AppAreaName.Models.CustomerArticles;
using Pb.Wechat.UserMps;
using Pb.Wechat.CustomerArticles.Dto;
using System;
using System.Linq;
using System.IO;
using Pb.Wechat.MpEvents.Dto;
using Abp.IO.Extensions;
using Pb.Wechat.Web.Resources.FileServers;
using Newtonsoft.Json;
using Abp.Domain.Uow;
using Pb.Wechat.CustomerArticles.Dto.CustomerArticles;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_CustomerService_CustomerMartial_CustomerArticles)]
    public class CustomerArticlesController : AbpZeroTemplateControllerBase
    {
        private readonly ICustomerArticleAppService _CustomerArticleAppService;
        private readonly IUserMpAppService _userMpAppService;
        private readonly IFileServer _fileServer;
        public CustomerArticlesController(ICustomerArticleAppService CustomerArticleAppService, IUserMpAppService userMpAppService,
            IFileServer fileServer)
        {
            _CustomerArticleAppService = CustomerArticleAppService;
            _userMpAppService = userMpAppService;
            _fileServer = fileServer;
        }
        // GET: /<controller>/
        [UnitOfWork(IsDisabled =true)]
        public async Task<IActionResult> Index()
        {
            var viewModel = new CustomerArticleViewModel();
            viewModel.MpID = await _userMpAppService.GetDefaultMpId();
            return View(viewModel);
        }
        [UnitOfWork(IsDisabled = true)]
        public async Task<PartialViewResult> CreateOrEditModal(int id)
        {
            if (id!=0)
            {
                var output = await _CustomerArticleAppService.Get(new EntityDto<int>(id));
                var viewModel = new CreateOrEditCustomerArticleViewModel(output);
                return PartialView("_CreateOrEditModal", viewModel);
            }
            else
            {
                var mpID= await _userMpAppService.GetDefaultMpId();
                var model = new CreateOrEditCustomerArticleViewModel() { MpID = mpID };
                return PartialView("_CreateOrEditModal", model);
            }

        }

        public PartialViewResult CustomerArticleSelector()
        {
            return PartialView("_CustomerArticleSelector");
        }

        /// <summary>
        /// 上传并保存微信图片素材
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(IsDisabled = true)]
        public async Task<IActionResult> SaveAndUpdate(CustomerArticleDto input)
        {
            CustomerArticleDto result = null;
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
                string fileUrl = await _fileServer.UploadFile(fileBytes, extra, MpMessageType.image.ToString());
                input.FilePathOrUrl = fileUrl;
                var filename = fileUrl.Substring(fileUrl.LastIndexOf("/") + 1);

            }
            if (string.IsNullOrWhiteSpace(input.MediaID))
                input.MediaID = Guid.NewGuid().ToString();
            if (input.Id == 0)
                result = await _CustomerArticleAppService.Create(input);
            else
                result = await _CustomerArticleAppService.Update(input);
            return Json(result);
        }
        [UnitOfWork(IsDisabled = true)]
        public PartialViewResult ImageSelectorModal()
        {
            return PartialView("_ImageSelector");
        }
    }
}
