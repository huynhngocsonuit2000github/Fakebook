using Fakebook.MessageQueueHandler.Models.Core;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Fakebook.MessageQueueHandler.Publisher
{
    public interface IRabbitMQPublisher
    {
        Task PublishMessageAsync<T>(string routingKey, T input);
    }

    public class RabbitMQPublisher : IRabbitMQPublisher
    {
        private readonly ConnectionFactory _factory;
        private IConnection connection = null!;

        public RabbitMQPublisher(IConfiguration configuration)
        {
            _factory = new ConnectionFactory()
            {
                HostName = configuration.GetValue<string>("RabbitMQ:Hostname"),
                UserName = configuration.GetValue<string>("RabbitMQ:Username"),
                Password = configuration.GetValue<string>("RabbitMQ:Password"),
                Port = configuration.GetValue<int>("RabbitMQ:Port")
            };
        }

        /// <summary>
        /// Base on routingKey, Identify the ServiceQueue, multiple routingKey can be in the same ServiceQueue
        /// RoutingKey is used to switch case within service
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="routingKey"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task PublishMessageAsync<T>(string routingKey, T input)
        {
            if (connection is null)
            {
                connection = await _factory.CreateConnectionAsync(); // Create connection
            }

            using var channel = await connection.CreateChannelAsync();   // Create channel

            var message = new Message<T>(input, routingKey);

            // Create the queue if it does not exists
            await channel.QueueDeclareAsync(
                queue: message.ServiceQueue, // name of queue
                durable: true, // the queue survives when server restarts
                exclusive: false, // can be accessed by multiple connections
                autoDelete: false, // the queue is not deleted when the last consumer
                arguments: null);

            var messageSerialize = JsonSerializer.Serialize(message);        // Serialize the message
            var body = Encoding.UTF8.GetBytes(messageSerialize);            // Convert to byte array

            await channel.BasicPublishAsync(
                exchange: string.Empty, // empty means that using default exchange
                routingKey: message.ServiceQueue, // determines the queue to route the message
                body: body,
                mandatory: true); // the message will be returned to the publisher if the routing fails

            Console.WriteLine($"Message Published: Message={messageSerialize}");
        }
    }
}
