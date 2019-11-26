using Abp.Timing.Timezone;
using Pb.Wechat.DataExporting.Excel.EpPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pb.Wechat.Dto;
using Pb.Wechat.MpMediaArticleGroupItems.Dto;
using Abp.Extensions;

namespace Pb.Wechat.MpMediaArticleGroupItems.Exporting
{
    public class MpMediaArticleGroupItemListExcelExporter : EpPlusExcelExporterBase, IMpMediaArticleGroupItemListExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;

        public MpMediaArticleGroupItemListExcelExporter(
            ITimeZoneConverter timeZoneConverter)
        {
            _timeZoneConverter = timeZoneConverter;
        }

        public FileDto ExportToFile(List<MpMediaArticleGroupItemDto> modelListDtos)
        {
            return CreateExcelPackage(
                "Data.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add("Data");
                    sheet.OutLineApplyStyle = true;
                    //var typeEnum = EnumHelper.GetEnum(typeof(RewardTypes));
                    AddHeader(
                        sheet
                        , "ID"
, "MpID"
, "GroupID"
, "ArticleID"
, "SortIndex"
                    );

                    AddObjects(
                        sheet, 2, modelListDtos
                        , _ => _.Id
, _ => _.MpID
, _ => _.GroupID
, _ => _.ArticleID
, _ => _.SortIndex
                        );

                  ;

                    for (var i = 1; i <= 12; i++)
                    {
                        if (i.IsIn(1, 12)) //Don't AutoFit Parameters and Exception
                        {
                            continue;
                        }

                        sheet.Column(i).AutoFit();
                    }
                });
        }
    }
}
