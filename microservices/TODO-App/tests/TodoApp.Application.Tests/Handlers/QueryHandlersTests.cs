using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using TodoApp.Application.Queries.FindTask;
using TodoApp.Application.Queries.FindSubTask;
using TodoApp.Application.Queries.GetAllTasks;
using TodoApp.Application.Queries.GetSubTasksByTask;
using TodoApp.Application.Queries.GetTask;
using TodoApp.Application.Tests.Helpers;
using TodoApp.Domain.Entities;
using Xunit;

namespace TodoApp.Application.Tests.Handlers;

public class QueryHandlersTests
{
    [Fact]
    public async Task GetAll_and_Find_queries_return_expected_results()
    {
        using var context = TestDbContextFactory.CreateInMemoryContext();
        for (int i = 0; i < 5; i++)
        {
            var t = new TaskItem { Id = Guid.NewGuid(), Title = $"Title{i}", CreatedAt = DateTime.UtcNow };
            await context.Tasks.AddAsync(t);
        }
        await context.SaveChangesAsync();

        var getAllHandler = new GetAllTasksQueryHandler(context);
        var paged = await getAllHandler.Handle(new GetAllTasksQuery(1, 10), CancellationToken.None);
        paged.Items.Should().HaveCount(5);

        var findHandler = new FindTasksByTitleQueryHandler(context);
        var found = await findHandler.Handle(new FindTasksByTitleQuery("Title1", 1, 10), CancellationToken.None);
        found.Items.Should().ContainSingle();
    }

    [Fact]
    public async Task GetTask_and_GetSubTasksAndFindSubTask_handlers()
    {
        using var context = TestDbContextFactory.CreateInMemoryContext();
        var task = new TaskItem { Id = Guid.NewGuid(), Title = "T", CreatedAt = DateTime.UtcNow };
        var st = new SubTask { Id = Guid.NewGuid(), TaskId = task.Id, Title = "s", CreatedAt = DateTime.UtcNow };
        task.SubTasks.Add(st);
        await context.Tasks.AddAsync(task);
        await context.SubTasks.AddAsync(st);
        await context.SaveChangesAsync();

        var getTask = new GetTaskQueryHandler(context);
        var dto = await getTask.Handle(new GetTaskQuery(task.Id), CancellationToken.None);
        dto.Should().NotBeNull();
        dto!.Title.Should().Be("T");

        var getSubs = new GetSubTasksByTaskQueryHandler(context);
        var paged = await getSubs.Handle(new GetSubTasksByTaskQuery(task.Id, 1, 10), CancellationToken.None);
        paged.Items.Should().ContainSingle();

        var findSub = new FindSubTaskQueryHandler(context);
        var found = await findSub.Handle(new FindSubTaskQuery(st.Id, null), CancellationToken.None);
        found.Should().NotBeNull();
        found!.Title.Should().Be("s");
    }
}
