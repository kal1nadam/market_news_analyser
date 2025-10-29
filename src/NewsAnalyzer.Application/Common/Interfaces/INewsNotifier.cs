namespace NewsAnalyzer.Application.Common.Interfaces;

public interface INewsNotifier
{
    Task BroadcastNewsAsync(Guid newsId); // TODO use record with more info
}