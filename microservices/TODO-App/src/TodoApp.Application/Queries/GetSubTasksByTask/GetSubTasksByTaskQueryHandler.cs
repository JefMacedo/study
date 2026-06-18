using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Queries.GetTask;
using TodoApp.Application.Common;

namespace TodoApp.Application.Queries.GetSubTasksByTask;

public class GetSubTasksByTaskQueryHandler : IRequestHandler<GetSubTasksByTaskQuery, PagedResult<SubTaskDto>>
{
    private readonly IAppDbContext _context;

    public GetSubTasksByTaskQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<SubTaskDto>> Handle(GetSubTasksByTaskQuery request, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks
            .AsNoTracking()
            .Include(x => x.SubTasks)
            .FirstOrDefaultAsync(x => x.Id == request.TaskId, cancellationToken);

        if (task == null)
            return new PagedResult<SubTaskDto>(new List<SubTaskDto>(), 0, request.PageNumber, request.PageSize);

        var total = task.SubTasks.Count;
        var items = task.SubTasks
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(st => new SubTaskDto(st.Id, st.Title, st.Description, st.IsCompleted))
            .ToList();

        return new PagedResult<SubTaskDto>(items, total, request.PageNumber, request.PageSize);
    }
}
