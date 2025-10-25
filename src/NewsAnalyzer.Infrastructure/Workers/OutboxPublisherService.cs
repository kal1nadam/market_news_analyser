using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NewsAnalyzer.Application.Common.Interfaces;
using NewsAnalyzer.Application.Common.Interfaces.Persistence;
using NewsAnalyzer.Application.Events;

namespace NewsAnalyzer.Infrastructure.Workers;

public sealed class OutboxPublisherService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly string _workerId = Environment.MachineName + ":" + Guid.NewGuid().ToString("N");
    
    public OutboxPublisherService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var bus = scope.ServiceProvider.GetRequiredService<IEventBus>();
            var outboxStore = scope.ServiceProvider.GetRequiredService<IOutboxStore>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            
            // Get batch of unprocessed messages
            var batch = await outboxStore.DequeueAsync(10, ct);
            if (batch.Count == 0)
            {
                await Task.Delay(TimeSpan.FromSeconds(2), ct);
                continue;
            }
            
            // Mark as locked and save
            batch.ToList().ForEach(b => b.Lock(_workerId));
            await unitOfWork.SaveChangesAsync(ct);

            // Publish messages into the bus
            foreach (var msg in batch)
            {
                try
                {
                    switch (msg.Type)
                    {
                        case nameof(NewsCreated):
                        {
                            var evt = JsonSerializer.Deserialize<NewsCreated>(msg.Payload);
                            await bus.PublishAsync(evt, ct);
                            break;
                        }
                        case nameof(NewsAnalyzed):
                        {
                            var evt = JsonSerializer.Deserialize<NewsAnalyzed>(msg.Payload);
                            await bus.PublishAsync(evt, ct);
                            break;
                        }
                    }

                    // Mark as processed and clear error
                    msg.MarkProcessed();
                    msg.SetError(null);
                }
                catch (Exception ex)
                {
                    // If error occurs, increment attempts and set error message
                    msg.IncrementAttempts();
                    msg.SetError(ex.Message);
                }
            }
            
            await unitOfWork.SaveChangesAsync(ct);
            await Task.Delay(TimeSpan.FromSeconds(2), ct);
        }
    }
}