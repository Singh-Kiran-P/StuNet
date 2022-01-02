using System.Threading.Tasks;

namespace Server.Api.Services
{
    public interface IEmailSender
    {
        Task<bool> SendEmail(string to, string subject, EmailTemplate template, object model);
    }
}
