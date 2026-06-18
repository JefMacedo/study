using MediatR;

namespace TodoApp.Application.Queries.GetTask;

public record GetTaskQuery(Guid Id) : IRequest<TaskDto?>;
