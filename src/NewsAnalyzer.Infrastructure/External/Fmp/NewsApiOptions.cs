namespace NewsAnalyzer.Infrastructure.External.Fmp;

public sealed class NewsApiOptions
{
    public string BaseUrl { get; set; } = default!;
    public string ApiKey { get; set; } = default!;
}