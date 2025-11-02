using NewsAnalyzer.Application.ValueObjects;

namespace NewsAnalyzer.Application.Common.Interfaces;

public interface INewsNotifier
{
    Task BroadcastNewsAsync(NewsNotification notification);
}