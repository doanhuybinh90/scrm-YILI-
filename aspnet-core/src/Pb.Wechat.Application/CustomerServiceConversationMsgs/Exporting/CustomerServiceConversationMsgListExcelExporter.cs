using Abp.Extensions;
using Abp.Timing.Timezone;
using Pb.Wechat.DataExporting.Excel.EpPlus;
using Pb.Wechat.Dto;
using Pb.Wechat.CustomerServiceConversationMsgs.Dto;
using System.Collections.Generic;
using System.Linq;

namespace Pb.Wechat.CustomerServiceConversationMsgs.Exporting
{
    public class CustomerServiceConversationMsgListExcelExporter : EpPlusExcelExporterBase, ICustomerServiceConversationMsgListExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;

        public CustomerServiceConversationMsgListExcelExporter(
            ITimeZoneConverter timeZoneConverter)
        {
            _timeZoneConverter = timeZoneConverter;
        }

        public FileDto ExportToFile(List<CustomerServiceConversationMsgDto> modelListDtos)
        {
            return CreateExcelPackage(
                "Data.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add("Data");
                    sheet.OutLineApplyStyle = true;
                    AddHeader(
                        sheet
,"CreationTime"
,"CustomerId"
,"FanId"
,"MediaId"
,"MediaUrl"
,"MpID"
,"MsgContent"
,"MsgType"
,"Sender"
                    );

                    AddObjects(
                        sheet, 2, modelListDtos
,_ => _.CreationTime
,_ => _.CustomerId
,_ => _.FanId
,_ => _.MediaId
,_ => _.MediaUrl
,_ => _.MpID
,_ => _.MsgContent
,_ => _.MsgType
,_ => _.Sender
                        );

                    //Formatting cells

                    //var timeColumn = sheet.Column(7);
                    //timeColumn.Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";

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
