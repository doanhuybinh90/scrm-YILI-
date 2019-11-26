using Abp.Application.Services;
using Pb.Wechat.Dto;
using Pb.Wechat.CustomerServiceResponseTexts.Dto;
using System.Threading.Tasks;

namespace Pb.Wechat.CustomerServiceResponseTexts
{
    public interface ICustomerServiceResponseTextAppService : IAsyncCrudAppService<CustomerServiceResponseTextDto, int, GetCustomerServiceResponseTextsInput, CustomerServiceResponseTextDto, CustomerServiceResponseTextDto>
    {
        Task<FileDto> GetListToExcel(GetCustomerServiceResponseTextsInput input);

        Task<CustomerServiceResponseTextDto> GetFirstOrDefaultByInput(GetCustomerServiceResponseTextsInput input);
        Task<string> GetCustomerDefaultResponseString(int mpId);
        Task<bool> CheckIsWorkingTime(int mpId);
        Task<string> GetWaitResponseString(int mpId);
    }
}
