using NewsAnalyzer.Application.DTO;
using NewsAnalyzer.Application.DTO.External;

namespace NewsAnalyzer.Application.Entities;

/// <summary>
/// Domain entity.
/// Database table: News
/// </summary>
public sealed class News
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public DateTime PublishedAt { get; set; }

    public string Headline { get; set; } = null!;
    
    public string Summary { get; set; } = null!;
    
    public string Url { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public bool Analyzed { get; set; } = false;
    
    // News analyzing
    public decimal? ImpactPercentage { get; set; }
    
    public string? MarketTrend { get; set; }
    
    public string? ReasonForMarketTrend { get; set; }

    // TODO use automapper - news mapping profile
    public static News Create(ImportNewsDto dto) => new()
    {
        PublishedAt = dto.PublishedDate,
        Headline = dto.Title,
        Summary = dto.Text,
        Url = dto.Url
    };
    
    public void MarkAnalyzed() => Analyzed = true;
}