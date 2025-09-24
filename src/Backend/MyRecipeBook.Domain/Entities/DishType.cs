namespace MyRecipeBook.Domain.Entities;

public class DishType
{
    public Enums.DishType Type { get; set; }
    public long RecipeId { get; set; }
}