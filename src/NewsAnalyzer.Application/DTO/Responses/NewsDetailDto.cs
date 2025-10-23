namespace NewsAnalyzer.Application.DTO.Responses;

public class NewsDetailDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public DateTime PublishedAt { get; set; }
    
    public string Headline { get; set; } = null!;
    
    public string Summary { get; set; } = null!;
    
    public string Url { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public bool Analyzed { get; set; } = false;
    
    public decimal? ImpactPercentage { get; set; }
    
    public string? MarketTrend { get; set; }
    
    public string? ReasonForMarketTrend { get; set; }
}