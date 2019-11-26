using System;
using System.Collections.Generic;
using System.Text;

namespace Pb.Wechat.Url
{
    public interface IMatialFileService
    {
        string MatialFileTempPath { get; }
        string MapDirPath { get; }
        string MapDirUserName { get; }
        string MapDirPwd { get; }
        string MapDirRemotingName { get; }
        string MapDirRemotingIP { get; }
        string MaMaBanToYun2One { get; }
        string JNHToLongLine { get; }
        string WxOnlineUrl { get; }
        string Auth2Url { get; }
        string PreviewUrl { get; }
    }
}
