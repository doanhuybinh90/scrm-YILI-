using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pb.Wechat.Web.Kefu
{
    public class StaticObject
    {
        public static Dictionary<string, string> ConnectionStrings = null;
        public static Dictionary<string, int> CacheDictionary = null;
        public static string RedisConnectString = null;
        public static string RedisServer = null;
        public static int? RedisPort = null;
        public static string RedisPassword = null;
        public static long? RedisDb = null;
        public const string Cache_Kf_LoginToken = "KF_LoginToken";
        public const string Cache_Kf_MessageToken2CustomerOpenId = "KF_MessageToken2CustomerOpenId";
        public const string Cache_Kf_OpenId2Customer = "KF_OpenId2Customer";
        public const string Cache_OpenId2Customer = "OpenId2Customer";
        public const string Cache_Kf_MpFansByOpenId = "KF_MpFansByOpenId";
        public const string Cache_Kf_FanOpenId2Conversation = "KF_FanOpenId2Conversation";
        public const string Cache_FanOpenId2Conversation = "FanOpenId2Conversation";
        public const string Cache_DefaultWxKf = "DefaultWxKf";
        public static string ApiTokenUrl = null;
        public static string Token = null;

        public static string MapDirPath = null;
        public static string MapDirUserName = null;
        public static string MapDirPwd = null;
        public static string MapDirRemotingName = null;
        public static string MapDirRemotingIP = null;
        public static string RemotingFileDownloadUrl = null;
    }
}
