using MediatR;

namespace TodoApp.Application.Commands.ReopenTask;

public record ReopenTaskCommand(Guid TaskId) : IRequest<Unit>;
