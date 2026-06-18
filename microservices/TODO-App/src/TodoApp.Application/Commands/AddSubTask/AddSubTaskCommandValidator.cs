using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Interfaces;

namespace TodoApp.Application.Commands.AddSubTask;

public class AddSubTaskCommandValidator : AbstractValidator<AddSubTaskCommand>
{
    private readonly IAppDbContext _context;

    public AddSubTaskCommandValidator(IAppDbContext context)
    {
        _context = context;

        RuleFor(x => x.TaskId)
            .NotEmpty()
            .MustAsync(TaskExists).WithMessage("Task not found.");

        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(500);
    }

    private async Task<bool> TaskExists(Guid taskId, CancellationToken cancellationToken)
    {
        return await _context.Tasks.AnyAsync(x => x.Id == taskId, cancellationToken);
    }
}
