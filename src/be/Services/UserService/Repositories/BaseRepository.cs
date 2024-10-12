using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UserService.Data;
using UserService.Entity;

namespace UserService.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly ServiceContext _context;
        private readonly DbSet<T> _dbSet;

        public BaseRepository(ServiceContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> GetByIdAsync(string id)
        {
            return (await _dbSet.FindAsync(id))!;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(string id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<T?> FindFirstAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }
    }
}
