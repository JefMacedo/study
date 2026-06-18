using MediatR;
using TodoApp.Application.Queries.GetTask;

namespace TodoApp.Application.Queries.FindTask;

public record FindTaskQuery(Guid? Id, string? Title) : IRequest<TaskDto?>;
