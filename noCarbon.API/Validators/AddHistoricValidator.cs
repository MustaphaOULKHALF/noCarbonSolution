using FluentValidation;
using noCarbon.API.Models;

namespace noCarbon.API.Validators;

/// <summary>
/// Add Category validator
/// </summary>
public class AddHistoricValidator : AbstractValidator<HistoricInput>
{
    /// <summary>
    /// Ctor 
    /// </summary>
    public AddHistoricValidator()
    {
        RuleFor(m => m.ActionId).NotEqual(0).WithMessage("{PropertyName} should be not eqyal a {PropertyValue}.");
        RuleFor(m => m.CategoryId).NotEqual(0).WithMessage("{PropertyName} should be not eqyal a {PropertyValue}.");
    }
}
