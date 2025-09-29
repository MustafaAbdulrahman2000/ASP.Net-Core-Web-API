using FluentValidation;
using Module_01.Requests;

namespace Module_01.Validations;

public class CreateProjectRequestValidator : AbstractValidator<CreateProjectRequest>
{
    public CreateProjectRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(500);
        RuleFor(x => x.ExpectedStartDate).GreaterThan(DateTime.MinValue);
        RuleFor(x => x.Budget).GreaterThanOrEqualTo(0);
    }
}

