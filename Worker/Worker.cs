using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using QueueLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Worker
{

    public class Worker : QueueHandler
    {
        private ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override async Task HandleMessage(Message message,
                                                 CancellationToken cancellationToken)
        {
            //Write your business logic here.
            _logger.LogInformation($"Message Recieved: {Encoding.UTF8.GetString(message.Body)}");
            //Simulate some work.
            await Task.Delay(1000);
        }
    }
}
