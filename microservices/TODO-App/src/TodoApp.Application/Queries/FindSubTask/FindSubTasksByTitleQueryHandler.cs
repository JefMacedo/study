using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Common;
using TodoApp.Application.Queries.GetTask;

namespace TodoApp.Application.Queries.FindSubTask;

public class FindSubTasksByTitleQueryHandler(IAppDbContext context) : IRequestHandler<FindSubTasksByTitleQuery, PagedResult<SubTaskDto>>
{
    private readonly IAppDbContext _context = context;

    public async Task<PagedResult<SubTaskDto>> Handle(FindSubTasksByTitleQuery request, CancellationToken cancellationToken)
    {
        var query = _context.SubTasks.AsNoTracking().Where(x => x.Title.ToLower().Contains(request.Title.ToLower()));
        var total = await query.CountAsync(cancellationToken);
        var items = await query.OrderBy(x => x.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(st => new SubTaskDto(st.Id, st.Title, st.Description, st.IsCompleted))
            .ToListAsync(cancellationToken);

        return new PagedResult<SubTaskDto>(items, total, request.PageNumber, request.PageSize);
    }
}
