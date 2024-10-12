namespace Fakebook.DataAccessLayer.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CompleteAsync();
    }
}
