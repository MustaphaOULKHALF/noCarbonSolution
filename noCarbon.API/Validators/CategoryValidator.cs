using FluentValidation;
using noCarbon.API.Models;

namespace noCarbon.API.Validators;

/// <summary>
/// Add Category validator
/// </summary>
public class CategoryValidator : AbstractValidator<CategoryInput>
{
    /// <summary>
    /// Ctor 
    /// </summary>
    public CategoryValidator()
    {
        RuleFor(m => m.Name).NotEmpty().WithMessage("{PropertyName} should be not empty.");
        RuleFor(m => m.Name).MaximumLength(256).WithMessage("{PropertyName} should be shorter than {MaxLength}.");
        RuleFor(m => m.Description).MaximumLength(500).WithMessage("{PropertyName} should be shorter than {MaxLength}.");
    }
}