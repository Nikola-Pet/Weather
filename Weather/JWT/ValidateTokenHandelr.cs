using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Orion.WeatherApi.JWT
{
    public class ValidateTokenHandelr : DelegatingHandler
    {
        public static bool TryRetriveToken(HttpRequestMessage request, out string token)
        {
            token = null;
            IEnumerable<string> authzHeaders;
            
            if (!request.Headers.TryGetValues("Authorization", out authzHeaders))
            {
                return false;
            }
            var bearerToken = authzHeaders.ElementAt(0);
            token = token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
            return true;
        }

        

        protected override Task<HttpResponseMessage>SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpStatusCode statusCode;
            string token;

            if (!TryRetriveToken(request, out token))
            {
                statusCode = HttpStatusCode.Unauthorized;
                return base.SendAsync(request, cancellationToken);
            }

            try
            {
                const string sec = "simplekeysimplekeysimplekey";
                var now = DateTime.UtcNow;
                var securytyKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(sec));

                SecurityToken securityToken;
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    LifetimeValidator = this.LifetimeValidator,
                    IssuerSigningKey = securytyKey
                };

                Thread.CurrentPrincipal = handler.ValidateToken(token, validationParameters, out securityToken);
                HttpContext.Current.User = handler.ValidateToken(token, validationParameters, out securityToken);

                return base.SendAsync(request, cancellationToken);
            }
            catch (SecurityTokenValidationException e)
            {

                statusCode = HttpStatusCode.Unauthorized;
            }
            catch (Exception ex)
            {
                statusCode = HttpStatusCode.InternalServerError;
            }

            return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(statusCode) { });
        }

        public bool LifetimeValidator(DateTime? notBefore,
                                      DateTime? expires,
                                      SecurityToken securityToken,
                                      TokenValidationParameters validationParameters)
        {
            if (expires != null)
            {
                if (DateTime.UtcNow < expires)
                {
                    return true;
                }

            }
            return false;
        }

    }
}