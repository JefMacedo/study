using System;
using FluentAssertions;
using TodoApp.Application.Common;
using TodoApp.Application.Queries.FindTask;
using TodoApp.Application.Queries.FindSubTask;
using TodoApp.Application.Queries.GetAllTasks;
using TodoApp.Application.Queries.GetSubTasksByTask;
using TodoApp.Application.Queries.GetTask;
using Xunit;

namespace TodoApp.Application.Tests.Misc;

public class ApplicationDtoAndQueriesTests
{
    [Fact]
    public void TaskDto_and_PagedResult_and_queries_properties()
    {
        var sub = new SubTaskDto(Guid.NewGuid(), "s", "d", true);
        var taskDto = new TaskDto(Guid.NewGuid(), "t", "d", DateTime.UtcNow, DateTime.UtcNow.AddDays(1), true, new[] { sub });

        // access all properties
        taskDto.Id.Should().NotBeEmpty();
        taskDto.Title.Should().Be("t");
        taskDto.Description.Should().Be("d");
        taskDto.CreatedAt.Should().BeAfter(DateTime.MinValue);
        taskDto.DueDate.Should().NotBeNull();
        taskDto.IsCompleted.Should().BeTrue();
        taskDto.SubTasks.Should().ContainSingle();

        var paged = new PagedResult<TaskDto>([taskDto], 1, 1, 10);
        paged.TotalCount.Should().Be(1);
        paged.Items.Should().Contain(taskDto);

        var getAll = new GetAllTasksQuery(1, 10);
        getAll.PageNumber.Should().Be(1);
        getAll.PageSize.Should().Be(10);

        var findTasks = new FindTasksByTitleQuery("x", 2, 5);
        findTasks.Title.Should().Be("x");
        findTasks.PageNumber.Should().Be(2);

        var findSub = new FindSubTaskQuery(Guid.NewGuid(), null);
        findSub.Id.Should().NotBeEmpty();

        var findSubsByTitle = new FindSubTasksByTitleQuery("y", 1, 10);
        findSubsByTitle.Title.Should().Be("y");

        var getSubByTask = new GetSubTasksByTaskQuery(Guid.NewGuid(), 1, 10);
        getSubByTask.PageNumber.Should().Be(1);
    }
}
