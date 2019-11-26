//------------------------------------------------------------------------------
// <自动生成>
//     此代码由工具生成。
//     //
//     对此文件的更改可能导致不正确的行为，并在以下条件下丢失:
//     代码重新生成。
// </自动生成>
//------------------------------------------------------------------------------

namespace yilibabyOffice
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://mmp.meichis.com/DataInterface/", ConfigurationName="yilibabyOffice.OfficialCityServiceSoap")]
    public interface OfficialCityServiceSoap
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/HelloWorld", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> HelloWorldAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetSubCitysBySuperJson", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetSubCitysBySuperJsonAsync(string AuthKey, int CityID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetAllOfficialCitysJson", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetAllOfficialCitysJsonAsync(string AuthKey);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetCityFullName", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetCityFullNameAsync(string AuthKey, int CityID);
        
        // CODEGEN: 正在生成消息协定，因为操作具有多个返回值。
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetSuperOfficialCity", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<yilibabyOffice.GetSuperOfficialCityResponse> GetSuperOfficialCityAsync(yilibabyOffice.GetSuperOfficialCityRequest request);
        
        // CODEGEN: 正在生成消息协定，因为操作具有多个返回值。
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetSuperOfficialCityJson", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<yilibabyOffice.GetSuperOfficialCityJsonResponse> GetSuperOfficialCityJsonAsync(yilibabyOffice.GetSuperOfficialCityJsonRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetSuperOfficialCity", WrapperNamespace="http://mmp.meichis.com/DataInterface/", IsWrapped=true)]
    public partial class GetSuperOfficialCityRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=0)]
        public string AuthKey;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=1)]
        public int OfficialCity;
        
        public GetSuperOfficialCityRequest()
        {
        }
        
        public GetSuperOfficialCityRequest(string AuthKey, int OfficialCity)
        {
            this.AuthKey = AuthKey;
            this.OfficialCity = OfficialCity;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetSuperOfficialCityResponse", WrapperNamespace="http://mmp.meichis.com/DataInterface/", IsWrapped=true)]
    public partial class GetSuperOfficialCityResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=0)]
        public int GetSuperOfficialCityResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=1)]
        public int ProvID;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=2)]
        public int CityID;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=3)]
        public int AreaID;
        
        public GetSuperOfficialCityResponse()
        {
        }
        
        public GetSuperOfficialCityResponse(int GetSuperOfficialCityResult, int ProvID, int CityID, int AreaID)
        {
            this.GetSuperOfficialCityResult = GetSuperOfficialCityResult;
            this.ProvID = ProvID;
            this.CityID = CityID;
            this.AreaID = AreaID;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetSuperOfficialCityJson", WrapperNamespace="http://mmp.meichis.com/DataInterface/", IsWrapped=true)]
    public partial class GetSuperOfficialCityJsonRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=0)]
        public string AuthKey;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=1)]
        public int OfficialCity;
        
        public GetSuperOfficialCityJsonRequest()
        {
        }
        
        public GetSuperOfficialCityJsonRequest(string AuthKey, int OfficialCity)
        {
            this.AuthKey = AuthKey;
            this.OfficialCity = OfficialCity;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetSuperOfficialCityJsonResponse", WrapperNamespace="http://mmp.meichis.com/DataInterface/", IsWrapped=true)]
    public partial class GetSuperOfficialCityJsonResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=0)]
        public int GetSuperOfficialCityJsonResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=1)]
        public string Prov;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=2)]
        public string City;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://mmp.meichis.com/DataInterface/", Order=3)]
        public string Area;
        
        public GetSuperOfficialCityJsonResponse()
        {
        }
        
        public GetSuperOfficialCityJsonResponse(int GetSuperOfficialCityJsonResult, string Prov, string City, string Area)
        {
            this.GetSuperOfficialCityJsonResult = GetSuperOfficialCityJsonResult;
            this.Prov = Prov;
            this.City = City;
            this.Area = Area;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    public interface OfficialCityServiceSoapChannel : yilibabyOffice.OfficialCityServiceSoap, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    public partial class OfficialCityServiceSoapClient : System.ServiceModel.ClientBase<yilibabyOffice.OfficialCityServiceSoap>, yilibabyOffice.OfficialCityServiceSoap
    {
        
    /// <summary>
    /// 实现此分部方法，配置服务终结点。
    /// </summary>
    /// <param name="serviceEndpoint">要配置的终结点</param>
    /// <param name="clientCredentials">客户端凭据</param>
    static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public OfficialCityServiceSoapClient(EndpointConfiguration endpointConfiguration) : 
                base(OfficialCityServiceSoapClient.GetBindingForEndpoint(endpointConfiguration), OfficialCityServiceSoapClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public OfficialCityServiceSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(OfficialCityServiceSoapClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public OfficialCityServiceSoapClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(OfficialCityServiceSoapClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public OfficialCityServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public System.Threading.Tasks.Task<string> HelloWorldAsync()
        {
            return base.Channel.HelloWorldAsync();
        }
        
        public System.Threading.Tasks.Task<string> GetSubCitysBySuperJsonAsync(string AuthKey, int CityID)
        {
            return base.Channel.GetSubCitysBySuperJsonAsync(AuthKey, CityID);
        }
        
        public System.Threading.Tasks.Task<string> GetAllOfficialCitysJsonAsync(string AuthKey)
        {
            return base.Channel.GetAllOfficialCitysJsonAsync(AuthKey);
        }
        
        public System.Threading.Tasks.Task<string> GetCityFullNameAsync(string AuthKey, int CityID)
        {
            return base.Channel.GetCityFullNameAsync(AuthKey, CityID);
        }
        
        public System.Threading.Tasks.Task<yilibabyOffice.GetSuperOfficialCityResponse> GetSuperOfficialCityAsync(yilibabyOffice.GetSuperOfficialCityRequest request)
        {
            return base.Channel.GetSuperOfficialCityAsync(request);
        }
        
        public System.Threading.Tasks.Task<yilibabyOffice.GetSuperOfficialCityJsonResponse> GetSuperOfficialCityJsonAsync(yilibabyOffice.GetSuperOfficialCityJsonRequest request)
        {
            return base.Channel.GetSuperOfficialCityJsonAsync(request);
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
            if ((endpointConfiguration == EndpointConfiguration.OfficialCityServiceSoap))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.OfficialCityServiceSoap12))
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
            if ((endpointConfiguration == EndpointConfiguration.OfficialCityServiceSoap))
            {
                return new System.ServiceModel.EndpointAddress("http://t.jifen.yilibabyclub.com/MClubIF2/OfficialCityService.asmx");
            }
            if ((endpointConfiguration == EndpointConfiguration.OfficialCityServiceSoap12))
            {
                return new System.ServiceModel.EndpointAddress("http://t.jifen.yilibabyclub.com/MClubIF2/OfficialCityService.asmx");
            }
            throw new System.InvalidOperationException(string.Format("找不到名称为“{0}”的终结点。", endpointConfiguration));
        }
        
        public enum EndpointConfiguration
        {
            
            OfficialCityServiceSoap,
            
            OfficialCityServiceSoap12,
        }
    }
}
