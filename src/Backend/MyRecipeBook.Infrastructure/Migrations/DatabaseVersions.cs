namespace MyRecipeBook.Infrastructure.Migrations;

public abstract class DatabaseVersions
{
    /*
     * quando usamos o "const" conseguimos chamar as propriedade como se fosse um metodo static
     * DatabaseVersions.TableUser
     */
    public const int TableUser = 1;
}