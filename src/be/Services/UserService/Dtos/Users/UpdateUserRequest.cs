namespace UserService.Dtos.Users
{
    public class UpdateUserRequest
    {
        public string UserId { get; set; } = null!;
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
    }
}