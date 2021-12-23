// @kiran, jochem

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
        private readonly pgMessageRepository _messageRepository;
        private readonly UserManager<User> _userManager;
        private readonly IQuestionSubscriptionRepository _questionSubscriptionRepository;
        private readonly ICourseSubscriptionRepository _courseSubscriptionRepository;

        public ChatHub(pgMessageRepository messageRepository, UserManager<User> userManager, IQuestionSubscriptionRepository questionSubscriptionRepository, ICourseSubscriptionRepository courseSubscriptionRepository)
        {
            _messageRepository = messageRepository;
            _userManager = userManager;
            _questionSubscriptionRepository = questionSubscriptionRepository;
            _courseSubscriptionRepository = courseSubscriptionRepository;
        }

        public async Task SendMessageToChannel(string message, int channelId)
        {
            System.Console.WriteLine(Context.ConnectionId + " sent message to " + channelId.ToString());

            string userEmail = getCurrentUserEmail();

            Message m = new()
            {
                userMail = userEmail,
                channelId = channelId,
                body = message,
                time = DateTime.UtcNow
            };

            await _messageRepository.createAsync(m);

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

            System.Console.WriteLine("connect: " + Context.ConnectionId);

            UserHandler.ConnectedIds[getCurrentUserId()] = Context.ConnectionId;
            await AddUserToSubscribedGroups();
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);

            System.Console.WriteLine(Context.ConnectionId + " disconnected");
            UserHandler.ConnectedIds.Remove(getCurrentUserId());
            await base.OnDisconnectedAsync(exception);
        }

        public async Task AddUserToSubscribedGroups()
        {
            string userId = getCurrentUserId();
            ICollection<CourseSubscription> subscribedCourses = await _courseSubscriptionRepository.getByUserId(userId);
            ICollection<QuestionSubscription> subscribedQuestions = await _questionSubscriptionRepository.getByUserId(userId);

            await Task.WhenAll(subscribedCourses.Select(sc => Groups.AddToGroupAsync(Context.ConnectionId, "Course " + sc.courseId.ToString())));
            await Task.WhenAll(subscribedQuestions.Select(sq => Groups.AddToGroupAsync(Context.ConnectionId, "Question " + sq.questionId.ToString())));
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

        public string getCurrentUserId()
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
