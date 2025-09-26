using Moq;
using MyRecipeBook.Domain.Repositories.Recipes;
using MyRecipeBook.Domain.Repositories.Users;

namespace CommonTestUtilities.Repositories;

public class RecipeWriteOnlyRepositoryBuilder
{
    public static IRecipeWriteOnlyRepository Build()
    {
        var mock = new Mock<IRecipeWriteOnlyRepository>();

        return mock.Object;
    }
}
