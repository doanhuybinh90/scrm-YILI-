using System.Collections.Generic;
using System.Linq;
using Abp.Extensions;
using Abp.MultiTenancy;
using Microsoft.AspNetCore.Hosting;
using Pb.Wechat.Configuration;

namespace Pb.Wechat.Web.Url
{
    public abstract class WebUrlServiceBase
    {
        public const string TenancyNamePlaceHolder = "{TENANCY_NAME}";

        public abstract string WebSiteRootAddressFormatKey { get; }

        public abstract string WebSiteRootIPFormatKey { get; }

        public abstract string ServerRootAddressFormatKey { get; }
        public abstract string RemotingFileUploadUrl { get;  }
        public abstract string RemotingFileDownloadUrl { get;  }
        public abstract string KindEditorSavePath { get; }
        public string WebSiteRootAddressFormat => _hostingEnvironment.GetAppConfiguration()[WebSiteRootAddressFormatKey] ?? "http://localhost:62114/";

        public string WebSiteRootIPFormat => _hostingEnvironment.GetAppConfiguration()[WebSiteRootIPFormatKey] ?? "127.0.0.1";

        public string ServerRootAddressFormat => _hostingEnvironment.GetAppConfiguration()[ServerRootAddressFormatKey] ?? "http://localhost:62114/";

        public string RemotingFileUploadFormat => _hostingEnvironment.GetAppConfiguration()[RemotingFileUploadUrl] ?? "http://localhost:1284/Upload";
        public string RemotingFileDownloadFormat => _hostingEnvironment.GetAppConfiguration()[RemotingFileDownloadUrl] ?? "http://localhost:1284/Temp/Downloads";
        public string KindEditorSavePathFormat => _hostingEnvironment.GetAppConfiguration()[KindEditorSavePath] ?? "/Temp/KindEditor/Upload";
        public bool SupportsTenancyNameInUrl
        {
            get
            {
                var siteRootFormat = WebSiteRootAddressFormat;
                return !siteRootFormat.IsNullOrEmpty() && siteRootFormat.Contains(TenancyNamePlaceHolder);
            }
        }

        private readonly ITenantCache _tenantCache;
        private readonly IHostingEnvironment _hostingEnvironment;

        public WebUrlServiceBase(
            IHostingEnvironment hostingEnvironment,
            ITenantCache tenantCache
        )
        {
            _hostingEnvironment = hostingEnvironment;
            _tenantCache = tenantCache;
        }

        public string GetSiteRootAddress(string tenancyName = null)
        {
            return ReplaceTenancyNameInUrl(WebSiteRootAddressFormat, tenancyName);
        }

        public string GetServerRootAddress(string tenancyName = null)
        {
            return ReplaceTenancyNameInUrl(ServerRootAddressFormat, tenancyName);
        }

        public string GetRemotingFileUploadUrl()
        {
            return RemotingFileUploadFormat;
        }
        public string GetRemotingFileDownloadUrl()
        {
            return RemotingFileDownloadFormat;
        }
        public string GetKindEditorSavePath ()
        {
            return KindEditorSavePathFormat;
        }
        public List<string> GetRedirectAllowedExternalWebSites()
        {
            var values = _hostingEnvironment.GetAppConfiguration()["App:RedirectAllowedExternalWebSites"];
            return values?.Split(',').ToList() ?? new List<string>();
        }

        private string ReplaceTenancyNameInUrl(string siteRootFormat, string tenancyName)
        {
            if (!siteRootFormat.Contains(TenancyNamePlaceHolder))
            {
                return siteRootFormat;
            }

            if (siteRootFormat.Contains(TenancyNamePlaceHolder + "."))
            {
                siteRootFormat = siteRootFormat.Replace(TenancyNamePlaceHolder + ".", TenancyNamePlaceHolder);
            }

            if (tenancyName.IsNullOrEmpty())
            {
                return siteRootFormat.Replace(TenancyNamePlaceHolder, "");
            }

            return siteRootFormat.Replace(TenancyNamePlaceHolder, tenancyName + ".");
        }
    }
}