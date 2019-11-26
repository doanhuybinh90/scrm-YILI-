using Abp.Application.Services;
using Pb.Wechat.Dto;
using Pb.Wechat.Logging.Dto;

namespace Pb.Wechat.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
