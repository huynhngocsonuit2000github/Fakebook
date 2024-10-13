namespace FakebookIntegrationTests.Models.UserService.UserController
{
    public class GetUserModel
    {
        public string Id { get; set; } = null!;
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}