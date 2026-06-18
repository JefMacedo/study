namespace TodoApp.Application.Commands.CreateTask;

public record CreateTaskResponse(Guid Id, string Title, string? Description, DateTime? DueDate, bool IsCompleted, int SubTaskCount);
