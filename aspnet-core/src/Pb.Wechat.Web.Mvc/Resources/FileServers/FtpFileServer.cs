using Abp.IO.Extensions;
using Pb.Wechat.Url;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Resources.FileServers
{
    public class FtpFileServer: IFileServer
    {
        private readonly IAppFolders _appFolders;
        private readonly IWebUrlService _webUrlService;
        private readonly IFtpUploadService _ftpUploadService;
        public FtpFileServer(IAppFolders appFolders, IWebUrlService webUrlService, IFtpUploadService ftpUploadService)
        {
            _appFolders = appFolders;
            _webUrlService = webUrlService;
            _ftpUploadService = ftpUploadService;
        }

        public async Task<string> UploadFile(byte[] bytes, string extra, string type = "")
        {
            var typepath = string.IsNullOrEmpty(type) ? "" : $"{type}/";
            var currentPath = "";
            if (!string.IsNullOrEmpty(type))
            {
                if (!FtpHelper.DirectoryExist($"{_ftpUploadService.ServerDomain}{currentPath}", _ftpUploadService.UserName, _ftpUploadService.Password, type))
                {
                    FtpHelper.MakeDir($"{_ftpUploadService.ServerDomain}{currentPath}", _ftpUploadService.UserName, _ftpUploadService.Password, type);
                    currentPath = $"{currentPath}{type}/";
                }
            }
            var date = DateTime.Now.ToString("yyyy-MM-dd");
            if (!FtpHelper.DirectoryExist($"{_ftpUploadService.ServerDomain}{currentPath}", _ftpUploadService.UserName, _ftpUploadService.Password, date))
            {
                FtpHelper.MakeDir($"{_ftpUploadService.ServerDomain}{currentPath}", _ftpUploadService.UserName, _ftpUploadService.Password, date);
                currentPath = $"{currentPath}{date}/";
            }
            var filename = $"{GuidHelper.CreateTimeGuid()}.{extra}";
            await FtpHelper.Upload($"{_ftpUploadService.ServerDomain}{currentPath}", _ftpUploadService.UserName, _ftpUploadService.Password, bytes, filename);
            return $"{_ftpUploadService.ViewUrl}{currentPath}{filename}";
        }
        public async Task<string> UploadFile(Stream stream, string extra, string type = "")
        {
            return await UploadFile(stream.GetAllBytes(), extra, type);
        }

        public async Task<string> Download(string url, string extra = "", string type = "")
        {
            return await UploadFile(await new WebClient().DownloadDataTaskAsync(url), string.IsNullOrEmpty(extra) ? url.Split('/').LastOrDefault() : extra, type);
        }
    }
}
