namespace Server.Api.Dtos
{
    public record LoginJwtDto
    {
        public string JwtBearerToken { get; }
        public LoginJwtDto(string token)
        {
            JwtBearerToken = token;
        }
    }
}
