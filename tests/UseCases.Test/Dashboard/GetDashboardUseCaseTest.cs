using CommonTestUtilities.BlobStorage;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.Dashboard;

namespace UseCases.Test.Dashboard;

public class GetDashboardUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();
        var recipes = RecipeBuilder.Collection(user);

        var useCase = CreateUseCase(user, recipes);

        var result = await useCase.Execute();

        result.Should().NotBeNull();
        result.Recipes.Should().NotBeNullOrEmpty();
        result.Recipes.Should().HaveCount(recipes.Count);
        result.Recipes.Should().AllSatisfy(recipe =>
        {
            recipe.Id.Should().NotBeNullOrWhiteSpace();
            recipe.Title.Should().NotBeNullOrWhiteSpace();
            recipe.AmountIngredients.Should().BeGreaterThan(0);
            recipe.ImageUrl.Should().NotBeNullOrWhiteSpace();
        });
    }

    [Fact]
    public async Task Success_NoRecipes()
    {
        (var user, _) = UserBuilder.Build();
        var recipes = new List<MyRecipeBook.Domain.Entities.Recipe>();

        var useCase = CreateUseCase(user, recipes);

        var result = await useCase.Execute();

        result.Should().NotBeNull();
        result.Recipes.Should().BeEmpty();
    }

    private static DashboardUseCase CreateUseCase(
        MyRecipeBook.Domain.Entities.User user,
        IList<MyRecipeBook.Domain.Entities.Recipe> recipes)
    {
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new RecipeReadOnlyRepositoryBuilder().GetDashboards(user, recipes).Build();
        var blobStorage = new BlobStorageServiceBuilder().GetImageUrl(user, recipes).Build();

        return new DashboardUseCase(repository, mapper, loggedUser, blobStorage);
    }
}
