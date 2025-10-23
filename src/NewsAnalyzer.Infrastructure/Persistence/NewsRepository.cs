using Microsoft.EntityFrameworkCore;
using NewsAnalyzer.Application.Common.Interfaces;
using NewsAnalyzer.Application.Common.Interfaces.Persistence;
using NewsAnalyzer.Application.Entities;

namespace NewsAnalyzer.Infrastructure.Persistence;

public sealed class NewsRepository : INewsRepository
{
    private readonly AppDbContext _dbContext;
    
    public NewsRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IReadOnlyList<Guid>> InsertAsync(IReadOnlyList<News> items, CancellationToken ct)
    {
        await _dbContext.News.AddRangeAsync(items, ct);
        return items.Select(i => i.Id).ToList();
    }

    public async Task<IReadOnlyList<News>> GetAsync(int page, int pageSize, CancellationToken ct)
    {
        return await _dbContext.News
            .AsNoTracking()
            .OrderByDescending(n => n.PublishedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }
}