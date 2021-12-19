using System;
using Microsoft.AspNetCore.SignalR;

public class UserIdProvider : IUserIdProvider
{
    public string GetUserId(HubConnectionContext connection)
    {
        System.Console.WriteLine(connection.User);
        return connection.User?.Identity?.Name;
    }
}
