using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

public class AuthService
{
private const string PublicKey = "your-public-key"; // Replace with your actual public key

    public bool ValidateToken1(string token)
    {
        var rsa = RSA.Create();
        rsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(PublicKey), out _);

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
        catch (Exception)
        {
            return false;
        }
    }

}
