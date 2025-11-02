using Microsoft.AspNetCore.SignalR;
using NewsAnalyzer.Application.Common.Interfaces;
using NewsAnalyzer.Application.ValueObjects;

namespace NewsAnalyzer.Infrastructure.SignalR;

public sealed class NewsNotifier : INewsNotifier
{
    private readonly IHubContext<NewsHub> _hubContext;
    
    public NewsNotifier(IHubContext<NewsHub> hubContext)
    {
        _hubContext = hubContext;
    }
    
    public Task BroadcastNewsAsync(NewsNotification notification)
    {
        return _hubContext.Clients.All.SendAsync(GroupName.NewsNotifications, notification);
    }
}