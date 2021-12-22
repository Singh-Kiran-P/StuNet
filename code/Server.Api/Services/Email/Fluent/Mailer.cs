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

// https://blog.zhaytam.com/2019/06/08/emailsender-service-fluent-email-razor-templates/

// https://markscodingspot.com/send-html-emails-with-attachments-using-fluent-email-csharp-and-net-5/
namespace Server.Api.Services
{

    public class Mailer : IEmailSender
    {

        private const string TemplatePath = "Server.Api.Services.Email.Templates.{0}.cshtml";
        private readonly IFluentEmail _email;

        public Mailer(IFluentEmail email)
        {
            _email = email;
        }

        public async Task<bool> SendUsingTemplate(string to, string subject, EmailTemplate template, object model)
        {
            Console.WriteLine(to);
            Console.WriteLine(subject);
            Console.WriteLine(template);
            Console.WriteLine(model);
            var result = await _email.To(to)
                .Subject(subject)
                .UsingTemplateFromEmbedded("Server.Api.Services.Email.Templates.ConfirmEmail.cshtml", new { link = "sdqfqdsfsfob" }, 
		TypeFromYourEmbeddedAssembly.GetType().GetTypeInfo().Assembly)
                .SendAsync();
 
            return result.Successful;
        }

        private static ExpandoObject ToExpando(object model)
        {
            if (model is ExpandoObject exp)
            {
                return exp;
            }

            IDictionary<string, object> expando = new ExpandoObject();
            foreach (var propertyDescriptor in model.GetType().GetTypeInfo().GetProperties())
            {
                var obj = propertyDescriptor.GetValue(model);

                if (obj != null && IsAnonymousType(obj.GetType()))
                {
                    obj = ToExpando(obj);
                }

                expando.Add(propertyDescriptor.Name, obj);
            }

            return (ExpandoObject)expando;
        }

        private static bool IsAnonymousType(Type type)
        {
            bool hasCompilerGeneratedAttribute = type.GetTypeInfo()
                .GetCustomAttributes(typeof(CompilerGeneratedAttribute), false)
                .Any();

            bool nameContainsAnonymousType = type.FullName.Contains("AnonymousType");
            bool isAnonymousType = hasCompilerGeneratedAttribute && nameContainsAnonymousType;

            return isAnonymousType;
        }

        public void SendEmail(string email, string subject, string message)
        {
            throw new NotImplementedException();
        }
    }
}
