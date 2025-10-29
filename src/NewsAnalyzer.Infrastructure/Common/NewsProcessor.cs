using NewsAnalyzer.Application.Common.Interfaces;
using NewsAnalyzer.Application.Common.Interfaces.Persistence;

namespace NewsAnalyzer.Infrastructure.Common;

public sealed class NewsProcessor : INewsProcessor
{
    private readonly INewsRepository _newsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INewsNotifier _newsNotifier;
    
    public NewsProcessor(INewsRepository newsRepository, IUnitOfWork unitOfWork, INewsNotifier newsNotifier)
    {
        _newsRepository = newsRepository;
        _unitOfWork = unitOfWork;
        _newsNotifier = newsNotifier;
    }
    
    public async Task EnrichNewsAsync(Guid newsId, CancellationToken ct = default)
    {
        var news = await _newsRepository.GetByIdAsync(newsId, ct);
        
        // TODO call openAI api to analyze
        if (news is not null)
        {
            news.ImpactPercentage = new Random().Next(1, 101);
            news.MarketTrend = "Up";
            news.ReasonForMarketTrend = "Positive news impact";
            
            await _unitOfWork.SaveChangesAsync(ct);
        }
        
    }

    public async Task EvaluateAndNotifyAsync(Guid newsId, CancellationToken ct = default)
    {
        var news = await _newsRepository.GetByIdAsync(newsId, ct);
        
        // Notify if impact percentage is greater than 70%
        if (news?.ImpactPercentage is > 70)
        {
            await _newsNotifier.BroadcastNewsAsync(newsId);
        }
    }
}