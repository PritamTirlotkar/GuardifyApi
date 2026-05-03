using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GuardifyApi.Models;

namespace GuardifyApi.Helpers
{
    public class JwtHelper
    {
        public static string GenerateToken(AppUser user, IConfiguration config)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(config["Jwt:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("userId", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role),   // 🔥 FIX HERE
                new Claim("societyId", user.SocietyId?.ToString() ?? ""),
                new Claim("email", user.Email ?? ""),
                new Claim("mobile", user.Mobile ?? "")
            };

            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1), // token valid for 1 day
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}