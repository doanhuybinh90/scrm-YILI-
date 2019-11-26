using System;
using System.IO;
using System.Text;
using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.Configuration.Startup;
using Abp.Hangfire;
using Abp.IO;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Runtime.Caching.Redis;
using Abp.Zero.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Pb.Wechat.Configuration;
using Pb.Wechat.EntityFrameworkCore;
using Pb.Wechat.Web.Authentication.JwtBearer;
using Pb.Wechat.Web.Authentication.TwoFactor;
using Pb.Wechat.Web.Configuration;
using Abp.Hangfire.Configuration;
using Pb.Hangfire;
#if FEATURE_SIGNALR
using Abp.Web.SignalR;
#endif

namespace Pb.Wechat.Web
{
    [DependsOn(
        typeof(AbpZeroTemplateApplicationModule),
        typeof(AbpZeroTemplateEntityFrameworkCoreModule),
        typeof(AbpAspNetCoreModule),
#if FEATURE_SIGNALR
        typeof(AbpWebSignalRModule),
#endif
        typeof(PbHangfireModule),
        typeof(AbpRedisCacheModule), //AbpRedisCacheModule dependency (and Abp.RedisCache nuget package) can be removed if not using Redis cache
        typeof(AbpHangfireAspNetCoreModule) //AbpHangfireModule dependency (and Abp.Hangfire.AspNetCore nuget package) can be removed if not using Hangfire
    )] 
    public class AbpZeroTemplateWebCoreModule : AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public AbpZeroTemplateWebCoreModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }
        
        public override void PreInitialize()
        {
            //Set default connection string
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                AbpZeroTemplateConsts.ConnectionStringName
            );

            //Use database for language management
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            Configuration.Modules.AbpAspNetCore()
                .CreateControllersForAppServices(
                    typeof(AbpZeroTemplateApplicationModule).GetAssembly()
                );

            Configuration.Caching.Configure(TwoFactorCodeCacheItem.CacheName, cache =>
            {
                cache.DefaultAbsoluteExpireTime = TimeSpan.FromMinutes(2);
            });

            if (_appConfiguration["Authentication:JwtBearer:IsEnabled"] != null && bool.Parse(_appConfiguration["Authentication:JwtBearer:IsEnabled"]))
            {
                ConfigureTokenAuth();
            }
            
            Configuration.ReplaceService<IAppConfigurationAccessor, AppConfigurationAccessor>();

            //Uncomment this line to use Hangfire instead of default background job manager (remember also to uncomment related lines in Startup.cs file(s)).
            Configuration.BackgroundJobs.UseHangfire();

            //Uncomment this line to use Redis cache instead of in-memory cache.
            //See app.config for Redis configuration and connection string

            if (_env.IsProduction())
                Configuration.Caching.UseRedis(options =>
                {
                    options.ConnectionString = _appConfiguration["Abp:RedisCache:ConnectionString"];
                    options.DatabaseId = _appConfiguration.GetValue<int>("Abp:RedisCache:DatabaseId");
                });
            var cacheNames = new string[] {
                AppConsts.Cache_AuthKey2OpenID,
                AppConsts.Cache_OpenID2AuthKey,
                AppConsts.Cache_MpUserMemberKey,
                AppConsts.Cache_JsApiTicket,
                AppConsts.Cache_AccessToken,
                AppConsts.Cache_MpAccount,
                AppConsts.Cache_MpAccountToken,
                AppConsts.Cache_CurrentUserMp,
                AppConsts.Cache_AesKeyModal,
                AppConsts.Cache_MCAuthKey,
                AppConsts.Cache_MpFansByOpenId,
                AppConsts.Cache_MpFansByUserId,
                AppConsts.Cache_CallBack,
                AppConsts.Cache_Service,
                AppConsts.Cache_PreviewMartial,
                AppConsts.Cache_ChunyuConfig,
                AppConsts.Cache_ChunyuLogin,
                AppConsts.Cache_ChunyuProblemByOpenId,
                AppConsts.Cache_ChunyuProblemByCYId,
                AppConsts.Cache_ChunyuProblemFirstContent,
                AppConsts.Cache_ChunyuDoctor,
                AppConsts.Cache_ChunyuProblemPreCloseLock,
                AppConsts.Cache_ChunyuProblemCloseLock,
                AppConsts.Cache_ActivityInfo,
                AppConsts.Cache_FirstKeyWordReply,
                AppConsts.Cache_OpenId2Customer,
                AppConsts.Cache_FanOpenId2Conversation,
                AppConsts.Cache_SolicitudeDelayMinutes
            };
            foreach (var item in cacheNames)
            {
                Configuration.Caching.Configure(item, cache =>
                {
                    var expireType = _appConfiguration.GetValue<string>($"Abp:RedisCache:Expire:{item}:Type");
                    if (expireType == "Absolute")
                    {
                        cache.DefaultAbsoluteExpireTime = TimeSpan.FromMinutes(_appConfiguration.GetValue<int>($"Abp:RedisCache:Expire:{item}:Length"));
                    }
                    else
                    {
                        cache.DefaultSlidingExpireTime = TimeSpan.FromMinutes(_appConfiguration.GetValue<int>($"Abp:RedisCache:Expire:{item}:Length"));
                    }
                });
            }
        }

        private void ConfigureTokenAuth()
        {
            IocManager.Register<TokenAuthConfiguration>();
            var tokenAuthConfig = IocManager.Resolve<TokenAuthConfiguration>();

            tokenAuthConfig.SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appConfiguration["Authentication:JwtBearer:SecurityKey"]));
            tokenAuthConfig.Issuer = _appConfiguration["Authentication:JwtBearer:Issuer"];
            tokenAuthConfig.Audience = _appConfiguration["Authentication:JwtBearer:Audience"];
            tokenAuthConfig.SigningCredentials = new SigningCredentials(tokenAuthConfig.SecurityKey, SecurityAlgorithms.HmacSha256);
            tokenAuthConfig.Expiration = TimeSpan.FromDays(1);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AbpZeroTemplateWebCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            SetAppFolders();
        }

        private void SetAppFolders()
        {
            var appFolders = IocManager.Resolve<AppFolders>();

            appFolders.SampleProfileImagesFolder = Path.Combine(_env.WebRootPath, @"Common/Images/SampleProfilePics");
            appFolders.TempFileDownloadFolder = Path.Combine(_env.WebRootPath, @"Temp/Downloads");
            appFolders.WebLogsFolder = Path.Combine(_env.ContentRootPath, @"App_Data/Logs");

            appFolders.QrCodeDownloadFolder= Path.Combine(_env.WebRootPath, @"QrCode/Downloads");

#if NET461
            if (_env.IsDevelopment())
            {
                var currentAssemblyDirectoryPath = typeof(AbpZeroTemplateWebCoreModule).GetAssembly().GetDirectoryPathOrNull();
                if (currentAssemblyDirectoryPath != null)
                {
                    appFolders.WebLogsFolder = Path.Combine(currentAssemblyDirectoryPath, @"App_Data\Logs");
                }
            }
#endif

            try
            {
                DirectoryHelper.CreateIfNotExists(appFolders.TempFileDownloadFolder);
                DirectoryHelper.CreateIfNotExists(appFolders.QrCodeDownloadFolder);
            }
            catch { }
        }
    }
}
