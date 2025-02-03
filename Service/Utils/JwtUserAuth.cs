using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Crosscutting.Extensions;
using Domain.Domains.Users;
using Microsoft.IdentityModel.Tokens;

namespace Service.Utils;

public static class JwtUserAuth
{
    public static (string, DateTime) GenerateToken(User user, string keyJwt, int validateTokenInDays)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(keyJwt);
        var expiration = DateTime.UtcNow.AddDays(validateTokenInDays);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new (ClaimTypes.Name, user.Name),
                new (ClaimTypes.Email, user.Email),
                new (ClaimTypes.Role, user.AuthorizationRole.GetEnumDescription()),
                new ("UserId", user.Id)
            }),
            Expires = expiration,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return (tokenHandler.WriteToken(token), expiration);
    }
}