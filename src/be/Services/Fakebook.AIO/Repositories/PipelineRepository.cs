using Fakebook.DataAccessLayer.Implementaions;
using Fakebook.AIO.Entity;
using Fakebook.AIO.Data;
using Fakebook.UserService.Services;

namespace Fakebook.AIO.Repositories
{
    public class PipelineRepository : BaseRepository<Pipeline>, IPipelineRepository
    {
        private readonly ServiceContext _serviceContext;
        public PipelineRepository(ServiceContext context) : base(context)
        {
            _serviceContext = context;
        }
    }
}
