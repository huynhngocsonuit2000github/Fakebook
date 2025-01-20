using Fakebook.PostService.Entity;
using Fakebook.PostService.Enums;

namespace Fakebook.PostService.Dtos.Posts
{
    public class GetPostModel
    {
        public string Id { get; set; } = null!;
        public string CreatedBy { get; set; } = null!;
        public string LastModifiedBy { get; set; } = null!;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime LastModifiedDate { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
        public string Content { get; set; } = null!;
        public string MediaUrl { get; set; } = null!;
        public ViewMode ViewMode { get; set; } = ViewMode.Public;
        public string OwnerId { get; set; } = null!;

        public IEnumerable<GetLikeModel> Likes { get; set; } = null!;
        public IEnumerable<GetCommentModel> Comments { get; set; } = null!;

        public static GetPostModel FromEntity(Post post)
        {
            return new GetPostModel()
            {
                Id = post.Id,
                CreatedBy = post.CreatedBy,
                LastModifiedBy = post.LastModifiedBy,
                CreatedDate = post.CreatedDate,
                LastModifiedDate = post.LastModifiedDate,
                IsDeleted = post.IsDeleted,
                Content = post.Content,
                MediaUrl = string.Empty,
                ViewMode = post.ViewMode,
                OwnerId = post.OwnerId,

                Likes = post.Likes.Select(GetLikeModel.FromEntity),
                Comments = post.Comments.Select(GetCommentModel.FromEntity),
            };
        }



        public class GetLikeModel
        {
            public string Id { get; set; } = null!;
            public string CreatedBy { get; set; } = null!;
            public DateTime CreatedDate { get; set; } = DateTime.Now;
            public bool IsDeleted { get; set; }

            public static GetLikeModel FromEntity(Like Like)
            {
                return new GetLikeModel()
                {
                    Id = Like.Id,
                    CreatedBy = Like.CreatedBy,
                    CreatedDate = Like.CreatedDate,
                    IsDeleted = Like.IsDeleted,
                };
            }
        }

        public class GetCommentModel
        {
            public string Id { get; set; } = null!;
            public string Content { get; set; } = null!;
            public string CreatedBy { get; set; } = null!;
            public DateTime CreatedDate { get; set; } = DateTime.Now;

            public static GetCommentModel FromEntity(Comment comment)
            {
                return new GetCommentModel()
                {
                    Id = comment.Id,
                    Content = comment.Content,
                    CreatedBy = comment.CreatedBy,
                    CreatedDate = comment.CreatedDate,
                };
            }
        }
    }
}