using System.ComponentModel.DataAnnotations;

namespace Module_01.Validators;

public static class LaunchDateValidator
{
    public static ValidationResult? MustBeTodayOrFuture(DateTime dateTime, ValidationContext context)
    {
        if (dateTime.Date >= DateTime.UtcNow.Date)
            return ValidationResult.Success;

        return new ValidationResult(errorMessage: "Launch date must be today or in the future.", 
        [context.MemberName ?? "LaunchDate"]);
    }
}
