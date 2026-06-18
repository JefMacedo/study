using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Queries.GetTask;
using TodoApp.Application.Common;

namespace TodoApp.Application.Queries.GetAllTasks;

public class GetAllTasksQueryHandler(IAppDbContext context) : IRequestHandler<GetAllTasksQuery, PagedResult<TaskDto>>
{
    private readonly IAppDbContext _context = context;

    public async Task<PagedResult<TaskDto>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
    {
        var total = await _context.Tasks.CountAsync(cancellationToken);

        var tasks = await _context.Tasks
            .AsNoTracking()
            .Include(t => t.SubTasks)
            .OrderBy(t => t.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var items = tasks.Select(t => new TaskDto(
            t.Id,
            t.Title,
            t.Description,
            t.CreatedAt,
            t.DueDate,
            t.IsCompleted,
            t.SubTasks.Select(st => new SubTaskDto(st.Id, st.Title, st.Description, st.IsCompleted)).ToList()
        ));

        return new PagedResult<TaskDto>(items, total, request.PageNumber, request.PageSize);
    }
}
