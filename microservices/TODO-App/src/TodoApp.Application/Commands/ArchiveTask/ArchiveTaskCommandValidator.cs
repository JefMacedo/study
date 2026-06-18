using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Interfaces;

namespace TodoApp.Application.Commands.ArchiveTask;

public class ArchiveTaskCommandValidator : AbstractValidator<ArchiveTaskCommand>
{
    private readonly IAppDbContext _context;

    public ArchiveTaskCommandValidator(IAppDbContext context)
    {
        _context = context;

        RuleFor(x => x.TaskId)
            .NotEmpty()
            .MustAsync(TaskExists).WithMessage("Task not found.");
    }

    private async Task<bool> TaskExists(Guid taskId, CancellationToken cancellationToken)
    {
        return await _context.Tasks.AnyAsync(x => x.Id == taskId, cancellationToken);
    }
}
