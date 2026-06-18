using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using TodoApp.Application.Commands.CreateTask;
using TodoApp.Application.Commands.UpdateTask;
using TodoApp.Application.Queries.GetTask;
using TodoApp.Application.Tests.Helpers;
using TodoApp.Domain.Entities;
using Xunit;

namespace TodoApp.Application.Tests.Handlers;

public class RemainingHandlersAndValidatorsTests
{
    [Fact]
    public async Task CreateTask_handler_and_validators_and_dtos()
    {
        using var context = TestDbContextFactory.CreateInMemoryContext();

        // prepare existing data
        var existing = new TaskItem { Id = Guid.NewGuid(), Title = "exist", CreatedAt = DateTime.UtcNow };
        await context.Tasks.AddAsync(existing);
        await context.SaveChangesAsync();

        // CreateTask handler
        var createHandler = new CreateTaskCommandHandler(context);
        var createResponse = await createHandler.Handle(new CreateTaskCommand("new-title", "desc", null, null), CancellationToken.None);
        createResponse.Should().NotBeNull();
        createResponse.Title.Should().Be("new-title");

        // UpdateTask validator
        var updateValidator = new UpdateTaskCommandValidator(context);
        var v = await updateValidator.ValidateAsync(new UpdateTaskCommand(existing.Id, "updated", "d"));
        v.IsValid.Should().BeTrue();

        // DTOs
        var subDto = new SubTaskDto(Guid.NewGuid(), "sub", null, false);
        var taskDto = new TaskDto(Guid.NewGuid(), "t", "d", DateTime.UtcNow, null, false, new[] { subDto });
        taskDto.Title.Should().Be("t");
        taskDto.SubTasks.Should().ContainSingle();
    }
}
