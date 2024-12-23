using Fakebook.IdPService.Models.Users;
using Fakebook.SynchronousModel.Models.IdPService.Users;

namespace Fakebook.IdPService.Services;
public interface IUserUservice
{
    Task<LoginUserModel> LoginAsync(string username, string password);
    Task<UserDetailModel?> GetUserDetailAsync(string email);
}