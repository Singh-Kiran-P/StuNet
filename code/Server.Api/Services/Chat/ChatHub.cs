using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Server.Api.Repositories;
using Server.Api.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;

namespace ChatSample.Hubs
{
    public static class UserHandler
    {
        public static HashSet<string> ConnectedIds = new HashSet<string>();
    }

    // [Authorize]
    public class ChatHub : Hub
    {
        private readonly pgMessageRepository _messageRepository;

        public ChatHub(pgMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task SendMessageToChannel(string message, string channelName, int channelId)
        {
            System.Console.WriteLine(Context.ConnectionId + " sent message to " + channelName);

            string userEmail = getCurrentUserEmail();

            Message m = new()
            {
                userMail = userEmail,
                channelId = channelId,
                body = message,
                dateTime = DateTime.Now
            };

            await _messageRepository.createAsync(m);

            await Clients.Group(channelName).SendAsync("messageReceived", userEmail, message, DateTime.Now);
        }

        public Task JoinChannel(string channelName)
        {

            System.Console.WriteLine(Context.ConnectionId + " joined Channel: " + channelName);
            return Groups.AddToGroupAsync(Context.ConnectionId, channelName);
        }

        public Task LeaveChannel(string channelName)
        {
            System.Console.WriteLine(Context.ConnectionId + " left Channel: " + channelName);
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, channelName);
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("UserConnected", Context.ConnectionId);

            System.Console.WriteLine("connect: " + Context.ConnectionId);

            UserHandler.ConnectedIds.Add(Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.All.SendAsync("UserConnected", Context.ConnectionId);

            System.Console.WriteLine("disconnect:" + Context.ConnectionId);

            UserHandler.ConnectedIds.Remove(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        public string getCurrentUserEmail()
        {
            string userEmail = null;
            ClaimsPrincipal currentUser = Context.GetHttpContext().User;
            if (currentUser.HasClaim(c => c.Type == "username"))
            {
                userEmail = currentUser.Claims.FirstOrDefault(c => c.Type == "username").Value;
                System.Console.WriteLine("email: " + userEmail);
            }
            return userEmail;
        }
    }
}
