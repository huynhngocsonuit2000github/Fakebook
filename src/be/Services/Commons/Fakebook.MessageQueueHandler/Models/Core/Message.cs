namespace Fakebook.MessageQueueHandler.Models.Core;

public class Message<T>
{
    public string ServiceQueue { get; set; }
    public string Type { get; set; }
    public DateTime Timestamp { get; set; }
    public T Data { get; set; }
    public Message()
    {
    }
    public Message(T input, string routingKey)
    {
        var serviceName = GetServiceNameFromRoutingKey(routingKey);
        if (serviceName == null)
            throw new ArgumentException($"Invalid routing key: {routingKey}");

        ServiceQueue = serviceName;
        Type = routingKey;
        Timestamp = DateTime.Now;
        Data = input;
    }

    private string? GetServiceNameFromRoutingKey(string routingKey)
    {
        var serviceTypes = typeof(Utils.RoutingQueueConstants)
            .GetNestedTypes()
            .Select(t => new
            {
                ServiceName = t.Name,
                Keys = t.GetFields().Select(f => f.GetValue(null)?.ToString())
            });

        foreach (var serviceType in serviceTypes)
        {
            if (serviceType.Keys.Contains(routingKey))
            {
                return serviceType.ServiceName;
            }
        }

        return null;
    }
}