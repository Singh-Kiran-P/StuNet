using System;
using System.Linq;
using Server.Api.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using Server.Api.Repositories;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<User> _userManager;
        private readonly IMessageRepository _messageRepository;
        private readonly ISubscriptionRepository<CourseSubscription> _courseSubscriptionRepository;
        private readonly ISubscriptionRepository<QuestionSubscription> _questionSubscriptionRepository;

        public ChatHub(IMessageRepository messageRepository, UserManager<User> userManager, ISubscriptionRepository<QuestionSubscription> questionSubscriptionRepository, ISubscriptionRepository<CourseSubscription> courseSubscriptionRepository)
        {
            _userManager = userManager;
            _messageRepository = messageRepository;
            _courseSubscriptionRepository = courseSubscriptionRepository;
            _questionSubscriptionRepository = questionSubscriptionRepository;
        }

        public async Task SendMessageToChannel(string message, int channelId)
        {
            string userEmail = GetCurrentUserEmail();
            Message m = new() {
                body = message,
                userMail = userEmail,
                channelId = channelId,
                time = DateTime.UtcNow
            };
            await _messageRepository.CreateAsync(m);
            await Clients.Group("Channel " + channelId.ToString()).SendAsync("messageReceived", userEmail, message, DateTime.UtcNow);
        }

        public Task JoinChannel(int channelId)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, "Channel " + channelId.ToString());
        }

        public Task LeaveChannel(int channelId)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, "Channel " + channelId.ToString());
        }

        public override async Task OnConnectedAsync()
        {
            UserHandler.ConnectedIds[GetCurrentUserId()] = Context.ConnectionId;
            await AddUserToSubscribedGroups();
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
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
            if (currentUser.HasClaim(c => c.Type == "username")) {
                userEmail = currentUser.Claims.FirstOrDefault(c => c.Type == "username").Value;
            }
            return userEmail;
        }

        public string GetCurrentUserId()
        {
            string userId = null;
            ClaimsPrincipal currentUser = Context.GetHttpContext().User;
            if (currentUser.HasClaim(c => c.Type == "userref")) {
                userId = currentUser.Claims.FirstOrDefault(c => c.Type == "userref").Value;
            }
            return userId;
        }
    }
}
