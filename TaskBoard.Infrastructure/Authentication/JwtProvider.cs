using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskBoard.Domain.Extensions;
using TaskBoard.Domain.Models;
using TaskBoard.Service.Interfice;

namespace TaskBoard.Infrastructure.Authentication
{
    public class JwtProvider (IOptions<JwtOptions> options) : IJwtProvider
    {
        private readonly JwtOptions _options = options.Value;
        public string GenerateToken(UserModel user)
        {
            var claims = new List<Claim>
            {
                new Claim("userId", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.GetDisplayName()) 
            };

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)), 
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddDays(_options.ExpiresHours));
           
            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }
   
    }
}
