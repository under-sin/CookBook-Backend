using Moq;
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

    public UserReadOnlyRepositoryBuilder() => _repository = new Mock<IUserReadOnlyRepository>();

    public void ExistActiveUserWithEmail(string email)
    {
        // Configuração do mock para o método ExistActiveUserWithEmail retornar true
        _repository.Setup(x => x.ExistActiveUserWithEmail(email)).ReturnsAsync(true);
    }
    public IUserReadOnlyRepository Build() => _repository.Object;
}
