using Microsoft.Extensions.Hosting;
using NewsAnalyzer.Application.Common.Interfaces;
using NewsAnalyzer.Application.Events;

namespace NewsAnalyzer.Infrastructure.Workers;

public sealed class NewsEnricherHostedService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IEventBus _bus;
    // TODO logging
    
    public NewsEnricherHostedService(IServiceProvider serviceProvider, IEventBus bus)
    {
        _serviceProvider = serviceProvider;
        _bus = bus;
    }
    
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            await foreach (var evt in _bus.SubscribeAsync<NewsCreated>(ct))
            {
                Console.WriteLine($"{nameof(NewsEnricherHostedService)} - Enrich START : newsId [{evt.NewsId}]");
            }
        }
    }
}