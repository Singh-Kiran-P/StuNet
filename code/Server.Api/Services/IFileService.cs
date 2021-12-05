using Microsoft.AspNetCore.Http;
namespace Server.Api.Services
{
    public interface IFileService
    {
        string Upload(IFormFile file);
    }
}