using System;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abp.Extensions;
using Abp.IO.Extensions;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Abp.UI;
using Abp.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pb.Wechat.IO;
using Pb.Wechat.MpAccessTokenClib;
using Pb.Wechat.MpAccounts;
using Pb.Wechat.MpEvents.Dto;
using Pb.Wechat.MpMediaImages;
using Pb.Wechat.MpMediaVideos;
using Pb.Wechat.MpMediaVoices;
using Pb.Wechat.Url;
using Pb.Wechat.UserMps;
using Pb.Wechat.Web.Helpers;
using Pb.Wechat.WxMedias;

namespace Pb.Wechat.Web.Controllers
{
    public class UploadController : AbpZeroTemplateControllerBase
    {

        private readonly IAppFolders _appFolders;
        private readonly ICacheManager _cacheManager;
        private readonly IMpMediaImageAppService _mpMediaImageAppService;
        private readonly IUserMpAppService _userMpAppService;
        private readonly IMpAccountAppService _mpAccountAppService;
        private readonly IAccessTokenContainer _accessTokenContainer;
        private readonly IWxMediaAppService _wxMediaAppService;
        private readonly IMpMediaVideoAppService _mpMediaVideoAppService;
        private readonly IMpMediaVoiceAppService _mpMediaVoiceAppService;
        private readonly IMatialFileService _matialFileService;
        public UploadController(IAppFolders appFolders, ICacheManager cacheManager, IMpMediaImageAppService mpMediaImageAppService, IUserMpAppService userMpAppService, IMpAccountAppService mpAccountAppService, IAccessTokenContainer accessTokenContainer, IWxMediaAppService wxMediaAppService, IMpMediaVideoAppService mpMediaVideoAppService, IMpMediaVoiceAppService mpMediaVoiceAppService,
            IMatialFileService matialFileService)
        {
            _appFolders = appFolders;
            _cacheManager = cacheManager;
            _mpMediaImageAppService = mpMediaImageAppService;
            _userMpAppService = userMpAppService;
            _mpAccountAppService = mpAccountAppService;
            _accessTokenContainer = accessTokenContainer;
            _wxMediaAppService = wxMediaAppService;
            _mpMediaVideoAppService = mpMediaVideoAppService;
            _mpMediaVoiceAppService = mpMediaVoiceAppService;
            _matialFileService = matialFileService;
        }
        /// <summary>
        /// 上传图片文件并上传至微信
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> UploadMatialPic()
        {
            try
            {
                var profilePictureFile = Request.Form.Files.First();

                //Check input
                if (profilePictureFile == null)
                {
                    throw new UserFriendlyException(L("ProfilePicture_Change_Error"));
                }

                if (profilePictureFile.Length > 2097152) //2MB.
                {
                    throw new UserFriendlyException(L("ProfilePicture_Warn_SizeLimit"));
                }

                byte[] fileBytes;
                using (var stream = profilePictureFile.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                if (!ImageFormatHelper.GetRawImageFormat(fileBytes).IsIn(ImageFormat.Jpeg, ImageFormat.Png, ImageFormat.Gif))
                {
                    throw new Exception("上传文件非图片文件");
                }

                //Delete old temp profile pictures
                AppFileHelper.DeleteFilesInFolderIfExists(_appFolders.TempFileDownloadFolder, "martialPic_" + AbpSession.GetUserId());

                //Save new picture
                var fileInfo = new FileInfo(profilePictureFile.FileName);
                var tempFileName = "martialPic_" + AbpSession.GetUserId() + Guid.NewGuid().ToString() + fileInfo.Extension;
                var tempFilePath = Path.Combine(_appFolders.TempFileDownloadFolder, tempFileName);
                await System.IO.File.WriteAllBytesAsync(tempFilePath, fileBytes);
                var virtualPath = _matialFileService.MatialFileTempPath + tempFileName;


                var mediaId = "";
                try
                {
                    mediaId = await _wxMediaAppService.UploadMedia(tempFilePath, "");//上传至微信
                }
                catch (Exception e)
                {
                    Logger.Error("上传微信错误，错误信息：" + e.Message + "；错误堆栈：" + e.StackTrace);
                }

                //var mediaId = "测试";


                return Json(new AjaxResponse(new { fileName = tempFileName, fileFullPath = tempFilePath, fileVirtualPath = virtualPath, mediaID = mediaId }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }
        /// <summary>
        /// 上传临时图片文件
        /// </summary>
        /// <returns></returns>

        public async Task<JsonResult> UploadMatialPicture()
        {
            try
            {
                var profilePictureFile = Request.Form.Files.First();

                //Check input
                if (profilePictureFile == null)
                {
                    throw new UserFriendlyException(L("ProfilePicture_Change_Error"));
                }

                if (profilePictureFile.Length > 2097152) //2MB.
                {
                    throw new UserFriendlyException(L("ProfilePicture_Warn_SizeLimit"));
                }

                byte[] fileBytes;
                using (var stream = profilePictureFile.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                if (!ImageFormatHelper.GetRawImageFormat(fileBytes).IsIn(ImageFormat.Jpeg, ImageFormat.Png, ImageFormat.Gif))
                {
                    throw new Exception("上传文件非图片文件");
                }

                //Delete old temp profile pictures
                AppFileHelper.DeleteFilesInFolderIfExists(_appFolders.TempFileDownloadFolder, "martialImage_" + AbpSession.GetUserId());

                //Save new picture
                var fileInfo = new FileInfo(profilePictureFile.FileName);
                var tempFileName = "martialImage_" + AbpSession.GetUserId() + Guid.NewGuid().ToString() + fileInfo.Extension;
                var tempFilePath = Path.Combine(_appFolders.TempFileDownloadFolder, tempFileName);
                await System.IO.File.WriteAllBytesAsync(tempFilePath, fileBytes);
                var virtualPath = _matialFileService.MatialFileTempPath + tempFileName;

                return Json(new AjaxResponse(new { fileName = tempFileName, fileFullPath = tempFilePath, fileVirtualPath = virtualPath }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }
        /// <summary>
        /// 上传临时视频文件
        /// </summary>
        /// <returns></returns>

        public async Task<JsonResult> UploadMatialVideo()
        {
            try
            {
                var profilePictureFile = Request.Form.Files.First();

                //Check input
                if (profilePictureFile == null)
                {
                    throw new UserFriendlyException(L("ProfilePicture_Change_Error"));
                }

                if (profilePictureFile.Length > 20971520) //20MB.
                {
                    throw new UserFriendlyException(L("ProfilePicture_Warn_SizeLimit"));
                }

                byte[] fileBytes;
                using (var stream = profilePictureFile.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var fileInfo = new FileInfo(profilePictureFile.FileName);
                if (fileInfo.Extension.ToLower() != ".mp4" && fileInfo.Extension.ToLower() != ".rmvb" && fileInfo.Extension.ToLower() != ".ts" && fileInfo.Extension.ToLower() != ".avi" && fileInfo.Extension.ToLower() != ".mxf" && fileInfo.Extension.ToLower() != ".mts" && fileInfo.Extension.ToLower() != ".mpg")
                {
                    throw new Exception("上传文件非视频文件");
                }

                //Delete old temp profile pictures
                //AppFileHelper.DeleteFilesInFolderIfExists(_appFolders.TempFileDownloadFolder, "martialVideo_" + AbpSession.GetUserId());

                //Save new picture
                var _tempfName = "martialVideo_" + AbpSession.GetUserId();

                var tempFileName = "martialVideo_" + AbpSession.GetUserId() + Guid.NewGuid().ToString() + fileInfo.Extension;
                var tempFilePath = Path.Combine(_appFolders.TempFileDownloadFolder, tempFileName);

                using (FileStream fs = System.IO.File.Create(tempFilePath))
                {
                    await fs.WriteAsync(fileBytes, 0, fileBytes.Length);
                }

                var virtualPath = _matialFileService.MatialFileTempPath + tempFileName;

                return Json(new AjaxResponse(new { fileName = tempFileName, fileFullPath = tempFilePath, fileVirtualPath = virtualPath }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }
        /// <summary>
        /// 上传临时音频文件
        /// </summary>
        /// <returns></returns>

        public async Task<JsonResult> UploadMatialVoice()
        {
            try
            {
                var profilePictureFile = Request.Form.Files.First();

                //Check input
                if (profilePictureFile == null)
                {
                    throw new UserFriendlyException(L("ProfilePicture_Change_Error"));
                }

                if (profilePictureFile.Length > 5242880) //5MB.
                {
                    throw new UserFriendlyException(L("ProfilePicture_Warn_SizeLimit"));
                }

                byte[] fileBytes;
                using (var stream = profilePictureFile.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var fileInfo = new FileInfo(profilePictureFile.FileName);
                if (fileInfo.Extension.ToLower() != ".mp3" && fileInfo.Extension.ToLower() != ".wma" && fileInfo.Extension.ToLower() != ".wav" && fileInfo.Extension.ToLower() != ".amr")
                {
                    throw new Exception("上传文件非音频文件");
                }

                //Delete old temp profile pictures
                //AppFileHelper.DeleteFilesInFolderIfExists(_appFolders.TempFileDownloadFolder, "martialVoice_" + AbpSession.GetUserId());

                //Save new picture

                var tempFileName = "martialVoice_" + AbpSession.GetUserId() + Guid.NewGuid().ToString() + fileInfo.Extension;
                var tempFilePath = Path.Combine(_appFolders.TempFileDownloadFolder, tempFileName);
                using (FileStream fs = System.IO.File.Create(tempFilePath))
                {
                    await fs.WriteAsync(fileBytes, 0, fileBytes.Length);
                }


                var virtualPath = _matialFileService.MatialFileTempPath + tempFileName;

                return Json(new AjaxResponse(new { fileName = tempFileName, fileFullPath = tempFilePath, fileVirtualPath = virtualPath }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        public async Task<IActionResult> Download(string mediaType, int id)
        {
            var tempProfilePicturePath = "";
            string contentType = "";
            if (mediaType == MpMessageType.video.ToString())
            {
                var data = await _mpMediaVideoAppService.Get(new Abp.Application.Services.Dto.EntityDto<int> { Id = id });
                tempProfilePicturePath = data.FileID;
                var extraFileType = tempProfilePicturePath.Substring(tempProfilePicturePath.LastIndexOf(".") + 1);
                if (extraFileType == "mp4")
                    contentType = "video/mpeg4";
                else if (extraFileType == "rmvb")
                    contentType = "application/vnd.rn-realmedia-vbr";
                else if (extraFileType == "avi")
                    contentType = "video/avi";
                else if (extraFileType == "mpg" || extraFileType == "mpeg")
                    contentType = "video/mpg";
                else
                    contentType = "application/file";


            }
            if (mediaType == MpMessageType.voice.ToString())
            {
                var data = await _mpMediaVoiceAppService.Get(new Abp.Application.Services.Dto.EntityDto<int> { Id = id });
                tempProfilePicturePath = data.FileID;
                var extraFileType = tempProfilePicturePath.Substring(tempProfilePicturePath.LastIndexOf(".") + 1);
                if (extraFileType == "mp3")
                    contentType = "audio/mp3";
                else if (extraFileType == "wma")
                    contentType = "audio/x-ms-wma";
                else if (extraFileType == "wav")
                    contentType = "	audio/wav";
                else
                    contentType = "application/file";

            }

            if (!string.IsNullOrWhiteSpace(tempProfilePicturePath))
            {
                var imgStream = new MemoryStream(System.IO.File.ReadAllBytes(tempProfilePicturePath));
                return File(imgStream, contentType);
            }
            else
                return null;
        }

        [DontWrapResult]
        public async Task<JsonResult> RemotingFileUpload(IFormFile file)
        {
            //var extra = Request.Query["extra"].ToString();
            //var file = Request.Form.Files.First();
            var type = Request.Query["type"].ToString();
            var typepath = string.IsNullOrEmpty(type) ? "" : $"{type}/";
            var relatepath = $"{typepath}{DateTime.Now.ToString("yyyy-MM-dd")}/";
            var dirPath = $"D:\\Temp/{relatepath}";

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            var webPath = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/Temp/{relatepath}";
            //var filename = $"{Guid.NewGuid().ToString()}.{extra}";
            byte[] fileBytes;
            using (var filestream = file.OpenReadStream())
            {
                fileBytes = filestream.GetAllBytes();
            }
            var fileInfo = new FileInfo(file.FileName);
            var tempFileName =  Guid.NewGuid().ToString() + fileInfo.Extension;
            var tempFilePath = Path.Combine(dirPath, tempFileName);
            await System.IO.File.WriteAllBytesAsync(tempFilePath, fileBytes);
            var aa = $"{webPath}{tempFileName}";
            return Json(new { success = true, data = aa });

            //var filename = Request.Query["fileName"].ToString();

            //Stream stream = HttpContext.Request.Body;
            //byte[] buffer = new byte[HttpContext.Request.ContentLength.Value];
            //stream.Read(buffer, 0, buffer.Length);
            //string content = Encoding.UTF8.GetString(buffer);
            //var filePath = $"{_appFolders.TempFileDownloadFolder}/{filename} ";
            //var dirPath = filePath.Substring(0,filePath.LastIndexOf("/"));
            //if (!Directory.Exists(dirPath))
            //    Directory.CreateDirectory(dirPath);
            //var tempFileName = filename;
            //var tempFilePath = Path.Combine(_appFolders.TempFileDownloadFolder, tempFileName);
            //using (FileStream fs = System.IO.File.Create(tempFilePath))
            //{
            //    await fs.WriteAsync(buffer, 0, buffer.Length);
            //}

            //return Json("");
        }
    }
}