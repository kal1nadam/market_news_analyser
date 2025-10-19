using MediatR;
using NewsAnalyzer.Application.Common.Interfaces;

namespace NewsAnalyzer.Application.Health.Queries;

public sealed class GetHealthStatusQueryHandler : IRequestHandler<GetHealthStatusQuery, HealthStatusDto>
{
    private readonly IDateTimeProvider _clock;
    
    public GetHealthStatusQueryHandler(IDateTimeProvider clock)
    {
        _clock = clock;
    }
    
    public Task<HealthStatusDto> Handle(GetHealthStatusQuery request, CancellationToken cancellationToken)
    {
        var dto = new HealthStatusDto(
            Status: "OK",
            ServerUtc: _clock.UtcNow,
            ServiceName: "NewsAnalyzer"
        );
        
        return Task.FromResult(dto);
    }
}