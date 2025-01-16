namespace Fakebook.DataAccessLayer.HttpRequestHandling
{
    public interface IUserContextService
    {
        UserContext? GetAuthenticatedUserContext();
    }
}