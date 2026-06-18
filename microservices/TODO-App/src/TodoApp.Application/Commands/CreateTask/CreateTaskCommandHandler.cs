using MediatR;
using TodoApp.Application.Interfaces;
using TodoApp.Domain.Entities;

namespace TodoApp.Application.Commands.CreateTask;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, CreateTaskResponse>
{
    private readonly IAppDbContext _context;

    public CreateTaskCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<CreateTaskResponse> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var entity = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            DueDate = request.DueDate,
            CreatedAt = DateTime.UtcNow,
            IsCompleted = false,
            SubTasks = new List<SubTask>()
        };

        if (request.SubTasks != null && request.SubTasks.Any())
        {
            foreach (var subTaskDto in request.SubTasks)
            {
                var subTask = new SubTask
                {
                    Id = Guid.NewGuid(),
                    TaskId = entity.Id,
                    Title = subTaskDto.Title,
                    Description = subTaskDto.Description,
                    IsCompleted = false,
                    CreatedAt = DateTime.UtcNow
                };

                entity.SubTasks.Add(subTask);
            }
        }
        else
        {
            entity.IsCompleted = true;
        }

        await _context.Tasks.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateTaskResponse(
            entity.Id,
            entity.Title,
            entity.Description,
            entity.DueDate,
            entity.IsCompleted,
            entity.SubTasks.Count);
    }
}
