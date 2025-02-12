﻿namespace MyRecipeBook.Domain.Repositories.Users;

public interface IUserReadOnlyRepository
{
    public Task<bool> ExistActiveUserWithEmail(string email);
    public Task<bool> EmailExistsForOtherUser(string email, Guid userIdentifier);
    public Task<Entities.User?> GetUserByEmailAndPassword(string email, string password);
    public Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifier);
}