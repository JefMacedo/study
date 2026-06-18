using MediatR;
using TodoApp.Application.Common;
using TodoApp.Application.Queries.GetTask;

namespace TodoApp.Application.Queries.FindSubTask;

public record FindSubTasksByTitleQuery(string Title, int PageNumber = 1, int PageSize = 20) : IRequest<PagedResult<SubTaskDto>>;
