using System.Threading.Tasks;
using Abp.Configuration;

namespace Pb.Wechat.Timing
{
    public interface ITimeZoneService
    {
        Task<string> GetDefaultTimezoneAsync(SettingScopes scope, int? tenantId);
    }
}
