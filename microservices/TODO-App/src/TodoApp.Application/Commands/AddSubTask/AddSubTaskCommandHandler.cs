using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Interfaces;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Commands.AddSubTask;

public class AddSubTaskCommandHandler : IRequestHandler<AddSubTaskCommand, AddSubTaskResponse>
{
    private readonly IAppDbContext _context;

    public AddSubTaskCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<AddSubTaskResponse> Handle(AddSubTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks
            .Include(x => x.SubTasks)
            .FirstOrDefaultAsync(x => x.Id == request.TaskId, cancellationToken);

        if (task == null)
            throw new InvalidOperationException($"Task with ID {request.TaskId} not found.");

        var subTask = new SubTask
        {
            Id = Guid.NewGuid(),
            TaskId = task.Id,
            Title = request.Title,
            Description = request.Description,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        task.SubTasks.Add(subTask);

        if (task.IsCompleted)
        {
            task.IsCompleted = false;
        }

        await _context.SubTasks.AddAsync(subTask, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new AddSubTaskResponse(subTask.Id, task.Id, task.IsCompleted);
    }
}
