using RabbitMQ.Client.Events;
using Fakebook.MessageQueueHandler.Models.Core;

namespace Fakebook.MessageQueueHandler.Consumer;

public interface IConsumer<T>
{
    Task HandlerAsync(object sender, BasicDeliverEventArgs e, Message<T> message, IServiceProvider serviceProvider);
}