using Fakebook.DataAccessLayer.Entity;

namespace Fakebook.AIO.Entity
{
    public class Case : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string JobName { get; set; } = null!;
        public int NumberOfSuccess { get; set; }
        public int NumberOfFailed { get; set; }
    }
}