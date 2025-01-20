using Fakebook.PostService.Entity;

namespace Fakebook.PostService.Dtos.Comments
{
    public class CreateLikeModel
    {
        public string? PostId { get; set; }
        public string? CommentId { get; set; }

        public Like ToEntity()
        {
            return new Like()
            {
                PostId = PostId,
                CommentId = CommentId,
            };
        }
    }
}