using Microsoft.AspNetCore.SignalR;
using NewsAnalyzer.Application.Common.Interfaces;

namespace NewsAnalyzer.Infrastructure.SignalR;

public sealed class NewsNotifier : INewsNotifier
{
    private readonly IHubContext<NewsHub> _hubContext;
    
    public NewsNotifier(IHubContext<NewsHub> hubContext)
    {
        _hubContext = hubContext;
    }
    
    public Task BroadcastNewsAsync(Guid newsId)
    {
        // TODO use record with more info
        return _hubContext.Clients.All.SendAsync("ReceiveNewsUpdate", newsId);
    }
}