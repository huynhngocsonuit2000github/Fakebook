using Fakebook.PostService.Entity;
using Fakebook.PostService.Enums;

namespace Fakebook.PostService.Dtos.Comments
{
    public class CreateCommentModel
    {
        public string Content { get; set; } = null!;
        public string? PostId { get; set; }
        public string? ParentCommentId { get; set; }

        public Comment ToEntity()
        {
            return new Comment()
            {
                Content = Content,
                PostId = PostId,
                ParentCommentId = ParentCommentId,
            };
        }
    }
}