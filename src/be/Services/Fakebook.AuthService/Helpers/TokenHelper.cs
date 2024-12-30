using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Fakebook.AuthService.Helpers;

public interface ITokenHelper
{
    bool ValidateIdPToken(string token);
}

public class TokenHelper : ITokenHelper
{
    private readonly string _publicKeyPath;

    public TokenHelper(IConfiguration configuration)
    {
        _publicKeyPath = configuration["PublicKeyPath"]
                          ?? throw new ArgumentNullException("PublicKeyPath not configured.");
    }

    public bool ValidateIdPToken(string token)
    {
        System.Console.WriteLine("Start verify token");
        System.Console.WriteLine(token);

        var publicKey = File.ReadAllText(_publicKeyPath);

        System.Console.WriteLine("Public key");
        System.Console.WriteLine(publicKey);

        var rsa = RSA.Create();
        rsa.ImportFromPem(publicKey.ToCharArray());

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false, // Update as per your requirements
            ValidateAudience = false,
            ValidateLifetime = true,
            IssuerSigningKey = new RsaSecurityKey(rsa)
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            return true;
        }
        catch (SecurityTokenException ex)
        {
            System.Console.WriteLine("Exception SecurityTokenException: " + ex.Message);
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            return false;
        }
    }
}
