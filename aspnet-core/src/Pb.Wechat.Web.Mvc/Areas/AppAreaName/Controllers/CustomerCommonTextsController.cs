using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Pb.Wechat.Authorization;
using Pb.Wechat.Web.Controllers;
using Pb.Wechat.CustomerServiceResponseTexts;
using Abp.Application.Services.Dto;
using Pb.Wechat.Web.Areas.AppAreaName.Models.CustomerServiceResponseTexts;
using Pb.Wechat.UserMps;
using Pb.Wechat.CustomerServiceConversations.Dto;
using Pb.Wechat.CustomerServiceResponseTexts.Dto;
using System;
using System.IO;
using Pb.Wechat.Web.Resources.FileServers;
using Pb.Wechat.MpEvents.Dto;
using Pb.Wechat.Web.Resources.WxMediaHelper;
using System.Linq;
using Abp.IO.Extensions;
using Microsoft.AspNetCore.Http;
using Abp.Domain.Uow;

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_CustomerService_CustomerServiceResponseText)]
    public class CustomerCommonTextsController : AbpZeroTemplateControllerBase
    {
        private readonly ICustomerServiceResponseTextAppService _CustomerServiceResponseTextAppService;
        private readonly IUserMpAppService _userMpAppService;
        private readonly IFileServer _fileServer;
        private readonly IWxMediaUpload _wxMediaUpload;
        public CustomerCommonTextsController(ICustomerServiceResponseTextAppService CustomerServiceResponseTextAppService, IUserMpAppService userMpAppService, IFileServer fileServer, IWxMediaUpload wxMediaUpload)
        {
            _CustomerServiceResponseTextAppService = CustomerServiceResponseTextAppService;
            _userMpAppService = userMpAppService;
            _fileServer = fileServer;
            _wxMediaUpload = wxMediaUpload;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var mpid = await _userMpAppService.GetDefaultMpId();
            var model = new CustomerServiceResponseTextViewModel() { MpID=mpid};
            return View(model);
        }
        [UnitOfWork(IsDisabled = true)]
        public async Task<PartialViewResult> CreateOrEditModal(int id)
        {
            if (id!=0)
            {
                var output = await _CustomerServiceResponseTextAppService.Get(new EntityDto<int>(id));
                var viewModel = new CreateOrEditCustomerServiceResponseTextViewModel(output);

                return PartialView("_CreateOrEditModal", viewModel);
            }
            else
            {
                var mpid = await _userMpAppService.GetDefaultMpId();
                return PartialView("_CreateOrEditModal", new CreateOrEditCustomerServiceResponseTextViewModel() { MpID = mpid,ReponseContentType= (int)CustomerServiceMsgType.text,ResponseType=ResponseType.common.ToString() });
            }
                
        }
        [UnitOfWork(IsDisabled =true)]
        public async Task<JsonResult> SaveAndUpdate(CustomerServiceResponseTextDto input)
        {
            CustomerServiceResponseTextDto result = null;
            input.LastModificationTime = DateTime.Now;
            if (Request.Form.Files.Count > 0)
            {
                IFormFile profilePictureFile = null;
                string fileUrl = null;
                if (input.ReponseContentType==(int) CustomerServiceMsgType.image)
                    profilePictureFile = Request.Form.Files.First(m=>m.Name== "UploadPic");
                else if (input.ReponseContentType == (int)CustomerServiceMsgType.video)
                    profilePictureFile = Request.Form.Files.First(m => m.Name == "UploadVideo");
                else if (input.ReponseContentType == (int)CustomerServiceMsgType.voice)
                    profilePictureFile = Request.Form.Files.First(m => m.Name == "UploadVoice");

                if (profilePictureFile!=null)
                {
                    Logger.Info("开始上传文件");
                    byte[] fileBytes;
                    using (var stream = profilePictureFile.OpenReadStream())
                    {
                        fileBytes = stream.GetAllBytes();
                    }
                    Logger.Info("文件大小:" + fileBytes.Length);
                    var fileInfo = new FileInfo(profilePictureFile.FileName);
                    var extra = fileInfo.Extension.Substring(fileInfo.Extension.IndexOf(".") + 1);
                    fileUrl = await _fileServer.UploadFile(fileBytes, extra, "CustomerCommon");

                    input.PreviewImgUrl = fileUrl;
                    var filename = fileUrl.Substring(fileUrl.LastIndexOf("/") + 1);
                }

                if (!string.IsNullOrWhiteSpace(fileUrl))
                {
                    if (input.ReponseContentType == (int)CustomerServiceMsgType.image)
                        input.MediaId = await _wxMediaUpload.UploadAndGetMediaID(input.MpID, fileUrl, MpMessageType.image);
                    else if (input.ReponseContentType == (int)CustomerServiceMsgType.video)
                        input.MediaId = await _wxMediaUpload.UploadAndGetMediaID(input.MpID, fileUrl, MpMessageType.voice);
                    else if (input.ReponseContentType == (int)CustomerServiceMsgType.voice)
                        input.MediaId = await _wxMediaUpload.UploadVideoAndGetMediaID(input.MpID, fileUrl, MpMessageType.video,input.Title,input.Description);
                    else
                    {
                        if (string.IsNullOrEmpty(input.MediaId))
                            input.MediaId = Guid.NewGuid().ToString();
                    }
                }

            }

            if (input.Id == 0)
                result = await _CustomerServiceResponseTextAppService.Create(input);
            else
                result = await _CustomerServiceResponseTextAppService.Update(input);
            return Json(result);
        }

        public PartialViewResult MpImageSelector()
        {
            return PartialView("_MpImageSelector");
        }
       
        public PartialViewResult MpVoiceSelector()
        {
            return PartialView("_MpVoiceSelector");
        }
        public PartialViewResult MpVideoSelector()
        {
            return PartialView("_MpVideoSelector");
        }
    }
}
