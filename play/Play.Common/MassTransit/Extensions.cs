using GreenPipes;
using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Play.Common.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Play.Common.MassTransit
{
    public static class Extensions
    {
        public static IServiceCollection 
            AddMassTransitWithRabbitMQ(this IServiceCollection services)
        {
            services.AddMassTransit(configare =>
            {
                configare.AddConsumers(Assembly.GetEntryAssembly());
                configare.UsingRabbitMq((context, configurator) =>
                {
                    var _config = context.GetService<IConfiguration>();
                    var serviceSettings = _config.GetSection(nameof(ServiceSettings))
                                                 .Get<ServiceSettings>();
                    var rabbitMQSettings = _config.GetSection(nameof(RabbitMQSettings))
                                                   .Get<RabbitMQSettings>();
                    configurator.Host(rabbitMQSettings.Host);
                    configurator.ConfigureEndpoints(context,
                        new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName, false));

                    configurator.UseMessageRetry(retryConfig =>
                    {
                        retryConfig.Interval(3, TimeSpan.FromSeconds(5));
                    });
                });
            });

            services.AddMassTransitHostedService();
            return services;
        }
    }
}
