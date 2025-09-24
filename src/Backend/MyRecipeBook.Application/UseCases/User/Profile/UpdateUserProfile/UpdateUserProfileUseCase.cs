using FluentValidation.Results;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Users;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.User.Profile.UpdateUserProfile;

public class UpdateUserProfileUseCase(
    ILoggedUser loggedUser,
    IUserWriteOnlyRepository writeOnlyRepository,
    IUserReadOnlyRepository readOnlyRepository,
    IUnitOfWork unitOfWork)
    : IUpdateUserProfileUseCase
{
    public async Task<ResponseUserProfileJson> Execute(RequestUpdateUserJson request) {
        var user = await loggedUser.User();
        await Validate(request, user.UserIdentifier);
        user.Name = request.Name;
        user.Email = request.Email;

        writeOnlyRepository.UpdateUserProfile(user);
        await unitOfWork.Commit();

        return new ResponseUserProfileJson {
            Email = user.Email,
            Name = user.Name
        };
    }

    public async Task Validate(RequestUpdateUserJson request, Guid userIdentifier) {
        var validator = new UpdateUserProfileValidator();
        var result = validator.Validate(request);

        var emailExists = await readOnlyRepository.EmailExistsForOtherUser(request.Email, userIdentifier);
        if (emailExists)
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.EMAIL_ALREADY_IN_USE));

        if (!result.IsValid) {
            var errorMessages = result.Errors
                .Select(e => e.ErrorMessage)
                .ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}