using System.Collections.Generic;
using Abp.Extensions;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Pb.Wechat.DataExporting.Excel.EpPlus;
using Pb.Wechat.Dto;
using Pb.Wechat.CustomerServiceResponseTexts.Dto;
using System.Linq;

namespace Pb.Wechat.Auditing.Exporting
{
    public class CustomerServiceResponseTextListExcelExporter : EpPlusExcelExporterBase, ICustomerServiceResponseTextListExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CustomerServiceResponseTextListExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<CustomerServiceResponseTextDto> modelListDtos)
        {
            return CreateExcelPackage(
                "Data.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add("Data");
                    var typeEnum = EnumHelper.GetEnum(typeof(ResponseType));
                    sheet.OutLineApplyStyle = true;
                    //var typeEnum = EnumHelper.GetEnum(typeof(CustomerServiceResponseTextType));
                    AddHeader(
                        sheet,
                        "回复类型",
                        "回复文案",
                        "平台ID"
                        
                    );

                    AddObjects(
                        sheet, 2, modelListDtos,
                        _ => typeEnum.Where(c => c.Item1 == _.ResponseType).FirstOrDefault()?.Item2,
                       _=>_.ResponseText,
                       _=>_.MpID
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