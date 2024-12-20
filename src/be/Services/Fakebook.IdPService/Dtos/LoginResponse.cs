namespace Fakebook.IdPService.Dtos;
public class LoginResponse
{
    public string Email { get; set; } = null!;
    public string IdPToken { get; set; } = null!;
}