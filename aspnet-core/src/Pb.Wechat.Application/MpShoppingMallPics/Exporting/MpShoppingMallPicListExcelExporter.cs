using System.Collections.Generic;
using Abp.Extensions;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Pb.Wechat.DataExporting.Excel.EpPlus;
using Pb.Wechat.Dto;
using Pb.Wechat.MpShoppingMallPics.Dto;

namespace Pb.Wechat.Auditing.Exporting
{
    public class MpShoppingMallPicListExcelExporter : EpPlusExcelExporterBase, IMpShoppingMallPicListExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public MpShoppingMallPicListExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<MpShoppingMallPicDto> modelListDtos)
        {
            return CreateExcelPackage(
                "Data.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add("Data");
                    sheet.OutLineApplyStyle = true;
                    //var typeEnum = EnumHelper.GetEnum(typeof(MpShoppingMallPicType));
                    AddHeader(
                        sheet,
                        "名称",
                        "图片链接",
                        "本地图片Url",
                        "本地图片路径",
                        "平台ID"
                        
                    );

                    AddObjects(
                        sheet, 2, modelListDtos,
                        _ => _.Name,
                       _=>_.LinkUrl,
                       _ => _.LocalPicUrl,
                       _ => _.LocalPicPath,
                       _ =>_.MpID
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