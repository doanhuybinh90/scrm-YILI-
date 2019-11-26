using Abp.Application.Services;
using Pb.Wechat.Dto;
using Pb.Wechat.CustomerServiceOnlines.Dto;
using System.Threading.Tasks;

namespace Pb.Wechat.CustomerServiceOnlines
{
    public interface ICustomerServiceOnlineAppService : IAsyncCrudAppService<CustomerServiceOnlineDto, int, GetCustomerServiceOnlinesInput, CustomerServiceOnlineDto, CustomerServiceOnlineDto>
    {
        Task<FileDto> GetListToExcel(GetCustomerServiceOnlinesInput input);
        //Task SyncCustomerList(int mpId);
        Task GetKfList();
        Task<CustomerServiceOnlineDto> GetFirstOrDefault(GetCustomerServiceOnlinesInput input);

        Task<CustomerServiceOnlineDto> GetAutoJoinCustomer(int mpId);
        Task<CustomerServiceOnline> GetByMessageToken(string messageToken);
        Task<CustomerServiceOnlineDto> GetLastCustomerService(int customerId);
    }
}
