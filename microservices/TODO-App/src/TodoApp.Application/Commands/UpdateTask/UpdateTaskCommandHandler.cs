using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Interfaces;

namespace TodoApp.Application.Commands.UpdateTask;

public class UpdateTaskCommandHandler(IAppDbContext context) : IRequestHandler<UpdateTaskCommand, Unit>
{
    private readonly IAppDbContext _context = context;

    public async Task<Unit> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == request.TaskId, cancellationToken)
            ?? throw new InvalidOperationException($"Task with ID {request.TaskId} not found.");

        if (task.IsDeleted)
            throw new InvalidOperationException("Deleted tasks cannot be edited.");

        if (task.IsDeleted)
            throw new InvalidOperationException("Deleted tasks cannot be edited.");

        if (request.Title is not null)
        {
            task.Title = request.Title;
        }

        if (request.Description is not null)
        {
            task.Description = request.Description;
        }

        if (task.IsArchived)
        {
            task.IsArchived = false;
        }

        task.EditedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
