using System;
using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Repositories.Users;

public interface IUserUpdateOnlyRepository
{
    Task<User> GetByIdAsync(long userId);
    void Update(User user);
}
