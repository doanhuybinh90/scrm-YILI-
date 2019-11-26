using Abp.Extensions;
using Abp.Timing.Timezone;
using Pb.Wechat.DataExporting.Excel.EpPlus;
using Pb.Wechat.Dto;
using Pb.Wechat.MpSelfArticles.Exporting;
using System.Collections.Generic;

namespace Pb.Wechat.MpSelfArticles.Dto.MpSelfArticles.Exporting
{
    public class MpSelfArticleListExcelExporter : EpPlusExcelExporterBase, IMpSelfArticleListExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;

        public MpSelfArticleListExcelExporter(
            ITimeZoneConverter timeZoneConverter)
        {
            _timeZoneConverter = timeZoneConverter;
        }

        public FileDto ExportToFile(List<MpSelfArticleDto> modelListDtos)
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
						,"ID"
,"MpID"
,"Title"
,"Description"
,"PicFileID"
, "AUrl"
                    );

                    AddObjects(
                        sheet, 2, modelListDtos
						,_ => _.Id
,_ => _.MpID
,_ => _.Title
,_ => _.Description
,_ => _.PicFileID
,_ => _.AUrl
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
