namespace Fakebook.AuthService.HttpRequestHandling
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public UserContext? GetAuthenticatedUserContext()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context != null && context.Items.ContainsKey("UserContext"))
            {
                return context.Items["UserContext"] as UserContext;
            }

            return null;
        }
    }
}