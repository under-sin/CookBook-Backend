using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Repositories.Recipes;

public interface IRecipeWriteOnlyRepository
{
    public Task Add(Recipe recipe);
}