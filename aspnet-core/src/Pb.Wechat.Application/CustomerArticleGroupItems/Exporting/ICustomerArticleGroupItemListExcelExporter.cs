
using Pb.Wechat.Dto;
using Pb.Wechat.CustomerArticleGroupItems.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.CustomerArticleGroupItems.Exporting
{
    public interface ICustomerArticleGroupItemListExcelExporter
    {
        FileDto ExportToFile(List<CustomerArticleGroupItemDto> modelListDtos);
    }
}
