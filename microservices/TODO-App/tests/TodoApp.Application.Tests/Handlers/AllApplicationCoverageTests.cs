using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Commands.AddSubTask;
using TodoApp.Application.Commands.ArchiveTask;
using TodoApp.Application.Commands.CompleteSubTask;
using TodoApp.Application.Commands.CompleteTask;
using TodoApp.Application.Commands.CreateTask;
using TodoApp.Application.Commands.DeleteSubTask;
using TodoApp.Application.Commands.DeleteTask;
using TodoApp.Application.Commands.ReopenSubTask;
using TodoApp.Application.Commands.ReopenTask;
using TodoApp.Application.Commands.UpdateSubTask;
using TodoApp.Application.Commands.UpdateTask;
using TodoApp.Application.Commands.UnarchiveTask;
using TodoApp.Application.Tests.Helpers;
using TodoApp.Domain.Entities;
using Xunit;

namespace TodoApp.Application.Tests.Handlers;

public class AllApplicationCoverageTests
{
    [Fact]
    public async Task Exercise_all_handlers_and_validators()
    {
        using var context = TestDbContextFactory.CreateInMemoryContext();

        var task = new TaskItem { Id = Guid.NewGuid(), Title = "T", CreatedAt = DateTime.UtcNow };
        var st = new SubTask { Id = Guid.NewGuid(), TaskId = task.Id, Title = "s", CreatedAt = DateTime.UtcNow };
        task.SubTasks.Add(st);
        await context.Tasks.AddAsync(task);
        await context.SubTasks.AddAsync(st);
        await context.SaveChangesAsync();

        // CreateTask handler (already tested elsewhere) - exercise validator
        var createValidator = new TodoApp.Application.Commands.CreateTask.CreateTaskCommandValidator();
        var valid = createValidator.Validate(new CreateTaskCommand("t", null, null, null));
        valid.IsValid.Should().BeTrue();

        // UpdateTask
        var updateHandler = new UpdateTaskCommandHandler(context);
        await updateHandler.Handle(new UpdateTaskCommand(task.Id, "new", "desc"), CancellationToken.None);

        // Archive / Unarchive
        var archive = new ArchiveTaskCommandHandler(context);
        await archive.Handle(new ArchiveTaskCommand(task.Id), CancellationToken.None);
        var unarchive = new UnarchiveTaskCommandHandler(context);
        await unarchive.Handle(new UnarchiveTaskCommand(task.Id), CancellationToken.None);

        // Add SubTask
        var add = new AddSubTaskCommandHandler(context);
        var addRes = await add.Handle(new AddSubTaskCommand(task.Id, "s2", null), CancellationToken.None);
        addRes.TaskId.Should().Be(task.Id);

        // Update SubTask
        var updateSub = new UpdateSubTaskCommandHandler(context);
        await updateSub.Handle(new UpdateSubTaskCommand(task.Id, st.Id, "s-upd", null), CancellationToken.None);

        // Complete SubTask
        var completeSub = new CompleteSubTaskCommandHandler(context);
        await completeSub.Handle(new CompleteSubTaskCommand(task.Id, st.Id), CancellationToken.None);

        // Reopen SubTask
        var reopenSub = new ReopenSubTaskCommandHandler(context);
        await reopenSub.Handle(new ReopenSubTaskCommand(task.Id, st.Id), CancellationToken.None);

        // Complete Task
        var completeTask = new TodoApp.Application.Commands.CompleteTask.CompleteTaskCommandHandler(context);
        await completeTask.Handle(new CompleteTaskCommand(task.Id), CancellationToken.None);

        // Reopen Task
        var reopen = new ReopenTaskCommandHandler(context);
        await reopen.Handle(new ReopenTaskCommand(task.Id), CancellationToken.None);

        // Delete subtask
        var deleteSub = new DeleteSubTaskCommandHandler(context);
        await deleteSub.Handle(new DeleteSubTaskCommand(task.Id, st.Id), CancellationToken.None);

        // Delete task
        var deleteTask = new DeleteTaskCommandHandler(context);
        await deleteTask.Handle(new DeleteTaskCommand(task.Id), CancellationToken.None);

        // Ensure no exceptions and state set
        var t = await context.Tasks.IgnoreQueryFilters().FirstAsync(x => x.Id == task.Id);
        t.IsDeleted.Should().BeTrue();
    }
}
