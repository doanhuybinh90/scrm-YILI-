//------------------------------------------------------------------------------
// <自动生成>
//     此代码由工具生成。
//     //
//     对此文件的更改可能导致不正确的行为，并在以下条件下丢失:
//     代码重新生成。
// </自动生成>
//------------------------------------------------------------------------------

namespace yilibabyWCIF
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://mmp.meichis.com/DataInterface/", ConfigurationName="yilibabyWCIF.WCIFServiceSoap")]
    public interface WCIFServiceSoap
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/HelloWorld", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> HelloWorldAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetAccessToken", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetAccessTokenAsync(string OpenId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetTicket", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetTicketAsync(string OpenId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetCardTicket", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetCardTicketAsync(string OpenId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetQRCodeTicket", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetQRCodeTicketAsync(string OpenId, int scene_id, int expire_seconds);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetQRStrCodeTicket", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetQRStrCodeTicketAsync(string OpenId, string scene_str, int expire_seconds);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetQRCodeLimitTicket", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetQRCodeLimitTicketAsync(string OpenId, int scene_id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetQRCodeLimitTicketEx", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetQRCodeLimitTicketExAsync(string OpenId, int ApplySource, int AssignType, int AssignRelateID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetQRCodeLimitTicketEx2", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetQRCodeLimitTicketEx2Async(string OpenId, int ApplySource, int AssignType, int AssignRelateID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetQRCodeLimitTicketWithStr", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetQRCodeLimitTicketWithStrAsync(string OpenId, string scene_str);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/SendTemplateMessage", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> SendTemplateMessageAsync(string AuthKey, string FansID, string TemplateID, string url, string first, string remark, string keynotes, string keywords, string flag, int source);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/SendMessage", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<int> SendMessageAsync(string FansID, string MsgType, string Content, string flag);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetPromotorInfo", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetPromotorInfoAsync(int scene_id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetLocalQRCode", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetLocalQRCodeAsync(int RelateType, int RelateID, string QRString);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetQRCodeTicketToBase64", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetQRCodeTicketToBase64Async(string OpenId, int scene_id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://mmp.meichis.com/DataInterface/GetStringQRCodeToBase64", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<string> GetStringQRCodeToBase64Async(string StringQR);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    public interface WCIFServiceSoapChannel : yilibabyWCIF.WCIFServiceSoap, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.0")]
    public partial class WCIFServiceSoapClient : System.ServiceModel.ClientBase<yilibabyWCIF.WCIFServiceSoap>, yilibabyWCIF.WCIFServiceSoap
    {
        
    /// <summary>
    /// 实现此分部方法，配置服务终结点。
    /// </summary>
    /// <param name="serviceEndpoint">要配置的终结点</param>
    /// <param name="clientCredentials">客户端凭据</param>
    static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public WCIFServiceSoapClient(EndpointConfiguration endpointConfiguration) : 
                base(WCIFServiceSoapClient.GetBindingForEndpoint(endpointConfiguration), WCIFServiceSoapClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public WCIFServiceSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(WCIFServiceSoapClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public WCIFServiceSoapClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(WCIFServiceSoapClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public WCIFServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public System.Threading.Tasks.Task<string> HelloWorldAsync()
        {
            return base.Channel.HelloWorldAsync();
        }
        
        public System.Threading.Tasks.Task<string> GetAccessTokenAsync(string OpenId)
        {
            return base.Channel.GetAccessTokenAsync(OpenId);
        }
        
        public System.Threading.Tasks.Task<string> GetTicketAsync(string OpenId)
        {
            return base.Channel.GetTicketAsync(OpenId);
        }
        
        public System.Threading.Tasks.Task<string> GetCardTicketAsync(string OpenId)
        {
            return base.Channel.GetCardTicketAsync(OpenId);
        }
        
        public System.Threading.Tasks.Task<string> GetQRCodeTicketAsync(string OpenId, int scene_id, int expire_seconds)
        {
            return base.Channel.GetQRCodeTicketAsync(OpenId, scene_id, expire_seconds);
        }
        
        public System.Threading.Tasks.Task<string> GetQRStrCodeTicketAsync(string OpenId, string scene_str, int expire_seconds)
        {
            return base.Channel.GetQRStrCodeTicketAsync(OpenId, scene_str, expire_seconds);
        }
        
        public System.Threading.Tasks.Task<string> GetQRCodeLimitTicketAsync(string OpenId, int scene_id)
        {
            return base.Channel.GetQRCodeLimitTicketAsync(OpenId, scene_id);
        }
        
        public System.Threading.Tasks.Task<string> GetQRCodeLimitTicketExAsync(string OpenId, int ApplySource, int AssignType, int AssignRelateID)
        {
            return base.Channel.GetQRCodeLimitTicketExAsync(OpenId, ApplySource, AssignType, AssignRelateID);
        }
        
        public System.Threading.Tasks.Task<string> GetQRCodeLimitTicketEx2Async(string OpenId, int ApplySource, int AssignType, int AssignRelateID)
        {
            return base.Channel.GetQRCodeLimitTicketEx2Async(OpenId, ApplySource, AssignType, AssignRelateID);
        }
        
        public System.Threading.Tasks.Task<string> GetQRCodeLimitTicketWithStrAsync(string OpenId, string scene_str)
        {
            return base.Channel.GetQRCodeLimitTicketWithStrAsync(OpenId, scene_str);
        }
        
        public System.Threading.Tasks.Task<int> SendTemplateMessageAsync(string AuthKey, string FansID, string TemplateID, string url, string first, string remark, string keynotes, string keywords, string flag, int source)
        {
            return base.Channel.SendTemplateMessageAsync(AuthKey, FansID, TemplateID, url, first, remark, keynotes, keywords, flag, source);
        }
        
        public System.Threading.Tasks.Task<int> SendMessageAsync(string FansID, string MsgType, string Content, string flag)
        {
            return base.Channel.SendMessageAsync(FansID, MsgType, Content, flag);
        }
        
        public System.Threading.Tasks.Task<string> GetPromotorInfoAsync(int scene_id)
        {
            return base.Channel.GetPromotorInfoAsync(scene_id);
        }
        
        public System.Threading.Tasks.Task<string> GetLocalQRCodeAsync(int RelateType, int RelateID, string QRString)
        {
            return base.Channel.GetLocalQRCodeAsync(RelateType, RelateID, QRString);
        }
        
        public System.Threading.Tasks.Task<string> GetQRCodeTicketToBase64Async(string OpenId, int scene_id)
        {
            return base.Channel.GetQRCodeTicketToBase64Async(OpenId, scene_id);
        }
        
        public System.Threading.Tasks.Task<string> GetStringQRCodeToBase64Async(string StringQR)
        {
            return base.Channel.GetStringQRCodeToBase64Async(StringQR);
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
            if ((endpointConfiguration == EndpointConfiguration.WCIFServiceSoap))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.WCIFServiceSoap12))
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
            if ((endpointConfiguration == EndpointConfiguration.WCIFServiceSoap))
            {
                return new System.ServiceModel.EndpointAddress("http://t.interface.yilibabyclub.com/wcif/wcifservice.asmx");
            }
            if ((endpointConfiguration == EndpointConfiguration.WCIFServiceSoap12))
            {
                return new System.ServiceModel.EndpointAddress("http://t.interface.yilibabyclub.com/wcif/wcifservice.asmx");
            }
            throw new System.InvalidOperationException(string.Format("找不到名称为“{0}”的终结点。", endpointConfiguration));
        }
        
        public enum EndpointConfiguration
        {
            
            WCIFServiceSoap,
            
            WCIFServiceSoap12,
        }
    }
}
