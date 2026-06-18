using MediatR;
using TodoApp.Application.Queries.GetTask;

namespace TodoApp.Application.Queries.FindSubTask;

public record FindSubTaskQuery(Guid? Id, string? Title) : IRequest<SubTaskDto?>;
