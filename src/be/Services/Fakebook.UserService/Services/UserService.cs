using Fakebook.DataAccessLayer.Interfaces;
using Fakebook.UserService.Dtos.Users;
using Fakebook.UserService.Entity;
using Fakebook.UserService.Repositories;

namespace Fakebook.UserService.Services
{
    public class UserUservice : IUserUservice
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserUservice(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _userRepository.FindFirstAsync(u => u.Username == username);
        }

        public async Task UpdateAsync(UpdateUserRequest userRequest)
        {
            var existingUser = await _userRepository.GetByIdAsync(userRequest.UserId);
            if (existingUser is null) throw new Exception("The user is not exists");

            var authenticatedUser = await _userRepository.GetAuthenticatedUserAsync();
            if (authenticatedUser is null) throw new Exception("Authenticated user is invalid");

            existingUser.Firstname = userRequest.Firstname ?? existingUser.Firstname;
            existingUser.Lastname = userRequest.Lastname ?? existingUser.Lastname;
            existingUser.LastModifiedBy = authenticatedUser.Id;
            existingUser.LastModifiedDate = DateTime.Now;

            await _unitOfWork.CompleteAsync();
        }
    }
}
