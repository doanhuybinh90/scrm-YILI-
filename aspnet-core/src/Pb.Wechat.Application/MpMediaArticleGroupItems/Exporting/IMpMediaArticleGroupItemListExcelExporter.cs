using Pb.Wechat.Dto;
using Pb.Wechat.MpMediaArticleGroupItems.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pb.Wechat.MpMediaArticleGroupItems.Exporting
{
    public interface IMpMediaArticleGroupItemListExcelExporter
     {
        FileDto ExportToFile(List<MpMediaArticleGroupItemDto> modelListDtos);
    }

}
