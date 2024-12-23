
namespace Fakebook.AuthService.Helpers;

public interface IHttpClientProvider
{
    HttpClient GetHttpClientByServiceName(string serviceName);
}

public class HttpClientProvider : IHttpClientProvider
{
    // TODO: implement the core to dictionary, factory HttpClient by enum or constant string
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _clientFactory;

    public HttpClientProvider(IConfiguration configuration, IHttpClientFactory clientFactory)
    {
        _configuration = configuration;
        _clientFactory = clientFactory;
    }

    public HttpClient GetHttpClientByServiceName(string serviceName)
    {
        var client = _clientFactory.CreateClient();
        client.BaseAddress = new Uri(_configuration[$"InternalApis:{serviceName}:Url"]);

        return client;
    }
}
