using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

using MailKit;
using MailKit.Net.Imap;
using MailKit.Security;
using Microsoft.Extensions.DependencyInjection;
using Server.Api.Dtos;
using Server.Api.Repositories;

// https://github.com/jstedfast/MailKit/blob/master/Documentation/Examples/ImapIdleExample.cs

namespace Server.Api.Services
{
    public class IdleClient : IDisposable
    {
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

        public IdleClient(string host, int port, SecureSocketOptions options, string email, string pass, IServiceScopeFactory serviceScopeFactory)
        {
            this.client = new ImapClient();
            this.request = new FetchRequest(MessageSummaryItems.Full | MessageSummaryItems.UniqueId);
            this.messages = new List<IMessageSummary>();
            this.cancel = new CancellationTokenSource();
            this.options = options;
            this.email = email;
            this.pass = pass;
            this.host = host;
            this.port = port;
            _serviceScopeFactory = serviceScopeFactory;

        }

        public async Task Run()
        {
            try
            {
                await Reconnect();
                await FetchMessageSummaries(false);
                Console.WriteLine("d");

            }
            catch (OperationCanceledException)
            {
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
            if (!client.IsAuthenticated)
            {
                await client.AuthenticateAsync(email, pass, cancel.Token);
                await client.Inbox.OpenAsync(FolderAccess.ReadOnly, cancel.Token);
            }
        }

        async Task Idle()
        {
            do
            {
                try
                {
                    await WaitForNewMessagesAsync();
                    if (messagesArrived)
                    {
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
            if (folder.Count > messages.Count)
            {
                messagesArrived = true;
                done?.Cancel();
            }
        }

        async Task WaitForNewMessagesAsync()
        {
            while (true)
            {
                try
                {
                    if (client.Capabilities.HasFlag(ImapCapabilities.Idle))
                    {
                        done = new CancellationTokenSource(new TimeSpan(0, 9, 0));
                        try { await client.IdleAsync(done.Token, cancel.Token); }
                        finally
                        {
                            done.Dispose();
                            done = null;
                        }
                    }
                    else
                    {
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
                try
                {
                    int startIndex = messages.Count;
                    IList<IMessageSummary> fetched = client.Inbox.Fetch(startIndex, -1, request, cancel.Token);
                    foreach (var message in fetched)
                    {
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
                var _questionRepository = scope.ServiceProvider.GetRequiredService<IQuestionRepository>();
                var questions = _questionRepository.getAllAsync().GetAwaiter().GetResult();
                List<questionDto> res = new List<questionDto>();
                foreach (var q in questions)
                {
                    Console.Write(q.title + '\n');
                }
                Console.WriteLine(message.HtmlBody); // TODO implement
            }
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
