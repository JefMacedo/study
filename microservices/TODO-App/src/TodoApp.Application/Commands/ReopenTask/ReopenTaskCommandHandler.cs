using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Interfaces;

namespace TodoApp.Application.Commands.ReopenTask;

public class ReopenTaskCommandHandler(IAppDbContext context) : IRequestHandler<ReopenTaskCommand, Unit>
{
    private readonly IAppDbContext _context = context;

    public async Task<Unit> Handle(ReopenTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == request.TaskId, cancellationToken)
            ?? throw new InvalidOperationException($"Task with ID {request.TaskId} not found.");

        task.IsCompleted = false;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
