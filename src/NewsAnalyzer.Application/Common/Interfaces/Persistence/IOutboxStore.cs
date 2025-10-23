using NewsAnalyzer.Application.DTO;
using NewsAnalyzer.Application.Entities;

namespace NewsAnalyzer.Application.Common.Interfaces.Persistence;

public interface IOutboxStore
{
    Task AddNewsCreated(IReadOnlyList<Guid> newsIds, CancellationToken ct);
    Task<IReadOnlyList<OutboxMessage>> DequeueAsync(int batchSize, CancellationToken ct);
}