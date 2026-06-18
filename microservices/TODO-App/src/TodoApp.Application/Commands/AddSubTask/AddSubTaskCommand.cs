using MediatR;

namespace TodoApp.Application.Commands.AddSubTask;

public record AddSubTaskCommand(Guid TaskId, string Title, string? Description) : IRequest<AddSubTaskResponse>;
