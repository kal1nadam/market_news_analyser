using NewsAnalyzer.Application.Common.Interfaces.Persistence;

namespace NewsAnalyzer.Infrastructure.Persistence;

/// <summary>
/// Unit of Work implementation for managing database transactions.
/// </summary>
public sealed class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;
    
    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public Task<int> SaveChangesAsync(CancellationToken ct) => _dbContext.SaveChangesAsync(ct);
}