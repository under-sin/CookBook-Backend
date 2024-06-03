using Microsoft.Extensions.Configuration;

namespace MyRecipeBook.Infrastructure.Extensions;

public static class ConfigurationExtension
{

    /* Configuração para fazer a verificação se estamos em ambiente de test ou não */
    public static bool IsUnitTestEnvironment(this IConfiguration configuration)
    {
        string inMemoryTestValue = configuration.GetSection("InMemoryTest").Value!;
        if (inMemoryTestValue is null)
            return false;

        return true;
    }

    /*
     * O this antes do IConfiguration é para dizer que esse método é uma extensão do IConfiguration
     * Assim podemos chamar esse método como se fosse um método da classe IConfiguration em qualquer instância dela
     */
    public static string ConnectionString(this IConfiguration configuration)
        => configuration.GetConnectionString("Connection")!;
}