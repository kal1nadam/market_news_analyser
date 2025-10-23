using MediatR;
using NewsAnalyzer.Application.Common.Interfaces;

namespace NewsAnalyzer.Application.News.Commands;

public class ImportNewsCommandHandler : IRequestHandler<ImportNewsCommand, int>
{
    private readonly INewsProvider _newsProvider;
    private readonly IAppDbContext _dbContext;
    private readonly IOutboxStore _outboxStore;
    
    public ImportNewsCommandHandler(INewsProvider newsProvider, IAppDbContext dbContext, IOutboxStore outboxStore)
    {
        _newsProvider = newsProvider;
        _dbContext = dbContext;
        _outboxStore = outboxStore;
    }
    
    public Task<int> Handle(ImportNewsCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}