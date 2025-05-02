using System.ComponentModel.DataAnnotations;

namespace Foodsharing.API.Extensions.Attributes;

public class MaxFileSizeAttribute : ValidationAttribute
{
    private readonly int _maxSize;

    public MaxFileSizeAttribute(int maxSize)
    {
        _maxSize = maxSize;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is IFormFile file && file.Length > _maxSize)
        {
            return new ValidationResult(ErrorMessage);
        }
        return ValidationResult.Success;
    }
}
