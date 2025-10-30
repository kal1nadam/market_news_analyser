using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NewsAnalyzer.Application.DTO.External;

public sealed class NewsAnalysisDto
{
    // Short, push-worthy line (<= 80 chars)
    [JsonPropertyName("hook")]
    public required string Hook { get; init; }

    // 0–100, be STRICT: only high values for truly breaking items
    [JsonPropertyName("impact_percent")]
    [Range(0,100)]
    public required int ImpactPercent { get; init; }
    
    [JsonPropertyName("market_trend")]
    public string? MarketTrend { get; init; }
    
    [JsonPropertyName("reason_for_market_trend")]
    public string? ReasonForMarketTrend { get; init; }
}