using Pb.Wechat.Dto;
using Pb.Wechat.CustomerServiceConversationMsgs.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.CustomerServiceConversationMsgs.Exporting
{
    public interface ICustomerServiceConversationMsgListExcelExporter
    {
        FileDto ExportToFile(List<CustomerServiceConversationMsgDto> modelListDtos);
    }
}
