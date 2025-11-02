namespace NewsAnalyzer.Application.ValueObjects;

public sealed record NewsNotification(Guid NewsId, string TickerSymbol, string Headline, DateTime CreatedAt, decimal? ImpactPercentage, string? MarketTrend, string? ReasonForMarketTrend);