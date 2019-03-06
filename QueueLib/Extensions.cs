using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QueueLib
{
    public static class DIExtensions
    {
        public static QueueBuilder AddAzureServiceBus(this IServiceCollection services,
                                                            string connStr)
        {
            return new QueueBuilder(services, connStr);
        }

    }

    public class QueueBuilder
    {
        private IServiceCollection services;
        private readonly string connStr;

        public QueueBuilder(IServiceCollection services, string connStr)
        {
            this.services = services;
            this.connStr = connStr;
        }

        public void BindHandler<T>(string queueName) where T : QueueHandler
        {
            BindHandler<T>(queueName, null);
        }

        public void BindHandler<T>(string queueName, MessageHandlerOptions options) where T : QueueHandler
        {
            services.AddSingleton<IHostedService>(sp => new QueueWorkerHostedService<T>(sp, connStr, queueName, options));

        }
    }
}
