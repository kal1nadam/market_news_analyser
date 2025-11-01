using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NewsAnalyzer.Application.Common.Interfaces;
using NewsAnalyzer.Application.Events;

namespace NewsAnalyzer.Infrastructure.Workers;

public sealed class NewsAnalyzerHostedService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IEventBus _bus;
    private readonly ILogger _logger;
    
    public NewsAnalyzerHostedService(IServiceProvider serviceProvider, IEventBus bus, ILogger<NewsAnalyzerHostedService> logger)
    {
        _serviceProvider = serviceProvider;
        _bus = bus;
        _logger = logger;
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
                    _logger.LogError(ex, "Error processing NewsCreated event for NewsId: {NewsId}", evt.NewsId);
                }
            }
        }
    }
}