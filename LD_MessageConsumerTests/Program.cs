using System;
using System.Threading.Tasks;
using LD_BootstrapSetup;
using LD_Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LD_MessageConsumerTests
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            await Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<ConsumerAppServiceHost>();
            })
            .RunConsoleAsync();
        }
    }
}
