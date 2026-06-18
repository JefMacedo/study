namespace TodoApp.Application.Queries.GetTask;

public record SubTaskDto(Guid Id, string Title, string? Description, bool IsCompleted);

public record TaskDto(
    Guid Id,
    string Title,
    string? Description,
    DateTime CreatedAt,
    DateTime? DueDate,
    bool IsCompleted,
    IEnumerable<SubTaskDto> SubTasks);
