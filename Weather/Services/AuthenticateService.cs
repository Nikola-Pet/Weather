using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace Weather.Services
{
    public class AuthenticateService
    {
        public string GenerateToken(int id, string username)
        {

            DateTime expires = DateTime.UtcNow.AddDays(1);
            DateTime issuedAt = DateTime.UtcNow;

            var tokenHandler = new JwtSecurityTokenHandler();

            ClaimsIdentity claimIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Name, username),
            });

            const string sec = "simplekeysimplekeysimplekey";
            var now = DateTime.UtcNow;
            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(sec));
            var singingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var token = (JwtSecurityToken)
                tokenHandler.CreateJwtSecurityToken(
                    subject: claimIdentity,
                    notBefore: issuedAt,
                    expires: expires,
                    signingCredentials: singingCredentials
                    );

            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }



    }
}