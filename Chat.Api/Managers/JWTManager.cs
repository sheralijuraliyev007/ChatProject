using Chat.Api.Entities;
using Chat.Api.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Chat.Api.Managers
{
    public class JWTManager(IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration;

        public string GenerateToken(User user)
        {
            var jwtParam = _configuration.GetSection("JwtParameters").Get<JwtParameters>();

            var key = System.Text.Encoding.UTF32.GetBytes(jwtParam.Key);

            var signingKey = new SigningCredentials(new SymmetricSecurityKey(key), "HS256");



            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.Username),
                new Claim(ClaimTypes.Name,user.Role!)
            };




            var security = new JwtSecurityToken(
                issuer: jwtParam.Issuer,
                audience: jwtParam.Audience,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signingKey,
                claims: claims);




            var token = new JwtSecurityTokenHandler().WriteToken(security);

            return token;
        }
    }
}
