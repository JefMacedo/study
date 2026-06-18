using MediatR;

namespace TodoApp.Application.Commands.DeleteTask;

public record DeleteTaskCommand(Guid TaskId) : IRequest<Unit>;
