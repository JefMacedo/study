using MediatR;

namespace TodoApp.Application.Commands.UpdateSubTask;

public record UpdateSubTaskCommand(Guid TaskId, Guid SubTaskId, string? Title, string? Description) : IRequest<Unit>;
