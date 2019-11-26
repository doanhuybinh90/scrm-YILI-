using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Pb.Wechat.Authorization;
using Pb.Wechat.Web.Controllers;
using Pb.Wechat.CustomerServiceOnlines;
using Abp.Application.Services.Dto;
using Pb.Wechat.Web.Areas.AppAreaName.Models.CustomerServiceOnlines;
using Pb.Wechat.UserMps;
using Pb.Wechat.MpAccounts;
using Pb.Wechat.CustomerServiceOnlines.Dto;
using System.IO;
using System.Linq;
using System;
using Pb.Wechat.Web.Resources.FileServers;
using Abp.IO.Extensions;
using Pb.Wechat.Web.Resources.WxMediaHelper;
using Pb.Wechat.WxMedias;
using Abp.Domain.Uow;
using Pb.Wechat.Url;

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_CustomerService_CustomerServiceOnline)]
    public class CustomerServiceOnlinesController : AbpZeroTemplateControllerBase
    {
        private readonly ICustomerServiceOnlineAppService _CustomerServiceOnlineAppService;
        private readonly IUserMpAppService _userMpAppService;
        public readonly IMpAccountAppService _mpAccountAppService;
        private readonly IFileServer _fileServer;
        public readonly IWxMediaUpload _wxMediaUpload;
        private readonly IWxMediaAppService _wxMediaAppService;
        private readonly IWebUrlService _webUrlService;
        private readonly IMatialFileService _matialFileService;
        public CustomerServiceOnlinesController(ICustomerServiceOnlineAppService CustomerServiceOnlineAppService, IUserMpAppService userMpAppService, IMpAccountAppService mpAccountAppService, IFileServer fileServer, IWxMediaUpload wxMediaUpload,
            IWxMediaAppService wxMediaAppService, IWebUrlService webUrlService, IMatialFileService matialFileService)
        {
            _CustomerServiceOnlineAppService = CustomerServiceOnlineAppService;
            _userMpAppService = userMpAppService;
            _mpAccountAppService = mpAccountAppService;
            _fileServer = fileServer;
            _wxMediaUpload = wxMediaUpload;
            _wxMediaAppService = wxMediaAppService;
            _webUrlService = webUrlService;
            _matialFileService = matialFileService;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var mpid = await _userMpAppService.GetDefaultMpId();
            var model = new CustomerServiceOnlineViewModel() { MpID=mpid};
            return View(model);
        }
        
        public async Task<PartialViewResult> CreateOrEditModal(int id)
        {
            if (id!=0)
            {
                var output = await _CustomerServiceOnlineAppService.Get(new EntityDto<int>(id));
                var viewModel = new CreateOrEditCustomerServiceOnlineViewModel(output);

                return PartialView("_CreateOrEditModal", viewModel);
            }
            else
            {
                var mpid = await _userMpAppService.GetDefaultMpId();
               var account=await _mpAccountAppService.Get(new EntityDto<int> { Id = mpid });
                return PartialView("_CreateOrEditModal", new CreateOrEditCustomerServiceOnlineViewModel() { MpID = mpid,PublicNumberAccount=account.WxAccount,AutoJoin=false,ConnectState=(int)ConnectState.UnConnect,OnlineState=(int)OnlineState.Quit, KfType=KFType.YL.ToString(),CustomerServiceManager=false });
            }
                
        }
        [UnitOfWork(IsDisabled =true)]
        public async Task<IActionResult> SaveAndUpdate(CustomerServiceOnlineDto input)
        {
            CustomerServiceOnlineDto result = null;
            input.LastModificationTime = DateTime.Now;
            input.KfAccount = input.PreKfAccount + "@" + input.PublicNumberAccount;
            string fileUrl = string.Empty;

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
                fileUrl = await _fileServer.UploadFile(fileBytes, extra, "CustomerServiceOnline");
                input.LocalHeadingUrl = fileUrl;
                input.KfHeadingUrl = fileUrl;
                var filename = fileUrl.Substring(fileUrl.LastIndexOf("/") + 1);
              
            }

            Logger.Info($"客服信息上传开始：客服账号：{input.KfAccount}，客服昵称：{input.KfNick}，密码：{input.KfPassWord}，id={input.Id}");
            if (input.KfType == KFType.WX.ToString())
            {
                //上传客服信息
                if (input.Id == 0)
                    await _wxMediaAppService.AddCustom(input.MpID, input.KfAccount, input.KfNick, input.KfPassWord, input.KfWx);
                else
                    await _wxMediaAppService.UpdateCustom(input.MpID, input.KfAccount, input.KfNick, input.KfPassWord);

                if (Request.Form.Files.Count > 0)
                {

                    Logger.Info($"客服信息上传结束，开始上传客服头像,头像url：{fileUrl}");
                    //上传客服头像
                    await _wxMediaUpload.UploadKfHeading(input.MpID, fileUrl, input.KfAccount);
                }

            }
            else
                input.KfType = KFType.YL.ToString();

            if (input.Id == 0)
                 await _CustomerServiceOnlineAppService.Create(input);
            else
                await _CustomerServiceOnlineAppService.Update(input);
            return Json("");
        }

        [UnitOfWork(IsDisabled = true)]
        public async Task<PartialViewResult> BindingOpenID(int id)
        {
            ViewBag.ImageUrl = "";
            if (id != 0)
            {
                var head = _webUrlService.GetSiteRootAddress();
                var output = await _CustomerServiceOnlineAppService.Get(new EntityDto<int>(id));
                var access = await _mpAccountAppService.Get(new EntityDto<int> { Id = output.MpID });
                var reurl = Base64Helper.EncodeBase64(head + $"MpApi/BindOpenID?id={id}");
                ViewBag.BindingUrl = head+_matialFileService.Auth2Url;
                ViewBag.Token = access.TaskAccessToken;
                ViewBag.Reurl = reurl;
            }
            return PartialView("_BindingOpenID");
        }
       
    }
}
