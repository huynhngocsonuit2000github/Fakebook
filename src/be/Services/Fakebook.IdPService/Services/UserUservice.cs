using Fakebook.DataAccessLayer.Interfaces;
using Fakebook.IdPService.Models;
using Fakebook.IdPService.Models.Users;
using Fakebook.IdPService.Repositories;
using Fakebook.SynchronousModel.Models.IdPService.Users;

namespace Fakebook.IdPService.Services;
public class UserUservice : IUserUservice
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UserUservice(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<LoginUserModel> LoginAsync(string username, string password)
    {
        return await _userRepository.GetUserByCredentialAsync(username, password);
    }

    public async Task<UserDetailModel?> GetUserDetailAsync(string email)
    {
        if (string.IsNullOrEmpty(email)) throw new Exception("The user email is invalid");

        var user = await _userRepository.FindFirstAsync(e => string.Equals(e.Email, email, StringComparison.OrdinalIgnoreCase));

        if (user is null) throw new Exception("The user email is invalid");

        if (!user.IsActive) throw new Exception("The user status is inactive");

        return new UserDetailModel()
        {
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            Username = user.Username,
            Email = user.Email,
        };
    }
}