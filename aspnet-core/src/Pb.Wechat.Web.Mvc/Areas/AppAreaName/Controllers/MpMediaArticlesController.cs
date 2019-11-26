using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pb.Wechat.Authorization;
using Abp.AspNetCore.Mvc.Authorization;
using Pb.Wechat.Web.Controllers;
using Abp.Application.Services.Dto;
using Pb.Wechat.MpMediaArticles;
using Pb.Wechat.Web.Areas.AppAreaName.Models.MpMediaArticles;
using Pb.Wechat.UserMps;
using Microsoft.AspNetCore.Hosting;
using System;
using Pb.Wechat.MpMediaArticles.Dto;
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
using Pb.Wechat.MpUserMembers;
using Pb.Wechat.WxMedias;
using Abp.Web.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pb.Wechat.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_MaterialManage_MpMediaArticles)]
    public class MpMediaArticlesController : AbpZeroTemplateControllerBase
    {
        private readonly IMpMediaArticleAppService _mpMediaArticleAppService;
        private readonly IUserMpAppService _userMpAppService;
        private readonly IHostingEnvironment _host = null;
        private readonly IWxMediaUpload _wxMediaUpload;
        private readonly IFileServer _fileServer;
        private readonly IMpMediaImageAppService _mpMediaImageAppService;
        private readonly IWebUrlService _webUrlService;
        private readonly IMatialFileService _matialFileService;
        private readonly IMpAccountAppService _mpAccountAppService;
        private readonly IMpUserMemberAppService _mpUserMemberAppService;
        private readonly IWxMediaAppService _wxMediaAppService;
        public MpMediaArticlesController(IMpMediaArticleAppService mpMediaArticleAppService,
            IUserMpAppService userMpAppService,
            IHostingEnvironment host,
            IWxMediaUpload wxMediaUpload,
            IFileServer fileServer
            , IMpMediaImageAppService mpMediaImageAppService
            ,IWebUrlService webUrlService
            , IMatialFileService matialFileService
            , IMpAccountAppService mpAccountAppService
            , IMpUserMemberAppService mpUserMemberAppService
            , IWxMediaAppService wxMediaAppService)
            
        {
            _mpMediaArticleAppService = mpMediaArticleAppService;
            _userMpAppService = userMpAppService;
            _host = host;
            _wxMediaUpload = wxMediaUpload;
            _fileServer = fileServer;
            _mpMediaImageAppService = mpMediaImageAppService;
            _webUrlService = webUrlService;
            _matialFileService = matialFileService;
            _mpAccountAppService = mpAccountAppService;
            _mpUserMemberAppService = mpUserMemberAppService;
            _wxMediaAppService = wxMediaAppService;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var viewModel = new MpMediaArticleViewModel();
            viewModel.MpID = await _userMpAppService.GetDefaultMpId();
            var account =await  _mpAccountAppService.Get(new EntityDto<int> { Id = viewModel.MpID });
            viewModel.MpName = account.Name;
            return View(viewModel);
        }

        public async Task<PartialViewResult> CreateOrEditModal(int id)
        {
            if (id != 0)
            {
                var output = await _mpMediaArticleAppService.Get(new EntityDto<int>(id));
                var viewModel = new CreateOrEditMpMediaArticleViewModel(output);
                viewModel.HostName = Request.Scheme + "://" + Request.Host.ToString();
                viewModel.SavePath =_webUrlService.KindEditorSavePath;
               
                return PartialView("_CreateOrEditModal", viewModel);
            }
            else
            {
                var mpID = await _userMpAppService.GetDefaultMpId();
                var model = new CreateOrEditMpMediaArticleViewModel() { MpID = mpID, MediaID = "", ShowPic = "1", HostName = Request.Scheme + "://" + Request.Host.ToString(), SavePath = _webUrlService.KindEditorSavePath };
                model.ArticleGrid = Guid.NewGuid().ToString().Replace("-", "");
                model.OnlyFansComment = 0;
                model.EnableComment = 1;
                return PartialView("_CreateOrEditModal", model);
            }

        }

        public PartialViewResult ImageSelectorModal()
        {
            return PartialView("_ImageSelector");
        }

        public PartialViewResult InputNickName()
        {
            return PartialView("_InputNickName");
        }

        public PartialViewResult Preview(string token, string mediaID, string mpId, string messageType)
        {

            //var head =  "http://yiliscrm3.mgcc.com.cn";

            var head = _webUrlService.GetSiteRootAddress();
            var reurl = Base64Helper.EncodeBase64( head + _matialFileService.PreviewUrl+ $"?mediaID={mediaID},{mpId},{messageType}");
            var url = head + _matialFileService.Auth2Url;
            ViewBag.Url = url;
            ViewBag.Token = token;
            ViewBag.Reurl = reurl;
            ViewBag.MediaID = mediaID;
            ViewBag.MpId = mpId;
            ViewBag.MessageType = messageType;
            return PartialView("_Preview");
        }
        /// <summary>
        /// 根根微信号或者手机号发送预览
        /// </summary>
        /// <param name="mobileOrwxAccount"></param>
        /// <param name="mpId"></param>
        /// <param name="mediaId"></param>
        /// <param name="messageType"></param>
        /// <returns></returns>
        [DontWrapResult]
        public async Task<JsonResult> SendPreview(string mobileOrwxAccount,int mpId,string mediaId,string messageType)
        {

            int _mobile = -1;
            int.TryParse(mobileOrwxAccount, out _mobile);
            if (_mobile>0)
            {
                var result=await _mpUserMemberAppService.GetByMobile(mobileOrwxAccount);
                if (result!=null)
                {
                    string openId = result.OpenID;
                    if (!string.IsNullOrWhiteSpace(openId))
                    {
                        try
                        {
                            var wxResult = await _wxMediaAppService.PreviewMatial(new WxMedias.Dto.PreviewModel
                            {
                                MediaID = mediaId,
                                MessageType = messageType,
                                MpID = mpId,
                                OpenID = openId
                            });
                            if (wxResult.errcode == Senparc.Weixin.ReturnCode.请求成功)
                                return Json(new { Success = true, Msg = "发送成功" });
                            else
                                return Json(new { Success = false, Msg = wxResult.errmsg });
                        }
                        catch (Exception ex)
                        {

                            return Json(new { Success = false, Msg = ex.Message });
                        }
                       
                    }
                   else
                    {
                        return Json(new { Success=false,Msg="该用户OpenID有误，不能发送预览消息"});
                    }
                }
                else
                {
                    try
                    {
                        var wxResult=await _wxMediaAppService.PreviewMatial(new WxMedias.Dto.PreviewModel
                        {
                            MediaID = mediaId,
                            MessageType = messageType,
                            MpID = mpId,
                            WxAccount = mobileOrwxAccount
                        });
                        if (wxResult.errcode==Senparc.Weixin.ReturnCode.请求成功)
                            return Json(new { Success = true, Msg = "发送成功" });
                        else
                            return Json(new { Success = false, Msg = wxResult.errmsg });
                    }
                    catch (Exception ex)
                    {

                        return Json(new { Success = false, Msg = ex.Message });
                    }
                }
            }
            else
            {
                try
                {
                    var wxResult = await _wxMediaAppService.PreviewMatial(new WxMedias.Dto.PreviewModel
                    {
                        MediaID = mediaId,
                        MessageType = messageType,
                        MpID = mpId,
                        WxAccount = mobileOrwxAccount
                    });
                    if (wxResult.errcode == Senparc.Weixin.ReturnCode.请求成功)
                        return Json(new { Success = true, Msg = "发送成功" });
                    else
                        return Json(new { Success = false, Msg = wxResult.errmsg });
                }
                catch (Exception ex)
                {

                    return Json(new { Success = false, Msg = ex.Message });
                }
            }
        }

        /// <summary>
        /// 上传并保存微信素材
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IActionResult> SaveAndUpdate(MpMediaArticleDto input)
        {
            MpMediaArticleDto result = null;
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
                string fileUrl = await _fileServer.UploadFile(fileBytes, extra, MpMessageType.mpnews.ToString());
                input.FilePathOrUrl = fileUrl;
                Logger.Info($"封面图片URL：{fileUrl}");
                var filename = fileUrl.Substring(fileUrl.LastIndexOf("/") + 1);
                input.PicMediaID = await _wxMediaUpload.UploadAndGetMediaID(input.MpID, fileUrl, MpMessageType.image);
                Logger.Info($"封面图片MediaID：{input.PicMediaID}");
                await _mpMediaImageAppService.Create(new MpMediaImageDto
                {
                    FileID = input.PicFileID,
                    FilePathOrUrl = input.FilePathOrUrl,
                    MpID = input.MpID,
                    Name = input.Title + "封面图片",
                    MediaID = input.PicMediaID,
                    LastModificationTime = DateTime.Now
                });
            }

            //更换微信图文中图片链接
           
            var domain = _webUrlService.GetServerRootAddress();
            domain=domain.Substring(0, domain.Length - 1);
            input.WxContent = await _mpMediaArticleAppService.UpdateWxContentString(input.Content, input.ArticleGrid, domain);
            //获取图文mediaID
            if (string.IsNullOrEmpty(input.MediaID))
            {
                input.MediaID = await _wxMediaUpload.UploadArticleAndGetMediaID(input.MpID, new Senparc.Weixin.MP.AdvancedAPIs.GroupMessage.NewsModel
                {
                    author = input.Author,
                    title = input.Title,
                    digest = input.Description,
                    content = input.WxContent,
                    content_source_url = input.AUrl,
                    show_cover_pic = input.ShowPic,
                    thumb_media_id = input.PicMediaID,
                    need_open_comment=input.EnableComment,
                    only_fans_can_comment= input.OnlyFansComment
                });
            }
            else
            {
                input.MediaID = await _wxMediaUpload.UploadArticleAndGetMediaID(input.MpID, new Senparc.Weixin.MP.AdvancedAPIs.GroupMessage.NewsModel
                {
                    author = input.Author,
                    title = input.Title,
                    digest = input.Description,
                    content = input.WxContent,
                    content_source_url = input.AUrl,
                    show_cover_pic = input.ShowPic,
                    thumb_media_id = input.PicMediaID,
                    need_open_comment = input.EnableComment,
                    only_fans_can_comment = input.OnlyFansComment
                },false,input.MediaID);
            }

            if (input.Id == 0)
                result = await _mpMediaArticleAppService.Create(input);
            else
                result = await _mpMediaArticleAppService.Update(input);
            return Json(result);
        }
    }
}
