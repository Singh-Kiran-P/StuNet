// https://www.youtube.com/watch?v=7JP7V59X1sk&ab_channel=DotNetCoreCentral
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Server.Api.Dtos;
using Server.Api.Models;

namespace VmsApi.Services
{
    public interface ITokenRefresher
    {
        LoginJwtDto Refresh(RefreshCred refreshCred);
    }

    public class TokenRefresher : ITokenRefresher
    {
        private readonly byte[] key;
        public ITokenGenerator _tokenGenerator { get; }

        public TokenRefresher(byte[] key, ITokenGenerator tokenGenerator)
        {
            this.key = key;
            _tokenGenerator = tokenGenerator;
        }


        public LoginJwtDto Refresh(RefreshCred refreshCred)
        {
            var tokenHandle = new JwtSecurityTokenHandler();
            SecurityToken validatedToken = null;
            var principal = tokenHandle.ValidateToken(refreshCred.token,
                new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out validatedToken);

            var jwtToken = validatedToken as JwtSecurityToken;

            // check if null and [optional] check used algo
            if (jwtToken == null)
            {
                throw new SecurityTokenException("Invalid token passed!");
            }

            var userName = principal.Identity.Name;

            // refresh tokens do not match
            if (refreshCred.refreshToken != _tokenGenerator.UsersRefreshTokens[userName])
            {
                throw new SecurityTokenException("Invalid token passed!");
            }


        }

    }
}
