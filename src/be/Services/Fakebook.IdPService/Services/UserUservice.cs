using Fakebook.DataAccessLayer.Interfaces;
using Fakebook.IdPService.Models;
using Fakebook.IdPService.Models.Users;
using Fakebook.IdPService.Repositories;

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
}