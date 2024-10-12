using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UserService.Data;
using UserService.Entity;
using UserService.Repositories;

namespace UserService.Services
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

            var duplicatedEmail = await  _userRepository.FindFirstAsync(e => e.Email.ToLower() == user.Email.ToLower());
            if (duplicatedEmail is not null)
            {
                throw new Exception($"Duplicate email: {duplicatedEmail.Email}");
            }

            var duplicatedUserName = await  _userRepository.FindFirstAsync(e => e.Username.ToLower() == user.Username.ToLower());
            if(duplicatedUserName is not null)
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
    }
}
