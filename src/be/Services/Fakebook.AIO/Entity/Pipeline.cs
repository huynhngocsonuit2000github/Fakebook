using Fakebook.DataAccessLayer.Entity;

namespace Fakebook.AIO.Entity
{
    public class Pipeline : BaseEntity
    {
        public string JobName { get; set; } = null!;
        public string JobDescription { get; set; } = null!;
        public string AuthToken { get; set; } = null!;
        public string PipelineContent { get; set; } = null!;
    }
}