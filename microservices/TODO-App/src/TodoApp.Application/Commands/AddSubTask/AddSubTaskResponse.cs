namespace TodoApp.Application.Commands.AddSubTask;

public record AddSubTaskResponse(Guid SubTaskId, Guid TaskId, bool TaskIsCompleted);
