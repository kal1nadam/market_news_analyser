using Microsoft.EntityFrameworkCore;
using NewsAnalyzer.Application.Entities;

namespace NewsAnalyzer.Application.Common.Interfaces;

public interface IAppDbContext
{
    DbSet<IngestionLog> IngestionLogs { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}