namespace TodoApp.Domain.Entities;

public class SubTask
{
    public Guid Id { get; set; }
    public Guid TaskId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? EditedAt { get; set; }

    public TaskItem Task { get; set; } = null!;
}
