using System.ComponentModel.DataAnnotations;

namespace NotTinderAppMAUI.Validation;

public sealed class NotEqualAttribute : ValidationAttribute
{
    private readonly object _comparisonValue;

    public NotEqualAttribute(object comparisonValue)
    {
        _comparisonValue = comparisonValue;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (Equals(value, _comparisonValue))
            return new ValidationResult(ErrorMessage ?? $"The value must not be equal to {_comparisonValue}.");

        return ValidationResult.Success;
    }
}