
using Fakebook.AIO.Entity;

namespace Fakebook.AIO.Services
{
    public interface ICaseService
    {
        Task<IEnumerable<Case>> GetAllAsync();
        Task<Case?> GetByIdAsync(string id);
        Task UpdateAsync(string id, Case cas);
        Task<Case> CreateAsync(Case cas);
        Task DeleteAsync(string id);
    }
}
