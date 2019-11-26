using System.Collections.Generic;
using Pb.Wechat.Dto;
using Pb.Wechat.CustomerServiceResponseTexts.Dto;

namespace Pb.Wechat.Auditing.Exporting
{
    public interface ICustomerServiceResponseTextListExcelExporter
    {
        FileDto ExportToFile(List<CustomerServiceResponseTextDto> modelListDtos);
    }
}
