using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QueueLib
{
    public class QueueWorkerHostedService<T> : IHostedService where T: QueueHandler
                                                    
    {
        private readonly string connStr;
        private readonly string queueName;
        private readonly IServiceProvider serviceProvider;
        private MessageHandlerOptions handlerOptions;
        private QueueClient queueClient;
        private T queueHandler;

        public QueueWorkerHostedService(IServiceProvider provider, 
                                        string connStr, 
                                        string queueName,
                                        MessageHandlerOptions options)
        {
            this.connStr = connStr;
            this.queueName = queueName;
            this.serviceProvider = provider;
            this.handlerOptions = options;

            queueClient = new QueueClient(connStr, queueName);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            queueHandler = ActivatorUtilities.CreateInstance<T>(serviceProvider);

            if (handlerOptions == null)
            {
                handlerOptions = new MessageHandlerOptions(queueHandler.HandleError);
            }

            queueHandler.QueueClient = queueClient;
            queueHandler.Logger = serviceProvider.GetRequiredService<ILogger<T>>();

            queueClient.RegisterMessageHandler(queueHandler.HandleMessage, handlerOptions);

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await queueClient.CloseAsync();
        }
    }
}
