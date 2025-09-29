using FluentValidation;
using Module_01.Requests;

namespace Module_01.Validations;

public class AssignUserToTaskRequestValidator : AbstractValidator<AssignUserToTaskRequest>
{
    public AssignUserToTaskRequestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}

