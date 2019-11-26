using Abp.Extensions;
using Abp.Timing.Timezone;
using Pb.Wechat.DataExporting.Excel.EpPlus;
using Pb.Wechat.Dto;
using Pb.Wechat.UserMps.Dto;
using System.Collections.Generic;

namespace Pb.Wechat.UserMps.Exporting
{
    public class UserMpListExcelExporter : EpPlusExcelExporterBase, IUserMpListExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;

        public UserMpListExcelExporter(
            ITimeZoneConverter timeZoneConverter)
        {
            _timeZoneConverter = timeZoneConverter;
        }

        public FileDto ExportToFile(List<UserMpDto> modelListDtos)
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
, "UserId"
, "CurrentMpID"
                    );

                    AddObjects(
                        sheet, 2, modelListDtos
						,_ => _.Id
,_ => _.UserId
,_ => _.CurrentMpID
                        );

                    //Formatting cells

                    //var timeColumn = sheet.Column(7);
                    //timeColumn.Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";

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
