using CommonTestUtilities.BlobStorage;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.Recipe.Register;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace UseCases.Test.Recipe.Register;

public class RegisterRecipeUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();
        var request = RequestRegisterRecipeFormDataBuilder.Build();

        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Id.Should().NotBeNullOrWhiteSpace();
        result.Title.Should().Be(request.Title);
    }

    [Fact]
    public async Task Success_With_Png_Image()
    {
        var (user, _) = UserBuilder.Build();
        var request = RequestRegisterRecipeFormDataBuilder.BuildWithPngImage();

        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Id.Should().NotBeNullOrWhiteSpace();
        result.Title.Should().Be(request.Title);
    }

    [Fact]
    public async Task Success_With_Jpeg_Image()
    {
        var (user, _) = UserBuilder.Build();
        var request = RequestRegisterRecipeFormDataBuilder.BuildWithJpegImage();

        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Id.Should().NotBeNullOrWhiteSpace();
        result.Title.Should().Be(request.Title);
    }

    [Fact]
    public async Task Error_Title_Empty()
    {
        var (user, _) = UserBuilder.Build();
        var request = RequestRegisterRecipeFormDataBuilder.Build();
        request.Title = string.Empty;

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.ErrorMessages.Count == 1
                        && e.ErrorMessages.Contains(ResourceMessagesException.RECIPE_TITLE_EMPTY));
    }

    [Fact]
    public async Task Error_Invalid_Image_Format()
    {
        var (user, _) = UserBuilder.Build();
        var request = RequestRegisterRecipeFormDataBuilder.BuildWithInvalidImage();

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.ErrorMessages.Count == 1
                        && e.ErrorMessages.Contains(ResourceMessagesException.INVALID_IMAGE_FORMAT));
    }

    [Fact]
    public async Task Error_Multiple_Validation_Errors()
    {
        var (user, _) = UserBuilder.Build();
        var request = RequestRegisterRecipeFormDataBuilder.Build();
        request.Title = string.Empty;
        request.Ingredients.Clear();
        request.Instructions.Clear();

        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);

        var exception = await act.Should().ThrowAsync<ErrorOnValidationException>();
        exception.Which.ErrorMessages.Should().Contain(ResourceMessagesException.RECIPE_TITLE_EMPTY);
        exception.Which.ErrorMessages.Should().Contain(ResourceMessagesException.AT_LEAST_ONE_INGREDIENT);
        exception.Which.ErrorMessages.Should().Contain(ResourceMessagesException.AT_LEAST_ONE_INSTRUCTION);
    }

    private static RegisterRecipeUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user)
    {
        var loggedUser = LoggedUserBuilder.Build(user!);
        var unitOfWork = UnitOfWorkBuilder.Build();
        var mapper = MapperBuilder.Build();
        var writeOnlyRepository = RecipeWriteOnlyRepositoryBuilder.Build();
        var blobStorageService = new BlobStorageServiceBuilder().Build();

        return new RegisterRecipeUseCase(writeOnlyRepository, blobStorageService, loggedUser, unitOfWork, mapper);
    }
}