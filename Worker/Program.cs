using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using QueueLib;

namespace Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) => {
                    //K8s TCP HealthCheck integration (No HTTP stack in the app).
                    services.AddHealthChecks()
                            .AddSocketListener(8080);

                    //Add Worker
                    services.AddAzureServiceBus(context.Configuration["QueueConnectionString"])
                            .BindHandler<Worker>("testqueue");
                });
    }
}
