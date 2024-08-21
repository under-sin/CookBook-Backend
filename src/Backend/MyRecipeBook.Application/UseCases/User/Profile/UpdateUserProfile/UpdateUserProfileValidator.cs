using FluentValidation;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.User.Profile.UpdateUserProfile;

public class UpdateUserProfileValidator : AbstractValidator<RequestUpdateUserJson>
{
    public UpdateUserProfileValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.NAME_EMPTY);

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.EMAIL_EMPTY);
        
        // se o email não for vazio, então valida se é um email válido
        When(user => string.IsNullOrEmpty(user.Email).IsFalse(), () =>
        {
            RuleFor(x => x.Email).EmailAddress().WithMessage(ResourceMessagesException.EMAIL_INVALID);
        });
    }
}