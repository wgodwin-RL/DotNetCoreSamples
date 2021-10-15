using LD_Models.Interfaces;
using LD_Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LDAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    
                }).ConfigureServices(services => {
                    services.AddHostedService<ConsumerAppServiceHost>();
                    services.AddTransient<IConsumerAppService, ConsumerAppService>();
                });

    }
}