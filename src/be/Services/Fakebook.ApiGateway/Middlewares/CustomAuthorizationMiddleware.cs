using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace Fakebook.ApiGateway.Middlewares
{
    public class CustomAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public CustomAuthorizationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!IsRequireAuthenticate(context))
            {
                await _next(context);

                return;
            }

            // Step 1: Get the Authorization token from the header
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Authorization token is missing.");
                return;
            }

            // Step 2: Validate the token and fetch permissions from the Auth service
            var permissions = await GetPermissionsFromAuthService(token);

            if (permissions == null || !permissions.Any())
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Permissions are missing or invalid.");
                return;
            }

            // Step 3: Get the required permission from Ocelot's configuration
            var requiredPermission = GetRequiredPermissionForRoute(context);

            if (requiredPermission is not null
                && requiredPermission.Any()
                && !requiredPermission.Any(e => permissions.Contains(e)))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync($"Require permission: {string.Join(',', requiredPermission)}");
                return;
            }

            // Step 4: Allow the request to proceed to the next middleware
            await _next(context);
        }

        /// <summary>
        /// Default is true
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private bool IsRequireAuthenticate(HttpContext context)
        {
            var route = GetSpecificSectionByUpstreamPathTemplate(context);

            if (route == null)
            {
                Console.Write("Not found");
                return true;
            }

            return route.GetValue<bool>("RequireAuthenticate");
        }

        private List<string> GetRequiredPermissionForRoute(HttpContext context)
        {
            var route = GetSpecificSectionByUpstreamPathTemplate(context);

            if (route == null)
            {
                Console.Write("Not found");
                return null!;
            }

            return route.GetSection("CustomRequiredPermissions").GetChildren().Select(e => e.Value).ToList();
        }

        private IConfigurationSection? GetSpecificSectionByUpstreamPathTemplate(HttpContext context)
        {
            return _configuration.GetSection("Routes")
                .GetChildren()
                .FirstOrDefault(r =>
                {
                    var upstreamPathTemplate = r.GetValue<string>("UpstreamPathTemplate");

                    // Normalize both paths to lowercase for case-insensitive matching
                    string normalizedRequestPath = context.Request.Path.Value!.ToLowerInvariant();
                    string normalizedUpstreamPathTemplate = upstreamPathTemplate.ToLowerInvariant();

                    // Convert the Ocelot route template to a regex pattern
                    var regexPattern = ConvertToRegexPattern(normalizedUpstreamPathTemplate);

                    // Match the normalized request path with the regex pattern
                    return Regex.IsMatch(normalizedRequestPath, regexPattern, RegexOptions.IgnoreCase);
                });
        }

        private string ConvertToRegexPattern(string ocelotRoute)
        {
            ocelotRoute = ocelotRoute.Replace("{everything}", @"[^/]+")
                                     .Replace("{id}", @"\d+")
                                     .Replace("{.*?}", @"[^/]+");

            return "^" + ocelotRoute + "$";
        }

        private async Task<List<string>> GetPermissionsFromAuthService(string token)
        {
            // Step 5: Call the Auth service to validate the token and retrieve permissions
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var authUrl = _configuration.GetValue<string>("GlobalConfiguration:AuthUrl");
            var response = await client.GetAsync($"{authUrl}/get-user-permission");

            if (!response.IsSuccessStatusCode)
            {
                return null!;
            }

            return (await response.Content.ReadFromJsonAsync<List<string>>())!;
        }
    }
}
