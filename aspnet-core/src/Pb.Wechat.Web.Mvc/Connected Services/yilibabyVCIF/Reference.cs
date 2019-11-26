//------------------------------------------------------------------------------
// <自动生成>
//     此代码由工具生成。
//     //
//     对此文件的更改可能导致不正确的行为，并在以下条件下丢失:
//     代码重新生成。
// </自动生成>
//------------------------------------------------------------------------------

namespace yilibabyVCIF
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://mmp.meichis.com/DataInterface/", ConfigurationName="yilibabyVCIF.VCIFServiceSoap")]
    public interface VCIFServiceSoap
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/HelloWorld", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> HelloWorldAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/SendVerifyCodeWithParam", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> SendVerifyCodeWithParamAsync(string AuthKey, int Classify, string Mobile, string MessageParam);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/SendSMSWithParamByCRMID", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> SendSMSWithParamByCRMIDAsync(string AuthKey, int Classify, int CRMID, string MessageParam);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/ReSendVerifyCode", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> ReSendVerifyCodeAsync(string AuthKey, int VerifyID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/VerifyCode", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> VerifyCodeAsync(string AuthKey, int VerifyID, string Mobile, string Code);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    public interface VCIFServiceSoapChannel : yilibabyVCIF.VCIFServiceSoap, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    public partial class VCIFServiceSoapClient : System.ServiceModel.ClientBase<yilibabyVCIF.VCIFServiceSoap>, yilibabyVCIF.VCIFServiceSoap
    {
        
    /// <summary>
    /// 实现此分部方法，配置服务终结点。
    /// </summary>
    /// <param name="serviceEndpoint">要配置的终结点</param>
    /// <param name="clientCredentials">客户端凭据</param>
    static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public VCIFServiceSoapClient(EndpointConfiguration endpointConfiguration) : 
                base(VCIFServiceSoapClient.GetBindingForEndpoint(endpointConfiguration), VCIFServiceSoapClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public VCIFServiceSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(VCIFServiceSoapClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public VCIFServiceSoapClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(VCIFServiceSoapClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public VCIFServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public System.Threading.Tasks.Task<string> HelloWorldAsync()
        {
            return base.Channel.HelloWorldAsync();
        }
        
        public System.Threading.Tasks.Task<int> SendVerifyCodeWithParamAsync(string AuthKey, int Classify, string Mobile, string MessageParam)
        {
            return base.Channel.SendVerifyCodeWithParamAsync(AuthKey, Classify, Mobile, MessageParam);
        }
        
        public System.Threading.Tasks.Task<int> SendSMSWithParamByCRMIDAsync(string AuthKey, int Classify, int CRMID, string MessageParam)
        {
            return base.Channel.SendSMSWithParamByCRMIDAsync(AuthKey, Classify, CRMID, MessageParam);
        }
        
        public System.Threading.Tasks.Task<int> ReSendVerifyCodeAsync(string AuthKey, int VerifyID)
        {
            return base.Channel.ReSendVerifyCodeAsync(AuthKey, VerifyID);
        }
        
        public System.Threading.Tasks.Task<int> VerifyCodeAsync(string AuthKey, int VerifyID, string Mobile, string Code)
        {
            return base.Channel.VerifyCodeAsync(AuthKey, VerifyID, Mobile, Code);
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
            if ((endpointConfiguration == EndpointConfiguration.VCIFServiceSoap))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.VCIFServiceSoap12))
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
            if ((endpointConfiguration == EndpointConfiguration.VCIFServiceSoap))
            {
                return new System.ServiceModel.EndpointAddress("http://t.jifen.yilibabyclub.com/MClubIF2/VCIFService.asmx");
            }
            if ((endpointConfiguration == EndpointConfiguration.VCIFServiceSoap12))
            {
                return new System.ServiceModel.EndpointAddress("http://t.jifen.yilibabyclub.com/MClubIF2/VCIFService.asmx");
            }
            throw new System.InvalidOperationException(string.Format("找不到名称为“{0}”的终结点。", endpointConfiguration));
        }
        
        public enum EndpointConfiguration
        {
            
            VCIFServiceSoap,
            
            VCIFServiceSoap12,
        }
    }
}
