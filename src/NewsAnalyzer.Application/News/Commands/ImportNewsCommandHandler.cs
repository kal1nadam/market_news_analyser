using MediatR;
using NewsAnalyzer.Application.Common.Interfaces;
using NewsAnalyzer.Application.Common.Interfaces.Persistence;
using NewsAnalyzer.Application.Entities;

namespace NewsAnalyzer.Application.News.Commands;

public class ImportNewsCommandHandler : IRequestHandler<ImportNewsCommand, int>
{
    private readonly INewsProvider _newsProvider;
    private readonly INewsRepository _newsRepository;
    private readonly IOutboxStore _outboxStore;
    private readonly IUnitOfWork _unitOfWork;
    
    public ImportNewsCommandHandler(INewsProvider newsProvider, INewsRepository newsRepository, IOutboxStore outboxStore, IUnitOfWork unitOfWork)
    {
        _newsProvider = newsProvider;
        _newsRepository = newsRepository;
        _outboxStore = outboxStore;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<int> Handle(ImportNewsCommand request, CancellationToken ct)
    {
        var news = await _newsProvider.GetNewsAsync(ct);
        
        // Map DTOs to Entities and save to database
        var newsEntities = news.Select(Entities.News.Create).ToList();
        var insertedNewsIds = await _newsRepository.InsertAsync(newsEntities, ct);

        // Add outbox message records for the newly created news items
        await _outboxStore.AddNewsCreated(insertedNewsIds, ct);
        
        // Save all changes in a single transaction
        await _unitOfWork.SaveChangesAsync(ct);
        
        // Return how many news items were inserted
        return insertedNewsIds.Count;
    }
}