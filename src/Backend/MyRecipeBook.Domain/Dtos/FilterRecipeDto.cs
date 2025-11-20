using MyRecipeBook.Domain.Enums;

namespace MyRecipeBook.Domain.Dtos;

public record FilterRecipeDto
{
    public string? RecipeTitleIngredient { get; init; }
    public IList<CookingTime> CookingTimes { get; init; } = [];
    public IList<Difficulty> Difficulties { get; init; } = [];
    public IList<DishType> DishTypes { get; init; } = [];
}

