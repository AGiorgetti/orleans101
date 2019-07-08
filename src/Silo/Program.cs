using Grains;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Silo
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            return RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            try
            {
                var host = await StartSilo().ConfigureAwait(false);
                Console.WriteLine("\n\n Press Enter to terminate...\n\n");
                Console.ReadLine();

                await host.StopAsync().ConfigureAwait(false);

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
        }

        private static async Task<ISiloHost> StartSilo()
        {
            // define the cluster configuration
            var builder = new SiloHostBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "OrleansBasics";
                })
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(HelloGrain).Assembly).WithReferences())
                .ConfigureLogging(logging => logging.AddConsole())
                .UseDashboard();

            var host = builder.Build();
            await host.StartAsync().ConfigureAwait(false);
            return host;
        }
    }
}
