using FluentValidation;
using noCarbon.API.Models;

namespace noCarbon.API.Validators;

/// <summary>
/// login validator
/// </summary>
public class LoginValidator : AbstractValidator<LoginInput>
{
    /// <summary>
    /// Ctor 
    /// </summary>
    public LoginValidator()
    {
        RuleFor(m => m.Username).NotEmpty().WithMessage("{PropertyName} should be not empty.");
        RuleFor(m => m.Username).MinimumLength(8).WithMessage("{PropertyName} should be longer than 10.");
        RuleFor(m => m.Password).NotEmpty().WithMessage("{PropertyName} should be not empty.");
    }
}
