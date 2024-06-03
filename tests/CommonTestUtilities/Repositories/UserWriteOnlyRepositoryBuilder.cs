using Moq;
using MyRecipeBook.Domain.Repositories.Users;

namespace CommonTestUtilities.Repositories;

public class UserWriteOnlyRepositoryBuilder
{
    public static IUserWriteOnlyRepository Build()
    {
        /* Essas são as únicas implementações que precisam ser feitas 
         * já que esse repositorio não tem retorno 
         */
        var mock = new Mock<IUserWriteOnlyRepository>();

        return mock.Object;
    }
}
