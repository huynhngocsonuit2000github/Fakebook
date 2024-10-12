using System;
using System.Threading.Tasks;

namespace UserService.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CompleteAsync();
    }
}
