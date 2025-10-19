using MediatR;
using Microsoft.AspNetCore.Mvc;
using NewsAnalyzer.Application.Health.Queries;

namespace NewsAnalyzer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController
{
    private readonly IMediator _mediator;
    
    public HealthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(HealthStatusDto), StatusCodes.Status200OK)]
    public async Task<IResult> Get()
    {
        var result = await _mediator.Send(new GetHealthStatusQuery());
        return Results.Ok(result);
    }
}