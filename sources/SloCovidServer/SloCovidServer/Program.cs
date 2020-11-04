using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using SloCovidServer.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace SloCovidServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var communicator = host.Services.GetService<ICommunicator>();
            var cts = new CancellationTokenSource();
            var refresher = communicator.StartCacheRefresherAsync(cts.Token);
            host.Run();
            cts.Cancel();
            // should wait but refreshed really doesn't do anything worth waiting for
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                 .ConfigureAppConfiguration((hostingContext, config) =>
                 {
                     config.AddEnvironmentVariables(prefix: "SloCovidServer_");
                 })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://0.0.0.0:5000");
                });
    }
}
