using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
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
using TodoApp.Application.Commands.UpdateTask;
using TodoApp.Domain.Entities;
using TodoApp.Application.Tests.Helpers;

namespace TodoApp.Application.Tests.Handlers;

public class TaskHandlersTests
{
    [Fact]
    public async Task CreateTaskCommandHandler_creates_task_and_subtasks()
    {
        using var context = TestDbContextFactory.CreateInMemoryContext();

        var handler = new TodoApp.Application.Commands.CreateTask.CreateTaskCommandHandler(context);

        var cmd = new CreateTaskCommand("T1", "D1", null, new[] { new CreateSubTaskDto("s1", "d") });

        var res = await handler.Handle(cmd, CancellationToken.None);

        res.Should().NotBeNull();
        var t = await context.Tasks.Include(x => x.SubTasks).FirstOrDefaultAsync();
        t.Should().NotBeNull();
        t.Title.Should().Be("T1");
        t.SubTasks.Should().HaveCount(1);
    }

    [Fact]
    public async Task AddSubTaskCommandHandler_adds_subtask_and_uncompletes_task()
    {
        using var context = TestDbContextFactory.CreateInMemoryContext();
        var task = new TaskItem { Id = Guid.NewGuid(), Title = "T", CreatedAt = DateTime.UtcNow, IsCompleted = true };
        await context.Tasks.AddAsync(task);
        await context.SaveChangesAsync();

        var handler = new AddSubTaskCommandHandler(context);
        var cmd = new AddSubTaskCommand(task.Id, "st", "d");

        var res = await handler.Handle(cmd, CancellationToken.None);
        res.TaskId.Should().Be(task.Id);
        var t = await context.Tasks.Include(x => x.SubTasks).FirstAsync();
        t.IsCompleted.Should().BeFalse();
    }

    [Fact]
    public async Task CompleteSubTask_marks_subtask_and_task_if_all_complete()
    {
        using var context = TestDbContextFactory.CreateInMemoryContext();
        var task = new TaskItem { Id = Guid.NewGuid(), Title = "T", CreatedAt = DateTime.UtcNow };
        var st1 = new SubTask { Id = Guid.NewGuid(), TaskId = task.Id, Title = "s1", CreatedAt = DateTime.UtcNow };
        var st2 = new SubTask { Id = Guid.NewGuid(), TaskId = task.Id, Title = "s2", CreatedAt = DateTime.UtcNow };
        task.SubTasks.Add(st1); task.SubTasks.Add(st2);
        await context.Tasks.AddAsync(task);
        await context.SubTasks.AddRangeAsync(st1, st2);
        await context.SaveChangesAsync();

        var handler = new CompleteSubTaskCommandHandler(context);
        await handler.Handle(new CompleteSubTaskCommand(task.Id, st1.Id), CancellationToken.None);
        var t = await context.Tasks.Include(x => x.SubTasks).FirstAsync();
        t.IsCompleted.Should().BeFalse();

        await handler.Handle(new CompleteSubTaskCommand(task.Id, st2.Id), CancellationToken.None);
        t = await context.Tasks.Include(x => x.SubTasks).FirstAsync();
        t.IsCompleted.Should().BeTrue();
    }

    [Fact]
    public async Task CompleteTask_marks_task_completed_only()
    {
        using var context = TestDbContextFactory.CreateInMemoryContext();
        var task = new TaskItem { Id = Guid.NewGuid(), Title = "T", CreatedAt = DateTime.UtcNow };
        await context.Tasks.AddAsync(task);
        await context.SaveChangesAsync();

        var handler = new TodoApp.Application.Commands.CompleteTask.CompleteTaskCommandHandler(context);
        await handler.Handle(new CompleteTaskCommand(task.Id), CancellationToken.None);
        var t = await context.Tasks.FirstAsync();
        t.IsCompleted.Should().BeTrue();
    }

