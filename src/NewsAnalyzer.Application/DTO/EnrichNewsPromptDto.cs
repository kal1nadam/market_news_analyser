namespace NewsAnalyzer.Application.DTO;

public sealed class EnrichNewsPromptDto
{
    public required string Headline { get; set; }
    
    public required string Summary { get; set; }
    
    public required string TickerSymbol { get; set; }
    
    public required string Url { get; set; }
}