using FluentValidation;
using noCarbon.API.Models;

namespace noCarbon.API.Validators;

/// <summary>
/// Add Action validator
/// </summary>
public class AddActionValidator : AbstractValidator<ActionsInput>
{
    /// <summary>
    /// Ctor 
    /// </summary>
    public AddActionValidator()
    {
        RuleFor(m => m.CategoryId).NotEqual(0).WithMessage("{PropertyName} should be not equal 0.");
        RuleFor(m => m.Name).NotEmpty().WithMessage("{PropertyName} should be not empty.");
        RuleFor(m => m.Name).MaximumLength(400).WithMessage("{PropertyName} should be shorter than {MaxLength}.");
        RuleFor(m => m.Description).MaximumLength(500).WithMessage("{PropertyName} should be shorter than {MaxLength}.");
        RuleFor(m => m.Points).Equal(0).WithMessage("{PropertyName} should be be not equal 0.");
        RuleFor(m => m.ReducedCarb).Equal(0).WithMessage("{PropertyName} should be be not equal 0.");
    }   
}