    [Fact]
    public async Task ReopenTask_and_ReopenSubTask_work()
    {
        using var context = TestDbContextFactory.CreateInMemoryContext();
        var task = new TaskItem { Id = Guid.NewGuid(), Title = "T", CreatedAt = DateTime.UtcNow, IsCompleted = true };
        var st = new SubTask { Id = Guid.NewGuid(), TaskId = task.Id, Title = "s", CreatedAt = DateTime.UtcNow, IsCompleted = true };
        task.SubTasks.Add(st);
        await context.Tasks.AddAsync(task);
        await context.SubTasks.AddAsync(st);
        await context.SaveChangesAsync();

        var reopenSub = new ReopenSubTaskCommandHandler(context);
        await reopenSub.Handle(new ReopenSubTaskCommand(task.Id, st.Id), CancellationToken.None);
        var t = await context.Tasks.Include(x => x.SubTasks).FirstAsync();
        t.IsCompleted.Should().BeFalse();
        t.SubTasks.First().IsCompleted.Should().BeFalse();

        var reopen = new ReopenTaskCommandHandler(context);
        await reopen.Handle(new ReopenTaskCommand(task.Id), CancellationToken.None);
        t = await context.Tasks.FirstAsync();
        t.IsCompleted.Should().BeFalse();
    }

    [Fact]
    public async Task Update_and_Delete_Task_and_SubTask_behaviors()
    {
        using var context = TestDbContextFactory.CreateInMemoryContext();
        var task = new TaskItem { Id = Guid.NewGuid(), Title = "T", CreatedAt = DateTime.UtcNow };
        var st = new SubTask { Id = Guid.NewGuid(), TaskId = task.Id, Title = "s", CreatedAt = DateTime.UtcNow };
        task.SubTasks.Add(st);
        await context.Tasks.AddAsync(task);
        await context.SubTasks.AddAsync(st);
        await context.SaveChangesAsync();

        var updateTaskHandler = new UpdateTaskCommandHandler(context);
        await updateTaskHandler.Handle(new UpdateTaskCommand(task.Id, "T2", null), CancellationToken.None);
        var t = await context.Tasks.FirstAsync();
        t.Title.Should().Be("T2");
        t.EditedAt.Should().NotBeNull();

        var deleteSub = new DeleteSubTaskCommandHandler(context);
        await deleteSub.Handle(new DeleteSubTaskCommand(task.Id, st.Id), CancellationToken.None);
        var sub = await context.SubTasks.FirstAsync();
        sub.IsDeleted.Should().BeTrue();
        sub.EditedAt.Should().NotBeNull();

        var deleteTask = new DeleteTaskCommandHandler(context);
        await deleteTask.Handle(new DeleteTaskCommand(task.Id), CancellationToken.None);
        t = await context.Tasks.FirstAsync();
        t.IsDeleted.Should().BeTrue();
        t.EditedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task Archive_and_Unarchive_Task()
    {
        using var context = TestDbContextFactory.CreateInMemoryContext();
        var task = new TaskItem { Id = Guid.NewGuid(), Title = "T", CreatedAt = DateTime.UtcNow };
        await context.Tasks.AddAsync(task);
        await context.SaveChangesAsync();

        var archive = new ArchiveTaskCommandHandler(context);
        await archive.Handle(new ArchiveTaskCommand(task.Id), CancellationToken.None);
        var t = await context.Tasks.FirstAsync();
        t.IsArchived.Should().BeTrue();
        t.EditedAt.Should().NotBeNull();

        var unarchive = new TodoApp.Application.Commands.UnarchiveTask.UnarchiveTaskCommandHandler(context);
        await unarchive.Handle(new TodoApp.Application.Commands.UnarchiveTask.UnarchiveTaskCommand(task.Id), CancellationToken.None);
        t = await context.Tasks.FirstAsync();
        t.IsArchived.Should().BeFalse();
    }
}
