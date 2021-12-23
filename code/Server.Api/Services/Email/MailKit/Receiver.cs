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

namespace Server.Api.Services {

    public class Receiver
    {
	private readonly IQuestionRepository _questionRepository;
	private readonly IConfiguration _configuration;
    
        public Receiver(IConfiguration configuration, IQuestionRepository questionRepository)
        {
			_questionRepository = questionRepository;
            _configuration = configuration;
            Console.Write("Running email reciever");
            // tester();   
        }

        public async Task Run(){
            string pass = _configuration.GetSection("Mail")["G-password"];
            string email = _configuration.GetSection("Mail")["G-SenderEmail"];
            var client = new IdleClient("imap.gmail.com", 993, SecureSocketOptions.Auto, email, pass);
            await client.Run();
			// idle.GetAwaiter().GetResult();
            
        }
        public async void tester() {
            var questions = await _questionRepository.getAllAsync();
            List<questionDto> res = new List<questionDto>();
            foreach (var q in questions)
            {
            	Console.Write(q.title + '\n');
            }
        }
    }
}
