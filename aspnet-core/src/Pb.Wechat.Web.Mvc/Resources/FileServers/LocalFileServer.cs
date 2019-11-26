using Abp.IO.Extensions;
using Castle.Core.Logging;
using Pb.Wechat.Url;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Resources.FileServers
{
    public class LocalFileServer : IFileServer
    {
        private readonly IAppFolders _appFolders;
        private readonly IWebUrlService _webUrlService;
        private readonly string fileDownloadUrl;
        private readonly ILogger Logger;
        private readonly IMatialFileService _matialFileService;
        private static string filePath = "";
        public LocalFileServer(IAppFolders appFolders, IWebUrlService webUrlService, ILogger _Logger, IMatialFileService matialFileService) {
            _appFolders = appFolders;
            _webUrlService = webUrlService;
            fileDownloadUrl = webUrlService.GetRemotingFileDownloadUrl();
            Logger = _Logger;
            _matialFileService = matialFileService;
            filePath = _matialFileService.MapDirPath;
        }

        public async Task<string> UploadFile(byte[] bytes,string extra, string type = "")
        {
            Logger.Info("进入本地上传文件");
            var typepath = string.IsNullOrEmpty(type) ? "" : $"{type}/";
            if (!Directory.Exists($"{filePath}/{typepath}{DateTime.Now.ToString("yyyy-MM-dd")}"))
            {
                Logger.Info($"创建地址:{filePath}/{typepath}{DateTime.Now.ToString("yyyy-MM-dd")}");
                Directory.CreateDirectory($"{filePath}/{typepath}{DateTime.Now.ToString("yyyy-MM-dd")}");
            }
                

            string tmplname = $"{typepath}{DateTime.Now.ToString("yyyy-MM-dd")}/{GuidHelper.CreateTimeGuid()}.{extra}";
            var path = Path.Combine(filePath, tmplname);
            Logger.Info($"文件存储物理地址{path}");
            //using (FileStream fs = File.Create($"{_appFolders.TempFileDownloadFolder}/{tmplname}"))
            using (FileStream fs = File.Create(path))
            {
                await fs.WriteAsync(bytes, 0, bytes.Length);
            }
            //return _webUrlService.GetServerRootAddress() + $@"Temp/Downloads/{tmplname}";
            var result= fileDownloadUrl + $@"/{tmplname}";
            Logger.Info($"文件虚拟地址{result}");
            return result;
        }
        public async Task<string> UploadFile(Stream stream, string extra, string type = "")
        {
            return await UploadFile(stream.GetAllBytes(),extra, type);
        }

        public async Task<string> Download(string url, string extra = "", string type = "")
        {
            return await UploadFile(await new WebClient().DownloadDataTaskAsync(url), string.IsNullOrEmpty(extra) ? url.Split('/').LastOrDefault() : extra, type);
        }
    }
}
