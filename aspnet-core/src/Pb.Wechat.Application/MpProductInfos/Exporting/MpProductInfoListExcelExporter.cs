using System.Collections.Generic;
using Abp.Extensions;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Pb.Wechat.DataExporting.Excel.EpPlus;
using Pb.Wechat.Dto;
using Pb.Wechat.MpProductInfos.Dto;

namespace Pb.Wechat.Auditing.Exporting
{
    public class MpProductInfoListExcelExporter : EpPlusExcelExporterBase, IMpProductInfoListExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public MpProductInfoListExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<MpProductInfoDto> modelListDtos)
        {
            return CreateExcelPackage(
                "Data.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add("Data");
                    sheet.OutLineApplyStyle = true;
                    //var typeEnum = EnumHelper.GetEnum(typeof(MpProductInfoType));
                    AddHeader(
                        sheet,
                        "标题",
                        "副标题",
                        "产品介绍"
                        
                    );

                    AddObjects(
                        sheet, 2, modelListDtos,
                        _ => _.Title,
                       _=>_.SubTitle,
                       _=>_.ProductIntroduce
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