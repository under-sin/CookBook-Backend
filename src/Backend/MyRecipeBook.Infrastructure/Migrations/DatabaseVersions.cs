namespace MyRecipeBook.Infrastructure.Migrations;

public abstract class DatabaseVersions
{
    /*
     * quando usamos o "const" conseguimos chamar as propriedade como se fosse um metodo static
     * DatabaseVersions.TableUser
     */
    public const int TABLE_USER = 1;
    public const int TABLE_RECIPES = 2;
}