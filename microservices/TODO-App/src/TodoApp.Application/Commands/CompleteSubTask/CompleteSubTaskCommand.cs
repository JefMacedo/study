using MediatR;

namespace TodoApp.Application.Commands.CompleteSubTask;

public record CompleteSubTaskCommand(Guid TaskId, Guid SubTaskId) : IRequest<Unit>;
