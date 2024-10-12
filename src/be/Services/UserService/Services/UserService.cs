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

        public async Task<string> CreateUserAsync(User user)
        {
            using var unitOfWork = _unitOfWork;

            var duplicatedEmail = await _userRepository.FindFirstAsync(e => e.Email.ToLower() == user.Email.ToLower());
            if (duplicatedEmail is not null)
            {
                throw new Exception($"Duplicate email: {duplicatedEmail.Email}");
            }

            var duplicatedUserName = await _userRepository.FindFirstAsync(e => e.Username.ToLower() == user.Username.ToLower());
            if (duplicatedUserName is not null)
            {
                throw new Exception($"Duplicate username: {duplicatedUserName.Username}");
            }

            await _userRepository.AddAsync(user);
            await unitOfWork.CompleteAsync();

            return user.Id;
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
