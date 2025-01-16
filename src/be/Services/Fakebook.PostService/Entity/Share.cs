using System.ComponentModel.DataAnnotations.Schema;
using Fakebook.DataAccessLayer.Entity;

namespace Fakebook.PostService.Entity
{
    public class Share : BaseEntity
    {
        public string PostId { get; set; } = null!;
        public string UserId { get; set; } = null!;


        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
    }
}