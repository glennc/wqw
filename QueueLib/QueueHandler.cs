using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QueueLib
{
    public abstract class QueueHandler
    {
        public IQueueClient QueueClient { get; set; }
        public ILogger Logger { get; set; }

        public abstract Task HandleMessage(Message message, CancellationToken cancellationToken);

        public virtual Task HandleError(ExceptionReceivedEventArgs args)
        {
            Logger.LogError("Exception when handling message", args);
            return Task.CompletedTask;
        }
    }
}
