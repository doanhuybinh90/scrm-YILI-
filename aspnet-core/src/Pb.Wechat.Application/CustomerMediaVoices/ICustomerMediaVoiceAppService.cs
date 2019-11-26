using Abp.Application.Services;
using Pb.Wechat.Dto;
using Pb.Wechat.CustomerMediaVoices.Dto;
using System.Threading.Tasks;

namespace Pb.Wechat.CustomerMediaVoices
{
    public interface ICustomerMediaVoiceAppService : IAsyncCrudAppService<CustomerMediaVoiceDto, int, GetCustomerMediaVoicesInput, CustomerMediaVoiceDto, CustomerMediaVoiceDto>
    {
        Task<FileDto> GetListToExcel(GetCustomerMediaVoicesInput input);
    }
}
