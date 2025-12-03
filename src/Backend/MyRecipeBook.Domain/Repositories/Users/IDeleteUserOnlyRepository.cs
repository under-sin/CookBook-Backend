namespace MyRecipeBook.Domain.Repositories.Users;

public interface IDeleteUserOnlyRepository
{
    Task DeleteAccount(Guid userIdentifier);
}