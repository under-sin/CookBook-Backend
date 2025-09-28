using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Repositories.Recipes;

public interface IRecipeReadOnlyRepository
{
    Task<IList<Recipe>> Filter(User user, FilterRecipeDto filter);
}
