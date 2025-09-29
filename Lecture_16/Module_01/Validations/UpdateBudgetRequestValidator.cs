using FluentValidation;
using Module_01.Requests;

namespace Module_01.Validations;

public class UpdateBudgetRequestValidator : AbstractValidator<UpdateBudgetRequest>
{
    public UpdateBudgetRequestValidator()
    {
        RuleFor(x => x.Budget).GreaterThanOrEqualTo(0);
    }
}

