using NewsAnalyzer.Application.Common.Interfaces;
using NewsAnalyzer.Application.Common.Interfaces.Persistence;
using NewsAnalyzer.Application.DTO;
using NewsAnalyzer.Application.ValueObjects;

namespace NewsAnalyzer.Application.Common.Services;

public sealed class NewsProcessor : INewsProcessor
{
    private readonly INewsRepository _newsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INewsNotifier _newsNotifier;
    private readonly IAiAnalyzer _aiAnalyzer;
    
    public NewsProcessor(INewsRepository newsRepository, IUnitOfWork unitOfWork, INewsNotifier newsNotifier, IAiAnalyzer aiAnalyzer)
    {
        _newsRepository = newsRepository;
        _unitOfWork = unitOfWork;
        _newsNotifier = newsNotifier;
        _aiAnalyzer = aiAnalyzer;
    }
    
    public async Task EnrichNewsAsync(Guid newsId, CancellationToken ct = default)
    {
        var news = await _newsRepository.GetByIdAsync(newsId, ct);
        
        if (news is not null)
        {
            EnrichNewsPromptDto param = new()
            {
                Headline = news.Headline,
                Summary = news.Summary,
                TickerSymbol = news.TickerSymbol,
                Url = news.Url,
            };
            
            // Call AI analyzer - using OpenAI to analyze
            var analysis =  await _aiAnalyzer.AnalyzeNewsAsync(param, ct);
            
            if (analysis is not null && analysis.ImpactPercent > 0)
            {
                news.ImpactPercentage = (decimal)analysis.ImpactPercent / 100; // Convert into percentage
                news.MarketTrend = analysis.MarketTrend;
                news.ReasonForMarketTrend = analysis.ReasonForMarketTrend;
            }
            
            // Update news as analyzed
            news.MarkAnalyzed();
            await _unitOfWork.SaveChangesAsync(ct);
        }
        
    }

    public async Task EvaluateAndNotifyAsync(Guid newsId, CancellationToken ct = default)
    {
        var news = await _newsRepository.GetByIdAsync(newsId, ct);
        
        // Notify if impact percentage is greater than 70%
        if (news?.ImpactPercentage is > 70)
        {
            var notification = new NewsNotification(news.Id, news.TickerSymbol, news.Headline, news.CreatedAt, news.ImpactPercentage, news.MarketTrend, news.ReasonForMarketTrend);
            await _newsNotifier.BroadcastNewsAsync(notification);
        }
    }
}