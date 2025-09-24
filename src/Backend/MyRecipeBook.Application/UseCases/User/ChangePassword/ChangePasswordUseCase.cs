using FluentValidation.Results;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Users;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.User.ChangePassword;

public class ChangePasswordUseCase(
    ILoggedUser loggedUser,
    IUserUpdateOnlyRepository repository,
    IUnitOfWork unitOfWork,
    IPasswordEncripter encripter) : IChangePasswordUseCase
{
    public async Task Execute(RequestUpdateUserPasswordJson request)
    {
        var loggedUserResult = await loggedUser.User();

        Validate(request, loggedUserResult);

        var user = await repository.GetByIdAsync(loggedUserResult.Id);

        user.Password = encripter.Encrypt(request.NewPassword);

        repository.Update(user);

        await unitOfWork.Commit();
    }

    private void Validate(RequestUpdateUserPasswordJson request, Domain.Entities.User loggedUser) {
        var result = new ChangePasswordValidator().Validate(request);

        var currentPasswordEncrypted = encripter.Encrypt(request.Password);
        
        if(currentPasswordEncrypted.Equals(loggedUser.Password).IsFalse())
            result.Errors.Add(new ValidationFailure(nameof(request.Password), ResourceMessagesException.PASSWORD_DIFFERENT_CURRENT_PASSWORD));   

        if (result.IsValid.IsFalse())
        {
            var errorMessages = result.Errors
                .Select(e => e.ErrorMessage)
                .ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}