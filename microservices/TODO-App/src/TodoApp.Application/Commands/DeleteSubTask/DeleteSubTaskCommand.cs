using MediatR;

namespace TodoApp.Application.Commands.DeleteSubTask;

public record DeleteSubTaskCommand(Guid TaskId, Guid SubTaskId) : IRequest<Unit>;
