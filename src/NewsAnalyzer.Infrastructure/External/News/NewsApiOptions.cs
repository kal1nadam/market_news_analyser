namespace NewsAnalyzer.Infrastructure.External.News;

public sealed class NewsApiOptions
{
    public string BaseUrl { get; set; } = default!;
    public string ApiKey { get; set; } = default!;
}