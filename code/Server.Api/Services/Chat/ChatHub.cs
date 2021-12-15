using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Api.Repositories;
using Server.Api.Models;

namespace ChatSample.Hubs
{
    public static class UserHandler
    {
        public static HashSet<string> ConnectedIds = new HashSet<string>();
    }


	public class ChatHub : Hub
    {
        private readonly pgMessageRepository _messageRepository;

        public ChatHub(pgMessageRepository messageRepository) 
        {
			_messageRepository = messageRepository;
		}

		public async Task NewMessage(string email, string message, string channelName, int channelId)
		{
			System.Console.WriteLine(Context.ConnectionId + " sent message to " + channelName);

			Message m = new() {
				userMail = email,
                channelId = channelId,
                body = message,
                dateTime = DateTime.Now
			};

			await _messageRepository.createAsync(m);

			await Clients.Group(channelName).SendAsync("messageReceived", email, message, DateTime.Now);
        }

        public override Task OnConnectedAsync()
        {
            System.Console.WriteLine(Context.ConnectionId + " connected");

            UserHandler.ConnectedIds.Add(Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            System.Console.WriteLine(Context.ConnectionId + " disconnected");

            UserHandler.ConnectedIds.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public Task JoinChannel(string channelName) {

            System.Console.WriteLine(Context.ConnectionId + " joined Channel: " + channelName);
			return Groups.AddToGroupAsync(Context.ConnectionId, channelName);
		}
        
        public Task LeaveChannel(string channelName) {
            System.Console.WriteLine(Context.ConnectionId + " left Channel: " + channelName);
			return Groups.RemoveFromGroupAsync(Context.ConnectionId, channelName);
		}
    }
}
