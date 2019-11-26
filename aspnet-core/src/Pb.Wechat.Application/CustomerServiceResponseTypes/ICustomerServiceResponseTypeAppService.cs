using Abp.Application.Services;
using Pb.Wechat.Dto;
using Pb.Wechat.CustomerServiceResponseTypes.Dto;
using System.Threading.Tasks;
using Abp;
using System.Collections.Generic;

namespace Pb.Wechat.CustomerServiceResponseTypes
{
    public interface ICustomerServiceResponseTypeAppService : IAsyncCrudAppService<CustomerServiceResponseTypeDto, int, GetCustomerServiceResponseTypesInput, CustomerServiceResponseTypeDto, CustomerServiceResponseTypeDto>
    {
        Task<FileDto> GetListToExcel(GetCustomerServiceResponseTypesInput input);

        Task<List<NameValue<string>>> GetTypeSelected();
    }
}
