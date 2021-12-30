using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChatSample.Hubs;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MimeKit;
using Server.Api.Dtos;
using Server.Api.Models;
using Server.Api.Repositories;

// https://github.com/jstedfast/MailKit/blob/master/Documentation/Examples/ImapIdleExample.cs

namespace Server.Api.Services {
    public class IdleClient : IDisposable {
        readonly string host, email, pass;
        readonly int port;
        readonly SecureSocketOptions options;
        List<IMessageSummary> messages;
        CancellationTokenSource cancel;
        CancellationTokenSource done;
        FetchRequest request;
        bool messagesArrived;
        ImapClient client;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        public IdleClient (string host, int port, SecureSocketOptions options, string email, string pass, IServiceScopeFactory serviceScopeFactory) {
            this.client = new ImapClient ();
            this.request = new FetchRequest (MessageSummaryItems.Full | MessageSummaryItems.UniqueId);
            this.messages = new List<IMessageSummary> ();
            this.cancel = new CancellationTokenSource ();
            this.options = options;
            this.email = email;
            this.pass = pass;
            this.host = host;
            this.port = port;
            _serviceScopeFactory = serviceScopeFactory;

        }

        public async Task Run () {
            try {
                await Reconnect ();
                await FetchMessageSummaries (false);
                Console.WriteLine ("d");

            } catch (OperationCanceledException) {
                await client.DisconnectAsync (true);
                return;
            }

            var inbox = client.Inbox;
            inbox.CountChanged += OnCountChanged;
            await Idle ();
            inbox.CountChanged -= OnCountChanged;
            await client.DisconnectAsync (true);
        }

        async Task Reconnect () {
            if (!client.IsConnected) await client.ConnectAsync (host, port, options, cancel.Token);
            if (!client.IsAuthenticated) {
                await client.AuthenticateAsync (email, pass, cancel.Token);
                await client.Inbox.OpenAsync (FolderAccess.ReadOnly, cancel.Token);
            }
        }

        async Task Idle () {
            do {
                try {
                    await WaitForNewMessagesAsync ();
                    if (messagesArrived) {
                        await FetchMessageSummaries (true);
                        messagesArrived = false;
                    }
                } catch (OperationCanceledException) { break; }
            } while (!cancel.IsCancellationRequested);
        }

        void OnCountChanged (object sender, EventArgs e) {
            var folder = (ImapFolder) sender;
            if (folder.Count > messages.Count) {
                messagesArrived = true;
                done?.Cancel ();
            }
        }

        async Task WaitForNewMessagesAsync () {
            while (true) {
                try {
                    if (client.Capabilities.HasFlag (ImapCapabilities.Idle)) {
                        done = new CancellationTokenSource (new TimeSpan (0, 9, 0));
                        try { await client.IdleAsync (done.Token, cancel.Token); } finally {
                            done.Dispose ();
                            done = null;
                        }
                    } else {
                        await Task.Delay (new TimeSpan (0, 1, 0), cancel.Token);
                        await client.NoOpAsync (cancel.Token);
                    }
                    break;
                } catch (ImapProtocolException) { await Reconnect (); } catch (IOException) { await Reconnect (); }
            }
        }

        async Task FetchMessageSummaries (bool receive) {
            while (true) {
                try {
                    int startIndex = messages.Count;
                    IList<IMessageSummary> fetched = client.Inbox.Fetch (startIndex, -1, request, cancel.Token);
                    foreach (var message in fetched) {
                        messages.Add (message);
                        if (receive) OnMessageReceived (message);
                    }
                    break;
                } catch (ImapProtocolException) { await Reconnect (); } catch (IOException) { await Reconnect (); }
            }
        }

        void OnMessageReceived (IMessageSummary message) {

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var _questionRepository = scope.ServiceProvider.GetRequiredService<IQuestionRepository>();
                var _answerRepository = scope.ServiceProvider.GetRequiredService<IAnswerRepository>();
                var _courseRepository = scope.ServiceProvider.GetRequiredService<ICourseRepository>();
                var mailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();
                var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var _hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<ChatHub>>();
                var questions = _questionRepository.getAllAsync().GetAwaiter().GetResult();

                mailSender.SendEmail ("kiran.singh@student.uhasselt.be", "Test sent email on Recieve email", EmailTemplate.ConfirmEmail, new {
                    link = "Test sent email on Recieve email"
                }).GetAwaiter ();

                Console.WriteLine ("Email Sent"); // TODO implement
                Console.WriteLine (message.ToString ());

                (int questionId, string courseMail, string title, string body) = _parseEmail (message);

                User user = _userManager.FindByEmailAsync (courseMail).GetAwaiter ().GetResult ();

                Course course = _courseRepository.getByCourseMail(courseMail).GetAwaiter().GetResult();

                if(course == null) return;

                User user = _userManager.FindByEmailAsync(course.profEmail).GetAwaiter().GetResult();

                Question question = _questionRepository.getAsync(questionId).GetAwaiter().GetResult();
                if (user == null || question == null) { return; }
                Answer answer = new () {
                    userId = user.Id,
                    question = question,
                    title = title,
                    body = body,
                    // files = createAnswerDto.files
                    time = DateTime.UtcNow
                };
                try {
                    _answerRepository.createAsync (answer).GetAwaiter ().GetResult ();
                } catch (System.Exception e) {
                    Console.WriteLine (e);
                }
                _hubContext.Clients.Group ("Question " + question.id).SendAsync ("AnswerNotification", answer.id).GetAwaiter ().GetResult ();

            }
        }

        private (int, string, string, string) _parseEmail (IMessageSummary message) {
            int questionId = 2;
            string courseMail = "senn.robyns@student.uhasselt.be";
            string title = "Answer 2 from Prof";

            // courseMail = message.Envelope.From.Mailboxes[0]
            string mail = message.Envelope.From.Mailboxes.First ().Address;
            string name = message.Envelope.From.Mailboxes.First().Name;

            // IMessageSummary.TextBody is a convenience property that finds the 'text/plain' body part for us
            var bodyPart = message.TextBody;

            // download the 'text/plain' body part
            var body = (TextPart)client.Inbox.GetBodyPart(message.UniqueId, bodyPart);

            // TextPart.Text is a convenience property that decodes the content and converts the result to
            // a string for us
            var text = body.Text;

            return (questionId, courseMail, title, text);
        }
        public void Exit () {
            cancel.Cancel ();
        }

        public void Dispose () {
            client.Dispose ();
            cancel.Dispose ();
        }
    }
}