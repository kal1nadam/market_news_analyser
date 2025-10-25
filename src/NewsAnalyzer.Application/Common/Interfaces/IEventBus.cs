namespace NewsAnalyzer.Application.Common.Interfaces;

public interface IEventBus
{
    Task PublishAsync<T>(T message, CancellationToken ct = default);
    IAsyncEnumerable<T> SubscribeAsync<T>(CancellationToken ct = default);
}