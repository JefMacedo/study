using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Interfaces;

namespace TodoApp.Application.Commands.UpdateSubTask;

public class UpdateSubTaskCommandHandler(IAppDbContext context) : IRequestHandler<UpdateSubTaskCommand, Unit>
{
    private readonly IAppDbContext _context = context;

    public async Task<Unit> Handle(UpdateSubTaskCommand request, CancellationToken cancellationToken)
    {
        var subTask = await _context.SubTasks
            .FirstOrDefaultAsync(x => x.Id == request.SubTaskId && x.TaskId == request.TaskId, cancellationToken)
            ?? throw new InvalidOperationException($"SubTask with ID {request.SubTaskId} not found for task {request.TaskId}.");

        if (subTask.IsDeleted)
            throw new InvalidOperationException("Deleted subtasks cannot be edited.");

        if (request.Title is not null)
        {
            subTask.Title = request.Title;
        }

        if (request.Description is not null)
        {
            subTask.Description = request.Description;
        }

        subTask.EditedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
