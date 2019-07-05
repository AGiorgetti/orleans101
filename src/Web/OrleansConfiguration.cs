using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using System;

namespace Web
{
    public static class OrleansConfiguration
    {
        public static IServiceCollection AddOrleans(this IServiceCollection services)
        {
            services.AddSingleton<IClusterClient>(CreateClusterClient);

            return services;
        }

        private static IClusterClient CreateClusterClient(IServiceProvider arg)
        {
            IClusterClient client;
            client = new ClientBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "OrleansBasics";
                })
                .ConfigureLogging(logging => logging.AddConsole())
                .Build();

            client.Connect().Wait();
            return client;
        }
    }
}
