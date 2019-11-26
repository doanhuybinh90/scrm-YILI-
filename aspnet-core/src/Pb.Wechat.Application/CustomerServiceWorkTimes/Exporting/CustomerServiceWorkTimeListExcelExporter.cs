using System.Collections.Generic;
using Abp.Extensions;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Pb.Wechat.DataExporting.Excel.EpPlus;
using Pb.Wechat.Dto;
using Pb.Wechat.CustomerServiceWorkTimes.Dto;

namespace Pb.Wechat.Auditing.Exporting
{
    public class CustomerServiceWorkTimeListExcelExporter : EpPlusExcelExporterBase, ICustomerServiceWorkTimeListExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CustomerServiceWorkTimeListExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<CustomerServiceWorkTimeDto> modelListDtos)
        {
            return CreateExcelPackage(
                "Data.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add("Data");
                    //var typeEnum = EnumHelper.GetEnum(typeof(ResponseType));
                    sheet.OutLineApplyStyle = true;
                    //var typeEnum = EnumHelper.GetEnum(typeof(CustomerServiceWorkTimeType));
                    AddHeader(
                        sheet,
                        "周几",
                        "上午工作时间",
                        "下午工作时间",
                        "平台ID"

                    );

                    AddObjects(
                        sheet, 2, modelListDtos,
                        _=>_.WeekDay,
                        _ => _.MorningStartHour+":"+_.MorningStartMinute+"至"+_.MorningEndHour+":"+_.MorningEndMinute,
                       _ => _.AfternoonStartHour + ":" + _.AfternoonStartMinute + "至" + _.AfternoonEndHour + ":" + _.AfternoonEndMinute,
                       _ => _.MpID
                        );

                    //Formatting cells

                    //var timeColumn = sheet.Column(3);
                    //timeColumn.Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";
                    //var timeColumnx = sheet.Column(4);
                    //timeColumnx.Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";

                    for (var i = 1; i <= 10; i++)
                    {
                        if (i.IsIn(5, 10)) //Don't AutoFit Parameters and Exception
                        {
                            continue;
                        }

                        sheet.Column(i).AutoFit();
                    }
                });
        }
    }
}