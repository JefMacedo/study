using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Interfaces;

namespace TodoApp.Application.Commands.CompleteTask;

public class CompleteTaskCommandHandler(IAppDbContext context) : IRequestHandler<CompleteTaskCommand, Unit>
{
    private readonly IAppDbContext _context = context;

    public async Task<Unit> Handle(CompleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == request.TaskId, cancellationToken)
            ?? throw new InvalidOperationException($"Task with ID {request.TaskId} not found.");
        task.IsCompleted = true;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
