using System.ComponentModel.DataAnnotations;

namespace Module_01.Validators;

public class WarrantyValidator
{
    public static ValidationResult? MustBe12_24_36(int value, ValidationContext context)
    {
        return value == 0 || value == 12 || value == 24 || value == 36 ?
               ValidationResult.Success :
               new ValidationResult(errorMessage: "Warrany must be 0, 12, 24, 36 months only.",
               [context.MemberName ?? "WarrantyPeriodMonths"]);
    }
}

