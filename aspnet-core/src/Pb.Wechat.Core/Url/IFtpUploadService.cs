namespace Pb.Wechat.Url
{
    public interface IFtpUploadService
    {
        string ServerDomain { get; }

        string UserName { get; }

        string Password { get; }

        string ViewUrl { get; }
    }
}
