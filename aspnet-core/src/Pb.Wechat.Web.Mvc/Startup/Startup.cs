using System;
using Abp.AspNetCore;
using Abp.Castle.Logging.Log4Net;
using Abp.IdentityServer4;
using Castle.Facilities.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pb.Wechat.Configuration;
using Pb.Wechat.Identity;
using Pb.Wechat.Web.Authentication.JwtBearer;
using Pb.Wechat.Web.Resources;
using PaulMiami.AspNetCore.Mvc.Recaptcha;
using Pb.Wechat.Web.IdentityServer;
using System.Linq;
using Abp.Extensions;
using Hangfire;
using Pb.Hangfire;
using Abp.Hangfire;
using Pb.Wechat.Authorization;
using Microsoft.Extensions.Options;

using System.IO;
using UEditorNetCore;

#if FEATURE_SIGNALR
using Owin;
using Abp.Owin;
using Pb.Wechat.Web.Owin;
#endif

namespace Pb.Wechat.Web.Startup
{

    public class Startup
    {
        private readonly IConfigurationRoot _appConfiguration;
        private const string DefaultCorsPolicyName = "localhost";
        private readonly IHostingEnvironment _env;

        public Startup(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //services.AddSignalR();
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("SignalrCore",
            //        policy => policy.AllowAnyOrigin()
            //                        .AllowAnyHeader()
            //                        .AllowAnyMethod());
            //});
            //services.AddSingleton<IServiceProvider, ServiceProvider>();
            var ueitorPath = Path.Combine(_env.WebRootPath, _appConfiguration["App:UEditorJsonPath"]);
            services.AddUEditorService(ueitorPath);
            //MVC
            services.AddMvc(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            var identityBuilder = IdentityRegistrar.Register(services);

            //Identity server
            if (bool.Parse(_appConfiguration["IdentityServer:IsEnabled"]))
            {
                identityBuilder.AddAbpIdentityServer();
                IdentityServerRegistrar.Register(services, _appConfiguration);
            }

            AuthConfigurer.Configure(services, _appConfiguration);

            //Swagger - Enable this line and the related lines in Configure method to enable swagger UI
            //services.AddSwaggerGen(options =>
            //{
            //    options.SwaggerDoc("v1", new Info { Title = "AbpZeroTemplate API", Version = "v1" });
            //    options.DocInclusionPredicate((docName, description) => true);
            //});

            //Recaptcha
            services.AddRecaptcha(new RecaptchaOptions
            {
                SiteKey = _appConfiguration["Recaptcha:SiteKey"],
                SecretKey = _appConfiguration["Recaptcha:SecretKey"]
            });

            IConfiguration configuration = _appConfiguration.GetSection("HangfireConfig");
            IOptions<HangfireConfig> hangfireConfig = new ServiceCollection()
              .AddOptions()
              .Configure<HangfireConfig>(configuration)
              .BuildServiceProvider()
              .GetService<IOptions<HangfireConfig>>();


            JobConfig.InitConfig(hangfireConfig.Value);

            ////Hangfire(Enable to use Hangfire instead of default job manager)
            services.AddHangfire(config =>
            {
                config.UseSqlServerStorage(JobConfig.HangfireConnectionString);
            });

            services.AddScoped<IWebResourceManager, WebResourceManager>();

            services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicyName, builder =>
                {
                    //App:CorsOrigins in appsettings.json can contain more than one address with splitted by comma.
                    builder
                        .WithOrigins(_appConfiguration["App:CorsOrigins"].Split(",", StringSplitOptions.RemoveEmptyEntries).Select(o => o.RemovePostFix("/")).ToArray())
                        .AllowAnyOrigin() //TODO: Will be replaced by above when Microsoft releases microsoft.aspnetcore.cors 2.0 - https://github.com/aspnet/CORS/pull/94
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            AppConsts.MvcAppPath = _appConfiguration["App:ApplicationPath"];
            //Configure Abp and Dependency Injection
            return services.AddAbp<AbpZeroTemplateWebMvcModule>(options =>
            {
                //Configure Log4Net logging
                options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig("log4net.config")
                );
            });

            

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //app.UseCors("SignalrCore");
            //app.UseSignalR(routes =>
            //{
            //    routes.MapHub<LoginHub>("/loginHub");
            //});
            app.UseCors(DefaultCorsPolicyName); //Enable CORS!
            //Initializes ABP framework.
            app.UseAbp(options =>
            {
                options.UseAbpRequestLocalization = false; //used below: UseAbpRequestLocalization
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithRedirects("~/Error?statusCode={0}");
                app.UseExceptionHandler("/Error");
            }

            app.UseAuthentication();
            app.UseJwtTokenMiddleware();

            if (bool.Parse(_appConfiguration["IdentityServer:IsEnabled"]))
            {
                app.UseIdentityServer();

                /* We can not use app.UseIdentityServerAuthentication because IdentityServer4.AccessTokenValidation
                 * is not ported to asp.net core 2.0 yet. See issue: https://github.com/IdentityServer/IdentityServer4/issues/1055
                 * Once it's ported, add IdentityServer4.AccessTokenValidation to Web.Core project and enable following lines:
                 */

                //app.UseIdentityServerAuthentication(
                //    new IdentityServerAuthenticationOptions
                //    {
                //        Authority = _appConfiguration["App:WebSiteRootAddress"],
                //        RequireHttpsMetadata = false
                //    }
                //);
            }

            app.UseStaticFiles();
            app.UseAbpRequestLocalization();

#if FEATURE_SIGNALR
            //Integrate to OWIN
            app.UseAppBuilder(ConfigureOwinServices);
#endif


            //Hangfire dashboard & server (Enable to use Hangfire instead of default job manager)
            app.UseHangfireDashboard(JobConfig.HangfireDashboard, new DashboardOptions
            {
                Authorization = new[] { new AbpHangfireAuthorizationFilter(AppPermissions.Pages_Administration_HangfireDashboard) }
            });
            
            if (bool.Parse(_appConfiguration["HangfireConfig:UseHangfire"]))
            {
                app.UseHangfireServer();
            }

            JobConfig.LoadJobs();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "defaultWithArea",
                    template: "{area}/{controller=MpMediaImages}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint
            //app.UseSwagger();
            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            //app.UseSwaggerUI(options =>
            //{
            //    options.SwaggerEndpoint("/swagger/v1/swagger.json", "AbpZeroTemplate API V1");
            //}); //URL: /swagger
        }

#if FEATURE_SIGNALR
        private static void ConfigureOwinServices(IAppBuilder app)
        {
            app.Properties["host.AppName"] = "AbpZeroTemplate";

            app.UseAbp();

            app.MapSignalR();

            //Enable it to use HangFire dashboard (uncomment only if it's enabled in AbpZeroTemplateWebCoreModule)
            //app.UseHangfireDashboard("/hangfire", new DashboardOptions
            //{
            //    Authorization = new[] { new AbpHangfireAuthorizationFilter(AppPermissions.Pages_Administration_HangfireDashboard) }
            //});
        }
#endif
    }
}
