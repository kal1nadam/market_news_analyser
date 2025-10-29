namespace NewsAnalyzer.Application.Common.Interfaces;

public interface INewsProcessor
{
    Task EnrichNewsAsync(Guid newsId, CancellationToken ct = default);
    
    Task EvaluateAndNotifyAsync(Guid newsId, CancellationToken ct = default);
}