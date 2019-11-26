//------------------------------------------------------------------------------
// <自动生成>
//     此代码由工具生成。
//     //
//     对此文件的更改可能导致不正确的行为，并在以下条件下丢失:
//     代码重新生成。
// </自动生成>
//------------------------------------------------------------------------------

namespace yilibabyActivity
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="yilibabyActivity.ActivityServiceSoap")]
    public interface ActivityServiceSoap
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/HelloWorld", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> HelloWorldAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetActivityInfo", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<yilibabyActivity.Activity> GetActivityInfoAsync(string AuthKey, int ActivityID);
        
        // CODEGEN: 正在生成消息协定，因为操作具有多个返回值。
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetActivityTopic", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<yilibabyActivity.GetActivityTopicResponse> GetActivityTopicAsync(yilibabyActivity.GetActivityTopicRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetActivityInfoJson", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetActivityInfoJsonAsync(string AuthKey, int ActivityID);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class Activity
    {
        
        private int idField;
        
        private string activityCodeField;
        
        private string topicField;
        
        private string activityIntroduceField;
        
        private System.DateTime planBeginDateField;
        
        private System.DateTime planEndDateField;
        
        private int classifyField;
        
        private int officialCityField;
        
        private int defaultClientField;
        
        private string defaultClientCodeField;
        
        private string defaultClientFullNameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public int ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string ActivityCode
        {
            get
            {
                return this.activityCodeField;
            }
            set
            {
                this.activityCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string Topic
        {
            get
            {
                return this.topicField;
            }
            set
            {
                this.topicField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string ActivityIntroduce
        {
            get
            {
                return this.activityIntroduceField;
            }
            set
            {
                this.activityIntroduceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public System.DateTime PlanBeginDate
        {
            get
            {
                return this.planBeginDateField;
            }
            set
            {
                this.planBeginDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public System.DateTime PlanEndDate
        {
            get
            {
                return this.planEndDateField;
            }
            set
            {
                this.planEndDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        public int Classify
        {
            get
            {
                return this.classifyField;
            }
            set
            {
                this.classifyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=7)]
        public int OfficialCity
        {
            get
            {
                return this.officialCityField;
            }
            set
            {
                this.officialCityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=8)]
        public int DefaultClient
        {
            get
            {
                return this.defaultClientField;
            }
            set
            {
                this.defaultClientField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=9)]
        public string DefaultClientCode
        {
            get
            {
                return this.defaultClientCodeField;
            }
            set
            {
                this.defaultClientCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=10)]
        public string DefaultClientFullName
        {
            get
            {
                return this.defaultClientFullNameField;
            }
            set
            {
                this.defaultClientFullNameField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetActivityTopic", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class GetActivityTopicRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public string AuthKey;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        public int ActivityID;
        
        public GetActivityTopicRequest()
        {
        }
        
        public GetActivityTopicRequest(string AuthKey, int ActivityID)
        {
            this.AuthKey = AuthKey;
            this.ActivityID = ActivityID;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetActivityTopicResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class GetActivityTopicResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public string GetActivityTopicResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        public int ProvId;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=2)]
        public int CityId;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=3)]
        public int TownId;
        
        public GetActivityTopicResponse()
        {
        }
        
        public GetActivityTopicResponse(string GetActivityTopicResult, int ProvId, int CityId, int TownId)
        {
            this.GetActivityTopicResult = GetActivityTopicResult;
            this.ProvId = ProvId;
            this.CityId = CityId;
            this.TownId = TownId;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    public interface ActivityServiceSoapChannel : yilibabyActivity.ActivityServiceSoap, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    public partial class ActivityServiceSoapClient : System.ServiceModel.ClientBase<yilibabyActivity.ActivityServiceSoap>, yilibabyActivity.ActivityServiceSoap
    {
        
    /// <summary>
    /// 实现此分部方法，配置服务终结点。
    /// </summary>
    /// <param name="serviceEndpoint">要配置的终结点</param>
    /// <param name="clientCredentials">客户端凭据</param>
    static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public ActivityServiceSoapClient(EndpointConfiguration endpointConfiguration) : 
                base(ActivityServiceSoapClient.GetBindingForEndpoint(endpointConfiguration), ActivityServiceSoapClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public ActivityServiceSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(ActivityServiceSoapClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public ActivityServiceSoapClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(ActivityServiceSoapClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public ActivityServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public System.Threading.Tasks.Task<string> HelloWorldAsync()
        {
            return base.Channel.HelloWorldAsync();
        }
        
        public System.Threading.Tasks.Task<yilibabyActivity.Activity> GetActivityInfoAsync(string AuthKey, int ActivityID)
        {
            return base.Channel.GetActivityInfoAsync(AuthKey, ActivityID);
        }
        
        public System.Threading.Tasks.Task<yilibabyActivity.GetActivityTopicResponse> GetActivityTopicAsync(yilibabyActivity.GetActivityTopicRequest request)
        {
            return base.Channel.GetActivityTopicAsync(request);
        }
        
        public System.Threading.Tasks.Task<string> GetActivityInfoJsonAsync(string AuthKey, int ActivityID)
        {
            return base.Channel.GetActivityInfoJsonAsync(AuthKey, ActivityID);
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
            if ((endpointConfiguration == EndpointConfiguration.ActivityServiceSoap))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.ActivityServiceSoap12))
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
            if ((endpointConfiguration == EndpointConfiguration.ActivityServiceSoap))
            {
                return new System.ServiceModel.EndpointAddress("http://t.jifen.yilibabyclub.com/mclubif2/activityservice.asmx");
            }
            if ((endpointConfiguration == EndpointConfiguration.ActivityServiceSoap12))
            {
                return new System.ServiceModel.EndpointAddress("http://t.jifen.yilibabyclub.com/mclubif2/activityservice.asmx");
            }
            throw new System.InvalidOperationException(string.Format("找不到名称为“{0}”的终结点。", endpointConfiguration));
        }
        
        public enum EndpointConfiguration
        {
            
            ActivityServiceSoap,
            
            ActivityServiceSoap12,
        }
    }
}
