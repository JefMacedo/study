using MediatR;

namespace TodoApp.Application.Commands.CreateTask;

public record CreateTaskCommand(
    string Title, 
    string? Description, 
    DateTime? DueDate,
    IEnumerable<CreateSubTaskDto>? SubTasks = null) : IRequest<CreateTaskResponse>;
