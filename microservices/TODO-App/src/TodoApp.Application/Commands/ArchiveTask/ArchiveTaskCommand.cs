using MediatR;

namespace TodoApp.Application.Commands.ArchiveTask;

public record ArchiveTaskCommand(Guid TaskId) : IRequest<Unit>;
