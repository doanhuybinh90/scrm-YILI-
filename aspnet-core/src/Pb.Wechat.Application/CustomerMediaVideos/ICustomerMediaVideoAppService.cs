using Abp.Application.Services;
using Pb.Wechat.Dto;
using Pb.Wechat.CustomerMediaVideos.Dto;
using System.Threading.Tasks;

namespace Pb.Wechat.CustomerMediaVideos
{
    public interface ICustomerMediaVideoAppService : IAsyncCrudAppService<CustomerMediaVideoDto, int, GetCustomerMediaVideosInput, CustomerMediaVideoDto, CustomerMediaVideoDto>
    {
        Task<FileDto> GetListToExcel(GetCustomerMediaVideosInput input);
        Task<CustomerMediaVideoDto> GetModelByReplyTypeAsync(string mediaId, int mpId);
    }
}
