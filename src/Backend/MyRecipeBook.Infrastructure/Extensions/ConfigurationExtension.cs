using Microsoft.Extensions.Configuration;

namespace MyRecipeBook.Infrastructure.Extensions;

public static class ConfigurationExtension
{
    /*
     * O this antes do IConfiguration é para dizer que esse método é uma extensão do IConfiguration
     * Assim podemos chamar esse método como se fosse um método da classe IConfiguration em qualquer instância dela
     */
    public static string ConnectionString(this IConfiguration configuration)
        => configuration.GetConnectionString("Connection")!;
}