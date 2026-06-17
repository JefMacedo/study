using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Commands.CreateTask;

namespace TodoApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;

    public TasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { id = id }, new { id = id });
    }

    [HttpGet("{id}")]
    public IActionResult Get(Guid id)
    {
        return NotFound();
    }
}
