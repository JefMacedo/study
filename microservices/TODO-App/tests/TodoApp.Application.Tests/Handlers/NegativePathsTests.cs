using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using TodoApp.Application.Commands.AddSubTask;
using TodoApp.Application.Queries.GetSubTasksByTask;
using TodoApp.Application.Queries.GetTask;
using TodoApp.Application.Tests.Helpers;
using TodoApp.Domain.Entities;
using Xunit;

namespace TodoApp.Application.Tests.Handlers;

public class NegativePathsTests
{
    [Fact]
    public async Task GetTask_returns_null_when_not_found()
    {
        using var context = TestDbContextFactory.CreateInMemoryContext();
        var handler = new GetTaskQueryHandler(context);
        var res = await handler.Handle(new GetTaskQuery(Guid.NewGuid()), CancellationToken.None);
        res.Should().BeNull();
    }

    [Fact]
    public async Task AddSubTaskValidator_fails_for_missing_task()
    {
        using var context = TestDbContextFactory.CreateInMemoryContext();
        var validator = new AddSubTaskCommandValidator(context);
        var vr = await validator.ValidateAsync(new AddSubTaskCommand(Guid.NewGuid(), "x", null));
        vr.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task GetSubTasksByTask_returns_empty_when_no_subtasks()
    {
        using var context = TestDbContextFactory.CreateInMemoryContext();
        var task = new TaskItem { Id = Guid.NewGuid(), Title = "t", CreatedAt = DateTime.UtcNow };
        await context.Tasks.AddAsync(task);
        await context.SaveChangesAsync();

        var handler = new GetSubTasksByTaskQueryHandler(context);
        var res = await handler.Handle(new GetSubTasksByTaskQuery(task.Id, 1, 10), CancellationToken.None);
        res.Items.Should().BeEmpty();
    }
}
