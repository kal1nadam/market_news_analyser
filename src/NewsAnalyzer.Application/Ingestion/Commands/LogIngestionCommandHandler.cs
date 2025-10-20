using MediatR;
using NewsAnalyzer.Application.Common.Interfaces;
using NewsAnalyzer.Application.Entities;

namespace NewsAnalyzer.Application.Ingestion.Commands;

public sealed class LogIngestionCommandHandler : IRequestHandler<LogIngestionCommand, int>
{
    private readonly IAppDbContext _dbContext;
    private readonly IDateTimeProvider _clock;
    
    public LogIngestionCommandHandler(IAppDbContext dbContext, IDateTimeProvider clock)
    {
        _dbContext = dbContext;
        _clock = clock;
    }
    
    public async Task<int> Handle(LogIngestionCommand request, CancellationToken cancellationToken)
    {
        var entity = new IngestionLog
        {
            Source = request.Source,
            Message = request.Message,
            CreatedUtc = _clock.UtcNow
        };

        _dbContext.IngestionLogs.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}