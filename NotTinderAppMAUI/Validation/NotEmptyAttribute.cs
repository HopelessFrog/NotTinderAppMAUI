using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace NotTinderAppMAUI.Validation;

public sealed class NotEmptyAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is ICollection collection)
        {
            if (collection.Count > 0)
                return ValidationResult.Success;
            return new ValidationResult(ErrorMessage ?? "Collection is empty");
        }

        return new ValidationResult(ErrorMessage ?? "Incorrect property type");
    }
}