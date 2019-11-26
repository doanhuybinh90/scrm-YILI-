using Pb.Wechat.Dto;
using Pb.Wechat.CustomerArticles.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.CustomerArticles.Exporting
{
    public interface ICustomerArticleListExcelExporter
    {
        FileDto ExportToFile(List<CustomerArticleDto> modelListDtos);
    }
}
