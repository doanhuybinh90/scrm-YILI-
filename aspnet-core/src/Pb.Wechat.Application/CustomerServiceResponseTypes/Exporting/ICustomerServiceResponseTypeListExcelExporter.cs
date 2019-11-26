using System.Collections.Generic;
using Pb.Wechat.Dto;
using Pb.Wechat.CustomerServiceResponseTypes.Dto;

namespace Pb.Wechat.Auditing.Exporting
{
    public interface ICustomerServiceResponseTypeListExcelExporter
    {
        FileDto ExportToFile(List<CustomerServiceResponseTypeDto> modelListDtos);
    }
}
