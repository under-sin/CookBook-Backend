﻿using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.Users;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;

public class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository
{
    private readonly MyRecipeBookDbContext _context;

    public UserRepository(MyRecipeBookDbContext context) => _context = context;

    public async Task Add(User user) => await _context.Users.AddAsync(user);

    public void UpdateUserProfile(User user) => _context.Users.Update(user);

    public async Task<bool> ExistActiveUserWithEmail(string email)
        => await _context.Users.AnyAsync(e => e.Email.Equals(email) && e.Active);

    public async Task<bool> EmailExistsForOtherUser(string email, Guid userIdentifier)
        => await _context.Users
            .AnyAsync(u => u.Email.Equals(email) && u.UserIdentifier != userIdentifier && u.Active);

    public async Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifier) 
        => await _context.Users.AnyAsync(e => e.UserIdentifier.Equals(userIdentifier) && e.Active);

    public async Task<User?> GetUserByEmailAndPassword(string email, string password)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Active
                && user.Email.Equals(email)
                && user.Password.Equals(password));
    }
}