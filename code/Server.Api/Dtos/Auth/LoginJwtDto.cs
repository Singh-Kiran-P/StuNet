using System;

namespace Server.Api.Dtos
{
    public record LoginJwtDto
    {
        public LoginJwtDto(string jwt, string refresh)
        {
            JwtBearerToken = jwt;
            RefreshToken = refresh;
        }
        public string JwtBearerToken { get; }
        public string RefreshToken { get; }
    }

    public class RefreshCred
    {
        public string token { get; set; }
        public string refreshToken { get; set; }
    }
}
