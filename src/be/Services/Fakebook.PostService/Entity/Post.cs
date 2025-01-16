using System.ComponentModel.DataAnnotations.Schema;
using Fakebook.DataAccessLayer.Entity;
using Fakebook.PostService.Enums;

namespace Fakebook.PostService.Entity
{
    public class Post : BaseEntity
    {
        public Post()
        {
            Likes = new List<Like>();
            Comments = new List<Comment>();
            Shares = new List<Share>();
        }

        public string Content { get; set; } = null!;
        // public string MediaUrl { get; set; } = null!; // exclude in phase 1
        public ViewMode ViewMode { get; set; } = ViewMode.Public;

        public string OwnerId { get; set; } = null!;


        [ForeignKey(nameof(OwnerId))]
        public User Owner { get; set; } = null!;
        public ICollection<Like> Likes { get; set; } = null!;
        public ICollection<Comment> Comments { get; set; } = null!;
        public ICollection<Share> Shares { get; set; } = null!;
    }
}