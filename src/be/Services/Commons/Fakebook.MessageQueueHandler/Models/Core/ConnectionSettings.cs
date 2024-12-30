namespace Fakebook.MessageQueueHandler.Models.Core;

public class ConnectionSettings
{
    public string Hostname { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int Port { get; set; }
    public string QueueName { get; set; } = null!;
}