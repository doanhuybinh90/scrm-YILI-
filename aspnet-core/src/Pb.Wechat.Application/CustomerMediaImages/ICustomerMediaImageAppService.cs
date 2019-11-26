using Abp.Application.Services;
using Pb.Wechat.Dto;
using Pb.Wechat.CustomerMediaImages.Dto;
using System.Threading.Tasks;

namespace Pb.Wechat.CustomerMediaImages
{
    public interface ICustomerMediaImageAppService : IAsyncCrudAppService<CustomerMediaImageDto, int, GetCustomerMediaImagesInput, CustomerMediaImageDto, CustomerMediaImageDto>
    {
        Task<FileDto> GetListToExcel(GetCustomerMediaImagesInput input);
        Task<CustomerMediaImageDto> GetByMediaID(string mediaID);

    }
}
