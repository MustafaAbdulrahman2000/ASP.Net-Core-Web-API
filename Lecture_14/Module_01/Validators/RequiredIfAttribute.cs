using System.ComponentModel.DataAnnotations;

namespace Module_01.Validators;

public class RequiredIfAttribute : ValidationAttribute
{
    private readonly string _dependentProperty;
    private readonly object? _tragetValue;

    public RequiredIfAttribute(string dependentProperty, object? tragetValue)
    {
        _dependentProperty = dependentProperty;
        _tragetValue = tragetValue;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext context)
    {
        var containerType = context.ObjectInstance.GetType();
        var field = containerType.GetProperty(_dependentProperty);

        if (field is null)
            return new ValidationResult($"Unknown property: {_dependentProperty}.");
        
        var dependentValue = field.GetValue(context.ObjectInstance, null);
        
        if(Equals(dependentValue, _tragetValue))
        {
            if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
                return new ValidationResult(ErrorMessage ?? $"{context.DisplayName} is required.",
                [context.MemberName ?? "ReturnPolicyDescription"]);
        }

        return ValidationResult.Success;
    }

}
