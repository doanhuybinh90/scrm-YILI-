using System.IO;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Resources.FileServers
{
    public interface IFileServer
    {
        Task<string> UploadFile(byte[] bytes, string extra, string type = "");
        Task<string> UploadFile(Stream stream, string extra, string type = "");
        Task<string> Download(string url, string extra = "", string type = "");
    }
}
