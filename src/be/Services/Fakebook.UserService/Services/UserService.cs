using Fakebook.DataAccessLayer.Interfaces;
using Fakebook.SynchronousModel.Models.IdPService.Users;
using Fakebook.UserService.Dtos.Users;
using Fakebook.UserService.Entity;
using Fakebook.UserService.Repositories;

namespace Fakebook.UserService.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork)
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

        public async Task SyncCreatedIdPUserAsync(IdPLoginCreateUserModel input)
        {
            var existingUser = await _userRepository.GetByIdAsync(input.Id);
            if (existingUser is not null) throw new Exception("The user exists");

            var user = new User()
            {
                Id = input.Id,
                CreatedBy = input.CreatedBy,
                LastModifiedBy = input.LastModifiedBy,
                CreatedDate = input.CreatedDate,
                LastModifiedDate = input.LastModifiedDate,
                IsDeleted = input.IsDeleted,
                Firstname = input.Firstname,
                Lastname = input.Lastname,
                Username = input.Username,
                Email = input.Email,
                PasswordHash = input.PasswordHash,
                IsActive = input.IsActive,
                IsInternalUser = input.IsInternalUser,
            };
            await _userRepository.AddAsync(user);
            await _unitOfWork.CompleteAsync();
        }
    }
}
