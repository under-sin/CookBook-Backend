using FluentValidation;
using MyRecipeBook.Communication.Request;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.User.Register;

public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.NAME_EMPTY);

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.EMAIL_EMPTY)
            .EmailAddress()
            .WithMessage(ResourceMessagesException.EMAIL_INVALID);

        RuleFor(x => x.Password)
            .MinimumLength(6)
            .WithMessage(ResourceMessagesException.PASSWORD_EMPTY);
    }
}