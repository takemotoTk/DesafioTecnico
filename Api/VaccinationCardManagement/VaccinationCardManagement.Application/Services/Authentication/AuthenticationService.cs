using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VaccinationCardManagement.Application.Models;
using VaccinationCardManagement.Domain.Entities;

namespace VaccinationCardManagement.Application.Services.Authentication;

public class AuthenticationService
{
    private readonly IConfiguration _configuration;
    public AuthenticationService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public AuthenticationModel GenerateToken(int userId, string userIdentifier)
    {
        var secretKey = Encoding.ASCII.GetBytes(_configuration["Authentication:SecretKey"]);
        var issuer = _configuration["Authentication:Issuer"];
        var audience = _configuration["Authentication:Audience"];
        var expirationLimitInHours = Convert.ToInt32(_configuration["Authentication:ExpirationLimitInMinutes"]);
        var userIdToString = userId.ToString();

        var created = DateTime.UtcNow;
        var expirationDateTime = created.AddMinutes(expirationLimitInHours);

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(Constants.ClaimKeyUserId, userIdToString),
                new Claim(Constants.ClaimKeyIdentifier, userIdentifier)
            }),
            Issuer = issuer,
            Audience = audience,
            Expires = expirationDateTime,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
        };
        var createToken = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(createToken);

        return new AuthenticationModel {
            AccessToken = token,
            Created = created,
            UserIdentifier = userIdentifier,
            UserId = userIdToString,
            Expiration = expirationDateTime
        };
    }
}
