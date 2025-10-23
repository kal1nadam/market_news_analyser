using MediatR;
using NewsAnalyzer.Application.Common.Interfaces.Persistence;
using NewsAnalyzer.Application.DTO.Responses;

namespace NewsAnalyzer.Application.News.Queries;

public sealed class GetNewsQueryHandler : IRequestHandler<GetNewsQuery, List<NewsDetailDto>>
{
    private readonly INewsRepository _newsRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    
    public GetNewsQueryHandler(INewsRepository newsRepository, IUnitOfWork unitOfWork)
    {
        _newsRepository = newsRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<List<NewsDetailDto>> Handle(GetNewsQuery request, CancellationToken cancellationToken)
    {
        var newsItems = await _newsRepository.GetAsync(request.Page, request.PageSize, cancellationToken);
        
        // TODO use automapper - news mapping profile
        // Map Entities to DTOs
        var mappedNews = newsItems.Select(n => new NewsDetailDto
        {
            Id = n.Id,
            PublishedAt = n.PublishedAt,
            Headline = n.Headline,
            Summary = n.Summary,
            Url = n.Url,
            CreatedAt = n.CreatedAt,
            Analyzed = n.Analyzed,
            ImpactPercentage = n.ImpactPercentage,
            MarketTrend = n.MarketTrend,
            ReasonForMarketTrend = n.ReasonForMarketTrend
        }).ToList();
        
        return mappedNews;
    }
}