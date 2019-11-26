
using Pb.Wechat.Dto;
using Pb.Wechat.CustomerArticleGroups.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.CustomerArticleGroups.Exporting
{
    public interface ICustomerArticleGroupListExcelExporter
    {
        FileDto ExportToFile(List<CustomerArticleGroupDto> modelListDtos);
    }
}
