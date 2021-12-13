using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ChatSample.Hubs
{
    public class ChatHub : Hub
    {
        public async Task NewMessage(long username, string message)
        {
            System.Console.WriteLine("test");
            await Clients.All.SendAsync("messageReceived", username, message);
        }
    }
}
