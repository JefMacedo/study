using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Queries.GetTask;

namespace TodoApp.Application.Queries.FindSubTask;

public class FindSubTaskQueryHandler(IAppDbContext context) : IRequestHandler<FindSubTaskQuery, SubTaskDto?>
{
    private readonly IAppDbContext _context = context;

    public async Task<SubTaskDto?> Handle(FindSubTaskQuery request, CancellationToken cancellationToken)
    {
        if (request.Id.HasValue)
        {
            var st = await _context.SubTasks.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id.Value, cancellationToken);
            if (st == null) return null;
            return new SubTaskDto(st.Id, st.Title, st.Description, st.IsCompleted);
        }

        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            var st = await _context.SubTasks.AsNoTracking().Where(x => x.Title.ToLower() == request.Title.ToLower()).FirstOrDefaultAsync(cancellationToken);
            if (st == null) return null;
            return new SubTaskDto(st.Id, st.Title, st.Description, st.IsCompleted);
        }

        return null;
    }
}
