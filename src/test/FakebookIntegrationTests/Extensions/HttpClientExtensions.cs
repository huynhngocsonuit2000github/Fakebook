using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace FakebookIntegrationTests.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> PatchAsJsonAsync<T>(
            this HttpClient httpClient,
            string requestUri,
            T value)
        {
            var json = JsonSerializer.Serialize(value);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri)
            {
                Content = content
            };

            return await httpClient.SendAsync(request);
        }
    }
}