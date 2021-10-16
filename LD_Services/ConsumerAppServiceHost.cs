using LD.Models.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LD.Services
{
    public class ConsumerAppServiceHost : BackgroundService
    {
        private readonly ILogger<ConsumerAppServiceHost> _logger;

        public ConsumerAppServiceHost(IServiceProvider services, ILogger<ConsumerAppServiceHost> logger)
        {
            Services = services;
            _logger = logger;
        }

        public IServiceProvider Services { get; }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("consume scoped service hosted service running.");
            await Run(stoppingToken);
        }

        private async Task Run(CancellationToken stoppingToken)
        {
            _logger.LogInformation("consume");
            using (var scope = Services.CreateScope()) 
            {
                var scopedService = scope.ServiceProvider.GetRequiredService<IConsumerAppService>();
                await scopedService.Run(stoppingToken);
            }
        }
        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }
}
