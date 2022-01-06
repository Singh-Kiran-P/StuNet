using System.IO;
using FluentEmail.Core;
using System.Threading.Tasks;

namespace Server.Api.Services
{
    public class Mailer : IEmailSender
    {
        private readonly IFluentEmail _email;

        public Mailer(IFluentEmail email)
        {
            _email = email;
        }

        public async Task<bool> SendEmail(string to, string subject, EmailTemplate template, object model)
        {
            try {
                var result = await _email.To(to)
                    .Subject(subject)
                    .UsingTemplateFromFile(
                        $"{Directory.GetCurrentDirectory()}/Services/Email/Templates/{template}.cshtml",
                        model)
                    .SendAsync();

                return result.Successful;
            } catch { return false; }
        }
    }
}
