using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using NewsAnalyzer.Application.Common.Interfaces.Persistence;
using NewsAnalyzer.Application.Entities;
using NewsAnalyzer.Application.Events;

namespace NewsAnalyzer.Infrastructure.Persistence;

/// <summary>
/// Implementation of the outbox store for managing outbox messages.
/// Used as outbox message repository.
/// </summary>
public sealed class OutboxStore : IOutboxStore
{
    private readonly AppDbContext _dbContext;
    
    public OutboxStore(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task AddNewsCreated(IReadOnlyList<Guid> newsIds, CancellationToken ct)
    {
        List<OutboxMessage> outboxMessages = [];
        
        foreach(var newsId in newsIds)
        {
            var newsCreated = new NewsCreated(newsId);
            OutboxMessage outboxMessage = new()
            {
                Type = nameof(NewsCreated),
                Payload = JsonSerializer.Serialize(newsCreated)
            };
            
            outboxMessages.Add(outboxMessage);
        }
        
        await _dbContext.OutboxMessages.AddRangeAsync(outboxMessages, ct);
    }

    public async Task<IReadOnlyList<OutboxMessage>> DequeueAsync(int batchSize, CancellationToken ct)
    {
        return await _dbContext.OutboxMessages
            .Where(m => m.ProcessedAt == null)
            .OrderBy(m => m.OccurredAt)
            .Take(batchSize)
            .ToListAsync(ct);
    }
}