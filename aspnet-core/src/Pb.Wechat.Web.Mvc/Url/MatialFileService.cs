using Abp.Dependency;
using Microsoft.AspNetCore.Hosting;
using Pb.Wechat.Url;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Url
{
    public class MatialFileService : MatialFileServiceBase, IMatialFileService, ITransientDependency
    {
        public MatialFileService(
           IHostingEnvironment hostingEnvironment) :
           base(hostingEnvironment)
        {
        }
        public override string MatialFileTempPathFormatKey => "App:MatialFile:MatialFileTempPath";


        public override string MapDirPathFormatKey => "App:MatialFile:MapDirPath";

        public override string MapDirUserNameFormatKey => "App:MatialFile:MapDirUserName";

        public override string MapDirPwdFormatKey => "App:MatialFile:MapDirPwd";

        public override string MapDirRemotingNameFormatKey => "App:MatialFile:MapDirRemotingName";

        public override string MapDirRemotingIPFormatKey => "App:MatialFile:MapDirRemotingIP";

        public override string MaMaBanToYun2OneFormatKey => "App:MatialFile:MaMaBanToYun2One";
        public override string JNHToLongLineFormatKey => "App:MatialFile:JNHToLongLine";

        public override string WxOnlineUrlFormatKey => "App:MatialFile:WxOnlineUrl";

        public override string Auth2UrlFormatKey => "App:MatialFile:Auth2Url";
        public override string PreviewUrlFormatKey => "App:MatialFile:PreviewUrl";
    }
}
