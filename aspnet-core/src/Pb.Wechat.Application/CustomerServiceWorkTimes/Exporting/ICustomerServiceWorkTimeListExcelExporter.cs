using System.Collections.Generic;
using Pb.Wechat.Dto;
using Pb.Wechat.CustomerServiceWorkTimes.Dto;

namespace Pb.Wechat.Auditing.Exporting
{
    public interface ICustomerServiceWorkTimeListExcelExporter
    {
        FileDto ExportToFile(List<CustomerServiceWorkTimeDto> modelListDtos);
    }
}
