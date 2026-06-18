using MediatR;

namespace TodoApp.Application.Commands.ReopenSubTask;

public record ReopenSubTaskCommand(Guid TaskId, Guid SubTaskId) : IRequest<Unit>;
