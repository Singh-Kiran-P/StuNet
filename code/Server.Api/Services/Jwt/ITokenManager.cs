// @Kiran
using System.Threading.Tasks;
using Server.Api.Dtos;
using Server.Api.Models;

namespace Server.Api.Services
{
    public interface ITokenManager
    {
        Task<LoginJwtDto> GetTokenAsync(User user);
        bool ValidateToken(string token);
    }
}
