using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Interfaces;

namespace TodoApp.Application.Commands.UpdateTask;

public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
{
    private readonly IAppDbContext _context;

    public UpdateTaskCommandValidator(IAppDbContext context)
    {
        _context = context;

        RuleFor(x => x.TaskId)
            .NotEmpty()
            .MustAsync(TaskExists).WithMessage("Task not found.");

        RuleFor(x => x)
            .Must(x => x.Title is not null || x.Description is not null)
            .WithMessage("Provide at least one field to update.");

        When(x => x.Title is not null, () =>
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        });

        When(x => x.Description is not null, () =>
        {
            RuleFor(x => x.Description).MaximumLength(2000);
        });
    }

    private async Task<bool> TaskExists(Guid taskId, CancellationToken cancellationToken)
    {
        return await _context.Tasks.AnyAsync(x => x.Id == taskId, cancellationToken);
    }
}
