namespace Fakebook.AuthService.HttpRequestHandling
{
    public interface IUserContextService
    {
        UserContext? GetAuthenticatedUserContext();
    }
}