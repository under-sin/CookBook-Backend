using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Repositories.Users;

public interface IUserWriteOnlyRepository
{ 
   public Task Add(User user);
   public void UpdateUserProfile(User user);
}