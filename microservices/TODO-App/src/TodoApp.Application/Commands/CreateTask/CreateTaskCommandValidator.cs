using FluentValidation;

namespace TodoApp.Application.Commands.CreateTask;

public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(2000);

        RuleForEach(x => x.SubTasks)
            .SetValidator(new CreateSubTaskValidator())
            .When(x => x.SubTasks != null);
    }
}

public class CreateSubTaskValidator : AbstractValidator<CreateSubTaskDto>
{
    public CreateSubTaskValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(500);
    }
}
