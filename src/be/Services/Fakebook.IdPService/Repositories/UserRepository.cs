using Fakebook.DataAccessLayer.Implementaions;
using Fakebook.IdPService.Data;
using Fakebook.IdPService.Entity;
using Fakebook.IdPService.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Fakebook.IdPService.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private ServiceContext _serviceContext;
        public UserRepository(ServiceContext context) : base(context)
        {
            _serviceContext = context;
        }

        public async Task<LoginUserModel> GetUserByCredentialAsync(string username, string password)
        {
            var user = await _serviceContext.Users.FirstOrDefaultAsync(e => e.Username == username);

            if (user is null || user.PasswordHash != password)
                throw new Exception("The credential is invalid");

            return LoginUserModel.FromEntity(user);
        }
    }
}
