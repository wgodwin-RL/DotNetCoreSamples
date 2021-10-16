﻿using System;
using System.Threading.Tasks;
using LD.BootstrapSetup;
using LD.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LD.MessageConsumerTests
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<ConsumerAppServiceHost>();
            })
            .RunConsoleAsync();
        }
    }
}
