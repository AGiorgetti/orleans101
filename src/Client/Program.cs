﻿using GrainInterfaces;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    public static class Program
    {
        private static int Main(string[] args)
        {
            return RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            try
            {
                using (var client = await ConnectClient().ConfigureAwait(false))
                {
                    await DoClientWork(client).ConfigureAwait(false);
                    Console.ReadKey();
                }

                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"\nException while trying to run client: {e.Message}");
                Console.WriteLine("Make sure the silo the client is trying to connect to is running.");
                Console.WriteLine("\nPress any key to exit.");
                Console.ReadKey();
                return 1;
            }
        }

        private static async Task<IClusterClient> ConnectClient()
        {
            IClusterClient client = new ClientBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "OrleansBasics";
                })
                .ConfigureLogging(logging => logging.AddConsole())
                .Build();

            await client.Connect(ConnectionRetryFilter).ConfigureAwait(false);
            Console.WriteLine("Client successfully connected to silo host \n");
            return client;
        }

        private static Task<bool> ConnectionRetryFilter(Exception arg)
        {
            // infinite attempts, just wait for the server to come up.
            Thread.Sleep(1000);
            return Task.FromResult(true);
        }

        private static async Task DoClientWork(IClusterClient client)
        {
            // example of calling grains from the initialized client
            var friend = client.GetGrain<IHello>("Hello1");
            var response = await friend.SayHello("Good morning, HelloGrain!").ConfigureAwait(false);
            Console.WriteLine("\n\n{0}\n\n", response);
        }
    }
}
