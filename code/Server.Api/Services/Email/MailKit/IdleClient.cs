using System;
using MimeKit;
using MailKit;
using System.IO;
using System.Linq;
using Server.Api.Dtos;
using ChatSample.Hubs;
using HtmlAgilityPack;
using MailKit.Net.Imap;
using MailKit.Security;
using System.Threading;
using Server.Api.Models;
using System.Threading.Tasks;
using Server.Api.Repositories;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Identity;
using Fizzler.Systems.HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// https://github.com/jstedfast/MailKit/blob/master/Documentation/Examples/ImapIdleExample.cs

namespace Server.Api.Services
{
    public class IdleClient : IDisposable
    {
        readonly int port;
        ImapClient client;
        FetchRequest request;
        bool messagesArrived;
        CancellationTokenSource done;
        List<IMessageSummary> messages;
        CancellationTokenSource cancel;
        readonly string host, email, pass;
        readonly SecureSocketOptions options;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public IdleClient(string host, int port, SecureSocketOptions options, string email, string pass, IServiceScopeFactory serviceScopeFactory)
        {
            this.pass = pass;
            this.host = host;
            this.port = port;
            this.email = email;
            this.options = options;
            this.client = new ImapClient();
            this.messages = new List<IMessageSummary>();
            this.cancel = new CancellationTokenSource();
            this.request = new FetchRequest(MessageSummaryItems.Full | MessageSummaryItems.UniqueId);
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Run()
        {
            try {
                await Reconnect();
                await FetchMessageSummaries(false);

            }
            catch (OperationCanceledException) {
                await client.DisconnectAsync(true);
                return;
            }

            var inbox = client.Inbox;
            inbox.CountChanged += OnCountChanged;
            await Idle();
            inbox.CountChanged -= OnCountChanged;
            await client.DisconnectAsync(true);
        }

        async Task Reconnect()
        {
            if (!client.IsConnected) await client.ConnectAsync(host, port, options, cancel.Token);
            if (!client.IsAuthenticated) {
                await client.AuthenticateAsync(email, pass, cancel.Token);
                await client.Inbox.OpenAsync(FolderAccess.ReadOnly, cancel.Token);
            }
        }

        async Task Idle()
        {
            do {
                try {
                    await WaitForNewMessagesAsync();
                    if (messagesArrived) {
                        await FetchMessageSummaries(true);
                        messagesArrived = false;
                    }
                }
                catch (OperationCanceledException) { break; }
            } while (!cancel.IsCancellationRequested);
        }

        void OnCountChanged(object sender, EventArgs e)
        {
            var folder = (ImapFolder)sender;
            if (folder.Count > messages.Count) {
                messagesArrived = true;
                done?.Cancel();
            }
        }

        async Task WaitForNewMessagesAsync()
        {
            while (true) {
                try {
                    if (client.Capabilities.HasFlag(ImapCapabilities.Idle)) {
                        done = new CancellationTokenSource(new TimeSpan(0, 9, 0));
                        try { await client.IdleAsync(done.Token, cancel.Token); }
                        finally {
                            done.Dispose();
                            done = null;
                        }
                    }
                    else {
                        await Task.Delay(new TimeSpan(0, 1, 0), cancel.Token);
                        await client.NoOpAsync(cancel.Token);
                    }
                    break;
                }
                catch (ImapProtocolException) { await Reconnect(); }
                catch (IOException) { await Reconnect(); }
            }
        }

        async Task FetchMessageSummaries(bool receive)
        {
            while (true)
            {
                try {
                    int startIndex = messages.Count;
                    IList<IMessageSummary> fetched = client.Inbox.Fetch(startIndex, -1, request, cancel.Token);
                    foreach (var message in fetched) {
                        messages.Add(message);
                        if (receive) OnMessageReceived(message);
                    }
                    break;
                }
                catch (ImapProtocolException) { await Reconnect(); }
                catch (IOException) { await Reconnect(); }
            }
        }

        void OnMessageReceived(IMessageSummary message)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var mailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();
                var _configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var _hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<ChatHub>>();
                var _answerRepository = scope.ServiceProvider.GetRequiredService<IAnswerRepository>();
                var _courseRepository = scope.ServiceProvider.GetRequiredService<ICourseRepository>();
                var _questionRepository = scope.ServiceProvider.GetRequiredService<IQuestionRepository>();
                var _notificationRepository = scope.ServiceProvider.GetRequiredService<INotificationRepository<AnswerNotification>>();
                var _subscriptionRepository = scope.ServiceProvider.GetRequiredService<ISubscriptionRepository<QuestionSubscription>>();
                var questions = _questionRepository.GetAllAsync().GetAwaiter().GetResult();
                var sender =_configuration.GetSection("Mail")["SenderEmail"];

                (int questionId, string title, string body) = _parseEmail(message, sender);
                Question question = _questionRepository.GetAsync(questionId).GetAwaiter().GetResult();
                User answerUser = _userManager.FindByEmailAsync(question.course.profEmail).GetAwaiter().GetResult();
                User questionUser = _userManager.FindByIdAsync(question.userId).GetAwaiter().GetResult();
                if (question == null || answerUser == null || questionUser == null) return;
                Answer answer = new() {
                    body = body,
                    title = title,
                    isAccepted = true,
                    question = question,
                    userId = answerUser.Id,
                    time = DateTime.UtcNow
                };

                _answerRepository.CreateAsync(answer).GetAwaiter().GetResult();
                var subscribers = _subscriptionRepository.GetBySubscribedId(question.id).GetAwaiter().GetResult();
                _notificationRepository.CreateAllAync(subscribers.Select(sub => new AnswerNotification {
                    answer = answer,
                    time = answer.time,
                    userId = sub.userId,
                    answerId = answer.id
                })).GetAwaiter().GetResult();

                var ret = GetAnswerDto.Convert(answer, answerUser, questionUser);
                _hubContext.Clients.Group("Question " + question.id).SendAsync("AnswerNotification", ret);
            }
        }

        private (int, string, string) _parseEmail(IMessageSummary message, string sender)
        {
            var name = "";
            var email = message.Envelope.From.Mailboxes.First().Address;
            var index = email.LastIndexOf('@');
            var start = email.Substring(0, index < 0 ? email.Length : index);
            foreach (var s in start.Replace('.', ' ').Split(' ')) {
                name += s.ToUpper()[0] + s.ToLower().Substring(1) + ' ';
            }
            var title = "Answered by Prof. " + name.Substring(0, name.Length - 1);

            var text = ((TextPart)client.Inbox.GetBodyPart(message.UniqueId, message.TextBody)).Text;
            var content = text.Split(sender)[0];
            var body = content.Substring(0, content.LastIndexOf('\n'));

            var html = new HtmlDocument();
            var p = (TextPart)client.Inbox.GetBodyPart(message.UniqueId, message.HtmlBody);
            html.LoadHtml("<html><head></head><body>" + p.Text + "</body></html>");
            var id = html.DocumentNode.QuerySelectorAll("span").LastOrDefault();
            var questionId = Convert.ToInt32(id.InnerText);

            return (questionId, title, body);
        }

        public void Exit()
        {
            cancel.Cancel();
        }

        public void Dispose()
        {
            client.Dispose();
            cancel.Dispose();
        }
    }
}
