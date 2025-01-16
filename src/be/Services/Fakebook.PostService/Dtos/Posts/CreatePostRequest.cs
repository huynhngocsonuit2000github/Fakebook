using Fakebook.PostService.Enums;

namespace Fakebook.UserService.Dtos.Users
{
    public class CreatePostRequest
    {
        public string Content { get; set; } = null!;
        public ViewMode ViewMode { get; set; } = ViewMode.Public;
    }
}