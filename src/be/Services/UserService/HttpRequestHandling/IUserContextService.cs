namespace Fakebook.UserService.HttpRequestHandling
{
    public interface IUserContextService
    {
        UserContext? GetAuthenticatedUserContext();
    }
}