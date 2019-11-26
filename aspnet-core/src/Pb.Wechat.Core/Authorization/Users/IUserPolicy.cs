using System.Threading.Tasks;
using Abp.Domain.Policies;

namespace Pb.Wechat.Authorization.Users
{
    public interface IUserPolicy : IPolicy
    {
        Task CheckMaxUserCountAsync(int tenantId);
    }
}
