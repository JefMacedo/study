using System;
using System.Threading.Tasks;
using FluentAssertions;
using TodoApp.Application.Commands.AddSubTask;
using TodoApp.Application.Commands.ArchiveTask;
using TodoApp.Application.Commands.CompleteTask;
using TodoApp.Application.Commands.DeleteSubTask;
using TodoApp.Application.Commands.ReopenSubTask;
using TodoApp.Application.Commands.ReopenTask;
using TodoApp.Application.Commands.UnarchiveTask;
using TodoApp.Application.Commands.UpdateSubTask;
using TodoApp.Application.Tests.Helpers;
using TodoApp.Domain.Entities;
using Xunit;

namespace TodoApp.Application.Tests.Validators;

public class ValidatorsCoverageTests
{
    [Fact]
    public async Task Validators_with_db_checks_validate_successfully()
    {
        using var context = TestDbContextFactory.CreateInMemoryContext();

        var task = new TaskItem { Id = Guid.NewGuid(), Title = "T", CreatedAt = DateTime.UtcNow };
        var st = new SubTask { Id = Guid.NewGuid(), TaskId = task.Id, Title = "s", CreatedAt = DateTime.UtcNow };
        task.SubTasks.Add(st);
        await context.Tasks.AddAsync(task);
        await context.SubTasks.AddAsync(st);
        await context.SaveChangesAsync();

        var addValidator = new AddSubTaskCommandValidator(context);
        var vr1 = await addValidator.ValidateAsync(new AddSubTaskCommand(task.Id, "sub", null));
        vr1.IsValid.Should().BeTrue();

        var completeValidator = new CompleteTaskCommandValidator(context);
        var vr2 = await completeValidator.ValidateAsync(new CompleteTaskCommand(task.Id));
        vr2.IsValid.Should().BeTrue();

        var reopenSubValidator = new ReopenSubTaskCommandValidator(context);
        var vr3 = await reopenSubValidator.ValidateAsync(new ReopenSubTaskCommand(task.Id, st.Id));
        vr3.IsValid.Should().BeTrue();

        var archiveValidator = new ArchiveTaskCommandValidator(context);
        var vr4 = await archiveValidator.ValidateAsync(new ArchiveTaskCommand(task.Id));
        vr4.IsValid.Should().BeTrue();

        var unarchiveValidator = new UnarchiveTaskCommandValidator(context);
        var vr5 = await unarchiveValidator.ValidateAsync(new UnarchiveTaskCommand(task.Id));
        vr5.IsValid.Should().BeTrue();

        var deleteSubValidator = new DeleteSubTaskCommandValidator(context);
        var vr6 = await deleteSubValidator.ValidateAsync(new DeleteSubTaskCommand(task.Id, st.Id));
        vr6.IsValid.Should().BeTrue();

        var updateSubValidator = new UpdateSubTaskCommandValidator(context);
        var vr7 = await updateSubValidator.ValidateAsync(new UpdateSubTaskCommand(task.Id, st.Id, "x", null));
        vr7.IsValid.Should().BeTrue();

        var reopenTaskValidator = new TodoApp.Application.Commands.ReopenTask.ReopenTaskCommandValidator(context);
        var vr8 = await reopenTaskValidator.ValidateAsync(new ReopenTaskCommand(task.Id));
        vr8.IsValid.Should().BeTrue();
    }
}
