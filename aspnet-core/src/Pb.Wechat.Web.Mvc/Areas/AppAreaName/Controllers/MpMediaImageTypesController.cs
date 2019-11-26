using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pb.Wechat.Authorization;
using Abp.AspNetCore.Mvc.Authorization;
using Pb.Wechat.Web.Controllers;
using Abp.Application.Services.Dto;
using Pb.Wechat.MpMediaImageTypes;
using Pb.Wechat.Web.Areas.AppAreaName.Models.MpMediaImageTypes;
using Pb.Wechat.UserMps;
using Pb.Wechat.MpMediaImageTypes.Dto;
using System.Linq;
using Abp.IO.Extensions;
using Pb.Wechat.Web.Resources.FileServers;
using System.IO;
using Pb.Wechat.Web.Resources.WxMediaHelper;
using Pb.Wechat.MpEvents.Dto;
using System;
using Pb.Wechat.Url;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_MaterialManage_MpMediaImageTypes)]
    public class MpMediaImageTypesController : AbpZeroTemplateControllerBase
    {
        private readonly IMpMediaImageTypeAppService _MpMediaImageTypeAppService;
        private readonly IUserMpAppService _userMpAppService;
        private readonly IWxMediaUpload _wxMediaUpload;
        private readonly IFileServer _fileServer;
        private readonly IWebUrlService _webUrlService;
        private readonly string remotingUploadUrl;
        private readonly IAppFolders _appFolders;
        public MpMediaImageTypesController(IMpMediaImageTypeAppService MpMediaImageTypeAppService,
            IUserMpAppService userMpAppService,
            IWxMediaUpload wxMediaUpload
            ,IFileServer fileServer
            , IWebUrlService webUrlService, IAppFolders appFolders
            )
        {
            _MpMediaImageTypeAppService = MpMediaImageTypeAppService;
            _userMpAppService = userMpAppService;
            _wxMediaUpload = wxMediaUpload;
            _fileServer = fileServer;
            _webUrlService = webUrlService;
            remotingUploadUrl = _webUrlService.GetRemotingFileUploadUrl();
            _appFolders = appFolders;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var viewModel = new MpMediaImageTypeViewModel();
            viewModel.MpID = await _userMpAppService.GetDefaultMpId();
            return View(viewModel);
        }

        public async Task<PartialViewResult> CreateOrEditModal(int id)
        {
            if (id != 0)
            {
                var output = await _MpMediaImageTypeAppService.Get(new EntityDto<int>(id));
                var viewModel = new CreateOrEditMpMediaImageTypeViewModel(output);
                return PartialView("_CreateOrEditModal", viewModel);
            }
            else
            {
                var mpID = await _userMpAppService.GetDefaultMpId();
                var model = new CreateOrEditMpMediaImageTypeViewModel() { MpID = mpID};
                return PartialView("_CreateOrEditModal", model);
            }

        }


        
    }
}
