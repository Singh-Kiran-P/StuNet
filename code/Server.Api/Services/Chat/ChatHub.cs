using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ChatSample.Hubs {
    public static class UserHandler {
        public static HashSet<string> ConnectedIds = new HashSet<string> ();
    }

    public class ChatHub : Hub {
        public Task SendMessageToAll (string username, string message) {
            System.Console.WriteLine ("test");
            return Clients.All.SendAsync ("ReceiveMessage", username, message);
        }

        public Task SendMessageToCaller (string message) {
            return Clients.Caller.SendAsync ("messageReceived", message);
        }

        public Task SendMessageToUser (string username, string message) {
            return Clients.Client (username).SendAsync ("ReceiveMessage", message);

        }

        public Task JoinGroup (string group) {
            return Groups.AddToGroupAsync (Context.ConnectionId, group);

        }

        public Task SendMessageToGroup(string group,string message)
        {
            return Clients.Group(group).SendAsync("ReceiveMessage", message);

        }

        public override async Task OnConnectedAsync () {

            ClaimsPrincipal currentUser = Context.GetHttpContext().User;
            if (currentUser.HasClaim(c => c.Type == "username"))
            {
                string userEmail = currentUser.Claims.FirstOrDefault(c => c.Type == "username").Value;
                System.Console.WriteLine("email: " + userEmail);

            }

            await Clients.All.SendAsync ("UserConnected", Context.ConnectionId);

            System.Console.WriteLine ("connect: " + Context.ConnectionId);

            UserHandler.ConnectedIds.Add (Context.ConnectionId);
            await base.OnConnectedAsync ();
        }

        public override async Task OnDisconnectedAsync (Exception exception) {
            await Clients.All.SendAsync ("UserConnected", Context.ConnectionId);

            System.Console.WriteLine ("disconnect:" + Context.ConnectionId);

            UserHandler.ConnectedIds.Remove (Context.ConnectionId);
            await base.OnDisconnectedAsync (exception);
        }
    }
}
