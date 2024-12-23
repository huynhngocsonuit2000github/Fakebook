using System.Text.Json;
using Fakebook.AuthService.Helpers;
using Fakebook.AuthService.Services;
using Fakebook.SynchronousModel.Models.IdPService.Users;

namespace Fakebook.AuthService.SynchronousApi
{
    public interface IIdPSynchronousApiService
    {
        Task<UserDetailModel?> GetUserDetailByEmailAsync(string email);
    }
    public class IdPSynchronousApiService : IIdPSynchronousApiService
    {
        private readonly IHttpClientProvider _httpClientProvider;

        public IdPSynchronousApiService(IHttpClientProvider httpClientProvider)
        {
            _httpClientProvider = httpClientProvider;
        }

        /// <summary>
        /// TODO: Implement the extension for the common code
        /// public static class HttpClientExtensions
        // {
        //     public static async Task<T> GetAsync<T>(this HttpClient client, string requestUri)
        //         {
        //             var response = await client.GetAsync(requestUri);
        //             response.EnsureSuccessStatusCode();

        //             var responseBody = await response.Content.ReadAsStringAsync();
        //             return JsonSerializer.Deserialize<T>(responseBody, new JsonSerializerOptions
        //             {
        //                 PropertyNameCaseInsensitive = true
        //             });
        //         }
        // }
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<UserDetailModel?> GetUserDetailByEmailAsync(string email)
        {
            var endpoint = $"internalAuth/get-user-detail-by-email?email={email}";
            var client = _httpClientProvider.GetHttpClientByServiceName(SynchronousApiConstants.InternalServiceUrl.IdP);

            try
            {
                var response = await client.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var userDetail = JsonSerializer.Deserialize<UserDetailModel>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return userDetail;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user details: {ex.Message}");
                throw;
            }
        }
    }
}