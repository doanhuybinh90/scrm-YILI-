using Pb.Wechat.Dto;
using Pb.Wechat.CustomerServiceConversations.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.CustomerServiceConversations.Exporting
{
    public interface ICustomerServiceConversationListExcelExporter
    {
        FileDto ExportToFile(List<CustomerServiceConversationDto> modelListDtos);
    }
}
