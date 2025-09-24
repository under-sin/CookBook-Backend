using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.Users;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;

public class UserRepository(MyRecipeBookDbContext context)
    : IUserReadOnlyRepository, IUserWriteOnlyRepository, IUserUpdateOnlyRepository
{
    public async Task Add(User user) => await context.Users.AddAsync(user);

    public void UpdateUserProfile(User user) => context.Users.Update(user);

    public async Task<bool> ExistActiveUserWithEmail(string email)
        => await context.Users.AnyAsync(e => e.Email.Equals(email) && e.Active);

    public async Task<bool> EmailExistsForOtherUser(string email, Guid userIdentifier)
        => await context.Users
            .AnyAsync(u => u.Email.Equals(email) && u.UserIdentifier != userIdentifier && u.Active);

    public async Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifier)
        => await context.Users.AnyAsync(e => e.UserIdentifier.Equals(userIdentifier) && e.Active);

    public async Task<User?> GetUserByEmailAndPassword(string email, string password) {
        return await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Active
                                         && user.Email.Equals(email)
                                         && user.Password.Equals(password));
    }

    public async Task<User> GetByIdAsync(long userId)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == userId && u.Active);

        return user ?? throw new KeyNotFoundException("User not found.");
    }

    public void Update(User user)
    {
        // Garante que as datas sejam tratadas como UTC
        user.CreatedOn = DateTime.SpecifyKind(user.CreatedOn, DateTimeKind.Utc);
        context.Users.Update(user);
    }
}