using Abp.Extensions;
using Abp.Timing.Timezone;
using Pb.Wechat.DataExporting.Excel.EpPlus;
using Pb.Wechat.Dto;
using Pb.Wechat.MpMediaVideos.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.MpMediaVideos.Exporting
{
    public class MpMediaVideoListExcelExporter : EpPlusExcelExporterBase, IMpMediaVideoListExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;

        public MpMediaVideoListExcelExporter(
            ITimeZoneConverter timeZoneConverter)
        {
            _timeZoneConverter = timeZoneConverter;
        }

        public FileDto ExportToFile(List<MpMediaVideoDto> modelListDtos)
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
, "MediaID"
, "Title"
, "Description"
, "FileID"
                    );

                    AddObjects(
                        sheet, 2, modelListDtos
                        , _ => _.Id
, _ => _.MpID
, _ => _.MediaID
, _ => _.Title
, _ => _.Description
, _ => _.FileID
                        );

                    //Formatting cells

                    //var timeColumn = sheet.Column(7);
                    //timeColumn.Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";

                    for (var i = 1; i <= 13; i++)
                    {
                        if (i.IsIn(1, 13)) //Don't AutoFit Parameters and Exception
                        {
                            continue;
                        }

                        sheet.Column(i).AutoFit();
                    }
                });
        }
    }
}
