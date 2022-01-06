using System;
using MailKit.Security;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Server.Api.Services
{
    public class MailListener : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public MailListener(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
        {
            _configuration = configuration;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Listen();
            return Task.CompletedTask;
        }

        public void Listen()
        {
            try {
                string pass = _configuration.GetSection("Mail")["password"];
                string email = _configuration.GetSection("Mail")["SenderEmail"];
                string imapHost = _configuration.GetSection("Mail")["ImapHost"];
                int imapPort = Convert.ToInt32(_configuration.GetSection("Mail")["ImapPort"]);
                var client = new IdleClient(imapHost, imapPort, SecureSocketOptions.Auto, email, pass, _serviceScopeFactory);
                client.Run().GetAwaiter();
            } catch {}
        }
    }
}
