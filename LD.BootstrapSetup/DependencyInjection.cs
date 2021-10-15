using LD.Data;
using LD_Common;
using LD_Models.Configuration;
using LD_Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LD_BootstrapSetup
{
    public class DependencyInjection
    {
        public static IConfiguration GetConfiguration(string appSettingsPrefix = "")
        {
            var environmentName = "local";

            if (string.IsNullOrWhiteSpace(environmentName))
                throw new Exception("Could not resolve environment");

            // build config
            var configuration = new ConfigurationBuilder()
                //.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"{appSettingsPrefix}appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"{appSettingsPrefix}appsettings.{environmentName}.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();


            return configuration;
        }

        public IServiceCollection InitEnvironmentServiceCollection(IServiceCollection serviceCollection, string appSettingsPrefix = "")
        {
            return DependencyInjection.InitServiceCollection(serviceCollection, appSettingsPrefix);
        }

        public static IServiceCollection InitServiceCollection(IServiceCollection serviceCollection, string appSettingsPrefix = "")
        {
            if (serviceCollection == null)
                serviceCollection = new ServiceCollection();

            var config = GetConfiguration(appSettingsPrefix);
            ConfigureSettings<AppSettings>("app", config, serviceCollection);

            serviceCollection.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            });

            serviceCollection.AddEntityFrameworkSqlite()
                .AddDbContext<LD_Data.DatabaseContext>(x => x.UseInMemoryDatabase("LDDB"));
                //.AddScoped<AppSettings>();

            serviceCollection.AddScoped<IStudentData, StudentData>();
            serviceCollection.AddScoped<IExamData, ExamData>();
            serviceCollection.AddScoped<IEventMessageData, EventMessageData>();

            return serviceCollection;
        }

        public static T ConfigureSettings<T>(string configSectionName, IConfiguration configurationBuilder, IServiceCollection serviceCollection) where T : class
        {
            var config = configurationBuilder.GetSection(configSectionName);
            serviceCollection.Configure<T>(config);
            var settings = config.Get<T>();
            return settings;
        }
    }
}
