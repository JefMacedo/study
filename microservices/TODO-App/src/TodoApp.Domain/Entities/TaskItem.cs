using System;

namespace TodoApp.Domain.Entities
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}
