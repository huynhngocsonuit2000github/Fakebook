docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 --network docker_network rabbitmq:management
guest
guest

5672: For RabbitMQ messaging.
15672: For the RabbitMQ Management UI.

======
dotnet add package RabbitMQ.Client
dotnet add package Microsoft.Extensions.Hosting

-- publisher
builder.Services.AddScoped<IRabbitMQPublisher, RabbitMQPublisher>();
await \_rabbitMQPublisher.PublishMessageAsync(RoutingQueueConstants.UserService.UserService_IPD_Login_Create_User, user);

-- consumer

// Add a hosted service to run the consumer
builder.Services.AddHostedService<RabbitMQConsumerHostedService>(e => new(nameof(RoutingQueueConstants.UserService)));

sudo date --set="Mon Dec 30 00:01:37 +07 2024"
