namespace UserService.HttpRequestHandling
{
    public interface IUserContextService
    {
        UserContext? GetAuthenticatedUserContext();
    }
}