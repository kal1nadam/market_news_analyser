using MediatR;
using Microsoft.AspNetCore.Mvc;
using NewsAnalyzer.Application.Ingestion.Commands;
using NewsAnalyzer.Application.News.Commands;
using NewsAnalyzer.Application.News.Queries;

namespace NewsAnalyzer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public NewsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<int>> Import()
    {
        var newsAdded = await _mediator.Send(new ImportNewsCommand());
        
        return Ok(newsAdded);
    }
    
    [HttpGet]
    public async Task<ActionResult> Get(int page = 1, int pageSize = 10)
    {
        var news = await _mediator.Send(new GetNewsQuery(page, pageSize));
        
        return Ok(news);
    }
}