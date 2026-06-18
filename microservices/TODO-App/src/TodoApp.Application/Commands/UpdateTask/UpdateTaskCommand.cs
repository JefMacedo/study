using MediatR;

namespace TodoApp.Application.Commands.UpdateTask;

public record UpdateTaskCommand(Guid TaskId, string? Title, string? Description) : IRequest<Unit>;
