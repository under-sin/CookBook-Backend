using Moq;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.Users;

namespace CommonTestUtilities.Repositories;

public class UserReadOnlyRepositoryBuilder
{
    /*
    * No caso dos mocks de Read não podemos cria-los como static 
    * pois eles podem ter retorno e precisam ser configurados
    * Nesse caso precisamos criar uma instância do Mock<IUserReadOnlyRepository>()
    */
    private readonly Mock<IUserReadOnlyRepository> _repository;

    public UserReadOnlyRepositoryBuilder() => 
        _repository = new Mock<IUserReadOnlyRepository>();

    public void ExistActiveUserWithEmail(string email)
    {
        // Configuração do mock para o método ExistActiveUserWithEmail retornar true
        _repository.Setup(rep => rep.ExistActiveUserWithEmail(email))
            .ReturnsAsync(true);
    }

    public void GetUserByEmailAndPassword(User user) {
        _repository.Setup(rep => rep.GetUserByEmailAndPassword(user.Email, user.Password))
            .ReturnsAsync(user);
    }
    
    public IUserReadOnlyRepository Build() => _repository.Object;
}
