using Abp.IO.Extensions;
using Newtonsoft.Json;
using Pb.Wechat.Url;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Resources.FileServers
{
    public class RemotingFileServer : IFileServer
    {
        private readonly IWebUrlService _webUrlService;
        private readonly string remotingUploadUrl;
        private readonly IAppFolders _appFolders;
        public RemotingFileServer(IWebUrlService webUrlService, IAppFolders appFolders)
        {
            _webUrlService = webUrlService;
            remotingUploadUrl = _webUrlService.GetRemotingFileUploadUrl();
            _appFolders = appFolders;
        }
        public async Task<string> Download(string url, string extra = "", string type = "")
        {
            return await UploadFile(await new WebClient().DownloadDataTaskAsync(url), string.IsNullOrEmpty(extra) ? url.Split('/').LastOrDefault() : extra, type);
        }

        public async Task<string> UploadFile(Stream stream, string extra, string type = "")
        {
            return await UploadFile(stream.GetAllBytes(), extra, type);
        }

        public async Task<string> UploadFile(byte[] bytes, string extra, string type = "")
        {
            var typepath = string.IsNullOrEmpty(type) ? "" : $"{type}/";
            if (!Directory.Exists($"{_appFolders.TempFileDownloadFolder}/{typepath}{DateTime.Now.ToString("yyyy-MM-dd")}"))
                Directory.CreateDirectory($"{_appFolders.TempFileDownloadFolder}/{typepath}{DateTime.Now.ToString("yyyy-MM-dd")}");

            string tmplname = $"{typepath}{DateTime.Now.ToString("yyyy-MM-dd")}/{GuidHelper.CreateTimeGuid()}.{extra}";
            var filepath = $"{_appFolders.TempFileDownloadFolder}/{tmplname}";

            await System.IO.File.WriteAllBytesAsync(filepath, bytes);
           
            var client = new WebClient();
            client.Headers.Add("Content-Type", "x-www-form-urlencoded");
            client.Encoding = Encoding.UTF8;
            var uri = new Uri($"{remotingUploadUrl}?type={type}&extra={extra}");
            try
            {
                var result = client.UploadFile(uri, filepath);
                var model = JsonConvert.DeserializeObject<RemotingResultModel>(Encoding.UTF8.GetString(result));
                return model.success ? model.data : string.Empty;
            }
            catch (Exception e)
            {

                throw e;
            }
           
        }
    }

    public class RemotingResultModel {
        public bool success { get; set; }
        public string data { get; set; }
    }
}
