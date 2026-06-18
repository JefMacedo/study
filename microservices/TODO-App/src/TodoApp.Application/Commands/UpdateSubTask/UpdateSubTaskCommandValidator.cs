using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Interfaces;

namespace TodoApp.Application.Commands.UpdateSubTask;

public class UpdateSubTaskCommandValidator : AbstractValidator<UpdateSubTaskCommand>
{
    private readonly IAppDbContext _context;

    public UpdateSubTaskCommandValidator(IAppDbContext context)
    {
        _context = context;

        RuleFor(x => x.TaskId)
            .NotEmpty()
            .MustAsync(TaskExists).WithMessage("Task not found.");

        RuleFor(x => x.SubTaskId)
            .NotEmpty()
            .MustAsync((command, subTaskId, cancellationToken) => SubTaskExists(command.TaskId, subTaskId, cancellationToken))
            .WithMessage("SubTask not found.");

        RuleFor(x => x)
            .Must(x => x.Title is not null || x.Description is not null)
            .WithMessage("Provide at least one field to update.");

        When(x => x.Title is not null, () =>
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        });

        When(x => x.Description is not null, () =>
        {
            RuleFor(x => x.Description).MaximumLength(500);
        });
    }

    private async Task<bool> TaskExists(Guid taskId, CancellationToken cancellationToken)
    {
        return await _context.Tasks.AnyAsync(x => x.Id == taskId, cancellationToken);
    }

    private async Task<bool> SubTaskExists(Guid taskId, Guid subTaskId, CancellationToken cancellationToken)
    {
        return await _context.SubTasks.AnyAsync(x => x.Id == subTaskId && x.TaskId == taskId, cancellationToken);
    }
}
