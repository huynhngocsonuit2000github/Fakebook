using System.Linq.Expressions;
using System.Threading.Tasks;
using UserService.Entity;

namespace UserService.Repositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(string id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(string id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T?> FindFirstAsync(Expression<Func<T, bool>> predicate);
    }
}
