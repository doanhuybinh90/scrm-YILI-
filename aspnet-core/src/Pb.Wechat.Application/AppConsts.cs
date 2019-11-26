namespace Pb.Wechat
{
    /// <summary>
    /// Some consts used in the application.
    /// </summary>
    public class AppConsts
    {
        /// <summary>
        /// Default page size for paged requests.
        /// </summary>
        public const int DefaultPageSize = 10;

        /// <summary>
        /// Maximum allowed page size for paged requests.
        /// </summary>
        public const int MaxPageSize = 1000;

        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public const string DefaultPassPhrase = "gsKxGZ012HLL3MI5";

        public const string Cache_AuthKey2OpenID = "AuthKey2OpenID";
        public const string Cache_OpenID2AuthKey = "OpenID2AuthKey";
        public const string Cache_MpUserMemberKey = "MpUserMember";
        public const string Cache_JsApiTicket = "JsApiTicket";
        public const string Cache_AccessToken = "AccessToken";
        public const string Cache_MpAccount = "MpAccount";
        public const string Cache_MpAccountToken = "MpAccountToken";
        public const string Cache_CurrentUserMp = "CurrentUserMp";
        public const string Cache_AesKeyModal = "AesKeyModal";
        public const string Cache_MCAuthKey = "MCAuthKey";
        public const string Cache_CallBack = "CallBack";
        public const string Cache_Service = "Service";
        public const string Cache_PreviewMartial = "PreviewMartial";
        public const string Cache_MpFansByOpenId = "MpFansByOpenId";
        public const string Cache_MpFansByUserId = "MpFansByUserId";
        public const string Cache_ChunyuConfig = "ChunyuConfig";
        public const string Cache_ChunyuLogin = "ChunyuLogin";
        public const string Cache_ChunyuProblemByOpenId = "ChunyuProblemByOpenId";
        public const string Cache_ChunyuProblemByCYId = "ChunyuProblemByCYId";
        public const string Cache_ChunyuProblemFirstContent = "ChunyuProblemFirstContent";
        public const string Cache_ChunyuDoctor = "ChunyuDoctor";
        public const string Cache_ChunyuProblemPreCloseLock = "ChunyuProblemPreCloseLock";
        public const string Cache_ChunyuProblemCloseLock = "ChunyuProblemCloseLock";

        public const string Cache_ActivityInfo = "ActivityInfo";
        public const string Cache_FirstKeyWordReply = "FirstKeyWordReply";

        public const string FileType_ChunyuVoice = "ChunyuVoice";
        public const string FileType_ChunyuImage = "ChunyuImage";
        public const string FileType_WxMedia = "WxMedia";
        public const string Cache_Kf_FanOpenId2Conversation = "KF_FanOpenId2Conversation";
        public const string Cache_OpenId2Customer = "OpenId2Customer";
        public const string Cache_FanOpenId2Conversation = "FanOpenId2Conversation";

        public const string Cache_SolicitudeDelayMinutes = "SolicitudeDelayMinutes";
        public const string Cache_SolicitudeUserSend = "SolicitudeUserSend";
        public static string MvcAppPath { get; set; }
    }
}
