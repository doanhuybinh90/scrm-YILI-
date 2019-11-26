
using Pb.Wechat.Dto;
using Pb.Wechat.CustomerMediaVideos.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.CustomerMediaVideos.Exporting
{
    public interface ICustomerMediaVideoListExcelExporter
    {
        FileDto ExportToFile(List<CustomerMediaVideoDto> modelListDtos);
    }
}
