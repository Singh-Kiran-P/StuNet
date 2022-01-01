using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Server.Api.Repositories;
using Server.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;

namespace ChatSample.Hubs
{
    public static class UserHandler
    {
        public static Dictionary<string, string> ConnectedIds = new Dictionary<string, string>();
    }

    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IMessageRepository _messageRepository;
        private readonly UserManager<User> _userManager;
        private readonly ISubscriptionRepository<QuestionSubscription> _questionSubscriptionRepository;
        private readonly ISubscriptionRepository<CourseSubscription> _courseSubscriptionRepository;

        public ChatHub(IMessageRepository messageRepository, UserManager<User> userManager, ISubscriptionRepository<QuestionSubscription> questionSubscriptionRepository, ISubscriptionRepository<CourseSubscription> courseSubscriptionRepository)
        {
            _messageRepository = messageRepository;
            _userManager = userManager;
            _questionSubscriptionRepository = questionSubscriptionRepository;
            _courseSubscriptionRepository = courseSubscriptionRepository;
        }

        public async Task SendMessageToChannel(string message, int channelId)
        {
            System.Console.WriteLine(Context.ConnectionId + " sent message to " + channelId.ToString());
            string userEmail = GetCurrentUserEmail();
            Message m = new()
            {
                userMail = userEmail,
                channelId = channelId,
                body = message,
                time = DateTime.UtcNow
            };
            await _messageRepository.CreateAsync(m);
            await Clients.Group("Channel " + channelId.ToString()).SendAsync("messageReceived", userEmail, message, DateTime.UtcNow);
        }

        public Task JoinChannel(int channelId)
        {
            System.Console.WriteLine(Context.ConnectionId + " joined Channel: " + channelId.ToString());
            return Groups.AddToGroupAsync(Context.ConnectionId, "Channel " + channelId.ToString());
        }

        public Task LeaveChannel(int channelId)
        {
            System.Console.WriteLine(Context.ConnectionId + " left Channel: " + channelId.ToString());
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, "Channel " + channelId.ToString());
        }

        public override async Task OnConnectedAsync()
        {
            // await Clients.All.SendAsync("UserConnected", Context.ConnectionId);
            UserHandler.ConnectedIds[GetCurrentUserId()] = Context.ConnectionId;
            await AddUserToSubscribedGroups();
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
            UserHandler.ConnectedIds.Remove(GetCurrentUserId());
            await base.OnDisconnectedAsync(exception);
        }

        public async Task AddUserToSubscribedGroups()
        {
            string userId = GetCurrentUserId();
            ICollection<CourseSubscription> subscribedCourses = await _courseSubscriptionRepository.GetByUserId(userId);
            ICollection<QuestionSubscription> subscribedQuestions = await _questionSubscriptionRepository.GetByUserId(userId);
            await Task.WhenAll(subscribedCourses.Select(sc => Groups.AddToGroupAsync(Context.ConnectionId, "Course " + sc.subscribedItemId.ToString())));
            await Task.WhenAll(subscribedQuestions.Select(sq => Groups.AddToGroupAsync(Context.ConnectionId, "Question " + sq.subscribedItemId.ToString())));
        }

        public string GetCurrentUserEmail()
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

        public string GetCurrentUserId()
        {
            string userId = null;
            ClaimsPrincipal currentUser = Context.GetHttpContext().User;
            if (currentUser.HasClaim(c => c.Type == "userref"))
            {
                userId = currentUser.Claims.FirstOrDefault(c => c.Type == "userref").Value;
            }
            return userId;
        }
    }
}
