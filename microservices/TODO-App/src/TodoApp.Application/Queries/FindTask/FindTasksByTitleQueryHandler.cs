using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Common;
using TodoApp.Application.Queries.GetTask;

namespace TodoApp.Application.Queries.FindTask;

public class FindTasksByTitleQueryHandler(IAppDbContext context) : IRequestHandler<FindTasksByTitleQuery, PagedResult<TaskDto>>
{
    private readonly IAppDbContext _context = context;

    public async Task<PagedResult<TaskDto>> Handle(FindTasksByTitleQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Tasks.AsNoTracking()
            .Include(x => x.SubTasks)
            .Where(x => x.Title
            .Contains(request.Title, StringComparison.CurrentCultureIgnoreCase));
        var total = await query.CountAsync(cancellationToken);
        var tasks = await query.OrderBy(t => t.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var items = tasks.Select(t => new TaskDto(t.Id, t.Title, t.Description, t.CreatedAt, t.DueDate, t.IsCompleted,
            t.SubTasks.Select(st => new SubTaskDto(st.Id, st.Title, st.Description, st.IsCompleted)).ToList()));

        return new PagedResult<TaskDto>(items, total, request.PageNumber, request.PageSize);
    }
}
