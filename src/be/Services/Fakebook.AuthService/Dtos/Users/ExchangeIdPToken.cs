namespace Fakebook.AuthService.Dtos.Users;
public class ExchangeIdPToken
{
    public string Email { get; set; } = null!;
    public string IdPToken { get; set; } = null!;
}