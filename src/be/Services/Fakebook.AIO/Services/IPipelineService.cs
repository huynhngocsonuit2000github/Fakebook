
using Fakebook.AIO.Entity;

namespace Fakebook.AIO.Services
{
    public interface IPipelineService
    {
        Task<IEnumerable<Pipeline>> GetAllAsync();
        Task<Pipeline?> GetByIdAsync(string id);
        Task UpdateAsync(string id, Pipeline pipeline);
        Task<Pipeline> CreateAsync(Pipeline pipeline);
        Task DeleteAsync(string id);
    }
}
