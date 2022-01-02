using System.Threading.Tasks;
using FluentEmail.Core;
using System.IO;

namespace Server.Api.Services
{
    // https://blog.zhaytam.com/2019/06/08/emailsender-service-fluent-email-razor-templates/
    // https://markscodingspot.com/send-html-emails-with-attachments-using-fluent-email-csharp-and-net-5/
    public class Mailer : IEmailSender
    {
        private readonly IFluentEmail _email;

        public Mailer(IFluentEmail email)
        {
            _email = email;
        }

        public async Task<bool> SendEmail(string to, string subject, EmailTemplate template, object model)
        {
            var result = await _email.To(to)
                .Subject(subject)
                .UsingTemplateFromFile(
                    $"{Directory.GetCurrentDirectory()}/Services/Email/Templates/{template}.cshtml",
                    model)
                .SendAsync();


            return result.Successful;
        }
    }
}
