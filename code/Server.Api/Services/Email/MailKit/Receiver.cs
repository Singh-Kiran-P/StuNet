using System;
using MailKit;
using MailKit.Net.Imap;
using Microsoft.Extensions.Configuration;

namespace Server.Api.Services {
	public class Receiver
	{
        private ImapClient _client;
        private IdleClient _idleClient;

        public Receiver(IConfiguration configuration) {
            _client = new ImapClient();
            _client.Connect("imap.gmail.com", 993, true);
            _client.Authenticate(configuration.GetSection("Mail")["G-SenderEmail"], configuration.GetSection("Mail")["G-password"]);
            Receive();
        }

		public void Receive()
		{
            var inbox = _client.Inbox;
            inbox.Open(FolderAccess.ReadOnly);

            _client.

            Console.WriteLine("Total messages: {0}", inbox.Count);
            Console.WriteLine("Recent messages: {0}", inbox.Recent);

            for (int i = 0; i < inbox.Count; i++) {
                var message = inbox.GetMessage (i);
                Console.WriteLine("Subject: {0}", message.Subject);
            }

            _client.Disconnect(true);
		}
	}
}
