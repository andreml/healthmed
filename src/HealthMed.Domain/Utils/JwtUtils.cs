using HealthMed.Domain.Enum;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HealthMed.Domain.Utils;

public static class JwtUtils
{
    public static string GenerateToken(Guid id, Profile perfil, DateTime validade, IConfiguration configuration)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(configuration["Secret:Key"]!);
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, perfil.ToString()),
                new Claim("Id",id.ToString())
            }),

            Expires = validade,
            SigningCredentials = new SigningCredentials(
                         new SymmetricSecurityKey(key),
                         SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
