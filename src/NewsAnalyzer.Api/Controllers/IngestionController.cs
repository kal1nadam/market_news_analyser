using MediatR;
using Microsoft.AspNetCore.Mvc;
using NewsAnalyzer.Application.Ingestion.Commands;

namespace NewsAnalyzer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngestionController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public IngestionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<int>> Log(LogIngestionCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(Log), new { id }, id);
    }
}