namespace Pb.Wechat
{
    public interface IAppFolders
    {
        string TempFileDownloadFolder { get; }

        string SampleProfileImagesFolder { get; }

        string WebLogsFolder { get; set; }
        string QrCodeDownloadFolder { get; set; }
    }
}