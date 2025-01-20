using Fakebook.PostService.Entity;
using Fakebook.PostService.Enums;

namespace Fakebook.PostService.Dtos.Comments
{
    public class GetCommentModel
    {
        public string Id { get; set; } = null!;
        public string CreatedBy { get; set; } = null!;
        public string LastModifiedBy { get; set; } = null!;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime LastModifiedDate { get; set; } = DateTime.Now;
        public string Content { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string? PostId { get; set; }
        public string? ParentCommentId { get; set; }

        public IEnumerable<GetCommentModel> Replies { get; set; } = null!;
        public IEnumerable<GetLikeModel> Likes { get; set; } = null!;

        public static GetCommentModel FromEntity(Comment comment)
        {
            return new GetCommentModel()
            {
                Id = comment.Id,
                CreatedBy = comment.CreatedBy,
                LastModifiedBy = comment.LastModifiedBy,
                CreatedDate = comment.CreatedDate,
                LastModifiedDate = comment.LastModifiedDate,
                Content = comment.Content,
                UserId = comment.UserId,
                PostId = comment.PostId,
                ParentCommentId = comment.ParentCommentId,
                Replies = comment.Replies.Select(GetCommentModel.FromEntity),
                Likes = comment.Likes.Select(GetLikeModel.FromEntity)
            };
        }
    }

    public class GetLikeModel
    {
        public string Id { get; set; } = null!;
        public string CreatedBy { get; set; } = null!;
        public string LastModifiedBy { get; set; } = null!;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime LastModifiedDate { get; set; } = DateTime.Now;
        public string UserId { get; set; } = null!;
        public string? PostId { get; set; }
        public string? CommentId { get; set; }

        public static GetLikeModel FromEntity(Like like)
        {
            return new GetLikeModel()
            {
                Id = like.Id,
                CreatedBy = like.CreatedBy,
                LastModifiedBy = like.LastModifiedBy,
                CreatedDate = like.CreatedDate,
                LastModifiedDate = like.LastModifiedDate,
                UserId = like.UserId,
                PostId = like.PostId,
                CommentId = like.CommentId,
            };
        }
    }
}