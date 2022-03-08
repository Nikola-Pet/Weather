using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;

namespace Orion.WeatherApi.Services
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

        public string GetJwtTokenString(HttpRequestMessage request)
        {
            IEnumerable<string> authzHeaders;
            request.Headers.TryGetValues("Authorization", out authzHeaders);
            var bearerToken = authzHeaders.ElementAt(0);
            string token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;

            return token;
        }

        public JwtSecurityToken GetJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("simplekeysimplekeysimplekey");

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,

            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            return jwtToken;
        }

        public string ValidateUsernameJwtToken(string token)
        {
            var jwtToken = GetJwtToken(token);

            var r = jwtToken.Claims.First(x => x.Type == "unique_name").ToString();
            string rola = r.Remove(0, 13);
            return rola;
        }




    }
}