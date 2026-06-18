using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Commands.AddSubTask;
using TodoApp.Application.Commands.CompleteSubTask;
using TodoApp.Application.Commands.CompleteTask;
using TodoApp.Application.Commands.CreateTask;
using TodoApp.Application.Commands.ReopenTask;
using TodoApp.Application.Commands.ReopenSubTask;
using TodoApp.Application.Commands.UpdateTask;
using TodoApp.Application.Commands.DeleteTask;
using TodoApp.Application.Commands.ArchiveTask;
using TodoApp.Application.Commands.UnarchiveTask;
using TodoApp.Application.Commands.UpdateSubTask;
using TodoApp.Application.Commands.DeleteSubTask;
using TodoApp.Application.Queries.GetTask;
using TodoApp.Application.Queries.GetAllTasks;
using TodoApp.Application.Queries.FindTask;
using TodoApp.Application.Queries.GetSubTasksByTask;
using TodoApp.Application.Queries.FindSubTask;

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

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        var list = await _mediator.Send(new GetAllTasksQuery(pageNumber, pageSize));
        return Ok(list);
    }

    [HttpGet("search")]
    public async Task<IActionResult> FindTask([FromQuery] Guid? id, [FromQuery] string? title, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        if (!id.HasValue && string.IsNullOrWhiteSpace(title))
            return BadRequest("Provide either 'id' or 'title' as query parameter.");

        if (id.HasValue)
        {
            var result = await _mediator.Send(new FindTaskQuery(id, null));
            if (result == null) return NotFound();
            return Ok(result);
        }

        var paged = await _mediator.Send(new FindTasksByTitleQuery(title!, pageNumber, pageSize));
        return Ok(paged);
    }

    [HttpGet("{taskId}/subtasks")]
    public async Task<IActionResult> GetSubTasks(Guid taskId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        var list = await _mediator.Send(new GetSubTasksByTaskQuery(taskId, pageNumber, pageSize));
        return Ok(list);
    }

    [HttpGet("subtasks/search")]
    public async Task<IActionResult> FindSubTask([FromQuery] Guid? id, [FromQuery] string? title, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        if (!id.HasValue && string.IsNullOrWhiteSpace(title))
            return BadRequest("Provide either 'id' or 'title' as query parameter.");

        if (id.HasValue)
        {
            var result = await _mediator.Send(new FindSubTaskQuery(id, null));
            if (result == null) return NotFound();
            return Ok(result);
        }

        var paged = await _mediator.Send(new FindSubTasksByTitleQuery(title!, pageNumber, pageSize));
        return Ok(paged);
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

    [HttpPut("{taskId}/complete")]
    public async Task<IActionResult> CompleteTask(Guid taskId)
    {
        var command = new CompleteTaskCommand(taskId);
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{taskId}/reopen")]
    public async Task<IActionResult> ReopenTask(Guid taskId)
    {
        var command = new ReopenTaskCommand(taskId);
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{taskId}/subtasks/{subTaskId}/reopen")]
    public async Task<IActionResult> ReopenSubTask(Guid taskId, Guid subTaskId)
    {
        var command = new ReopenSubTaskCommand(taskId, subTaskId);
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{taskId}")]
    public async Task<IActionResult> UpdateTask(Guid taskId, [FromBody] UpdateTaskCommand command)
    {
        if (taskId != command.TaskId) return BadRequest("Mismatched task id.");
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{taskId}")]
    public async Task<IActionResult> DeleteTask(Guid taskId)
    {
        var command = new DeleteTaskCommand(taskId);
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{taskId}/archive")]
    public async Task<IActionResult> ArchiveTask(Guid taskId)
    {
        var command = new ArchiveTaskCommand(taskId);
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{taskId}/unarchive")]
    public async Task<IActionResult> UnarchiveTask(Guid taskId)
    {
        var command = new UnarchiveTaskCommand(taskId);
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{taskId}/subtasks/{subTaskId}")]
    public async Task<IActionResult> UpdateSubTask(Guid taskId, Guid subTaskId, [FromBody] UpdateSubTaskCommand command)
    {
        if (taskId != command.TaskId || subTaskId != command.SubTaskId) return BadRequest("Mismatched ids.");
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{taskId}/subtasks/{subTaskId}")]
    public async Task<IActionResult> DeleteSubTask(Guid taskId, Guid subTaskId)
    {
        var command = new DeleteSubTaskCommand(taskId, subTaskId);
        await _mediator.Send(command);
        return NoContent();
    }
}
