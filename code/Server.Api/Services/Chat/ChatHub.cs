using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatSample.Hubs
{
    public static class UserHandler
    {
        public static HashSet<string> ConnectedIds = new HashSet<string>();
    }

    public class ChatHub : Hub
    {
        public async Task NewMessage(long username, string message, string group)
        {
            System.Console.WriteLine(Context.ConnectionId + " sent message to " + group);

            await Clients.Group(group).SendAsync("messageReceived", username, message);
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
