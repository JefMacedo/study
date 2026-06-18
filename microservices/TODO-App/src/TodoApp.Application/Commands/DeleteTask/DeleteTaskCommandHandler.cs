using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Interfaces;

namespace TodoApp.Application.Commands.DeleteTask;

public class DeleteTaskCommandHandler(IAppDbContext context) : IRequestHandler<DeleteTaskCommand, Unit>
{
    private readonly IAppDbContext _context = context;

    public async Task<Unit> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == request.TaskId, cancellationToken)
            ?? throw new InvalidOperationException($"Task with ID {request.TaskId} not found.");

        if (task.IsDeleted)
            throw new InvalidOperationException("Task is already deleted.");

        task.IsDeleted = true;
        task.EditedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
