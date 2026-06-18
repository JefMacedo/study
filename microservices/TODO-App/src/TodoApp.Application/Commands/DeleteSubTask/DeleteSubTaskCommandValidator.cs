using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Interfaces;

namespace TodoApp.Application.Commands.DeleteSubTask;

public class DeleteSubTaskCommandValidator : AbstractValidator<DeleteSubTaskCommand>
{
    private readonly IAppDbContext _context;

    public DeleteSubTaskCommandValidator(IAppDbContext context)
    {
        _context = context;

        RuleFor(x => x.TaskId)
            .NotEmpty()
            .MustAsync(TaskExists).WithMessage("Task not found.");

        RuleFor(x => x.SubTaskId)
            .NotEmpty()
            .MustAsync((command, subTaskId, cancellationToken) => SubTaskExists(command.TaskId, subTaskId, cancellationToken))
            .WithMessage("SubTask not found.");
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
