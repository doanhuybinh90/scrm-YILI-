using Abp.Extensions;
using Abp.Timing.Timezone;
using Pb.Wechat.DataExporting.Excel.EpPlus;
using Pb.Wechat.Dto;
using Pb.Wechat.CustomerServiceReports.Dto;
using System.Collections.Generic;
using System.Linq;

namespace Pb.Wechat.CustomerServiceReports.Exporting
{
    public class CustomerServiceReportListExcelExporter : EpPlusExcelExporterBase, ICustomerServiceReportListExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;

        public CustomerServiceReportListExcelExporter(
            ITimeZoneConverter timeZoneConverter)
        {
            _timeZoneConverter = timeZoneConverter;
        }

        public FileDto ExportToFile(List<CustomerServiceReportDto> modelListDtos)
        {
            return CreateExcelPackage(
                "Data.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add("Data");
                    sheet.OutLineApplyStyle = true;
                    AddHeader(
                        sheet
, "昵称"
, "统计日期"
, "在线时长"
, "服务次数"
, "接待人数"
, "服务话数"
, "日均评分"

                    );

                    AddObjects(
                        sheet, 2, modelListDtos
, _ => _.NickName
, _ => _.ReportDate
, _ => _.OnlineTime
, _ => _.ServiceCount
, _ => _.ReceiveCount
, _ => _.ServiceMsgCount
, _ => _.AvgScore

                        );

                    //Formatting cells

                    var timeColumn = sheet.Column(2);
                    timeColumn.Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";

                    for (var i = 1; i <= 9; i++)
                    {
                        if (i.IsIn(1, 9)) //Don't AutoFit Parameters and Exception
                        {
                            continue;
                        }

                        sheet.Column(i).AutoFit();
                    }
                });
        }
    }
}
