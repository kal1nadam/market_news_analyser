using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NewsAnalyzer.Application.Common.Interfaces;
using NewsAnalyzer.Application.Events;

namespace NewsAnalyzer.Infrastructure.Workers;

public sealed class NewsAnalyzerHostedService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IEventBus _bus;
    // TODO logging
    
    public NewsAnalyzerHostedService(IServiceProvider serviceProvider, IEventBus bus)
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
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var processor = scope.ServiceProvider.GetRequiredService<INewsProcessor>();
                    await processor.EnrichNewsAsync(evt.NewsId, ct);
                    await processor.EvaluateAndNotifyAsync(evt.NewsId, ct);
                }
                catch (Exception ex)
                {
                    // TODO log error
                }
            }
        }
    }
}