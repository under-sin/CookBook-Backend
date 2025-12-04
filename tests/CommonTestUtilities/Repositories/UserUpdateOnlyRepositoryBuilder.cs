using Moq;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.Users;

namespace CommonTestUtilities.Repositories;

public class UserUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IUserUpdateOnlyRepository> _repository = new();

    public void GetBeId(User user)
    {
        _repository.Setup(rep => rep.GetByIdAsync(user.Id))
            .ReturnsAsync(user);
    }
    
    public IUserUpdateOnlyRepository Build() => _repository.Object;
}
