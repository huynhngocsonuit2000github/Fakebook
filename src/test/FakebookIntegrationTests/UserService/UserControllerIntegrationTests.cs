using System.Net.Http.Json;
using FluentAssertions;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Xunit;
using FakebookIntegrationTests.Extensions;
using FakebookIntegrationTests.Models.UserService.UserController;

namespace FakebookIntegrationTests.UserService
{
    public class UserControllerIntegrationTests
    {
        private readonly HttpClient _client;
        private string _jwtToken = null!;
        private string _userId = "c47b9e93-4b85-472a-acc3-dacf248fcc25";

        public UserControllerIntegrationTests()
        {
            // Load configuration from appsettings.json
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            // Get base URL from configuration
            var baseUrl = configuration["UserService:BaseUrl"] ?? throw new ArgumentNullException("Environment BaseURL");

            _client = new HttpClient { BaseAddress = new Uri(baseUrl) };
        }

        private async Task<string> GetJwtTokenAsync()
        {
            if (_jwtToken != null)
            {
                return _jwtToken;
            }

            var loginRequest = new
            {
                Username = "ustest1",
                Password = "pwtest1"
            };

            var response = await _client.PostAsJsonAsync("/user/login", loginRequest);
            response.EnsureSuccessStatusCode();

            _jwtToken = await response.Content.ReadAsStringAsync();

            return _jwtToken;
        }

        [Fact]
        public async Task RegisterUserAsync_ReturnsOkResponse_WhenAuthorized()
        {
            // Arrange
            var guid = Guid.NewGuid().ToString();
            var registrationRequest = new
            {
                Firstname = "Firstname" + guid,
                Lastname = "Lastname" + guid,
                Username = "Username" + guid,
                Password = "Password" + guid,
                Email = "Email" + guid + "@gmail.com",
            };

            // Act
            var response = await _client.PostAsJsonAsync("/user/register", registrationRequest);
            _userId = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            _userId.Should().NotBeEmpty();
            _userId.Length.Should().Be(36);
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsOkResponse_WhenAuthorized()
        {
            // Arrange
            var token = await GetJwtTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.GetAsync("/user");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetUserAsync_ReturnsNotFoundResponse_WhenAuthorized()
        {
            // Arrange
            var token = await GetJwtTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.GetAsync("/user/0");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetUserAsync_ReturnsOkResponse_WhenAuthorized()
        {
            // Arrange
            var token = await GetJwtTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.GetAsync($"/user/{_userId}");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNoContentResponse_WhenAuthorized()
        {
            // Arrange
            var token = await GetJwtTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var guid = Guid.NewGuid().ToString();
            var updateUserRequest = new
            {
                UserId = _userId,
                Firstname = "new firstname " + guid,
                Lastname = "new lastname " + guid,
            };

            // Act
            var response = await _client.PatchAsJsonAsync("/user/update", updateUserRequest);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            // Act
            var responseGetUser = await _client.GetAsync($"/user/{_userId}");
            responseGetUser.EnsureSuccessStatusCode();

            var userResponse = await responseGetUser.Content.ReadFromJsonAsync<GetUserModel>();
            userResponse.Should().NotBeNull();
            userResponse!.Firstname.Should().Be(updateUserRequest.Firstname);
            userResponse.Lastname.Should().Be(updateUserRequest.Lastname);
        }
    }
}
