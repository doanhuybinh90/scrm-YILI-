using System.Collections.Generic;
using Pb.Wechat.Dto;
using Pb.Wechat.CustomerServiceOnlines.Dto;

namespace Pb.Wechat.Auditing.Exporting
{
    public interface ICustomerServiceOnlineListExcelExporter
    {
        FileDto ExportToFile(List<CustomerServiceOnlineDto> modelListDtos);
    }
}
