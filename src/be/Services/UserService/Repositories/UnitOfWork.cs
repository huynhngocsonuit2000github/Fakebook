using System.Threading.Tasks;
using UserService.Data;

namespace UserService.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ServiceContext _context;

        public UnitOfWork(ServiceContext context)
        {
            _context = context;
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
