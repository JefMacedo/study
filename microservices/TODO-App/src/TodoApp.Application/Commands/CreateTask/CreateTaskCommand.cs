using System;
using MediatR;

namespace TodoApp.Application.Commands.CreateTask
{
    public record CreateTaskCommand(string Title, string? Description, DateTime? DueDate) : IRequest<Guid>;
}
