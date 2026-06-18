using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Interfaces;

namespace TodoApp.Application.Queries.GetTask;

public class GetTaskQueryHandler : IRequestHandler<GetTaskQuery, TaskDto?>
{
    private readonly IAppDbContext _context;

    public GetTaskQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<TaskDto?> Handle(GetTaskQuery request, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks
            .AsNoTracking()
            .Include(x => x.SubTasks)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (task == null)
            return null;

        return new TaskDto(
            task.Id,
            task.Title,
            task.Description,
            task.CreatedAt,
            task.DueDate,
            task.IsCompleted,
            [.. task.SubTasks.Select(st => new SubTaskDto(
                st.Id,
                st.Title,
                st.Description,
                st.IsCompleted))]);
    }
}
