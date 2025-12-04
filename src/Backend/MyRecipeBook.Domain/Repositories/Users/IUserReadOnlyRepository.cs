using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Repositories.Users;

public interface IUserReadOnlyRepository
{
    public Task<bool> ExistActiveUserWithEmail(string email);
    public Task<bool> EmailExistsForOtherUser(string email, Guid userIdentifier);
    public Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifier);
    public Task<User?> GetByEmail(string email);
}