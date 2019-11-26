using System.Collections.Generic;
using Pb.Wechat.Dto;
using Pb.Wechat.TaskGroupMessages.Dto;

namespace Pb.Wechat.Auditing.Exporting
{
    public interface ITaskGroupMessageListExcelExporter
    {
        FileDto ExportToFile(List<TaskGroupMessageDto> modelListDtos);
    }
}
