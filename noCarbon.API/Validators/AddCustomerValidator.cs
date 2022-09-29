using FluentValidation;
using noCarbon.API.Models;

namespace noCarbon.API.Validators;

/// <summary>
/// AddCustomer validator
/// </summary>
public class AddCustomerValidator : AbstractValidator<AddCustomerInput>
{
    /// <summary>
    /// Ctor 
    /// </summary>
    public AddCustomerValidator()
    {
        RuleFor(m => m.UserName).NotEmpty().WithMessage("{PropertyName} should be not empty.");
        RuleFor(m => m.UserName).MinimumLength(8).WithMessage("{PropertyName} should be longer than 10.");
        RuleFor(m => m.Mail).NotEmpty().WithMessage("{PropertyName} should be not empty.");
        RuleFor(m => m.Mail).EmailAddress().WithMessage("{PropertyName} should be a valid mail.");
        RuleFor(m => m.Password).NotEmpty().WithMessage("{PropertyName} should be not empty.");
    }
}