using Microsoft.Extensions.Configuration;

namespace Pb.Wechat.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
