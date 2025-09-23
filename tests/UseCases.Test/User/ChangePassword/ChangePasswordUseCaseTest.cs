using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.ChangePassword;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace UseCases.Test.User.ChangePassword;

public class ChangePasswordUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var (user, password) = UserBuilder.Build();
        var request = RequestUpdateUserPasswordJsonBuilder.Build();
        request.Password = password;

        var useCase = CreateUseCase(user);
        
        Func<Task> act = async () => await useCase.Execute(request);

        await act.Should().NotThrowAsync();

        var passwordEncripter = PasswordEncripterBuilder.Build();
        user.Password.Should().Be(passwordEncripter.Encrypt(request.NewPassword));
    }

    [Fact]
    public async Task Error_NewPassword_Empty()
    {
        var (user, password) = UserBuilder.Build();
        var request = new RequestUpdateUserPasswordJson()
        {
            NewPassword = string.Empty,
            Password = password
        };
        
        var useCase = CreateUseCase(user);
        Func<Task> act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(erro => erro.ErrorMessages.Count == 1 && erro.ErrorMessages.Contains(ResourceMessagesException.PASSWORD_EMPTY));
        
        var passwordEncripter = PasswordEncripterBuilder.Build();
        user.Password.Should().Be(passwordEncripter.Encrypt(password));
    }
    
    [Fact]
    public async Task Error_CurrentPassword_Different()
    {
        var (user, password) = UserBuilder.Build();
        var request = RequestUpdateUserPasswordJsonBuilder.Build();
        
        var useCase = CreateUseCase(user);
        Func<Task> act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(erro => erro.ErrorMessages.Count == 1 && erro.ErrorMessages.Contains(ResourceMessagesException.PASSWORD_DIFFERENT_CURRENT_PASSWORD));
        
        var passwordEncripter = PasswordEncripterBuilder.Build();
        user.Password.Should().Be(passwordEncripter.Encrypt(password));
    }
    
    private static ChangePasswordUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User? user = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user!);
        var unitOfWork = UnitOfWorkBuilder.Build();
        var passwordEncripter = PasswordEncripterBuilder.Build();
        var updateOnlyRepository = new UserUpdateOnlyRepositoryBuilder();
        
        if (user != null)
            updateOnlyRepository.GetBeId(user);

        return new ChangePasswordUseCase(
            loggedUser,
            updateOnlyRepository.Build(),
            unitOfWork,
            passwordEncripter);
    }
}