using MediatR;

namespace TodoApp.Application.Commands.CompleteTask;

public record CompleteTaskCommand(Guid TaskId) : IRequest<Unit>;
