using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Cubipool.API.Abstractions;
using Cubipool.API.Configuration;
using Cubipool.Entity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Cubipool.API.Implementations
{
    public class JwtService : IJwtService
    {
        private readonly AppSettings _appSettings;

        public JwtService(
            IOptions<AppSettings> appSettings
        )
        {
            _appSettings = appSettings.Value;
        }

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                }),
                Expires = DateTime.UtcNow.AddYears(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}