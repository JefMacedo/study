using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<TaskItem> Tasks { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
