using NewsAnalyzer.Application.Common.Interfaces;

namespace NewsAnalyzer.Infrastructure.Common;

public sealed class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}