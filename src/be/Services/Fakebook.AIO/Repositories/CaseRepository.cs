using Fakebook.DataAccessLayer.Implementaions;
using Fakebook.AIO.Entity;
using Fakebook.AIO.Data;

namespace Fakebook.AIO.Repositories
{
    public class CaseRepository : BaseRepository<Case>, ICaseRepository
    {
        private readonly ServiceContext _serviceContext;
        public CaseRepository(ServiceContext context) : base(context)
        {
            _serviceContext = context;
        }
    }
}
