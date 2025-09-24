namespace MyRecipeBook.Domain.Entities;

public class Ingredient : EntityBase
{
    public string Item { get; set; }
    public long RecipeId { get; set; }
}