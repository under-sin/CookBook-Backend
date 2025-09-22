using MyRecipeBook.Domain.Repositories;

namespace MyRecipeBook.Infrastructure.DataAccess;

public class UnitOfWork(MyRecipeBookDbContext context) : IUnitOfWork
{
    public async Task Commit() => await context.SaveChangesAsync();
}
