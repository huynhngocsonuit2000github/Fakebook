namespace Fakebook.AuthService.Authentication.Models
{
    public class JwtSettings
    {
        public string SecretKey { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public int ExpirationMinutes { get; set; }

        public new string ToString() => $"{SecretKey} {Issuer} {Audience} {ExpirationMinutes}";
    }
}