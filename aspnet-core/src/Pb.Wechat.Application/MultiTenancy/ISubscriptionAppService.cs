using System.Threading.Tasks;
using Abp.Application.Services;

namespace Pb.Wechat.MultiTenancy
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task UpgradeTenantToEquivalentEdition(int upgradeEditionId);
    }
}
