using Pb.Wechat.Dto;
using Pb.Wechat.CustomerMediaVoices.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.CustomerMediaVoices.Exporting
{
    public interface ICustomerMediaVoiceListExcelExporter
    {
        FileDto ExportToFile(List<CustomerMediaVoiceDto> modelListDtos);
    }
}
