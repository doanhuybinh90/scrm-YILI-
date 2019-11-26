//------------------------------------------------------------------------------
// <自动生成>
//     此代码由工具生成。
//     //
//     对此文件的更改可能导致不正确的行为，并在以下条件下丢失:
//     代码重新生成。
// </自动生成>
//------------------------------------------------------------------------------

namespace yilibabyUser
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://mmp.meichis.com/DataInterface/", ConfigurationName="yilibabyUser.UserLoginSoap")]
    public interface UserLoginSoap
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/HelloWorld", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> HelloWorldAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetServerSyncTime", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<System.DateTime> GetServerSyncTimeAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/Login1", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> Login1Async(string UserName, string Password, string DeviceCode);
        
        // CODEGEN: 正在生成消息协定，因为操作具有多个返回值。
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/LoginEx", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<yilibabyUser.LoginExResponse> LoginExAsync(yilibabyUser.LoginExRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/LoginAutoForSMS", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> LoginAutoForSMSAsync(string UserName, string Password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/Login_GetCurrentUser", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<yilibabyUser.UserInfo> Login_GetCurrentUserAsync(string AuthKey);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/Login_GetCurentUserInfo", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> Login_GetCurentUserInfoAsync(string AuthKey);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/Login_GetCurrentUserJson", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> Login_GetCurrentUserJsonAsync(string AuthKey);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/Login_GetNewMsg", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> Login_GetNewMsgAsync(string AuthKey);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/Logout", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> LogoutAsync(string AuthKey);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/CheckUsername", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<bool> CheckUsernameAsync(string AuthKey, string username);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/CheckMobile", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<bool> CheckMobileAsync(string AuthKey, string mobile);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/CheckEmail", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<bool> CheckEmailAsync(string AuthKey, string email);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/UserRegister", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> UserRegisterAsync(string AuthKey, string mobile, string username, string pwd, string email);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/UserRegister2", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> UserRegister2Async(string AuthKey, string mobile, string username, string pwd, string email);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/UserRegisterExWithMediaID", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> UserRegisterExWithMediaIDAsync(string AuthKey, string mobile, string username, string pwd, string email, string securityQuestion, string answer, int source, string registinfo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/UserRegisterEx", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> UserRegisterExAsync(string AuthKey, string mobile, string username, string pwd, string email, string securityQuestion, string answer, int source);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/UserRegisterEx2", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> UserRegisterEx2Async(string AuthKey, string mobile, string username, string pwd, string email, string securityQuestion, string answer, int source, string registinfo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/UserRegisterFaster", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> UserRegisterFasterAsync(string authkey, string mobile, string userName, string password, System.DateTime babyBrithday, string infoSource);
        
        // CODEGEN: 正在生成消息协定，因为操作具有多个返回值。
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/ResetPassword", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<yilibabyUser.ResetPasswordResponse> ResetPasswordAsync(yilibabyUser.ResetPasswordRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/SendResetPasswordMail", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> SendResetPasswordMailAsync(string AuthKey, string UserEmail);
        
        // CODEGEN: 正在生成消息协定，因为操作具有多个返回值。
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/SendSecurityCodeByEmail", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<yilibabyUser.SendSecurityCodeByEmailResponse> SendSecurityCodeByEmailAsync(yilibabyUser.SendSecurityCodeByEmailRequest request);
        
        // CODEGEN: 正在生成消息协定，因为操作具有多个返回值。
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/VerifyEmailSecurityCode", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<yilibabyUser.VerifyEmailSecurityCodeResponse> VerifyEmailSecurityCodeAsync(yilibabyUser.VerifyEmailSecurityCodeRequest request);
        
        // CODEGEN: 正在生成消息协定，因为操作具有多个返回值。
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/ResetPasswordByEmailSecurityCode", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<yilibabyUser.ResetPasswordByEmailSecurityCodeResponse> ResetPasswordByEmailSecurityCodeAsync(yilibabyUser.ResetPasswordByEmailSecurityCodeRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/ChangePassword", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> ChangePasswordAsync(string AuthKey, string OldPassword, string NewPassword);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetAllSecurityQuestions", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string[]> GetAllSecurityQuestionsAsync(string AuthKey);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetAllSecurityQuestionsJson", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetAllSecurityQuestionsJsonAsync(string AuthKey);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetUserSecurityQuestion", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetUserSecurityQuestionAsync(string AuthKey, string UserName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/ResetPasswordByQA", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> ResetPasswordByQAAsync(string AuthKey, string UserName, string QAAnswer, string NewPwd);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/ModifyUserSecurityQuestion", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> ModifyUserSecurityQuestionAsync(string AuthKey, string Password, string Question, string Answer);
        
        // CODEGEN: 正在生成消息协定，因为操作具有多个返回值。
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/ApplyAESEncryptKey", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<yilibabyUser.ApplyAESEncryptKeyResponse> ApplyAESEncryptKeyAsync(yilibabyUser.ApplyAESEncryptKeyRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/ApplyAESEncryptKey_Test", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> ApplyAESEncryptKey_TestAsync(string DeviceCode, string Modulus, string Exponent);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetAttachmentDownloadURL", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetAttachmentDownloadURLAsync(string Guid);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="LoginEx", WrapperNamespace="http://mmp.meichis.com/DataInterface/", IsWrapped=true)]
    public partial class LoginExRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=0)]
        public string UserName;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=1)]
        public string EncryptPassword;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=2)]
        public string DeviceCode;
        
        public LoginExRequest()
        {
        }
        
        public LoginExRequest(string UserName, string EncryptPassword, string DeviceCode)
        {
            this.UserName = UserName;
            this.EncryptPassword = EncryptPassword;
            this.DeviceCode = DeviceCode;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="LoginExResponse", WrapperNamespace="http://mmp.meichis.com/DataInterface/", IsWrapped=true)]
    public partial class LoginExResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=0)]
        public int LoginExResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=1)]
        public string AuthKey;
        
        public LoginExResponse()
        {
        }
        
        public LoginExResponse(int LoginExResult, string AuthKey)
        {
            this.LoginExResult = LoginExResult;
            this.AuthKey = AuthKey;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://mmp.meichis.com/DataInterface/")]
    public partial class UserInfo
    {
        
        private System.Guid aspnetUserIdField;
        
        private string userNameField;
        
        private int accountTypeField;
        
        private string memberNameField;
        
        private int cRMIDField;
        
        private bool isAnonymousField;
        
        private string deviceCodeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public System.Guid aspnetUserId
        {
            get
            {
                return this.aspnetUserIdField;
            }
            set
            {
                this.aspnetUserIdField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string UserName
        {
            get
            {
                return this.userNameField;
            }
            set
            {
                this.userNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public int AccountType
        {
            get
            {
                return this.accountTypeField;
            }
            set
            {
                this.accountTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string MemberName
        {
            get
            {
                return this.memberNameField;
            }
            set
            {
                this.memberNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public int CRMID
        {
            get
            {
                return this.cRMIDField;
            }
            set
            {
                this.cRMIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public bool IsAnonymous
        {
            get
            {
                return this.isAnonymousField;
            }
            set
            {
                this.isAnonymousField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        public string DeviceCode
        {
            get
            {
                return this.deviceCodeField;
            }
            set
            {
                this.deviceCodeField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ResetPassword", WrapperNamespace="http://mmp.meichis.com/DataInterface/", IsWrapped=true)]
    public partial class ResetPasswordRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=0)]
        public string AuthKey;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=1)]
        public string mobile;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=2)]
        public int VerifyID;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=3)]
        public string VerifyCode;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=4)]
        public string newpwd;
        
        public ResetPasswordRequest()
        {
        }
        
        public ResetPasswordRequest(string AuthKey, string mobile, int VerifyID, string VerifyCode, string newpwd)
        {
            this.AuthKey = AuthKey;
            this.mobile = mobile;
            this.VerifyID = VerifyID;
            this.VerifyCode = VerifyCode;
            this.newpwd = newpwd;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ResetPasswordResponse", WrapperNamespace="http://mmp.meichis.com/DataInterface/", IsWrapped=true)]
    public partial class ResetPasswordResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=0)]
        public int ResetPasswordResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=1)]
        public string username;
        
        public ResetPasswordResponse()
        {
        }
        
        public ResetPasswordResponse(int ResetPasswordResult, string username)
        {
            this.ResetPasswordResult = ResetPasswordResult;
            this.username = username;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="SendSecurityCodeByEmail", WrapperNamespace="http://mmp.meichis.com/DataInterface/", IsWrapped=true)]
    public partial class SendSecurityCodeByEmailRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=0)]
        public string AuthKey;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=1)]
        public string UserEmail;
        
        public SendSecurityCodeByEmailRequest()
        {
        }
        
        public SendSecurityCodeByEmailRequest(string AuthKey, string UserEmail)
        {
            this.AuthKey = AuthKey;
            this.UserEmail = UserEmail;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="SendSecurityCodeByEmailResponse", WrapperNamespace="http://mmp.meichis.com/DataInterface/", IsWrapped=true)]
    public partial class SendSecurityCodeByEmailResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=0)]
        public int SendSecurityCodeByEmailResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=1)]
        public string MailSecurityCode;
        
        public SendSecurityCodeByEmailResponse()
        {
        }
        
        public SendSecurityCodeByEmailResponse(int SendSecurityCodeByEmailResult, string MailSecurityCode)
        {
            this.SendSecurityCodeByEmailResult = SendSecurityCodeByEmailResult;
            this.MailSecurityCode = MailSecurityCode;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="VerifyEmailSecurityCode", WrapperNamespace="http://mmp.meichis.com/DataInterface/", IsWrapped=true)]
    public partial class VerifyEmailSecurityCodeRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=0)]
        public string AuthKey;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=1)]
        public string MailSecurityCode;
        
        public VerifyEmailSecurityCodeRequest()
        {
        }
        
        public VerifyEmailSecurityCodeRequest(string AuthKey, string MailSecurityCode)
        {
            this.AuthKey = AuthKey;
            this.MailSecurityCode = MailSecurityCode;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="VerifyEmailSecurityCodeResponse", WrapperNamespace="http://mmp.meichis.com/DataInterface/", IsWrapped=true)]
    public partial class VerifyEmailSecurityCodeResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=0)]
        public int VerifyEmailSecurityCodeResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=1)]
        public string UserName;
        
        public VerifyEmailSecurityCodeResponse()
        {
        }
        
        public VerifyEmailSecurityCodeResponse(int VerifyEmailSecurityCodeResult, string UserName)
        {
            this.VerifyEmailSecurityCodeResult = VerifyEmailSecurityCodeResult;
            this.UserName = UserName;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ResetPasswordByEmailSecurityCode", WrapperNamespace="http://mmp.meichis.com/DataInterface/", IsWrapped=true)]
    public partial class ResetPasswordByEmailSecurityCodeRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=0)]
        public string AuthKey;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=1)]
        public string MailSecurityCode;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=2)]
        public string NewPwd;
        
        public ResetPasswordByEmailSecurityCodeRequest()
        {
        }
        
        public ResetPasswordByEmailSecurityCodeRequest(string AuthKey, string MailSecurityCode, string NewPwd)
        {
            this.AuthKey = AuthKey;
            this.MailSecurityCode = MailSecurityCode;
            this.NewPwd = NewPwd;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ResetPasswordByEmailSecurityCodeResponse", WrapperNamespace="http://mmp.meichis.com/DataInterface/", IsWrapped=true)]
    public partial class ResetPasswordByEmailSecurityCodeResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=0)]
        public int ResetPasswordByEmailSecurityCodeResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=1)]
        public string UserName;
        
        public ResetPasswordByEmailSecurityCodeResponse()
        {
        }
        
        public ResetPasswordByEmailSecurityCodeResponse(int ResetPasswordByEmailSecurityCodeResult, string UserName)
        {
            this.ResetPasswordByEmailSecurityCodeResult = ResetPasswordByEmailSecurityCodeResult;
            this.UserName = UserName;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ApplyAESEncryptKey", WrapperNamespace="http://mmp.meichis.com/DataInterface/", IsWrapped=true)]
    public partial class ApplyAESEncryptKeyRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=0)]
        public string DeviceCode;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=1)]
        public string Modulus;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=2)]
        public string Exponent;
        
        public ApplyAESEncryptKeyRequest()
        {
        }
        
        public ApplyAESEncryptKeyRequest(string DeviceCode, string Modulus, string Exponent)
        {
            this.DeviceCode = DeviceCode;
            this.Modulus = Modulus;
            this.Exponent = Exponent;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="ApplyAESEncryptKeyResponse", WrapperNamespace="http://mmp.meichis.com/DataInterface/", IsWrapped=true)]
    public partial class ApplyAESEncryptKeyResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=0)]
        public int ApplyAESEncryptKeyResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=1)]
        public string CryptAESKey;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=2)]
        public string CryptAESIV;
        
        public ApplyAESEncryptKeyResponse()
        {
        }
        
        public ApplyAESEncryptKeyResponse(int ApplyAESEncryptKeyResult, string CryptAESKey, string CryptAESIV)
        {
            this.ApplyAESEncryptKeyResult = ApplyAESEncryptKeyResult;
            this.CryptAESKey = CryptAESKey;
            this.CryptAESIV = CryptAESIV;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    public interface UserLoginSoapChannel : yilibabyUser.UserLoginSoap, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    public partial class UserLoginSoapClient : System.ServiceModel.ClientBase<yilibabyUser.UserLoginSoap>, yilibabyUser.UserLoginSoap
    {
        
    /// <summary>
    /// 实现此分部方法，配置服务终结点。
    /// </summary>
    /// <param name="serviceEndpoint">要配置的终结点</param>
    /// <param name="clientCredentials">客户端凭据</param>
    static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public UserLoginSoapClient(EndpointConfiguration endpointConfiguration) : 
                base(UserLoginSoapClient.GetBindingForEndpoint(endpointConfiguration), UserLoginSoapClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public UserLoginSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(UserLoginSoapClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public UserLoginSoapClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(UserLoginSoapClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public UserLoginSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public System.Threading.Tasks.Task<string> HelloWorldAsync()
        {
            return base.Channel.HelloWorldAsync();
        }
        
        public System.Threading.Tasks.Task<System.DateTime> GetServerSyncTimeAsync()
        {
            return base.Channel.GetServerSyncTimeAsync();
        }
        
        public System.Threading.Tasks.Task<string> Login1Async(string UserName, string Password, string DeviceCode)
        {
            return base.Channel.Login1Async(UserName, Password, DeviceCode);
        }
        
        public System.Threading.Tasks.Task<yilibabyUser.LoginExResponse> LoginExAsync(yilibabyUser.LoginExRequest request)
        {
            return base.Channel.LoginExAsync(request);
        }
        
        public System.Threading.Tasks.Task<int> LoginAutoForSMSAsync(string UserName, string Password)
        {
            return base.Channel.LoginAutoForSMSAsync(UserName, Password);
        }
        
        public System.Threading.Tasks.Task<yilibabyUser.UserInfo> Login_GetCurrentUserAsync(string AuthKey)
        {
            return base.Channel.Login_GetCurrentUserAsync(AuthKey);
        }
        
        public System.Threading.Tasks.Task<string> Login_GetCurentUserInfoAsync(string AuthKey)
        {
            return base.Channel.Login_GetCurentUserInfoAsync(AuthKey);
        }
        
        public System.Threading.Tasks.Task<string> Login_GetCurrentUserJsonAsync(string AuthKey)
        {
            return base.Channel.Login_GetCurrentUserJsonAsync(AuthKey);
        }
        
        public System.Threading.Tasks.Task<int> Login_GetNewMsgAsync(string AuthKey)
        {
            return base.Channel.Login_GetNewMsgAsync(AuthKey);
        }
        
        public System.Threading.Tasks.Task<int> LogoutAsync(string AuthKey)
        {
            return base.Channel.LogoutAsync(AuthKey);
        }
        
        public System.Threading.Tasks.Task<bool> CheckUsernameAsync(string AuthKey, string username)
        {
            return base.Channel.CheckUsernameAsync(AuthKey, username);
        }
        
        public System.Threading.Tasks.Task<bool> CheckMobileAsync(string AuthKey, string mobile)
        {
            return base.Channel.CheckMobileAsync(AuthKey, mobile);
        }
        
        public System.Threading.Tasks.Task<bool> CheckEmailAsync(string AuthKey, string email)
        {
            return base.Channel.CheckEmailAsync(AuthKey, email);
        }
        
        public System.Threading.Tasks.Task<int> UserRegisterAsync(string AuthKey, string mobile, string username, string pwd, string email)
        {
            return base.Channel.UserRegisterAsync(AuthKey, mobile, username, pwd, email);
        }
        
        public System.Threading.Tasks.Task<int> UserRegister2Async(string AuthKey, string mobile, string username, string pwd, string email)
        {
            return base.Channel.UserRegister2Async(AuthKey, mobile, username, pwd, email);
        }
        
        public System.Threading.Tasks.Task<int> UserRegisterExWithMediaIDAsync(string AuthKey, string mobile, string username, string pwd, string email, string securityQuestion, string answer, int source, string registinfo)
        {
            return base.Channel.UserRegisterExWithMediaIDAsync(AuthKey, mobile, username, pwd, email, securityQuestion, answer, source, registinfo);
        }
        
        public System.Threading.Tasks.Task<int> UserRegisterExAsync(string AuthKey, string mobile, string username, string pwd, string email, string securityQuestion, string answer, int source)
        {
            return base.Channel.UserRegisterExAsync(AuthKey, mobile, username, pwd, email, securityQuestion, answer, source);
        }
        
        public System.Threading.Tasks.Task<int> UserRegisterEx2Async(string AuthKey, string mobile, string username, string pwd, string email, string securityQuestion, string answer, int source, string registinfo)
        {
            return base.Channel.UserRegisterEx2Async(AuthKey, mobile, username, pwd, email, securityQuestion, answer, source, registinfo);
        }
        
        public System.Threading.Tasks.Task<int> UserRegisterFasterAsync(string authkey, string mobile, string userName, string password, System.DateTime babyBrithday, string infoSource)
        {
            return base.Channel.UserRegisterFasterAsync(authkey, mobile, userName, password, babyBrithday, infoSource);
        }
        
        public System.Threading.Tasks.Task<yilibabyUser.ResetPasswordResponse> ResetPasswordAsync(yilibabyUser.ResetPasswordRequest request)
        {
            return base.Channel.ResetPasswordAsync(request);
        }
        
        public System.Threading.Tasks.Task<int> SendResetPasswordMailAsync(string AuthKey, string UserEmail)
        {
            return base.Channel.SendResetPasswordMailAsync(AuthKey, UserEmail);
        }
        
        public System.Threading.Tasks.Task<yilibabyUser.SendSecurityCodeByEmailResponse> SendSecurityCodeByEmailAsync(yilibabyUser.SendSecurityCodeByEmailRequest request)
        {
            return base.Channel.SendSecurityCodeByEmailAsync(request);
        }
        
        public System.Threading.Tasks.Task<yilibabyUser.VerifyEmailSecurityCodeResponse> VerifyEmailSecurityCodeAsync(yilibabyUser.VerifyEmailSecurityCodeRequest request)
        {
            return base.Channel.VerifyEmailSecurityCodeAsync(request);
        }
        
        public System.Threading.Tasks.Task<yilibabyUser.ResetPasswordByEmailSecurityCodeResponse> ResetPasswordByEmailSecurityCodeAsync(yilibabyUser.ResetPasswordByEmailSecurityCodeRequest request)
        {
            return base.Channel.ResetPasswordByEmailSecurityCodeAsync(request);
        }
        
        public System.Threading.Tasks.Task<int> ChangePasswordAsync(string AuthKey, string OldPassword, string NewPassword)
        {
            return base.Channel.ChangePasswordAsync(AuthKey, OldPassword, NewPassword);
        }
        
        public System.Threading.Tasks.Task<string[]> GetAllSecurityQuestionsAsync(string AuthKey)
        {
            return base.Channel.GetAllSecurityQuestionsAsync(AuthKey);
        }
        
        public System.Threading.Tasks.Task<string> GetAllSecurityQuestionsJsonAsync(string AuthKey)
        {
            return base.Channel.GetAllSecurityQuestionsJsonAsync(AuthKey);
        }
        
        public System.Threading.Tasks.Task<string> GetUserSecurityQuestionAsync(string AuthKey, string UserName)
        {
            return base.Channel.GetUserSecurityQuestionAsync(AuthKey, UserName);
        }
        
        public System.Threading.Tasks.Task<int> ResetPasswordByQAAsync(string AuthKey, string UserName, string QAAnswer, string NewPwd)
        {
            return base.Channel.ResetPasswordByQAAsync(AuthKey, UserName, QAAnswer, NewPwd);
        }
        
        public System.Threading.Tasks.Task<int> ModifyUserSecurityQuestionAsync(string AuthKey, string Password, string Question, string Answer)
        {
            return base.Channel.ModifyUserSecurityQuestionAsync(AuthKey, Password, Question, Answer);
        }
        
        public System.Threading.Tasks.Task<yilibabyUser.ApplyAESEncryptKeyResponse> ApplyAESEncryptKeyAsync(yilibabyUser.ApplyAESEncryptKeyRequest request)
        {
            return base.Channel.ApplyAESEncryptKeyAsync(request);
        }
        
        public System.Threading.Tasks.Task<string> ApplyAESEncryptKey_TestAsync(string DeviceCode, string Modulus, string Exponent)
        {
            return base.Channel.ApplyAESEncryptKey_TestAsync(DeviceCode, Modulus, Exponent);
        }
        
        public System.Threading.Tasks.Task<string> GetAttachmentDownloadURLAsync(string Guid)
        {
            return base.Channel.GetAttachmentDownloadURLAsync(Guid);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        public virtual System.Threading.Tasks.Task CloseAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.UserLoginSoap))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.UserLoginSoap12))
            {
                System.ServiceModel.Channels.CustomBinding result = new System.ServiceModel.Channels.CustomBinding();
                System.ServiceModel.Channels.TextMessageEncodingBindingElement textBindingElement = new System.ServiceModel.Channels.TextMessageEncodingBindingElement();
                textBindingElement.MessageVersion = System.ServiceModel.Channels.MessageVersion.CreateVersion(System.ServiceModel.EnvelopeVersion.Soap12, System.ServiceModel.Channels.AddressingVersion.None);
                result.Elements.Add(textBindingElement);
                System.ServiceModel.Channels.HttpTransportBindingElement httpBindingElement = new System.ServiceModel.Channels.HttpTransportBindingElement();
                httpBindingElement.AllowCookies = true;
                httpBindingElement.MaxBufferSize = int.MaxValue;
                httpBindingElement.MaxReceivedMessageSize = int.MaxValue;
                result.Elements.Add(httpBindingElement);
                return result;
            }
            throw new System.InvalidOperationException(string.Format("找不到名称为“{0}”的终结点。", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.UserLoginSoap))
            {
                return new System.ServiceModel.EndpointAddress("http://t.jifen.yilibabyclub.com/MClubIF2/UserLogin.asmx");
            }
            if ((endpointConfiguration == EndpointConfiguration.UserLoginSoap12))
            {
                return new System.ServiceModel.EndpointAddress("http://t.jifen.yilibabyclub.com/MClubIF2/UserLogin.asmx");
            }
            throw new System.InvalidOperationException(string.Format("找不到名称为“{0}”的终结点。", endpointConfiguration));
        }
        
        public enum EndpointConfiguration
        {
            
            UserLoginSoap,
            
            UserLoginSoap12,
        }
    }
}
