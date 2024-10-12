namespace Fakebook.DataAccessLayer.Entity
{
    public abstract class BaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string CreatedBy { get; set; } = null!;
        public string LastModifiedBy { get; set; } = null!;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime LastModifiedDate { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
    }
}