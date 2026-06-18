using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Interfaces;

namespace TodoApp.Application.Commands.UnarchiveTask;

public class UnarchiveTaskCommandHandler(IAppDbContext context) : IRequestHandler<UnarchiveTaskCommand, Unit>
{
    private readonly IAppDbContext _context = context;

    public async Task<Unit> Handle(UnarchiveTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == request.TaskId, cancellationToken)
            ?? throw new InvalidOperationException($"Task with ID {request.TaskId} not found.");

        if (task.IsDeleted)
            throw new InvalidOperationException("Deleted tasks cannot be unarchived.");

        task.IsArchived = false;
        task.EditedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
