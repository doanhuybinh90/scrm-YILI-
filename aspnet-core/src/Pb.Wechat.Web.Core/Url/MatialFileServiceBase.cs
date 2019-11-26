using Microsoft.AspNetCore.Hosting;
using Pb.Wechat.Configuration;

namespace Pb.Wechat.Web.Url
{
    public abstract class MatialFileServiceBase
    {
        public abstract string MatialFileTempPathFormatKey { get; }
        public abstract string MapDirPathFormatKey { get; }
        public abstract string MapDirUserNameFormatKey { get; }
        public abstract string MapDirPwdFormatKey { get; }
        public abstract string MapDirRemotingNameFormatKey { get; }
        public abstract string MapDirRemotingIPFormatKey { get; }
        public abstract string MaMaBanToYun2OneFormatKey { get; }
        public abstract string JNHToLongLineFormatKey { get; }
        public abstract string WxOnlineUrlFormatKey { get; }
        public abstract string Auth2UrlFormatKey { get; }
        public abstract string PreviewUrlFormatKey { get; }

        public string MatialFileTempPath => _hostingEnvironment.GetAppConfiguration()[MatialFileTempPathFormatKey] ?? "/Temp/Downloads/";
        public string MapDirPath => _hostingEnvironment.GetAppConfiguration()[MapDirPathFormatKey] ?? "Z:";
        public string MapDirUserName => _hostingEnvironment.GetAppConfiguration()[MapDirUserNameFormatKey] ?? "administrator";
        public string MapDirPwd => _hostingEnvironment.GetAppConfiguration()[MapDirPwdFormatKey] ?? "4DbF56Tt";
        public string MapDirRemotingName => _hostingEnvironment.GetAppConfiguration()[MapDirRemotingNameFormatKey] ?? "\\\\172.31.17.225\\Image";
        public string MapDirRemotingIP => _hostingEnvironment.GetAppConfiguration()[MapDirRemotingIPFormatKey] ?? "172.31.17.225";
        public string MaMaBanToYun2One => _hostingEnvironment.GetAppConfiguration()[MaMaBanToYun2OneFormatKey] ?? "6rrXF4WV8t_YTONIjULcHvXUr8N7VHIk7JFSXLR5EnU";
        public string JNHToLongLine => _hostingEnvironment.GetAppConfiguration()[JNHToLongLineFormatKey] ?? "6rrXF4WV8t_YTONIjULcHvbvoe3qOV-1J0-OJBlDpMM";
        public string WxOnlineUrl => _hostingEnvironment.GetAppConfiguration()[WxOnlineUrlFormatKey] ?? "yilibabyclub/registfromwx";
        public string Auth2Url => _hostingEnvironment.GetAppConfiguration()[Auth2UrlFormatKey] ?? "MpApi/OAuth2Base";
        public string PreviewUrl=> _hostingEnvironment.GetAppConfiguration()[PreviewUrlFormatKey] ?? "MpApi/Preview";
        private readonly IHostingEnvironment _hostingEnvironment;
        public MatialFileServiceBase(
           IHostingEnvironment hostingEnvironment
       )
        {
            _hostingEnvironment = hostingEnvironment;
        }
    }
}
