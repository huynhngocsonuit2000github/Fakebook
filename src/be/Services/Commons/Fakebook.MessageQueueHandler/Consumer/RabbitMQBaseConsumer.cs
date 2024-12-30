using Fakebook.MessageQueueHandler.Models.Core;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;
using System.Text.Json;

namespace Fakebook.MessageQueueHandler.Consumer
{
    public class RabbitMQBaseConsumer
    {
        private readonly ConnectionFactory _factory;
        private readonly string _queueName;
        private readonly IServiceProvider _serviceProvider;
        private static IConnection connection = null!;
        private static IChannel channel = null!;

        public RabbitMQBaseConsumer(ConnectionSettings _connectionSettings, IServiceProvider serviceProvider)
        {
            System.Console.WriteLine("_connectionSettings.Hostname: " + _connectionSettings.Hostname);
            System.Console.WriteLine("_connectionSettings.Username: " + _connectionSettings.Username);
            System.Console.WriteLine("_connectionSettings.Password: " + _connectionSettings.Password);
            System.Console.WriteLine("_connectionSettings.Port: " + _connectionSettings.Port);
            System.Console.WriteLine("_connectionSettings.QueueName: " + _connectionSettings.QueueName);
            _factory = new ConnectionFactory
            {
                HostName = _connectionSettings.Hostname, // Use "localhost" if not using Docker
                UserName = _connectionSettings.Username,
                Password = _connectionSettings.Password,
                Port = _connectionSettings.Port,
                NetworkRecoveryInterval = TimeSpan.FromMinutes(1),
            };
            _queueName = _connectionSettings.QueueName;
            _serviceProvider = serviceProvider;
        }

        public async Task StartConsumerAsync()
        {
            connection = connection ?? await _factory.CreateConnectionAsync();
            channel = await connection.CreateChannelAsync(); // Use CreateModelAsync to declare queue and channel at once

            // Declare the queue
            await channel.QueueDeclareAsync(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += OnMessageReceived;

            await channel.BasicConsumeAsync(queue: _queueName, autoAck: true, consumer: consumer);
        }

        public async Task OnMessageReceived(object sender, BasicDeliverEventArgs e)
        {
            var routingKey = GetRoutingKey(e);
            var type = GetHandler(routingKey);
            var instance = Activator.CreateInstance(type);
            var handlerMethod = type.GetMethod("HandlerAsync");
            var message = GetData(e, type);

            await (Task)handlerMethod!.Invoke(instance, new object[] { sender, e, message, _serviceProvider })!;
        }

        private object GetData(BasicDeliverEventArgs e, Type type)
        {
            // Get the generic argument type of IConsumer<T>
            var interfaceType = type.GetInterface(typeof(IConsumer<>).Name);
            if (interfaceType == null)
                throw new InvalidOperationException($"The type {type.FullName} does not implement IConsumer<T>.");

            var genericArgument = interfaceType.GetGenericArguments()[0];

            // Deserialize the message JSON into the correct generic type
            var messageType = typeof(Message<>).MakeGenericType(genericArgument);
            var messageJson = Encoding.UTF8.GetString(e.Body.ToArray());
            var message = JsonConvert.DeserializeObject(messageJson, messageType, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            });

            return message!;
        }

        private string GetRoutingKey(BasicDeliverEventArgs e)
        {
            var messageStr = Encoding.UTF8.GetString(e.Body.ToArray());
            using var document = JsonDocument.Parse(messageStr);
            if (document.RootElement.TryGetProperty("Type", out JsonElement typeElement))
            {
                return typeElement!.GetString()!;
            }

            throw new Exception("The routing key is invalid!");
        }

        private Type GetHandler(string routingKey)
        {
            // Get all loaded assemblies
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            // Find all types that implement the interface
            var consumerTypes = assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type =>
                    type.IsClass &&
                    !type.IsAbstract &&
                    type.GetInterfaces().Any(i =>
                        i.IsGenericType &&
                        i.GetGenericTypeDefinition() == typeof(IConsumer<>)));

            System.Console.WriteLine("Count: " + consumerTypes.Count());
            foreach (var type in consumerTypes)
            {
                var keyProperty = type.GetProperty("RoutingKey")
                    ?? throw new Exception("The implementation from interface IConsumer should specify the property RoutingKey");
                var keyValue = keyProperty!.GetValue(null)!.ToString();

                System.Console.WriteLine("value " + keyValue);
                if (keyValue == routingKey) return type;
            }

            throw new Exception("Something was wrong while implementing Consumer");
        }
    }
}
