using Fakebook.DataAccessLayer.Interfaces;
using Fakebook.AuthService.Dtos.Users;
using Fakebook.AuthService.Entity;
using Fakebook.AuthService.Repositories;
using Fakebook.AuthService.HttpRequestHandling;
using Fakebook.AuthService.SynchronousApi;
using Fakebook.AuthService.Helpers;

namespace Fakebook.AuthService.Services
{
    public class UserUservice : IUserUservice
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly IIdPSynchronousApiService _idPSynchronousApiService;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        public UserUservice(IUserRepository userRepository, IUnitOfWork unitOfWork, IUserContextService userContextService, IIdPSynchronousApiService idPSynchronousApiService, IRoleRepository roleRepository, IUserRoleRepository userRoleRepository)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _userContextService = userContextService;
            _idPSynchronousApiService = idPSynchronousApiService;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
        }

        public async Task<User> GetOrCreateUserByEmailAsync(string email)
        {
            var existingUser = await _userRepository.FindFirstAsync(e => string.Equals(e.Email, email, StringComparison.OrdinalIgnoreCase));

            if (existingUser is not null)
            {
                System.Console.WriteLine("Existing user");
                return existingUser;
            }

            System.Console.WriteLine("Create new user");

            var idpUser = await _idPSynchronousApiService.GetUserDetailByEmailAsync(email);

            var userId = Guid.NewGuid().ToString();
            var newUser = new User()
            {
                Id = userId,
                Firstname = idpUser!.Firstname,
                Lastname = idpUser.Lastname,
                Username = idpUser.Username.Substring(0, idpUser.Username.IndexOf("_idp")),
                Email = idpUser.Email,
                PasswordHash = AppConstants.DefaultPassword,
                IsActive = true,
                IsInternalUser = true,
                CreatedBy = userId,
                LastModifiedBy = userId,
            };

            var defaultRole = await _roleRepository.GetRoleByRoleNameAsync(AppConstants.DefaultRoleName);
            var defaultUserRole = new UserRole()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                RoleId = defaultRole.Id,
                CreatedBy = userId,
                LastModifiedBy = userId,
            };

            await _userRepository.AddAsync(newUser);
            await _userRoleRepository.AddAsync(defaultUserRole);
            await _unitOfWork.CompleteAsync();

            return newUser;
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

        public async Task<List<string>?> GetCurrentUserPermissionsAsync()
        {
            var currentUser = _userContextService.GetAuthenticatedUserContext();

            if (currentUser is null) throw new Exception("The authenticated user is failed");

            return await _userRepository.GetUserPermissionsByUserIdAsync(currentUser.UserId);

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
