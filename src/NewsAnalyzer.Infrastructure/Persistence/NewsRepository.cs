using NewsAnalyzer.Application.Common.Interfaces;

namespace NewsAnalyzer.Infrastructure.Persistence;

public sealed class NewsRepository : INewsRepository
{
    private readonly AppDbContext _dbContext;
    
    public NewsRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    // TODO retrieve news from the database
}