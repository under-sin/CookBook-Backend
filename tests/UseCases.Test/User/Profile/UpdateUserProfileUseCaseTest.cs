using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Profile.UpdateUserProfile;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace UseCases.Test.User.Profile;

public class UpdateUserProfileUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();
        var request = RequestUpdateUserJsonBuilder.Build();
        
        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(request);
        
        result.Name.Should().Be(request.Name);
        result.Email.Should().Be(request.Email);
        result.Should().NotBeNull();
    }
    
    [Fact]
    public async Task Error_Name_Empty()
    {
        (var user, _) = UserBuilder.Build();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.ErrorMessages.Count == 1
                        && e.ErrorMessages.Contains(ResourceMessagesException.NAME_EMPTY));
    }
    
    [Fact]
    public async Task Error_When_Email_Empty()
    {
        (var user, _) = UserBuilder.Build();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = string.Empty;

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.ErrorMessages.Count == 1
                        && e.ErrorMessages.Contains(ResourceMessagesException.EMAIL_EMPTY));
    }

    private static UpdateUserProfileUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var unitOfWork = UnitOfWorkBuilder.Build();
        var writeOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
        var readOnlyRepository = new UserReadOnlyRepositoryBuilder();

        return new UpdateUserProfileUseCase(
            loggedUser,
            writeOnlyRepository,
            readOnlyRepository.Build(),
            unitOfWork);
    }
}