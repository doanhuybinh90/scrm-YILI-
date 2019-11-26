using Abp.Application.Services;
using Pb.Wechat.Dto;
using Pb.Wechat.CustomerServiceWorkTimes.Dto;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.CustomerServiceWorkTimes
{
    public interface ICustomerServiceWorkTimeAppService : IAsyncCrudAppService<CustomerServiceWorkTimeDto, int, GetCustomerServiceWorkTimesInput, CustomerServiceWorkTimeDto, CustomerServiceWorkTimeDto>
    {
        Task<FileDto> GetListToExcel(GetCustomerServiceWorkTimesInput input);

        Task<CustomerServiceWorkTimeDto> GetFirstOrDefaultByInput(GetCustomerServiceWorkTimesInput input);
        Task<PagedResultDto<WorkTimeOutput>> GetWorkTimeList(GetCustomerServiceWorkTimesInput input);
        Task<bool> CheckIsWorkingTime(int mpId);
        Task<List<WorkTimeDto>> GetWorkTimeCache();
    }
}
