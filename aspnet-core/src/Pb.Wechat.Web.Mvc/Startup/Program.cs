using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Pb.Wechat.Web.Startup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string contentRoot = Directory.GetCurrentDirectory();
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .BindUrls(args, contentRoot)
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
