using Microsoft.Extensions.Hosting;

namespace NewsAnalyzer.Infrastructure.Workers;

public sealed class OutboxPublisherService : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }
}