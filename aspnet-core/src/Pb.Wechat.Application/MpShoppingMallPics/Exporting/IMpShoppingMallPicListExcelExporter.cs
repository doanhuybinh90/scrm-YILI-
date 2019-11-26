using System.Collections.Generic;
using Pb.Wechat.Dto;
using Pb.Wechat.MpShoppingMallPics.Dto;

namespace Pb.Wechat.Auditing.Exporting
{
    public interface IMpShoppingMallPicListExcelExporter
    {
        FileDto ExportToFile(List<MpShoppingMallPicDto> modelListDtos);
    }
}
