using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Interfaces;
using TodoApp.Domain.Entities;

namespace TodoApp.Infrastructure.Persistence;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<TaskItem> Tasks { get; set; } = null!;
    public DbSet<SubTask> SubTasks { get; set; } = null!;

    public new Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskItem>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Title).IsRequired().HasMaxLength(200);
            b.Property(x => x.Description).HasMaxLength(2000);
            b.Property(x => x.IsDeleted).HasDefaultValue(false);
            b.Property(x => x.IsArchived).HasDefaultValue(false);
            b.Property(x => x.EditedAt);
            b.HasMany(x => x.SubTasks)
                .WithOne(x => x.Task)
                .HasForeignKey(x => x.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<SubTask>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Title).IsRequired().HasMaxLength(200);
            b.Property(x => x.Description).HasMaxLength(500);
            b.Property(x => x.TaskId).IsRequired();
            b.Property(x => x.IsDeleted).HasDefaultValue(false);
            b.Property(x => x.EditedAt);
        });
    }
}
