using MediatR;

namespace TodoApp.Application.Commands.UnarchiveTask;

public record UnarchiveTaskCommand(Guid TaskId) : IRequest<Unit>;
