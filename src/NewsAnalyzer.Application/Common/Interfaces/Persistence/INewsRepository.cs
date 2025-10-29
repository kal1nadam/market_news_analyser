namespace NewsAnalyzer.Application.Common.Interfaces.Persistence;

/// <summary>
/// Repository for managing news items.
/// </summary>
public interface INewsRepository
{
    Task<IReadOnlyList<Guid>> InsertAsync(IReadOnlyList<Entities.News> items, CancellationToken ct);
    Task<Entities.News?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<Entities.News>> GetAsync(int page, int pageSize, CancellationToken ct);
}