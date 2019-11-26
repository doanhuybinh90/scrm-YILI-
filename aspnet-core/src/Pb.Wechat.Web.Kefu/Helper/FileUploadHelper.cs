using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace Pb.Wechat.Web.Kefu.Helper
{
    public class FileUploadHelper
    {
        private static string filePath = StaticObject.MapDirRemotingName;
        private static string mapDirPath = StaticObject.MapDirPath;
        private static string mapDirUserName =StaticObject.MapDirUserName;
        private static string mapDirPwd = StaticObject.MapDirPwd;
        private static string mapDirRemotingName = StaticObject.MapDirRemotingName;
        private static string mapDirRemotingIP = StaticObject.MapDirRemotingIP;
        private readonly string fileDownloadUrl=StaticObject.RemotingFileDownloadUrl;
        public async Task<string> UploadFile(byte[] bytes, string extra, string type = "")
        {
            using (SharedTool tool = new SharedTool(mapDirUserName, mapDirPwd, mapDirRemotingIP))
            {

                var typepath = string.IsNullOrEmpty(type) ? "" : $"{type}/";
                if (!Directory.Exists($"{filePath}/{typepath}{DateTime.Now.ToString("yyyy-MM-dd")}"))
                {
                  
                    Directory.CreateDirectory($"{filePath}/{typepath}{DateTime.Now.ToString("yyyy-MM-dd")}");
                }


                string tmplname = $"{typepath}{DateTime.Now.ToString("yyyy-MM-dd")}/{Guid.NewGuid().ToString().Replace("-","")}.{extra}";
                var path = Path.Combine(filePath, tmplname);
               
              
                using (FileStream fs = File.Create(path))
                {
                    await fs.WriteAsync(bytes, 0, bytes.Length);
                }
              
                var result = fileDownloadUrl + $@"{tmplname}";

               
                return result;
            }
        }
        public async Task<string> UploadFile(Stream stream, string extra, string type = "")
        {
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                bytes= memoryStream.ToArray();
            }
            return await UploadFile(bytes, extra, type);
        }

        public async Task<string> Download(string url, string extra = "", string type = "")
        {
            return await UploadFile(await new WebClient().DownloadDataTaskAsync(url), string.IsNullOrEmpty(extra) ? url.Split('/').LastOrDefault() : extra, type);
        }
    }
}