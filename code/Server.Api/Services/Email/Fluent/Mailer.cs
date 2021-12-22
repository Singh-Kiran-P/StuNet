using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using Server.Api.Models;
using System.Net.Mail;
using System.Text;
using FluentEmail.Core;
using Microsoft.Extensions.Logging;
using System.Dynamic;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Linq;
using System.IO;

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

            return result.Successful;
        }
    }
}
