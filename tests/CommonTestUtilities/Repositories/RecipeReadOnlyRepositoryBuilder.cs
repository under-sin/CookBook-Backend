using Moq;
using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.Recipes;

namespace CommonTestUtilities.Repositories;

public class RecipeReadOnlyRepositoryBuilder
{
    private readonly Mock<IRecipeReadOnlyRepository> _repository;

    public RecipeReadOnlyRepositoryBuilder()
    {
        _repository = new Mock<IRecipeReadOnlyRepository>();
    }

    public RecipeReadOnlyRepositoryBuilder Filter(User user, IList<Recipe> recipes)
    {
        _repository.Setup(rep => rep.Filter(user, It.IsAny<FilterRecipeDto>())).ReturnsAsync(recipes);

        return this;
    }

    public RecipeReadOnlyRepositoryBuilder GetById(User user, Recipe? recipe = null)
    {
        if (recipe is not null)
            _repository.Setup(rep => rep.GetById(user, It.IsAny<long>())).ReturnsAsync(recipe);

        return this;
    }

    public RecipeReadOnlyRepositoryBuilder GetDashboards(User user, IList<Recipe> recipes)
    {
        _repository.Setup(rep => rep.GetDashboards(user)).ReturnsAsync(recipes);

        return this;
    }

    public IRecipeReadOnlyRepository Build() => _repository.Object;
}
