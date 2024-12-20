using Fakebook.IdPService.Models.Users;

namespace Fakebook.IdPService.Services;
public interface IUserUservice
{
    Task<LoginUserModel> LoginAsync(string username, string password);
}