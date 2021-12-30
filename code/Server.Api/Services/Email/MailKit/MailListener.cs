// @kiran
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

using MailKit;
using MailKit.Net.Imap;
using MailKit.Security;

using Server.Api.Repositories;
using Server.Api.Dtos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Server.Api.Services
{

    public class MailListener : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _serviceScopeFactory;


        public MailListener(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;

            _configuration = configuration;
        }


        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Listen();
            return Task.CompletedTask;
        }

        public void Listen()
        {
            string pass = _configuration.GetSection("Mail")["G-password"];
            string email = _configuration.GetSection("Mail")["G-SenderEmail"];
            string imapHost = _configuration.GetSection("Mail")["G-ImapHost"];
            int imapPort = Convert.ToInt32(_configuration.GetSection("Mail")["G-ImapPort"]);
            var client = new IdleClient(imapHost, imapPort, SecureSocketOptions.Auto, email, pass, _serviceScopeFactory);
            client.Run().GetAwaiter();
        }

    }
}