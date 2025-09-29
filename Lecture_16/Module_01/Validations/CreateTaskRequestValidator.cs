using FluentValidation;
using Module_01.Requests;

namespace Module_01.Validations;

public class CreateTaskRequestValidator : AbstractValidator<CreateTaskRequest>
{
    public CreateTaskRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(500);
        RuleFor(x => x.AssignedUserId).NotEmpty();
    }
}

