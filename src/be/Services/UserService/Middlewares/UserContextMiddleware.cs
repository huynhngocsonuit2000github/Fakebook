using System.Security.Claims;
using UserService.HttpRequestHandling;

namespace UserService.Middlewares
{
    public class UserContextMiddleware
    {
        private readonly RequestDelegate _next;

        public UserContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Check if the user is authenticated
            if (context!.User!.Identity!.IsAuthenticated)
            {
                // Retrieve user information
                var userId = context.User.FindFirst("UserId")?.Value; 

                var userContext = new UserContext()
                {
                    UserId = userId!,
                };

                // Add the user context to the HttpContext Items
                context.Items["UserContext"] = userContext;
            }

            await _next(context);
        }
    }
}