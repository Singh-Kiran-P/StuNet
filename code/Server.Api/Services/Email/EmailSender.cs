using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using Server.Api.Models; //for gmail integration
using System.Net.Mail; //for gmail integration
using System.Text;

namespace Server.Api.Services
{
    public class EmailSender : IEmailSender
    {
        public EmailSender()
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: false).Build();
            _ApiKey = config.GetValue<string>("SendGridKey");
        }

        private string _ApiKey { get; } //set only via Secret Manager

        public void SendEmail(string email, string subject, string message)
        {
            // await ExecuteSendGrid(_ApiKey, subject, message, email);
            ExecuteGmail(subject, message, email);
        }

        private async Task<Response> ExecuteSendGrid(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("stunetUH@gmail.com", "StuNet"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // disable tracking settings
            // ref.: https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);
            msg.SetOpenTracking(false);
            msg.SetGoogleAnalytics(false);
            msg.SetSubscriptionTracking(false);

            return await client.SendEmailAsync(msg);
        }

        private void ExecuteGmail(string subject, string body, string email)
        {
            var fromAddress = new MailAddress("stunetUH@gmail.com", "StuNet");
            const string fromPassword = "BabyYoda123"; //Ik weet dat dit niet veilig is but idc heb niet veel tijd
            var toAddress = new MailAddress(email);

            StringBuilder template = new();
            template.AppendLine("Dear @Model.FirstName,");
            template.AppendLine("<p>Tanks for purchasing @Model.name");
            template.AppendLine("- StuNet Team");

            try
            {
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                };
                smtp.Send(message);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void sendQuestionEmailToProf(Question question)
        {
            throw new NotImplementedException();
        }

        private string QuestionToEmail(Question question)
        {
            throw new NotImplementedException();
        }

        private async Task recieveMail()
        { //Probably use some sort of webhooks since al mailing clients are paid
            throw new NotImplementedException();
        }

        public Task<bool> SendUsingTemplate(string to, string subject, EmailTemplate template, object model)
        {
            throw new NotImplementedException();
        }
    }
}
