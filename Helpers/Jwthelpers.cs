using System;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Threading;
namespace Restpelicula.Helpers
{
    public class Jwthelpers
    {
        private readonly byte[] secret;
        public Jwthelpers(string secretKey)
        {
            this.secret = Encoding.ASCII.GetBytes(secretKey);
        }
        public string CreateToken(string username)
        {
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, username));
            var tokenDescription = new SecurityTokenDescriptor()
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(this.secret), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var createdToken = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(createdToken);

        }
    }
}