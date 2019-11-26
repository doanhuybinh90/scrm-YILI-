using Pb.Wechat.Dto;
using Pb.Wechat.CustomerMediaImages.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.CustomerMediaImages.Exporting
{
    public interface ICustomerMediaImageListExcelExporter
    {
        FileDto ExportToFile(List<CustomerMediaImageDto> modelListDtos);
    }
}
