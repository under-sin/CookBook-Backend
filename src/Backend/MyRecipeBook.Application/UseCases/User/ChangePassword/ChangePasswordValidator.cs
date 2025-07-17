using FluentValidation;
using MyRecipeBook.Application.SharedValidators;
using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.UseCases.User.ChangePassword;

public class ChangePasswordValidator : AbstractValidator<RequestUpdateUserPasswordJson>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.Password).SetValidator(new PasswordValidator<RequestUpdateUserPasswordJson>());
    }
}
