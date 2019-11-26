using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Pb.Wechat.Web.FileStore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            AppConfig.FileDir = Configuration["App:FileDir"];
            AppConfig.BufferLength = long.Parse(Configuration["App:BufferLength"]);
            AppConfig.AppRoot = Path.Combine(env.WebRootPath, AppConfig.FileDir);

            AppConfig.mapDirPath = Configuration["App:MatialFile:MapDirPath"];
            AppConfig.mapDirUserName = Configuration["App:MatialFile:MapDirUserName"];
            AppConfig.mapDirPwd = Configuration["App:MatialFile:MapDirPwd"];
            AppConfig.mapDirRemotingName = Configuration["App:MatialFile:MapDirRemotingName"];
        }
    }
}
