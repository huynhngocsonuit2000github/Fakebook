using System.ComponentModel.DataAnnotations.Schema;
using Fakebook.DataAccessLayer.Entity;

namespace Fakebook.PostService.Entity
{
    public class Comment : BaseEntity
    {
        public Comment()
        {
            Likes = new List<Like>();
            Replies = new List<Comment>();
        }
        public string Content { get; set; } = null!;

        public string UserId { get; set; } = null!;
        public string? PostId { get; set; }
        public string? ParentCommentId { get; set; }


        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        [ForeignKey(nameof(PostId))]
        public Post? Post { get; set; }

        [ForeignKey(nameof(ParentCommentId))]
        public Comment? ParentComment { get; set; }

        public ICollection<Comment> Replies { get; set; } = null!;
        public ICollection<Like> Likes { get; set; } = null!;
    }

}