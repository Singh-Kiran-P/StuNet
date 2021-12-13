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
        public async Task NewMessage(long username, string message)
        {
            System.Console.WriteLine("test");
            await Clients.All.SendAsync("messageReceived", username, message);
        }

        public override Task OnConnectedAsync()
        {
            System.Console.WriteLine("connect: "+Context.ConnectionId);

            UserHandler.ConnectedIds.Add(Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            System.Console.WriteLine("disconnect:" +Context.ConnectionId);

            UserHandler.ConnectedIds.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
