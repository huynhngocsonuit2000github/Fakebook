namespace Fakebook.PostService.DataSeeding.Models
{

    public class SeedData
    {
        public List<User> Users { get; set; } = new();
        public List<Post> Posts { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();
        public List<Like> Likes { get; set; } = new();
        public List<Share> Shares { get; set; } = new();
    }

    public class User
    {
        public string Id { get; set; } = null!;
        public bool IsActive { get; set; }
        public bool IsInternalUser { get; set; }
        public string Username { get; set; } = null!;
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Email { get; set; } = null!;
    }

    public class Post
    {
        public string Id { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string OwnerId { get; set; } = null!;
    }

    public class Comment
    {
        public string Id { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string? PostId { get; set; }
        public string? ParentCommentId { get; set; }
    }

    public class Like
    {
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string? PostId { get; set; }
        public string? CommentId { get; set; }
    }

    public class Share
    {
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string PostId { get; set; } = null!;
    }

}