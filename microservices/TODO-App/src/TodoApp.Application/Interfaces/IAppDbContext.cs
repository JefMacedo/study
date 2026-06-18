using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Interfaces;

public interface IAppDbContext
{
    DbSet<TaskItem> Tasks { get; }
    DbSet<SubTask> SubTasks { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
