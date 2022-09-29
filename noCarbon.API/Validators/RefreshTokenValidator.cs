using FluentValidation;
using noCarbon.API.Models;

namespace noCarbon.API.Validators;
/// <summary>
/// Refresh Token validator
/// </summary>
public class RefreshTokenValidator : AbstractValidator<RefreshTokenInput>
{
    /// <summary>
    /// Ctor 
    /// </summary>
    public RefreshTokenValidator()
    {
        RuleFor(m => m.AccessToken).NotEmpty().WithMessage("{PropertyName} should be not empty.");
        RuleFor(m => m.RefreshToken).NotEmpty().WithMessage("{PropertyName} should be not empty.");
    }
}
