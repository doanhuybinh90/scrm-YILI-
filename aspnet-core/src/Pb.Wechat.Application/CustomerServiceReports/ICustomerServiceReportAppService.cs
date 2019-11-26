using Abp.Application.Services;
using Pb.Wechat.Dto;
using Pb.Wechat.CustomerServiceReports.Dto;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Pb.Wechat.CustomerServiceReports
{
    public interface ICustomerServiceReportAppService : IAsyncCrudAppService<CustomerServiceReportDto, long, GetCustomerServiceReportsInput, CustomerServiceReportDto, CustomerServiceReportDto>
    {
        Task<FileDto> GetListToExcel(GetCustomerServiceReportsInput input);
    }
}
