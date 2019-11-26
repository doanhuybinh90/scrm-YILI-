using Pb.Wechat.Dto;
using Pb.Wechat.CustomerServiceReports.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.CustomerServiceReports.Exporting
{
    public interface ICustomerServiceReportListExcelExporter
    {
        FileDto ExportToFile(List<CustomerServiceReportDto> modelListDtos);
    }
}
