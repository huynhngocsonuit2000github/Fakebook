using Fakebook.DataAccessLayer.Entity;

namespace Fakebook.PostService.Entity
{
    public class User : BaseEntity
    {
        public User()
        {
            Posts = new List<Post>();
            Likes = new List<Like>();
            Comments = new List<Comment>();
            Shares = new List<Share>();
        }

        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Bio { get; set; } = null!;
        public string? ProfilePictureUrl { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public bool IsInternalUser { get; set; }


        public ICollection<Post> Posts { get; set; } = null!;
        public ICollection<Like> Likes { get; set; } = null!;
        public ICollection<Comment> Comments { get; set; } = null!;
        public ICollection<Share> Shares { get; set; } = null!;
    }
}