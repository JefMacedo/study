using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Interfaces;

namespace TodoApp.Application.Commands.DeleteSubTask;

public class DeleteSubTaskCommandHandler(IAppDbContext context) : IRequestHandler<DeleteSubTaskCommand, Unit>
{
    private readonly IAppDbContext _context = context;

    public async Task<Unit> Handle(DeleteSubTaskCommand request, CancellationToken cancellationToken)
    {
        var subTask = await _context.SubTasks
            .FirstOrDefaultAsync(x => x.Id == request.SubTaskId && x.TaskId == request.TaskId, cancellationToken)
            ?? throw new InvalidOperationException($"SubTask with ID {request.SubTaskId} not found for task {request.TaskId}.");

        if (subTask.IsDeleted)
            throw new InvalidOperationException("SubTask is already deleted.");

        subTask.IsDeleted = true;
        subTask.EditedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
