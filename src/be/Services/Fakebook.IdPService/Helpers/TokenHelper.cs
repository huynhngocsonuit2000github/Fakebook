using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Fakebook.IdPService.Helpers;

public interface ITokenHelper
{
    string GenerateIdPToken(string email);
}
public class TokenHelper : ITokenHelper
{
    private readonly string _privateKeyPath;

    public TokenHelper(IConfiguration configuration)
    {
        _privateKeyPath = configuration["PrivateKeyPath"]
                          ?? throw new ArgumentNullException("PrivateKeyPath not configured.");
    }

    public string GenerateIdPToken(string email)
    {
        // Read the private key from the specified path
        var privateKey = File.ReadAllText(_privateKeyPath);

        var rsa = RSA.Create();
        rsa.ImportFromPem(privateKey.ToCharArray());

        // Create signing credentials using the private key
        var signingCredentials = new SigningCredentials(
            new RsaSecurityKey(rsa),
            SecurityAlgorithms.RsaSha256
        );

        // Generate the JWT token
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, email),
            }),
            Expires = DateTime.UtcNow.AddMinutes(1500),
            SigningCredentials = signingCredentials,
            NotBefore = DateTime.UtcNow
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
