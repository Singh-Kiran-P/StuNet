// @Kiran

using System;

namespace Server.Api.Dtos
{
    public record LoginJwtDto
    {
        public LoginJwtDto(string token)
        {
            JwtBearerToken = token;
        }
        public string JwtBearerToken { get; }
    }
}
