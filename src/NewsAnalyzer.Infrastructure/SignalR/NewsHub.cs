using Microsoft.AspNetCore.SignalR;

namespace NewsAnalyzer.Infrastructure.SignalR;

public class NewsHub : Hub
{
    public override Task OnConnectedAsync()
    {
        Console.WriteLine($"Client connected: {Context.ConnectionId}");
        return base.OnConnectedAsync();
    }
}