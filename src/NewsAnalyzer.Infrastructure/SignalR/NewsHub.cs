using Microsoft.AspNetCore.SignalR;

namespace NewsAnalyzer.Infrastructure.SignalR;

public class NewsHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, GroupName.NewsNotifications, Context.ConnectionAborted);
        await base.OnConnectedAsync();
    }
}