using Abp.AutoMapper;
using Pb.Wechat.Web.Authentication.External;

namespace Pb.Wechat.Web.Models.TokenAuth
{
    [AutoMapFrom(typeof(ExternalLoginProviderInfo))]
    public class ExternalLoginProviderInfoModel
    {
        public string Name { get; set; }

        public string ClientId { get; set; }
    }
}
