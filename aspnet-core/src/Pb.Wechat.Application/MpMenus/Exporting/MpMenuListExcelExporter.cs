using Abp.Extensions;
using Abp.Timing.Timezone;
using Pb.Wechat.DataExporting.Excel.EpPlus;
using Pb.Wechat.Dto;
using Pb.Wechat.MpEvents.Dto;
using Pb.Wechat.MpMenus.Exporting;
using System.Collections.Generic;

namespace Pb.Wechat.MpMenus.Dto.MpMenus.Exporting
{
    public class MpMenuListExcelExporter : EpPlusExcelExporterBase, IMpMenuListExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;

        public MpMenuListExcelExporter(
            ITimeZoneConverter timeZoneConverter)
        {
            _timeZoneConverter = timeZoneConverter;
        }

        public FileDto ExportToFile(List<MpMenuDto> modelListDtos)
        {
            return CreateExcelPackage(
                "Data.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add("Data");
                    sheet.OutLineApplyStyle = true;
                    var typeEnum = EnumHelper.GetEnum(typeof(MpMenuType));
                    var mediaTypeEnum = EnumHelper.GetEnum(typeof(MpMessageType));
                    AddHeader(
                        sheet
						,"ID"
,"MpID"
,"Name"
                    );

                    AddObjects(
                        sheet, 2, modelListDtos
						,_ => _.Id
,_ => _.MpID
,_ => _.Name
                        );

                    //Formatting cells

                    //var timeColumn = sheet.Column(7);
                    //timeColumn.Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";

                    for (var i = 1; i <= 10; i++)
                    {
                        if (i.IsIn(1, 10)) //Don't AutoFit Parameters and Exception
                        {
                            continue;
                        }

                        sheet.Column(i).AutoFit();
                    }
                });
        }
    }
}
