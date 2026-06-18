using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Commands.AddSubTask;
using TodoApp.Application.Commands.CompleteSubTask;
using TodoApp.Application.Commands.CreateTask;
using TodoApp.Application.Queries.GetTask;

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
        var response = await _mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
    }

    [HttpPost("{taskId}/subtasks")]
    public async Task<IActionResult> AddSubTask([FromBody] AddSubTaskCommand command)
    {
        var response = await _mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { id = response.TaskId }, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var query = new GetTaskQuery(id);
        var result = await _mediator.Send(query);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPut("{taskId}/subtasks/{subTaskId}/complete")]
    public async Task<IActionResult> CompleteSubTask(Guid taskId, Guid subTaskId)
    {
        var command = new CompleteSubTaskCommand(taskId, subTaskId);
        await _mediator.Send(command);
        return NoContent();
    }
}
