using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DatingAPI.Constants;

namespace DatingAPI.Helpers
{
    public class JwtHelper : IJwtHelper
    {
        private readonly IConfiguration _configuration;

        public JwtHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateRefreshToken()
        {
            var rngCryptoServiceProvider = RandomNumberGenerator.Create();
            byte[] randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            string guid = Guid.NewGuid().ToString();

            string randomString = Encoding.UTF8.GetString(randomBytes);
            byte[] randomStringGuid = Encoding.UTF8.GetBytes($"{randomString}_{guid}");
            string refreshToken = Convert.ToBase64String(randomStringGuid);

            return refreshToken;
        }

        public string GenerateToken(string userId, string username, string sessionId, long sessionDuration, DateTime sessionStart)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Authentication:EncryptionKey"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _configuration["Authentication:Audience"],
                Issuer = _configuration["Authentication:Issuer"],
                Subject = new ClaimsIdentity(
                    new[] {
                            new Claim(ClaimTypes.NameIdentifier, userId),
                            new Claim(ClaimTypes.Name, username),
                            new Claim("SessionId", sessionId)
                    }),
                Expires = sessionStart.AddSeconds(sessionDuration),
                SigningCredentials = signinCredentials
            };

            var rolesClaims = new List<Claim>();

            //foreach (var role in roles)
            //{
            //    rolesClaims.Add(new Claim(ClaimTypes.Role, role));
            //};

            tokenDescriptor.Subject.AddClaims(rolesClaims);

            // var token = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
