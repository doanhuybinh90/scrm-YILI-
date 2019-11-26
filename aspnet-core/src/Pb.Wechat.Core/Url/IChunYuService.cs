namespace Pb.Wechat.Url
{
    public interface IChunYuService
    {
        string ChunYuPartner { get; }

        string ChunYuPassword { get; }

        string ChunYuBaseUrl { get; }

        string ChunYuCreateUrl { get; }

        string ChunYuCreateAddUrl { get; }

        string ChunYuCloseUrl { get; }

        string ChunYuLoginUrl { get; }

    }
}
