using Abp.IO.Extensions;
using Abp.UI;
using Castle.Core.Logging;
using Pb.Wechat.Url;
using Pb.Wechat.Web.Helpers;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Resources.FileServers
{
    public class MapDirFileServer : IFileServer
    {
        private readonly IAppFolders _appFolders;
        private readonly IWebUrlService _webUrlService;
        private readonly string fileDownloadUrl;
        private readonly ILogger Logger;
        private readonly IMatialFileService _matialFileService;
        private static string filePath = "";
        private static string mapDirPath = "";
        private static string mapDirUserName = "";
        private static string mapDirPwd = "";
        private static string mapDirRemotingName = "";
        private static string mapDirRemotingIP = "";

        public MapDirFileServer(IAppFolders appFolders, IWebUrlService webUrlService, ILogger _Logger, IMatialFileService matialFileService)
        {
            _appFolders = appFolders;
            _webUrlService = webUrlService;
            fileDownloadUrl = webUrlService.GetRemotingFileDownloadUrl();
            Logger = _Logger;
            _matialFileService = matialFileService;
            filePath = _matialFileService.MapDirRemotingName;
            mapDirPath = _matialFileService.MapDirPath;
            mapDirPwd = _matialFileService.MapDirPwd;
            mapDirRemotingName = _matialFileService.MapDirRemotingName;
            mapDirRemotingIP = _matialFileService.MapDirRemotingIP;
            mapDirUserName = _matialFileService.MapDirUserName;
        }

        public async Task<string> UploadFile(byte[] bytes, string extra, string type = "")
        {
            //bool status = false;
            ////链接
            //status = TransportConnectHelper.connectState(mapDirPath, "waveaccess", "waveaccessxxx");


            Logger.Info("进入本地上传文件");
            //uint state = 0;
            //if (!Directory.Exists(mapDirPath))
            //{
            //    Logger.Info("不存在该地址");
            //    try
            //    {
            //        state = WNetHelper.WNetAddConnection(mapDirUserName, mapDirPwd, mapDirRemotingName, mapDirPath);
            //    }
            //    catch (Exception e)
            //    {
            //        WNetHelper.WNetCancelConnection(mapDirUserName, 1, true);
            //        Logger.Error($"网络地址错误：{e.Message}");
            //        throw e;
            //    }

            //}
            //if (state.Equals(0))
            using (SharedTool tool = new SharedTool(mapDirUserName, mapDirPwd, mapDirRemotingIP))
            {

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
                var result = fileDownloadUrl + $@"{tmplname}";

                Logger.Info($"文件虚拟地址{result}");
                return result;
            }
            //else
            //{
            //    WNetHelper.WNetCancelConnection(mapDirUserName, 1, true);
            //    Logger.Info($"添加网络驱动器错误，错误号：{state.ToString()}");
            //    throw new UserFriendlyException($"添加网络驱动器错误，错误号：{state.ToString()}");
            //}



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
