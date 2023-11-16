using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StudentInformationSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentInformationSystem.Application.JWT
{
    public class JwtService : IJwtService
    {
        private readonly string _secretKey;

        public JwtService(IConfiguration configuration)
        {
            _secretKey = configuration["JwtKey:Secret"];
        }

        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.UserRole.RoleName)
            };

            var identity = new ClaimsIdentity(claims, "Bearer");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "https://localhost:5001",
                audience: "https://localhost:5001",
                claims: claims,
                expires: DateTime.Now.AddHours(15),
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public List<string> GetRolesFromToken(string jwtToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(jwtToken) as JwtSecurityToken;

            if (jsonToken == null)
            {
                return new List<string>();
            }

            var rolesClaim = jsonToken.Claims.FirstOrDefault(claim => claim.Type == "roles");

            if (rolesClaim != null)
            {
                return rolesClaim.Value.Split(',').ToList();
            }

            return new List<string>();
        }
    }
}
