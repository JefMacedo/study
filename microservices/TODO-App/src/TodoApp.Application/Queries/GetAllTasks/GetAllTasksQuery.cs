using MediatR;
using TodoApp.Application.Common;
using TodoApp.Application.Queries.GetTask;

namespace TodoApp.Application.Queries.GetAllTasks;

public record GetAllTasksQuery(int PageNumber = 1, int PageSize = 20) : IRequest<PagedResult<TaskDto>>;
