using MediatR;
using TodoApp.Application.Common;
using TodoApp.Application.Queries.GetTask;

namespace TodoApp.Application.Queries.GetSubTasksByTask;

public record GetSubTasksByTaskQuery(Guid TaskId, int PageNumber = 1, int PageSize = 20) : IRequest<PagedResult<SubTaskDto>>;
