using System.ComponentModel.DataAnnotations.Schema;
using Fakebook.DataAccessLayer.Entity;

namespace Fakebook.PostService.Entity
{
    public class Like : BaseEntity
    {
        public string UserId { get; set; } = null!;

        public string? PostId { get; set; }
        public string? CommentId { get; set; }


        [ForeignKey(nameof(PostId))]
        public Post? Post { get; set; }

        [ForeignKey(nameof(CommentId))]
        public Comment? Comment { get; set; }
    }
}