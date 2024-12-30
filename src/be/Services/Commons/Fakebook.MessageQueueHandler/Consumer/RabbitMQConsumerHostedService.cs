using Fakebook.MessageQueueHandler.Models.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Fakebook.MessageQueueHandler.Consumer
{
    public class RabbitMQConsumerHostedService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConnectionSettings _connectionSettings;
        public RabbitMQConsumerHostedService(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _connectionSettings = configuration.GetSection("RabbitMQ").Get<ConnectionSettings>();

            var isQueueNameValid = typeof(Utils.RoutingQueueConstants)
                .GetNestedTypes()
                .Any(t => string.Equals(t.Name, _connectionSettings.QueueName));

            if (!isQueueNameValid)
            {
                throw new Exception("The configuration for ServiceQueue name is invalid");
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                System.Console.WriteLine("Start hosted service");

                await new RabbitMQBaseConsumer(_connectionSettings, _serviceProvider).StartConsumerAsync();

                System.Console.WriteLine("End hosted service");
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("There are some issues while hosting consumer");
                System.Console.WriteLine(ex.Message);
                System.Console.WriteLine(ex);
            }
        }
    }
}
