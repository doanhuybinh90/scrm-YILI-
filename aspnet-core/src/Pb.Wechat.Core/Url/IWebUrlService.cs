using System.Collections.Generic;

namespace Pb.Wechat.Url
{
    public interface IWebUrlService
    {
        string WebSiteRootAddressFormat { get; }

        string WebSiteRootIPFormat { get; }

        string ServerRootAddressFormat { get; }

        bool SupportsTenancyNameInUrl { get; }
        string RemotingFileUploadUrl { get; }
        string RemotingFileDownloadUrl { get; }
        string GetSiteRootAddress(string tenancyName = null);

        string GetServerRootAddress(string tenancyName = null);

        List<string> GetRedirectAllowedExternalWebSites();
        string GetRemotingFileUploadUrl();
        string GetRemotingFileDownloadUrl();
        string KindEditorSavePath { get; }
    }
}
