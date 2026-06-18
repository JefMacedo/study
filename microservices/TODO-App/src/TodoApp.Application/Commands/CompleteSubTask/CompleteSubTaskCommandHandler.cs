using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Interfaces;

namespace TodoApp.Application.Commands.CompleteSubTask;

public class CompleteSubTaskCommandHandler : IRequestHandler<CompleteSubTaskCommand, Unit>
{
    private readonly IAppDbContext _context;

    public CompleteSubTaskCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(CompleteSubTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks
            .Include(x => x.SubTasks)
            .FirstOrDefaultAsync(x => x.Id == request.TaskId, cancellationToken) 
                ?? throw new InvalidOperationException($"Task with ID {request.TaskId} not found.");
        
        var subTask = task.SubTasks.FirstOrDefault(x => x.Id == request.SubTaskId) 
            ?? throw new InvalidOperationException($"SubTask with ID {request.SubTaskId} not found for task {request.TaskId}.");
        subTask.IsCompleted = true;

        if (task.SubTasks.All(x => x.IsCompleted))
        {
            task.IsCompleted = true;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
