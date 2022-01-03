using Server.Api.Dtos;
using Server.Api.Models;
using System.Threading.Tasks;

namespace Server.Api.Services
{
    public interface ITokenManager
    {
        Task<LoginJwtDto> GetTokenAsync(User user);
        bool ValidateToken(string token);
    }
}
