using System.Threading.Tasks;
using FluentEmail.Core;
using System.IO;
using System;

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

            var result = await _email.To(to)
                .Subject(subject)
                .UsingTemplateFromFile(
                    $"{Directory.GetCurrentDirectory()}/Services/Email/Templates/{template}.cshtml",
                    model)
                .SendAsync();

            Console.WriteLine("-----------------------------------EMAIL SENT-------------------------------");
            Console.WriteLine("-----------------------------------EMAIL SENT-------------------------------");

            return result.Successful;
        }
    }
}
