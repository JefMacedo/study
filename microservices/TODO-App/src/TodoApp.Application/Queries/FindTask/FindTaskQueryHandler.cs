using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Queries.GetTask;

namespace TodoApp.Application.Queries.FindTask;

public class FindTaskQueryHandler(IAppDbContext context) : IRequestHandler<FindTaskQuery, TaskDto?>
{
    private readonly IAppDbContext _context = context;

    public async Task<TaskDto?> Handle(FindTaskQuery request, CancellationToken cancellationToken)
    {
        if (request.Id.HasValue)
        {
            var t = await _context.Tasks
                .AsNoTracking()
                .Include(x => x.SubTasks)
                .FirstOrDefaultAsync(x => x.Id == request.Id.Value, cancellationToken);

            if (t == null) return null;

            return new TaskDto(t.Id, t.Title, t.Description, t.CreatedAt, t.DueDate, t.IsCompleted,
                t.SubTasks.Select(st => new SubTaskDto(st.Id, st.Title, st.Description, st.IsCompleted)).ToList());
        }

        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            var t = await _context.Tasks
                .AsNoTracking()
                .Include(x => x.SubTasks)
                .Where(x => x.Title.Equals(request.Title, StringComparison.CurrentCultureIgnoreCase))
                .FirstOrDefaultAsync(cancellationToken);

            if (t == null) return null;

            return new TaskDto(t.Id, t.Title, t.Description, t.CreatedAt, t.DueDate, t.IsCompleted,
                t.SubTasks.Select(st => new SubTaskDto(st.Id, st.Title, st.Description, st.IsCompleted)).ToList());
        }

        return null;
    }
}
